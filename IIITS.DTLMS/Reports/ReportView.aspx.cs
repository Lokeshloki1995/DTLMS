using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.REPORTS.Internal;
using IIITS.DTLMS.REPORTS;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.TCRepair;
using IIITS.DTLMS.BL.WorkAward;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting.WebControls;
using System.Collections;
using CrystalDecisions.Web;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Configuration;

namespace IIITS.DTLMS.Reports
{
    public partial class ReportView : System.Web.UI.Page
    {
        clsSession objSession;
        ParameterFieldDefinitions crParameterFieldDefinitions;
        ParameterFieldDefinition crParameterFieldDefinition;
        ParameterValues crParameterValues = new ParameterValues();
        string strTodayDate = DateTime.Now.ToString("dd-MM-yyy");
        string strFormCode = "ReportView";

        #region Internal Application Report

        crpOperatorReport crpOper = new crpOperatorReport();
        crpDetailedReport crpDetailedFieldReport = new crpDetailedReport();
        crpDetStoreReport crpDetailedStoreReport = new crpDetStoreReport();
        crpLocOperator crpLocOperator = new crpLocOperator();
        crpFieldReport crpLocField = new crpFieldReport();
        crpStoreReport crpLocStore = new crpStoreReport();
        #endregion

        #region DTC Failure Transaction Report

        crpFailure crpPgrsFailure = new crpFailure();

        EstimationReport crpestimation = new EstimationReport();
        //EstimationReportrepairer crpestimationrepairer = new EstimationReportrepairer();
        // TestingRepairerEstimation obj = new TestingRepairerEstimation();

        RepairerEstimate crpestimationrepairer = new RepairerEstimate();
        EstimationReportso crpestimationso = new EstimationReportso();
        //  EstimationReportsorepairer crpestimationsorepairer = new EstimationReportsorepairer();
        RepairerEstimateso crpestimationsorepairer = new RepairerEstimateso();
        Crpschemecapacity objschemecap = new Crpschemecapacity();

        crpEstimationReport crpEstimationReport = new crpEstimationReport();
        CrpWorkOrder crpWorkOrder = new CrpWorkOrder();
        //crpFailEnEstimationReport crpFailEnEstimationReport = new crpFailEnEstimationReport();
        crpGatePass crpGatepassReport = new crpGatePass();
        CrReport crpCRReport = new CrReport();
        crpRiReport crpRIReport = new crpRiReport();
        crpRIAckReport crpRIAck = new crpRIAckReport();
        IndentReport crpIndent = new IndentReport();
        InvoiceReport crpInvoice = new InvoiceReport();
        crpWorkAward crpworkaward = new crpWorkAward();
        CrpMajorworkaward crpmajorwrkawrd = new CrpMajorworkaward();
        CregAbstract objreg = new CregAbstract();
        crpReparierPerformance crpReprierPer = new crpReparierPerformance();
        Transformerwisedetails crpTransformerwise = new Transformerwisedetails();
        crpCompletedPerformance crpCompleted = new crpCompletedPerformance();
        TransilOilNewReport TransilOilNewReport = new TransilOilNewReport();

        #endregion

        #region Capacity Enhanacement Transaction Report
        crpEnhanceEstimation crpEnhanceEst = new crpEnhanceEstimation();
        EnhanceCrReport crpEnhanceCR = new EnhanceCrReport();
        EnhanceIndentReport crpEnhanceIndent = new EnhanceIndentReport();
        EnhanceInvoiceReport crpEnhanceInvoice = new EnhanceInvoiceReport();
        #endregion



        crpNewDtcWorkorder crpNewdtcWo = new crpNewDtcWorkorder();
        crpNewDtcInvoiceReport crpnewdtcInvoice = new crpNewDtcInvoiceReport();
        crpNewDtcIndent crpnewdtcindent = new crpNewDtcIndent();
        crpNewDTcCR_Comm crpnewDTCCR = new crpNewDTcCR_Comm();



        crpCalcEstimation crpCalcEst = new crpCalcEstimation();
        Crpbankstock objbank = new Crpbankstock();
        crpDTRFailureAbstract objDTRFailureAbstract = new crpDTRFailureAbstract();
        Crprepaireryear objreyear = new Crprepaireryear();
        CrpAddedAbstract objaaddabst = new CrpAddedAbstract();
        CrpReplacedAbstract objreplabs = new CrpReplacedAbstract();
        Crprplrepair objrplrep = new Crprplrepair();
        Crpdetailsview objdetail = new Crpdetailsview();
        Crpschemedetails objscheme = new Crpschemedetails();
        crpRepairGatepass crpRepairGatepass = new crpRepairGatepass();
        crpGatePass crpGatePass = new crpGatePass();
        Crprepairerob objrepairerob = new Crprepairerob();
        CrpDtrwise objDtr = new CrpDtrwise();
        crpBufferStock objcrpBufferStock = new crpBufferStock();
        RepairerWorkOrder objrepwo = new RepairerWorkOrder();
        crpScrapInvoice crpScrapInvoice = new crpScrapInvoice();
        crpStoreInvoice crpStoreInvoice = new crpStoreInvoice();
        crpCRAbstract crpCRAbstract = new crpCRAbstract();
        crpWoRegDetails crpWoRegDetails = new crpWoRegDetails();

        crpDtrRepairerWise crpRepairerWise = new crpDtrRepairerWise();
        crpPendingcountRolewise crpRolewiseCount = new crpPendingcountRolewise();
        // CregAbstract crpCompleted = new CregAbstract();

        crpReceiveDtr crpRecieveDTr = new crpReceiveDtr();
        StoreIndentReport crpStoreIndent = new StoreIndentReport();
        crpDTCFailreport objRep = new crpDTCFailreport();
        crpDTCreport crpDTCRep = new crpDTCreport();
        crpAddDtcReport crpAddDTC = new crpAddDtcReport();
        CrReport crpCRDetails = new CrReport();
        CrReportAbstract crpReportDetails = new CrReportAbstract();
        FailureAbstract crpFailureAbstract = new FailureAbstract();
        crpDTCFailFrequent crpDtcFailFrequent = new crpDTCFailFrequent();

        crpRiPermanentReport crpriper = new crpRiPermanentReport();
        CrpPermanentEstimateSO crpperestimateso = new CrpPermanentEstimateSO();
        CrpPermanentEstimate crpperestimate = new CrpPermanentEstimate();
        Crpworkorderpermanent crpworkper = new Crpworkorderpermanent();

        CrReportAbstractpermanet crReportpermanent = new CrReportAbstractpermanet();
        IndentReportpermanent crpindentper = new IndentReportpermanent();
        crpRIAckperReport crpRIAckper = new crpRIAckperReport();

        crpFailedOb crpfailob = new crpFailedOb();
        Crprepairercost objrepcost = new Crprepairercost();
        PGRSReportDetail crpPgrsReport = new PGRSReportDetail();
        crpRepairerReport crprepairdetails = new crpRepairerReport();

        crpFeederBifurcation crpobjFeederBifurcation = new crpFeederBifurcation();
        crpFeederBifurcation_SO objcrpFeederBifurcationSO = new crpFeederBifurcation_SO();

        crpSubDivisionWiseFailureReport ObjcrpSubDivisionWiseFailureReport = new crpSubDivisionWiseFailureReport();

        string strReport = string.Empty;

        string stroffcode = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objSession = (clsSession)Session["clsSession"];
                stroffcode = objSession.OfficeName;
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
                {
                    strReport = Request.QueryString["id"].ToString();
                }

                #region Sub-division Wise Failure report

                if (Request.QueryString["id"] == "SubdivisionWiseFailureDetails")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sType = Request.QueryString["Type"].ToString();
                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                    objReport.sTodate = Request.QueryString["Todate"].ToString();
                    string Month = Request.QueryString["Month"].ToString();
                    string FinancialYear = Request.QueryString["Finyear"].ToString();
                    dt = objReport.PrintSubDivisionwiseTransformerFailureReport(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        ObjcrpSubDivisionWiseFailureReport.SetDataSource(dt);
                        crpPrint.ReportSource = ObjcrpSubDivisionWiseFailureReport;
                        ObjcrpSubDivisionWiseFailureReport.SetParameterValue("month", Month);
                        ObjcrpSubDivisionWiseFailureReport.SetParameterValue("finyear", FinancialYear);
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                #endregion

                #region Internal Application Report




                if (Request.QueryString["id"] == "EnumReport")
                {
                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();

                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.EnumerationReport(strFromdate, strTodate);


                    crpOper.SetDataSource(dt);
                    crpPrint.ReportSource = crpOper;
                    crpPrint.DataBind();


                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpOper.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpOper.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                }

                if (Request.QueryString["id"] == "FeederBifurcationSO")
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    clsReports objreport = new clsReports();
                    if ((Convert.ToString(Request.QueryString["FBS_Id"]).Length > 0))
                    {
                        objreport.sFeederBifurcationID = Request.QueryString["FBS_Id"].ToString();
                    }
                    dt = objreport.PrintFeederBifurcationReportSO(objreport);

                    // objcrpFeederBifurcationSO = new crpFeederBifurcationSO();
                    if (dt.Rows.Count > 0)
                    {
                        objcrpFeederBifurcationSO.SetDataSource(dt);

                        //  objcrpFeederBifurcationSO.OpenSubreport("crpFeederBfrc_so_sub.rpt").SetDataSource(dt1);

                        crpPrint.ReportSource = objcrpFeederBifurcationSO;
                        crpPrint.DataBind();
                        //crpPrint.ID = "DTR Report-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"] == "FeederBifurcation")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    if ((Convert.ToString(Request.QueryString["BifurcationID"]).Length > 0))
                    {
                        objReport.sFeederBifurcationID = Request.QueryString["BifurcationID"].ToString();
                    }

                    if ((Convert.ToString(Request.QueryString["officeCode"]) == " "))
                    {
                        objReport.sOfficeCode = Request.QueryString["officeCode"].ToString();
                    }
                    if ((Convert.ToString(Request.QueryString["oldFeederCode"]).Length > 0))
                    {
                        objReport.sOldFeederCode = Request.QueryString["oldFeederCode"].ToString();
                    }
                    if ((Convert.ToString(Request.QueryString["newFeederCode"]).Length > 0))
                    {
                        objReport.sNewFeederCode = Request.QueryString["newFeederCode"].ToString();
                    }


                    if ((Convert.ToString(Request.QueryString["ReportType"]).Length > 0))
                    {
                        objReport.sReportType = Request.QueryString["ReportType"].ToString();
                    }

                    if ((Request.QueryString["FromDate"]).ToString() != null && (Request.QueryString["FromDate"]).ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if ((Request.QueryString["ToDate"]).ToString() != null && (Request.QueryString["ToDate"]).ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }

                    dt = objReport.PrintFeederBifurcationReport(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        crpobjFeederBifurcation.SetDataSource(dt);
                        crpPrint.ReportSource = crpobjFeederBifurcation;
                        crpPrint.DataBind();
                        //crpPrint.ID = "DTR Report-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }


                if (Request.QueryString["id"] == "DetaiedField")
                {
                    DataTable dtDetailed = new DataTable();
                    clsReports objreport = new clsReports();


                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    dtDetailed = objreport.PrintDetailedFieldReport(strFromdate, strTodate);
                    crpDetailedFieldReport.SetDataSource(dtDetailed);
                    crpPrint.ReportSource = crpDetailedFieldReport;
                    crpPrint.DataBind();

                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpDetailedFieldReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpDetailedFieldReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }

                if (Request.QueryString["id"] == "DetailedStore")
                {
                    DataTable dtDetailed = new DataTable();
                    clsReports objreport = new clsReports();



                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    dtDetailed = objreport.PrintDetailedStoreReport(strFromdate, strTodate);
                    crpDetailedStoreReport.SetDataSource(dtDetailed);
                    crpPrint.ReportSource = crpDetailedStoreReport;
                    crpPrint.DataBind();


                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpDetailedStoreReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpDetailedStoreReport.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }

                if (Request.QueryString["id"] == "LocOperator")
                {
                    string strFromdate = Request.QueryString["FromDate"].ToString();
                    string strTodate = Request.QueryString["ToDate"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.EnumReportLocationWise(strFromdate, strTodate);


                    crpLocOperator.SetDataSource(dt);
                    crpPrint.ReportSource = crpLocOperator;
                    crpPrint.DataBind();


                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    crParameterDiscreteValue4.Value = strFromdate;
                    crParameterFieldDefinitions = crpLocOperator.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    crParameterDiscreteValue5.Value = strTodate;
                    crParameterFieldDefinitions = crpLocOperator.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }

                if (Request.QueryString["id"].ToString().Equals("FieldLoc"))
                {
                    string sFeederCode = Request.QueryString["sFeeder"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sFromdate = Request.QueryString["FromDate"].ToString();
                    string sTodate = Request.QueryString["ToDate"].ToString();
                    string sdatewise = Request.QueryString["Datewise"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.PrintFieldDetails(sFeederCode, sOfficeCode, sFromdate, sTodate, sdatewise);

                    if (dt.Rows.Count == 0)
                    {
                        ShowMsgBox("No Records Found");
                    }
                    else
                    {
                        crpLocField.SetDataSource(dt);
                        crpPrint.ReportSource = crpLocField;
                        crpPrint.DataBind();
                    }


                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    if (sFromdate != "")
                    {
                        crParameterDiscreteValue4.Value = "From " + sFromdate;
                    }
                    else
                    {
                        crParameterDiscreteValue4.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocField.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    if (sTodate != "")
                    {
                        crParameterDiscreteValue5.Value = "To " + sTodate;
                    }
                    else
                    {
                        crParameterDiscreteValue5.Value = "";
                    }

                    crParameterFieldDefinitions = crpLocField.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                }


                if (Request.QueryString["id"].ToString().Equals("StoreLoc"))
                {
                    string strOfficeCode = Request.QueryString["OfficeCode"].ToString();

                    string sFromdate = Request.QueryString["FromDate"].ToString();
                    string sTodate = Request.QueryString["ToDate"].ToString();
                    string sdatewise = Request.QueryString["Datewise"].ToString();

                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.PrintStoreDetails(strOfficeCode, sFromdate, sTodate, sdatewise);
                    if (dt.Rows.Count == 0)
                    {
                        ShowMsgBox("No Records Found");
                    }
                    else
                    {
                        crpLocStore.SetDataSource(dt);
                        crpPrint.ReportSource = crpLocStore;
                        crpPrint.DataBind();
                    }


                    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    if (sFromdate != "")
                    {
                        crParameterDiscreteValue4.Value = "From " + sFromdate;
                    }
                    else
                    {
                        crParameterDiscreteValue4.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocStore.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue4);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                    ParameterDiscreteValue crParameterDiscreteValue5 = new ParameterDiscreteValue();
                    if (sTodate != "")
                    {
                        crParameterDiscreteValue5.Value = "To " + sTodate;
                    }
                    else
                    {
                        crParameterDiscreteValue5.Value = "";
                    }
                    crParameterFieldDefinitions = crpLocStore.DataDefinition.ParameterFields;
                    crParameterFieldDefinition = crParameterFieldDefinitions["crpToDate"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(crParameterDiscreteValue5);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                }

                #endregion

                #region DTC Failure Transaction Report

                if (Request.QueryString["id"].ToString().Equals("Detailsviewaddedabstract"))
                {
                    DataTable dt;
                    DataTable dt1 = new DataTable();
                    clsReports objRep = new clsReports();
                    string presentyear = Request.QueryString["presentyear"].ToString();
                    string previosyear = Request.QueryString["previosyear"].ToString();
                    string month = Request.QueryString["month"].ToString();
                    string previousmonth = Request.QueryString["previousmonth"].ToString();
                    string financialyear = Request.QueryString["financialyear"].ToString();

                    dt = objRep.Detailsaddedabstract(previosyear, presentyear, month, previousmonth);

                    dt1.Columns.Add("OB");
                    dt1.Columns.Add("ADDED");
                    dt1.Columns.Add("ADDEDFY");
                    dt1.Columns.Add("EXISTTOTAL");

                    dt1.Rows.Add(dt.Rows[0]["DT_CODE"], dt.Rows[1]["DT_CODE"], dt.Rows[2]["DT_CODE"]);
                    int failpercet = Convert.ToInt32(dt1.Rows[0]["OB"]) + Convert.ToInt32(dt1.Rows[0]["ADDED"]);
                    dt1.Rows[0][3] = failpercet;

                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();
                    if (dt1.Rows.Count > 0)
                    {
                        objaaddabst.SetDataSource(dt1);
                        crpPrint.ReportSource = objaaddabst;
                        objaaddabst.SetParameterValue("month", month);
                        objaaddabst.SetParameterValue("financialyear", financialyear);
                        objaaddabst.SetParameterValue("previousmonth", previousmonth);
                        crpPrint.DataBind();
                        crParameterFieldDefinitions = objaaddabst.DataDefinition.ParameterFields;
                        objparmfy.Value = month;
                        objparmfy.Value = financialyear;
                        objparmfy.Value = previousmonth;

                        crParameterFieldDefinition = crParameterFieldDefinitions["month"];
                        crParameterFieldDefinition = crParameterFieldDefinitions["financialyear"];
                        crParameterFieldDefinition = crParameterFieldDefinitions["previousmonth"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"].ToString().Equals("ReplacedDetails"))
                {
                    DataTable dt;
                    DataTable dt1 = new DataTable();
                    clsReports objRep = new clsReports();
                    string presentyear = Request.QueryString["presentyear"].ToString();
                    string previosyear = Request.QueryString["previosyear"].ToString();
                    string month = Request.QueryString["Month"].ToString();
                    string financialyear = Request.QueryString["financialyear"].ToString();

                    dt = objRep.getreplacedetails(previosyear, presentyear, month);

                    dt1.Columns.Add("OB");
                    dt1.Columns.Add("FAILED");
                    dt1.Columns.Add("REPLACED");
                    dt1.Columns.Add("CB");
                    dt1.Columns.Add("FAILYEAR");
                    dt1.Columns.Add("REPLACEDYEAR");
                    dt1.Columns.Add("TOTALDTC");
                    dt1.Columns.Add("FAILPER");

                    dt1.Rows.Add(dt.Rows[0]["SUM"], dt.Rows[1]["SUM"], dt.Rows[2]["SUM"], dt.Rows[3]["SUM"],
                        dt.Rows[4]["SUM"], dt.Rows[5]["SUM"], dt.Rows[6]["SUM"]);

                    double failpercet = Convert.ToDouble(dt1.Rows[0]["FAILYEAR"]) / Convert.ToDouble(dt1.Rows[0]["TOTALDTC"]) * 100;
                    dt1.Rows[0][7] = failpercet;
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    if (dt1.Rows.Count > 0)
                    {
                        objreplabs.SetDataSource(dt1);
                        crpPrint.ReportSource = objreplabs;
                        objreplabs.SetParameterValue("month", month);
                        objreplabs.SetParameterValue("financialyear", financialyear);
                        crpPrint.DataBind();
                        crParameterFieldDefinitions = objreplabs.DataDefinition.ParameterFields;
                        objparmfy.Value = month;
                        objparmfy.Value = financialyear;

                        crParameterFieldDefinition = crParameterFieldDefinitions["month"];
                        crParameterFieldDefinition = crParameterFieldDefinitions["financialyear"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "DTRFailureAbstract")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strMonth = Request.QueryString["Month"].ToString();
                    dt = objReport.GetDTRFailureAbstract(strMonth);
                    objDTRFailureAbstract.SetDataSource(dt);
                    crpPrint.ReportSource = objDTRFailureAbstract;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"].ToString().Equals("ReplRepaireddetails"))
                {
                    DataTable dt;
                    DataTable dt1 = new DataTable();
                    clsReports objRep = new clsReports();
                    string presentyear = Request.QueryString["presentyear"].ToString();
                    string previosyear = Request.QueryString["previosyear"].ToString();
                    string month = Request.QueryString["Month"].ToString();
                    dt = objRep.getreplacerepairview(previosyear, presentyear, month);
                    dt1.Columns.Add("OB");
                    dt1.Columns.Add("FAILED");
                    dt1.Columns.Add("REPLACED");
                    dt1.Columns.Add("CB");
                    dt1.Columns.Add("RCENTER");
                    dt1.Columns.Add("REPAIREMONTH");
                    dt1.Columns.Add("REPAIREYEAR");
                    dt1.Rows.Add(dt.Rows[0]["SUM"], dt.Rows[1]["SUM"], dt.Rows[2]["SUM"], dt.Rows[3]["SUM"],
                        dt.Rows[4]["SUM"], dt.Rows[5]["SUM"], dt.Rows[6]["SUM"]);
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    if (dt1.Rows.Count > 0)
                    {
                        objrplrep.SetDataSource(dt1);
                        crpPrint.ReportSource = objrplrep;
                        objrplrep.SetParameterValue("month", month);
                        crpPrint.DataBind();
                        crParameterFieldDefinitions = objrplrep.DataDefinition.ParameterFields;
                        objparmfy.Value = month;

                        crParameterFieldDefinition = crParameterFieldDefinitions["month"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"].ToString().Equals("SchemeDetails"))
                {
                    DataTable dt;
                    DataTable dt1;
                    clsReports objRep = new clsReports();
                    string presentyear = Request.QueryString["presentyear"].ToString();
                    string previosyear = Request.QueryString["previosyear"].ToString();
                    string financialyear = Request.QueryString["financialyear"].ToString();
                    dt = objRep.getSchemedetails(previosyear, presentyear);
                    dt1 = objRep.getReplacedtails(previosyear, presentyear);
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    if (dt.Rows.Count > 0)
                    {
                        objscheme.SetDataSource(dt);
                        objscheme.OpenSubreport("Crpreplacedetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = objscheme;
                        objscheme.SetParameterValue("financialyear", financialyear);
                        crpPrint.DataBind();
                        crParameterFieldDefinitions = objscheme.DataDefinition.ParameterFields;
                        objparmfy.Value = financialyear;

                        crParameterFieldDefinition = crParameterFieldDefinitions["financialyear"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"].ToString().Equals("SchemeDetailscapacitywise"))
                {
                    DataTable dt;

                    clsReports objRep = new clsReports();
                    string fromdate = Request.QueryString["fromdate"].ToString();
                    string month = Request.QueryString["month"].ToString();
                    dt = objRep.getSchemedetailscapacitywise(fromdate);
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    if (dt.Rows.Count > 0)
                    {
                        objschemecap.SetDataSource(dt);
                        crpPrint.ReportSource = objschemecap;
                        objschemecap.SetParameterValue("month", month);
                        crpPrint.DataBind();
                        crParameterFieldDefinitions = objschemecap.DataDefinition.ParameterFields;
                        objparmfy.Value = month;

                        crParameterFieldDefinition = crParameterFieldDefinitions["month"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"].ToString().Equals("Detailsview"))
                {
                    DataTable dt;
                    DataTable dt1 = new DataTable();
                    clsReports objRep = new clsReports();
                    string presentyear = Request.QueryString["presentyear"].ToString();
                    string previosyear = Request.QueryString["previosyear"].ToString();
                    string financialyear = Request.QueryString["financialyear"].ToString();

                    dt = objRep.getdetailsview(previosyear, presentyear);
                    dt1.Columns.Add("REPAIRED");
                    dt1.Columns.Add("STOCK");
                    dt1.Columns.Add("REPAIRER");
                    dt1.Columns.Add("NEWTCS");
                    dt1.Rows.Add(dt.Rows[0]["REPAIREDS"], dt.Rows[1]["REPAIREDS"], dt.Rows[2]["REPAIREDS"], dt.Rows[3]["REPAIREDS"]);
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    if (dt1.Rows.Count > 0)
                    {
                        objdetail.SetDataSource(dt1);
                        crpPrint.ReportSource = objdetail;
                        objdetail.SetParameterValue("financialyear", financialyear);
                        crpPrint.DataBind();
                        crParameterFieldDefinitions = objdetail.DataDefinition.ParameterFields;
                        objparmfy.Value = financialyear;

                        crParameterFieldDefinition = crParameterFieldDefinitions["financialyear"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"].ToString().Equals("RepairerIncurred"))
                {
                    DataTable dt;
                    clsReports objRep = new clsReports();
                    string Officecode = Request.QueryString["Officecode"].ToString();
                    string fromdate = Request.QueryString["fromdate"].ToString();
                    string todate = Request.QueryString["todate"].ToString();
                    objRep.sOfficeCode = Officecode;
                    objRep.sFromDate = fromdate;
                    objRep.sTodate = todate;
                    dt = objRep.getrepairerincurreddetails(objRep);

                    if (dt.Rows.Count > 0)
                    {
                        objrepcost.SetDataSource(dt);
                        crpPrint.ReportSource = objrepcost;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"].ToString().Equals("FailedOb"))
                {
                    DataTable dt;
                    clsReports objRep = new clsReports();
                    string Officecode = Request.QueryString["Officecode"].ToString();
                    string month = Request.QueryString["Month"].ToString();
                    objRep.sOfficeCode = Officecode;
                    objRep.sFromDate = month;
                    dt = objRep.Getfailureob(objRep);
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    if (dt.Rows.Count > 0)
                    {
                        crpfailob.SetDataSource(dt);
                        crpPrint.ReportSource = crpfailob;
                        crpfailob.SetParameterValue("month", objRep.sFromDate);
                        crpPrint.DataBind();

                        crParameterFieldDefinitions = crpfailob.DataDefinition.ParameterFields;
                        objparmfy.Value = objRep.sFromDate;
                        crParameterFieldDefinition = crParameterFieldDefinitions["month"];
                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Add(objparmfy);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"].ToString().Equals("RepairerOb"))
                {
                    DataTable dt;
                    clsReports objRep = new clsReports();
                    string Officecode = Request.QueryString["Officecode"].ToString();
                    string month = Request.QueryString["Month"].ToString();
                    objRep.sOfficeCode = Officecode;
                    objRep.sFromDate = month;
                    dt = objRep.GetRepairerob(objRep);

                    if (dt.Rows.Count > 0)
                    {
                        objrepairerob.SetDataSource(dt);
                        crpPrint.ReportSource = objrepairerob;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"] == "RepairCenterDetails")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    dt = objReport.printDtcRepairDetails(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        crprepairdetails.SetDataSource(dt);
                        crpPrint.ReportSource = crprepairdetails;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                if (Request.QueryString["id"].ToString().Equals("Estimation"))
                {
                    string sFailureId = Request.QueryString["FailureId"].ToString();
                    clsReports objRep = new clsReports();
                    DataTable dtCommEstimation = new DataTable();
                    dtCommEstimation = objRep.PrintEstimatedReport(sFailureId, "", "", "", "", "");
                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimatedReport(sFailureId, "", "", "", "", "");
                    DataTable dtSurvey = new DataTable();
                    crpEstimationReport.OpenSubreport("CrpCommEstimationReport.rpt").SetDataSource(dtCommEstimation);
                    crpEstimationReport.OpenSubreport("crpDeCommEstReport.rpt").SetDataSource(dtDecomEstimation);
                    crpPrint.ReportSource = crpEstimationReport;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"].ToString().Equals("EstimationSO"))
                {
                    clsApproval objApp = new clsApproval();
                    DataTable dt = new DataTable();
                    string sTCcode = Request.QueryString["TCcode"].ToString();
                    string sWoId = Request.QueryString["WOId"].ToString();
                    string sRes = string.Empty;
                    string sEnhcCap = string.Empty;
                    clsReports objRep = new clsReports();
                    clsFailureEntry objFailure = new clsFailureEntry();
                    string sWfoID = objFailure.getWfoIDforEstimationSO(sWoId);
                    dt = objApp.GetDatatableFromXML(sWfoID);  //Get Xml Data

                    if (dt.Columns.Contains("DF_REASON"))
                        sRes = dt.Rows[0]["DF_REASON"].ToString().Replace("ç", ",");
                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                        sEnhcCap = dt.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();

                    DataTable dtCommEstimation;
                    dtCommEstimation = objRep.PrintEstimatedReportSO(sTCcode, sWoId, sEnhcCap);
                    DataTable dtDecomEstimation = new DataTable();
                    DataTable dtSurvey = new DataTable();


                    dtDecomEstimation = objRep.PrintDecomEstimationReportSO(sTCcode, sWoId, sRes, sEnhcCap);

                    //  dtSurvey = objRep.PrintSurveyReportSO(dt, sWoId, sTCcode, sEnhcCap);
                    crpEstimationReport.OpenSubreport("CrpCommEstimationReport.rpt").SetDataSource(dtCommEstimation);
                    crpEstimationReport.OpenSubreport("crpDeCommEstReport.rpt").SetDataSource(dtDecomEstimation);
                    //crpEstimationReport.OpenSubreport("crpServeyReport.rpt").SetDataSource(dtSurvey);
                    crpPrint.ReportSource = crpEstimationReport;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"].ToString().Equals("RefinedEstimation"))
                {
                    string sEstId = Request.QueryString["EstimationId"].ToString();
                    clsReports objRep = new clsReports();

                    DataTable dtEstimation = new DataTable();
                    dtEstimation = objRep.GetEstimationDetails(sEstId);

                    DataTable dtEstimationTotal = new DataTable();
                    dtEstimationTotal = objRep.GetEstimationAmount(sEstId);
                    dtEstimation.Columns.Add("DTR_CODE", typeof(string));
                    DataRow drow = dtEstimation.NewRow();
                    for (int i = 0; i < dtEstimation.Rows.Count; i++)
                    {
                        dtEstimation.Rows[i]["DTR_CODE"] = dtEstimationTotal.Rows[0]["DTR_CODE"].ToString();
                    }
                    dtEstimationTotal.Columns.Remove("DTR_CODE");
                    crpestimation.SetDataSource(dtEstimation);
                    crpestimation.OpenSubreport("EstimationSubReport").SetDataSource(dtEstimationTotal);

                    crpPrint.ReportSource = crpestimation;
                    crpPrint.DataBind();
                }



                if (Request.QueryString["id"].ToString().Equals("RefinedEstimationrepairer"))
                {
                    string sEstId = Request.QueryString["EstimationId"].ToString();
                    clsReports objRep = new clsReports();

                    DataTable dtEstimation = new DataTable();
                    dtEstimation = objRep.GetEstimationDetailsrepairer(sEstId);

                    DataTable dtEstimationTotal = new DataTable();
                    dtEstimationTotal = objRep.GetEstimationAmountrepairer(sEstId);
                    dtEstimation.Columns.Add("DTR_CODE", typeof(string));
                    DataRow drow = dtEstimation.NewRow();
                    for (int i = 0; i < dtEstimation.Rows.Count; i++)
                    {
                        dtEstimation.Rows[i]["DTR_CODE"] = dtEstimationTotal.Rows[0]["DTR_CODE"].ToString();
                    }
                    dtEstimationTotal.Columns.Remove("DTR_CODE");
                    crpestimationrepairer.SetDataSource(dtEstimation);
                    crpestimationrepairer.OpenSubreport("Repairerestimatetotal.rpt").SetDataSource(dtEstimationTotal);

                    crpPrint.ReportSource = crpestimationrepairer;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"].ToString().Equals("RefinedEstimationSO"))
                {
                    string sWfo_ID = Request.QueryString["sWFOID"].ToString();
                    string sDtrcode = Request.QueryString["sDtrcode"].ToString();



                    clsApproval objApp = new clsApproval();
                    clsReports objRep = new clsReports();
                    clsEstimation objest = new clsEstimation();

                    // string sLastRepair = Request.QueryString["sRepair"].ToString();
                    DataSet dtfailureEst = new DataSet();
                    dtfailureEst = objApp.GetDatatableFromMultipleXML(sWfo_ID);



                    DataTable dtaddrow = new DataTable();
                    DataRow dtrow = dtaddrow.NewRow();
                    DataTable dtFinalResult = new DataTable();
                    DataTable dtEstimationmaterial = new DataTable();
                    DataTable dtEstimationalmaterial = new DataTable();
                    DataTable dtEstimationaldetails = new DataTable();
                    DataTable dtRepairName = new DataTable();

                    dtEstimationaldetails = objest.gettcdetails(sDtrcode);
                    string sLastRepair = Convert.ToString(dtfailureEst.Tables[0].Rows[0]["EST_REPAIRER"]);
                    dtRepairName = objest.GetRepairName(sLastRepair);


                    dtaddrow.Columns.Add("EST_ID", typeof(string));
                    dtaddrow.Columns.Add("EST_FAILUREID", typeof(string));
                    dtaddrow.Columns.Add("EST_CAPACITY", typeof(string));
                    dtaddrow.Columns.Add("MRIM_REMARKS", typeof(string));
                    dtaddrow.Columns.Add("MRIM_ITEM_NAME", typeof(string));
                    dtaddrow.Columns.Add("MD_NAME", typeof(string));
                    dtaddrow.Columns.Add("ESTM_ITEM_QNTY", typeof(string));
                    dtaddrow.Columns.Add("ESTM_ITEM_RATE", typeof(string));
                    dtaddrow.Columns.Add("AMOUNT", typeof(string));
                    dtaddrow.Columns.Add("ESTM_ITEM_TAX", typeof(string));
                    dtaddrow.Columns.Add("ESTM_ITEM_TOTAL", typeof(string));
                    dtaddrow.Columns.Add("MRIM_ITEM_ID", typeof(string));
                    // dtaddrow.Columns.Add("TR_NAME", typeof(string));



                    for (int i = 0; i < dtfailureEst.Tables.Count; i++)
                    {
                        if (dtfailureEst.Tables[i].Rows.Count > 0)
                        {

                            if (i == 0)
                            {

                                objest.sFailureId = Convert.ToString(dtfailureEst.Tables[0].Rows[0]["EST_FAILUREID"]);
                                objest.sReplaceCapacity = Convert.ToString(dtfailureEst.Tables[0].Rows[0]["EST_CAPACITY"]);
                                //objest.sLastRepair = Convert.ToString(dtfailureEst.Tables[0].Rows[0]["EST_REPAIRER"]);

                                dtaddrow = CreateDatatable(objest);

                            }
                            else if (i == 1)
                            {


                                objest.sremarks = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                                objest.sMaterialQnty = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["ESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["ESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRIM_ITEM_ID"]);
                                dtFinalResult = CreateDatatableFromString(objest);



                            }
                            else if (i == 2)
                            {

                                objest.sremarks = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                                objest.sMaterialQnty = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["ESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["ESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtfailureEst.Tables[2].Rows[0]["MRIM_ITEM_ID"]);

                                dtEstimationmaterial = CreateDatatableFromString(objest);
                                dtFinalResult.Merge(dtEstimationmaterial);
                                dtEstimationmaterial.AcceptChanges();

                            }
                            else if (i == 3)
                            {

                                objest.sremarks = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                                objest.sMaterialQnty = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["ESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["ESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtfailureEst.Tables[3].Rows[0]["MRIM_ITEM_ID"]);

                                dtEstimationalmaterial = CreateDatatableFromString(objest);
                                dtFinalResult.Merge(dtEstimationalmaterial);
                                dtEstimationalmaterial.AcceptChanges();

                            }
                        }
                    }



                    DataColumn dcID = dtaddrow.Columns["EST_FAILUREID"];
                    DataColumn dcCap = dtaddrow.Columns["EST_CAPACITY"];
                    DataColumn dcRepName = dtRepairName.Columns["TR_NAME"];
                    DataColumn dcOmname = dtEstimationaldetails.Columns["OM_NAME"];
                    DataColumn dcName = dtEstimationaldetails.Columns["TM_NAME"];
                    DataColumn dcDtrcode = dtEstimationaldetails.Columns["DTR_CODE"];

                    dtFinalResult.Columns.Add(dcID.ColumnName, dcID.DataType);
                    dtFinalResult.Columns.Add(dcCap.ColumnName, dcCap.DataType);
                    dtFinalResult.Columns.Add(dcRepName.ColumnName, dcRepName.DataType);
                    dtFinalResult.Columns.Add(dcOmname.ColumnName, dcOmname.DataType);
                    dtFinalResult.Columns.Add(dcName.ColumnName, dcName.DataType);
                    dtFinalResult.Columns.Add(dcDtrcode.ColumnName, dcDtrcode.DataType);

                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {

                        dtFinalResult.Rows[i]["EST_FAILUREID"] = dtaddrow.Rows[0]["EST_FAILUREID"];
                        dtFinalResult.Rows[i]["EST_CAPACITY"] = dtaddrow.Rows[0]["EST_CAPACITY"];
                        dtFinalResult.Rows[i]["TR_NAME"] = dtRepairName.Rows[0]["TR_NAME"];
                        dtFinalResult.Rows[i]["OM_NAME"] = dtEstimationaldetails.Rows[0]["OM_NAME"];
                        dtFinalResult.Rows[i]["TM_NAME"] = dtEstimationaldetails.Rows[0]["TM_NAME"];
                        dtFinalResult.Rows[i]["DTR_CODE"] = dtEstimationaldetails.Rows[0]["DTR_CODE"];
                    }


                    dtFinalResult.Columns.Add("EstTotal", typeof(string));
                    dtFinalResult.Columns.Add("EstAmount", typeof(string));
                    dtFinalResult.Columns.Add("EstTax", typeof(string));


                    Double Mtotalamount = 0.0;
                    Double Mtotaltax = 0.0;
                    Double Mamount = 0.0;

                    Double Ltotalamount = 0.0;
                    Double Ltotaltax = 0.0;
                    Double Lamount = 0.0;

                    Double Stotalamount = 0.0;
                    Double Stotaltax = 0.0;
                    Double Samount = 0.0;

                    Double Totalamount = 0.0;
                    Double totaltax = 0.0;
                    Double amount = 0.0;

                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {
                        if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "1 Materials")
                        {

                            Mamount = Mamount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                            Mtotaltax = Mtotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["ESTM_ITEM_TAX"]);
                            Mtotalamount = Mtotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["ESTM_ITEM_TOTAL"]);

                        }
                        else if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "2 Labour Charges")
                        {
                            Lamount = Lamount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                            Ltotaltax = Ltotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["ESTM_ITEM_TAX"]);
                            Ltotalamount = Ltotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["ESTM_ITEM_TOTAL"]);
                        }

                        else if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "3 Salvage")
                        {
                            Samount = Samount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                            Stotaltax = Stotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["ESTM_ITEM_TAX"]);
                            Stotalamount = Stotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["ESTM_ITEM_TOTAL"]);
                        }


                    }
                    amount = (Mamount + Lamount) - Samount;
                    totaltax = (Mtotaltax + Ltotaltax) - Stotaltax;
                    Totalamount = (Mtotalamount + Ltotalamount) - Stotalamount;

                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {

                        dtFinalResult.Rows[i]["EstTotal"] = Totalamount;
                        dtFinalResult.Rows[i]["EstAmount"] = amount;
                        dtFinalResult.Rows[i]["EstTax"] = totaltax;

                    }

                    crpestimationso.SetDataSource(dtFinalResult);
                    crpPrint.ReportSource = crpestimationso;
                    crpPrint.DataBind();
                }





                //if (Request.QueryString["id"].ToString().Equals("RefinedEstimationSOrepairer"))
                //{
                //    string sWfo_ID = Request.QueryString["sWFOID"].ToString();
                //    string sDtrcode = Request.QueryString["sDtrcode"].ToString();

                //    clsApproval objApp = new clsApproval();
                //    clsReports objRep = new clsReports();
                //    ClsRepairerEstimate objest = new ClsRepairerEstimate();

                //    DataSet dtEst = new DataSet();
                //    dtEst = objApp.GetDatatableFromMultipleXML(sWfo_ID);

                //    DataTable dtaddrow = new DataTable();
                //    DataRow dtrow = dtaddrow.NewRow();
                //    DataTable dtFinalResult = new DataTable();
                //    DataTable dtEstimationmaterial = new DataTable();
                //    DataTable dtEstimationalmaterial = new DataTable();
                //    DataTable dtEstimationaldetails = new DataTable();
                //    // DataTable dtphase = new DataTable();

                //    dtEstimationaldetails = objest.gettcdetails(sDtrcode);

                //    dtaddrow.Columns.Add("RESTD_ID", typeof(string));
                //    // dtaddrow.Columns.Add("RESTD_FAILUREID", typeof(string));
                //    dtaddrow.Columns.Add("RESTD_CAPACITY", typeof(string));
                //    dtaddrow.Columns.Add("MRIM_REMARKS", typeof(string));
                //    dtaddrow.Columns.Add("MRIM_ITEM_NAME", typeof(string));
                //    dtaddrow.Columns.Add("MD_NAME", typeof(string));
                //    dtaddrow.Columns.Add("RESTM_ITEM_QNTY", typeof(string));
                //    dtaddrow.Columns.Add("RESTM_ITEM_RATE", typeof(string));
                //    dtaddrow.Columns.Add("AMOUNT", typeof(string));
                //    dtaddrow.Columns.Add("RESTM_ITEM_TAX", typeof(string));
                //    dtaddrow.Columns.Add("RESTM_ITEM_TOTAL", typeof(string));
                //    dtaddrow.Columns.Add("MRIM_ITEM_ID", typeof(string));
                //    dtaddrow.Columns.Add("RESTD_COIL_TYPE", typeof(string));
                //    dtaddrow.Columns.Add("RESTD_PHASE", typeof(string));




                //    for (int i = 0; i < dtEst.Tables.Count; i++)
                //    {
                //        if (dtEst.Tables[i].Rows.Count > 0)
                //        {

                //            if (i == 0)
                //            {

                //                objest.sEstid = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_ID"]);
                //                objest.sReplaceCapacity = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_CAPACITY"]);
                //                objest.coiltype = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_COIL_TYPE"]);
                //                objest.sPhases = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_PHASE"]);

                //                dtaddrow = CreateDatatable(objest);

                //            }
                //            else if (i == 1)
                //            {


                //                objest.sremarks = Convert.ToString(dtEst.Tables[1].Rows[0]["MRIM_REMARKS"]);
                //                objest.sMaterialName = Convert.ToString(dtEst.Tables[1].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                //                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[1].Rows[0]["RESTM_ITEM_QNTY"]);
                //                objest.sMaterialRate = Convert.ToString(dtEst.Tables[1].Rows[0]["MRI_BASE_RATE"]);
                //                objest.sAmount = Convert.ToString(dtEst.Tables[1].Rows[0]["AMOUNT"]);
                //                objest.sMaterialTax = Convert.ToString(dtEst.Tables[1].Rows[0]["RESTM_ITEM_TAX"]);
                //                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[1].Rows[0]["MRI_TOTAL"]);
                //                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[1].Rows[0]["MD_NAME"]);
                //                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[1].Rows[0]["MRIM_ITEM_ID"]);
                //                dtFinalResult = CreateDatatableFromString(objest);



                //            }
                //            else if (i == 2)
                //            {

                //                objest.sremarks = Convert.ToString(dtEst.Tables[2].Rows[0]["MRIM_REMARKS"]);
                //                objest.sMaterialName = Convert.ToString(dtEst.Tables[2].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                //                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[2].Rows[0]["RESTM_ITEM_QNTY"]);
                //                objest.sMaterialRate = Convert.ToString(dtEst.Tables[2].Rows[0]["MRI_BASE_RATE"]);
                //                objest.sAmount = Convert.ToString(dtEst.Tables[2].Rows[0]["AMOUNT"]);
                //                objest.sMaterialTax = Convert.ToString(dtEst.Tables[2].Rows[0]["RESTM_ITEM_TAX"]);
                //                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[2].Rows[0]["MRI_TOTAL"]);
                //                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[2].Rows[0]["MD_NAME"]);
                //                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[2].Rows[0]["MRIM_ITEM_ID"]);

                //                dtEstimationmaterial = CreateDatatableFromString(objest);
                //                dtFinalResult.Merge(dtEstimationmaterial);
                //                dtEstimationmaterial.AcceptChanges();

                //            }
                //            else if (i == 3)
                //            {

                //                objest.sremarks = Convert.ToString(dtEst.Tables[3].Rows[0]["MRIM_REMARKS"]);
                //                objest.sMaterialName = Convert.ToString(dtEst.Tables[3].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                //                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[3].Rows[0]["RESTM_ITEM_QNTY"]);
                //                objest.sMaterialRate = Convert.ToString(dtEst.Tables[3].Rows[0]["MRI_BASE_RATE"]);
                //                objest.sAmount = Convert.ToString(dtEst.Tables[3].Rows[0]["AMOUNT"]);
                //                objest.sMaterialTax = Convert.ToString(dtEst.Tables[3].Rows[0]["RESTM_ITEM_TAX"]);
                //                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[3].Rows[0]["MRI_TOTAL"]);
                //                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[3].Rows[0]["MD_NAME"]);
                //                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[3].Rows[0]["MRIM_ITEM_ID"]);

                //                dtEstimationalmaterial = CreateDatatableFromString(objest);
                //                dtFinalResult.Merge(dtEstimationalmaterial);
                //                dtEstimationalmaterial.AcceptChanges();

                //            }
                //        }
                //    }



                //    DataColumn dcID = dtaddrow.Columns["RESTD_ID"];
                //    DataColumn dcCap = dtaddrow.Columns["RESTD_CAPACITY"];
                //    DataColumn dcCoil = dtaddrow.Columns["RESTD_COIL_TYPE"];
                //    DataColumn dcPhases = dtaddrow.Columns["RESTD_PHASE"];
                //    DataColumn dcOmname = dtEstimationaldetails.Columns["OM_NAME"];
                //    DataColumn dcName = dtEstimationaldetails.Columns["TM_NAME"];
                //    DataColumn dcDtrcode = dtEstimationaldetails.Columns["DTR_CODE"];
                //    //if (Convert.ToString(dtaddrow.Columns["RESTD_COIL_TYPE"]) == "1")
                //    //  {
                //    //     // obj.sPhases = Convert.ToString(dtEstimation.Rows[0]["RESTD_PHASE"]);
                //    //  DataColumn dcCoil ="SINGLE COIL";
                //    //  }
                //    //  else{
                //    //       dtFinalResult.Rows[i]["RESTD_COIL_TYPE"] = "MULTI COIL";
                //    //      }

                //    dtFinalResult.Columns.Add(dcID.ColumnName, dcID.DataType);
                //    dtFinalResult.Columns.Add(dcCap.ColumnName, dcCap.DataType);
                //    dtFinalResult.Columns.Add(dcCoil.ColumnName, dcCoil.DataType);
                //    dtFinalResult.Columns.Add(dcPhases.ColumnName, dcPhases.DataType);
                //    dtFinalResult.Columns.Add(dcOmname.ColumnName, dcOmname.DataType);
                //    dtFinalResult.Columns.Add(dcName.ColumnName, dcName.DataType);
                //    dtFinalResult.Columns.Add(dcDtrcode.ColumnName, dcDtrcode.DataType);

                //    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                //    {

                //        dtFinalResult.Rows[i]["RESTD_ID"] = dtaddrow.Rows[0]["RESTD_ID"];
                //        dtFinalResult.Rows[i]["RESTD_CAPACITY"] = dtaddrow.Rows[0]["RESTD_CAPACITY"];
                //        dtFinalResult.Rows[i]["RESTD_COIL_TYPE"] = dtaddrow.Rows[0]["RESTD_COIL_TYPE"];
                //        dtFinalResult.Rows[i]["RESTD_PHASE"] = dtaddrow.Rows[0]["RESTD_PHASE"];
                //        dtFinalResult.Rows[i]["OM_NAME"] = dtEstimationaldetails.Rows[0]["OM_NAME"];
                //        dtFinalResult.Rows[i]["TM_NAME"] = dtEstimationaldetails.Rows[0]["TM_NAME"];
                //        dtFinalResult.Rows[i]["DTR_CODE"] = dtEstimationaldetails.Rows[0]["DTR_CODE"];


                //    }


                //    dtFinalResult.Columns.Add("EstTotal", typeof(string));
                //    dtFinalResult.Columns.Add("EstAmount", typeof(string));
                //    dtFinalResult.Columns.Add("EstTax", typeof(string));


                //    Double Mtotalamount = 0.0;
                //    Double Mtotaltax = 0.0;
                //    Double Mamount = 0.0;

                //    Double Ltotalamount = 0.0;
                //    Double Ltotaltax = 0.0;
                //    Double Lamount = 0.0;

                //    Double Stotalamount = 0.0;
                //    Double Stotaltax = 0.0;
                //    Double Samount = 0.0;

                //    Double Totalamount = 0.0;
                //    Double totaltax = 0.0;
                //    Double amount = 0.0;

                //    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                //    {
                //        if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "1 Materials")
                //        {

                //            Mamount = Mamount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                //            Mtotaltax = Mtotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TAX"]);
                //            Mtotalamount = Mtotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TOTAL"]);

                //        }
                //        else if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "2 Labour Charges")
                //        {
                //            Lamount = Lamount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                //            Ltotaltax = Ltotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TAX"]);
                //            Ltotalamount = Ltotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                //        }

                //        else if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "3 Salvage")
                //        {
                //            Samount = Samount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                //            Stotaltax = Stotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TAX"]);
                //            Stotalamount = Stotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                //        }


                //    }
                //    amount = (Mamount + Lamount) - Samount;
                //    totaltax = (Mtotaltax + Ltotaltax) - Stotaltax;
                //    Totalamount = (Mtotalamount + Ltotalamount) - Stotalamount;

                //    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                //    {

                //        dtFinalResult.Rows[i]["EstTotal"] = Totalamount;
                //        dtFinalResult.Rows[i]["EstAmount"] = amount;
                //        dtFinalResult.Rows[i]["EstTax"] = totaltax;

                //    }

                //    crpestimationsorepairer.SetDataSource(dtFinalResult);
                //    crpPrint.ReportSource = crpestimationsorepairer;
                //    crpPrint.DataBind();
                //}


                if (Request.QueryString["id"].ToString().Equals("RefinedEstimationSOrepairer"))
                {
                    string sWfo_ID = Request.QueryString["sWFOID"].ToString();
                    string sDtrcode = Request.QueryString["sDtrcode"].ToString();

                    string skavika = Convert.ToString(ConfigurationSettings.AppSettings["KAVIKA_NEW"]);

                    clsApproval objApp = new clsApproval();
                    clsReports objRep = new clsReports();
                    ClsRepairerEstimate objest = new ClsRepairerEstimate();

                    DataSet dtEst = new DataSet();
                    dtEst = objApp.GetDatatableFromMultipleXML(sWfo_ID);

                    DataTable dtaddrow = new DataTable();
                    DataRow dtrow = dtaddrow.NewRow();
                    DataTable dtFinalResult = new DataTable();
                    DataTable dtFinaloilResult = new DataTable();
                    DataTable dtEstimationmaterial = new DataTable();
                    DataTable dtEstimationalmaterial = new DataTable();
                    DataTable dtEstimationaldetails = new DataTable();
                    DataTable dtEstimationoildetails = new DataTable();
                    // DataTable dtphase = new DataTable();

                    dtEstimationaldetails = objest.gettcdetails(sDtrcode);

                    dtaddrow.Columns.Add("RESTD_ID", typeof(string));
                    // dtaddrow.Columns.Add("RESTD_FAILUREID", typeof(string));
                    dtaddrow.Columns.Add("RESTD_NO", typeof(string));
                    dtaddrow.Columns.Add("RESTD_CRON", typeof(string));
                    dtaddrow.Columns.Add("RESTD_CAPACITY", typeof(string));
                    dtaddrow.Columns.Add("MRIM_REMARKS", typeof(string));
                    dtaddrow.Columns.Add("MRIM_ITEM_NAME", typeof(string));
                    dtaddrow.Columns.Add("MD_NAME", typeof(string));
                    dtaddrow.Columns.Add("RESTM_ITEM_QNTY", typeof(string));
                    dtaddrow.Columns.Add("RESTM_ITEM_RATE", typeof(string));
                    dtaddrow.Columns.Add("AMOUNT", typeof(string));
                    dtaddrow.Columns.Add("RESTM_ITEM_TAX", typeof(string));
                    dtaddrow.Columns.Add("RESTM_ITEM_TOTAL", typeof(string));
                    dtaddrow.Columns.Add("MRIM_ITEM_ID", typeof(string));
                    dtaddrow.Columns.Add("RESTD_COIL_TYPE", typeof(string));
                    dtaddrow.Columns.Add("RESTD_PHASE", typeof(string));





                    for (int i = 0; i < dtEst.Tables.Count; i++)
                    {
                        if (dtEst.Tables[i].Rows.Count > 0)
                        {

                            if (i == 0)
                            {

                                objest.sEstid = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_ID"]);
                                objest.sEstimationNo = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_NO"]);
                                objest.sEstDate = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_DATE"]);
                                objest.sReplaceCapacity = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_CAPACITY"]);
                                objest.coiltype = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_COIL_TYPE"]);
                                objest.sPhases = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_PHASE"]);
                                objest.sRepairer = Convert.ToString(dtEst.Tables[0].Rows[0]["RESTD_REPAIRER"]);

                                dtaddrow = CreateDatatable(objest);

                            }
                            else if (i == 1)
                            {


                                objest.sremarks = Convert.ToString(dtEst.Tables[1].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtEst.Tables[1].Rows[0]["MRIM_ITEM_NAME"]).Replace("?", ",");
                                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[1].Rows[0]["RESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtEst.Tables[1].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtEst.Tables[1].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtEst.Tables[1].Rows[0]["RESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[1].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[1].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[1].Rows[0]["MRIM_ITEM_ID"]);
                                dtFinalResult = CreateDatatableFromString(objest);



                            }
                            else if (i == 2)
                            {

                                objest.sremarks = Convert.ToString(dtEst.Tables[2].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtEst.Tables[2].Rows[0]["MRIM_ITEM_NAME"]).Replace("?", ",");
                                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[2].Rows[0]["RESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtEst.Tables[2].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtEst.Tables[2].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtEst.Tables[2].Rows[0]["RESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[2].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[2].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[2].Rows[0]["MRIM_ITEM_ID"]);

                                dtEstimationmaterial = CreateDatatableFromString(objest);
                                dtFinalResult.Merge(dtEstimationmaterial);
                                dtEstimationmaterial.AcceptChanges();

                            }
                            else if (i == 3)
                            {

                                objest.sremarks = Convert.ToString(dtEst.Tables[3].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtEst.Tables[3].Rows[0]["MRIM_ITEM_NAME"]).Replace("?", ",");
                                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[3].Rows[0]["RESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtEst.Tables[3].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtEst.Tables[3].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtEst.Tables[3].Rows[0]["RESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[3].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[3].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[3].Rows[0]["MRIM_ITEM_ID"]);

                                dtEstimationalmaterial = CreateDatatableFromString(objest);
                                dtFinalResult.Merge(dtEstimationalmaterial);
                                dtEstimationalmaterial.AcceptChanges();

                            }

                            else if (i == 5)
                            {

                                objest.sremarks = Convert.ToString(dtEst.Tables[5].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtEst.Tables[5].Rows[0]["MRIM_ITEM_NAME"]).Replace("?", ",");
                                objest.sMaterialQnty = Convert.ToString(dtEst.Tables[5].Rows[0]["RESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtEst.Tables[5].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtEst.Tables[5].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtEst.Tables[5].Rows[0]["RESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtEst.Tables[5].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtEst.Tables[5].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtEst.Tables[5].Rows[0]["MRIM_ITEM_ID"]);

                                dtEstimationoildetails = CreateDatatableFromString(objest);
                                dtFinaloilResult.Merge(dtEstimationoildetails);
                                dtEstimationoildetails.AcceptChanges();

                            }
                        }
                    }


                    DataColumn dcID = dtaddrow.Columns["RESTD_ID"];
                    DataColumn dcCap = dtaddrow.Columns["RESTD_CAPACITY"];
                    DataColumn dcNO = dtaddrow.Columns["RESTD_NO"];
                    DataColumn dcDate = dtaddrow.Columns["RESTD_CRON"];
                    DataColumn dcCoil = dtaddrow.Columns["RESTD_COIL_TYPE"];
                    DataColumn dcPhases = dtaddrow.Columns["RESTD_PHASE"];
                    DataColumn dcOmname = dtEstimationaldetails.Columns["OM_NAME"];
                    DataColumn dcName = dtEstimationaldetails.Columns["TM_NAME"];
                    DataColumn dcDtrcode = dtEstimationaldetails.Columns["DTR_CODE"];

                    //if (Convert.ToString(dtaddrow.Columns["RESTD_COIL_TYPE"]) == "1")
                    //  {
                    //     // obj.sPhases = Convert.ToString(dtEstimation.Rows[0]["RESTD_PHASE"]);
                    //  DataColumn dcCoil ="SINGLE COIL";
                    //  }
                    //  else{
                    //       dtFinalResult.Rows[i]["RESTD_COIL_TYPE"] = "MULTI COIL";
                    //      }

                    dtFinalResult.Columns.Add(dcID.ColumnName, dcID.DataType);
                    dtFinalResult.Columns.Add(dcCap.ColumnName, dcCap.DataType);
                    dtFinalResult.Columns.Add(dcNO.ColumnName, dcNO.DataType);
                    dtFinalResult.Columns.Add(dcDate.ColumnName, dcDate.DataType);
                    dtFinalResult.Columns.Add(dcCoil.ColumnName, dcCoil.DataType);
                    dtFinalResult.Columns.Add(dcPhases.ColumnName, dcPhases.DataType);
                    dtFinalResult.Columns.Add(dcOmname.ColumnName, dcOmname.DataType);
                    dtFinalResult.Columns.Add(dcName.ColumnName, dcName.DataType);
                    dtFinalResult.Columns.Add(dcDtrcode.ColumnName, dcDtrcode.DataType);

                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {

                        dtFinalResult.Rows[i]["RESTD_ID"] = dtaddrow.Rows[0]["RESTD_ID"];
                        dtFinalResult.Rows[i]["RESTD_CAPACITY"] = dtaddrow.Rows[0]["RESTD_CAPACITY"];
                        dtFinalResult.Rows[i]["RESTD_COIL_TYPE"] = dtaddrow.Rows[0]["RESTD_COIL_TYPE"];
                        dtFinalResult.Rows[i]["RESTD_PHASE"] = dtaddrow.Rows[0]["RESTD_PHASE"];
                        dtFinalResult.Rows[i]["OM_NAME"] = dtEstimationaldetails.Rows[0]["OM_NAME"];
                        dtFinalResult.Rows[i]["TM_NAME"] = dtEstimationaldetails.Rows[0]["TM_NAME"];
                        dtFinalResult.Rows[i]["DTR_CODE"] = dtEstimationaldetails.Rows[0]["DTR_CODE"];
                        dtFinalResult.Rows[i]["RESTD_NO"] = dtEst.Tables[0].Rows[0]["RESTD_NO"];
                        dtFinalResult.Rows[i]["RESTD_CRON"] = dtEst.Tables[0].Rows[0]["RESTD_DATE"];


                    }


                    dtFinalResult.Columns.Add("EstTotal", typeof(string));
                    dtFinalResult.Columns.Add("EstAmount", typeof(string));
                    dtFinalResult.Columns.Add("EstTax", typeof(string));


                    dtFinaloilResult.Columns.Add("EstTotal", typeof(string));
                    dtFinaloilResult.Columns.Add("EstAmount", typeof(string));
                    dtFinaloilResult.Columns.Add("EstTax", typeof(string));


                    Double Mtotalamount = 0.0;
                    Double Mtotaltax = 0.0;
                    Double Mamount = 0.0;

                    Double Ltotalamount = 0.0;
                    Double Ltotaltax = 0.0;
                    Double Lamount = 0.0;

                    Double Stotalamount = 0.0;
                    Double Stotaltax = 0.0;
                    Double Samount = 0.0;


                    Double Ototalamount = 0.0;
                    Double Ototaltax = 0.0;
                    Double Oamount = 0.0;

                    Double FinalTotalamount = 0.0;
                    Double Totalamount = 0.0;
                    Double totaltax = 0.0;
                    Double amount = 0.0;


                    for (int i = 0; i < dtFinaloilResult.Rows.Count; i++)
                    {
                        if (Convert.ToString(dtFinaloilResult.Rows[i]["MRIM_REMARKS"]).Trim() == "4 Oil")
                        {

                            if (Convert.ToString(dtFinaloilResult.Rows[i]["MRIM_ITEM_NAME"]).Contains("BAD OIL"))
                            {
                                // OilTotal = Total - OilTotal;
                                Oamount = Oamount - Convert.ToDouble(dtFinaloilResult.Rows[i]["AMOUNT"]);
                                Ototaltax = Ototaltax - Convert.ToDouble(dtFinaloilResult.Rows[i]["RESTM_ITEM_TAX"]);
                                Ototalamount = Ototalamount - Convert.ToDouble(dtFinaloilResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                            }
                            else if (Convert.ToString(dtFinaloilResult.Rows[i]["MRIM_ITEM_NAME"]).Contains("RECLAIMED OIL") && skavika.Equals(objest.sRepairer))
                            {
                                // OilTotal = Total - OilTotal;
                                Oamount = Oamount - Convert.ToDouble(dtFinaloilResult.Rows[i]["AMOUNT"]);
                                Ototaltax = Ototaltax - Convert.ToDouble(dtFinaloilResult.Rows[i]["RESTM_ITEM_TAX"]);
                                Ototalamount = Ototalamount - Convert.ToDouble(dtFinaloilResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                            }
                            else
                            {
                                //OilTotal = OilTotal + Total;
                                Oamount = Oamount + Convert.ToDouble(dtFinaloilResult.Rows[i]["AMOUNT"]);
                                Ototaltax = Ototaltax + Convert.ToDouble(dtFinaloilResult.Rows[i]["RESTM_ITEM_TAX"]);
                                Ototalamount = Ototalamount + Convert.ToDouble(dtFinaloilResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                            }



                        }
                    }

                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {
                        if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "1 Materials")
                        {

                            Mamount = Mamount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                            Mtotaltax = Mtotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TAX"]);
                            Mtotalamount = Mtotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TOTAL"]);

                        }
                        else if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "2 Labour Charges")
                        {
                            Lamount = Lamount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                            Ltotaltax = Ltotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TAX"]);
                            Ltotalamount = Ltotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                        }

                        else if (Convert.ToString(dtFinalResult.Rows[i]["MRIM_REMARKS"]).Trim() == "3 Salvage")
                        {
                            Samount = Samount + Convert.ToDouble(dtFinalResult.Rows[i]["AMOUNT"]);
                            Stotaltax = Stotaltax + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TAX"]);
                            Stotalamount = Stotalamount + Convert.ToDouble(dtFinalResult.Rows[i]["RESTM_ITEM_TOTAL"]);
                        }





                    }
                    amount = (Mamount + Lamount) - Samount;
                    totaltax = (Mtotaltax + Ltotaltax) - Stotaltax;
                    Totalamount = (Mtotalamount + Ltotalamount) - Stotalamount;
                    FinalTotalamount = (Mtotalamount + Ltotalamount + Ototalamount) - Stotalamount;



                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {

                        dtFinalResult.Rows[i]["EstTotal"] = Totalamount;
                        dtFinalResult.Rows[i]["EstAmount"] = amount;
                        dtFinalResult.Rows[i]["EstTax"] = totaltax;

                    }

                    for (int i = 0; i < dtFinaloilResult.Rows.Count; i++)
                    {

                        dtFinaloilResult.Rows[i]["EstTotal"] = Ototalamount;
                        dtFinaloilResult.Rows[i]["EstAmount"] = Oamount;
                        dtFinaloilResult.Rows[i]["EstTax"] = Ototaltax;

                    }


                    crpestimationsorepairer.SetDataSource(dtFinalResult);
                    crpestimationsorepairer.SetParameterValue("finalamont", Convert.ToString(FinalTotalamount));

                    if (dtFinaloilResult.Rows.Count > 0)
                    {
                        crpestimationsorepairer.OpenSubreport("Repairerestimationoil.rpt").SetDataSource(dtFinaloilResult);
                        crpestimationsorepairer.SetParameterValue("subreportvalue", "1");
                    }
                    else
                    {
                        crpestimationsorepairer.OpenSubreport("Repairerestimationoil.rpt").SetDataSource(dtFinaloilResult);
                        crpestimationsorepairer.SetParameterValue("subreportvalue", "0");
                    }
                    crpPrint.ReportSource = crpestimationsorepairer;
                    crpPrint.DataBind();

                    //objaaddabst.SetParameterValue("month", month);
                    //crpPrint.DataBind();
                    crParameterFieldDefinitions = crpestimationsorepairer.DataDefinition.ParameterFields;
                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();
                    objparmfy.Value = FinalTotalamount;
                    crParameterFieldDefinition = crParameterFieldDefinitions["finalamont"];
                    crParameterValues.Add(objparmfy);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }

                if (Request.QueryString["id"].ToString().Equals("PgrsDocket"))
                {
                    string sFailureId = Request.QueryString["FailureId"].ToString();
                    clsReports objRep = new clsReports();

                    DataTable dtfailure = new DataTable();
                    dtfailure = objRep.GetPGRSReport(sFailureId);
                    crpPgrsFailure.SetDataSource(dtfailure);
                    crpPrint.ReportSource = crpPgrsFailure;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"].ToString().Equals("PgrsDocketSO"))
                {
                    string sWfo_ID = Request.QueryString["sWFOID"].ToString();
                    clsApproval objApp = new clsApproval();
                    clsReports objRep = new clsReports();
                    DataTable dtfailure = new DataTable();
                    dtfailure = objApp.GetDatatableFromXML(sWfo_ID);

                    string sDTC_CODE = dtfailure.Rows[0]["DF_DTC_CODE"].ToString();
                    DataTable dtFailLocation = objRep.GetFailLocation(sDTC_CODE);
                    string sPurpose = objRep.GetPurpose(dtfailure.Rows[0]["DF_PURPOSE"].ToString());
                    DataTable dtFinalResult = new DataTable();
                    dtFinalResult.Columns.Add("COMPLAINT_NAME", typeof(string));
                    dtFinalResult.Columns.Add("ZONE", typeof(string));
                    dtFinalResult.Columns.Add("CIRCLE", typeof(string));
                    dtFinalResult.Columns.Add("DIVISION", typeof(string));
                    dtFinalResult.Columns.Add("SUBDIV", typeof(string));
                    dtFinalResult.Columns.Add("TALUK", typeof(string));
                    dtFinalResult.Columns.Add("SECTION", typeof(string));
                    dtFinalResult.Columns.Add("DTCNAME", typeof(string));
                    dtFinalResult.Columns.Add("CAPACITY", typeof(string));
                    dtFinalResult.Columns.Add("MAKE", typeof(string));
                    dtFinalResult.Columns.Add("SLNO", typeof(string));
                    dtFinalResult.Columns.Add("DTCCODE", typeof(string));
                    dtFinalResult.Columns.Add("PURPOSE", typeof(string));
                    dtFinalResult.Columns.Add("FEEDERCODE", typeof(string));
                    dtFinalResult.Columns.Add("DOC", typeof(string));
                    dtFinalResult.Columns.Add("MAJOR", typeof(string));
                    dtFinalResult.Columns.Add("AGP", typeof(string));
                    dtFinalResult.Columns.Add("CON_LOAD", typeof(string));
                    dtFinalResult.Columns.Add("DTR_CODE", typeof(string));
                    dtFinalResult.Columns.Add("FAILURE_ID", typeof(string));
                    dtFinalResult.Columns.Add("PGRS_DOCNO", typeof(string));
                    dtFinalResult.Columns.Add("PGRS_DOCDATE", typeof(string));
                    dtFinalResult.Columns.Add("DOF", typeof(string));
                    dtFinalResult.Columns.Add("CONS_NAME", typeof(string));
                    dtFinalResult.Columns.Add("PHONENO", typeof(string));
                    dtFinalResult.Columns.Add("GUARANTY_TYPE", typeof(string));


                    DataRow dtrow = dtFinalResult.NewRow();
                    dtrow["COMPLAINT_NAME"] = dtFailLocation.Rows[0]["COMPLAINT_NAME"].ToString();
                    dtrow["ZONE"] = dtFailLocation.Rows[0]["ZONE"].ToString();
                    dtrow["CIRCLE"] = dtFailLocation.Rows[0]["CIRCLE"].ToString();
                    dtrow["DIVISION"] = dtFailLocation.Rows[0]["DIVISION"].ToString();
                    dtrow["SUBDIV"] = dtFailLocation.Rows[0]["SUBDIV"].ToString();
                    dtrow["TALUK"] = dtFailLocation.Rows[0]["TALUK"].ToString();
                    dtrow["SECTION"] = dtFailLocation.Rows[0]["SECTION"].ToString();
                    dtrow["DTCNAME"] = dtFailLocation.Rows[0]["DTCNAME"].ToString();
                    dtrow["CAPACITY"] = dtFailLocation.Rows[0]["CAPACITY"].ToString();
                    dtrow["MAKE"] = dtFailLocation.Rows[0]["MAKE"].ToString();
                    dtrow["SLNO"] = dtFailLocation.Rows[0]["SLNO"].ToString();
                    dtrow["DTCCODE"] = dtFailLocation.Rows[0]["DTCCODE"].ToString();
                    dtrow["FEEDERCODE"] = dtFailLocation.Rows[0]["FEEDERCODE"].ToString();
                    dtrow["DOC"] = dtFailLocation.Rows[0]["DOC"].ToString();
                    dtrow["MAJOR"] = dtFailLocation.Rows[0]["MAJOR"].ToString();
                    dtrow["AGP"] = dtFailLocation.Rows[0]["AGP"].ToString();
                    dtrow["CON_LOAD"] = dtFailLocation.Rows[0]["CON_LOAD"].ToString();
                    dtrow["DTR_CODE"] = dtFailLocation.Rows[0]["DTR_CODE"].ToString();

                    dtrow["PGRS_DOCNO"] = dtfailure.Rows[0]["DF_PGRS_DOCKET"].ToString();
                    dtrow["PGRS_DOCDATE"] = dtfailure.Rows[0]["DF_PGRS_DOCKET_DATE"].ToString();
                    //dtrow["PGRS_DOCNO"] = "";
                    dtrow["DOF"] = dtfailure.Rows[0]["DF_DATE"].ToString();
                    dtrow["CONS_NAME"] = dtfailure.Rows[0]["DF_CUSTOMER_NAME"].ToString();
                    dtrow["PHONENO"] = dtfailure.Rows[0]["DF_CUSTOMER_NUMBER"].ToString();
                    dtrow["GUARANTY_TYPE"] = dtfailure.Rows[0]["GUARENTEE"].ToString();
                    dtrow["PURPOSE"] = dtfailure.Rows[0]["DF_PURPOSE"].ToString();
                    dtrow["PURPOSE"] = Convert.ToString(sPurpose);

                    dtFinalResult.Rows.Add(dtrow);

                    crpPgrsFailure.SetDataSource(dtFinalResult);
                    crpPrint.ReportSource = crpPgrsFailure;
                    crpPrint.DataBind();
                }



                #region WorkOrderReport

                if (Request.QueryString["id"].ToString().Equals("WorkOrderPreview"))
                {
                    clsApproval objApproval = new clsApproval();
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    ArrayList sNameList = new ArrayList();
                    //string WO_NO;
                    //string WO_NO_DECOM;
                    //string WO_NO_OF;
                    //string sDivName = string.Empty;

                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    if (sOffCode == null)
                    {
                        if (objSession.OfficeCode.Length > Constants.Division)
                        {
                            sOffCode = objSession.OfficeCode.Substring(1, Constants.Division);
                        }

                    }
                    string sTaskType = Request.QueryString["TaskType"].ToString();
                    string sSubDivName = Request.QueryString["sSubDivName"].ToString();
                    string sWoId = Request.QueryString["WoId"].ToString();
                    if (sWoId != "")
                    {
                        clsReports objReport = new clsReports();
                        dtWorkOrderDetailsComm = objReport.PrintWorkOrderDetailsForNewDTC(sWoId);
                    }
                    if (sWFDataId != "")
                        dtWorkOrderDetailsComm = objApproval.GetDatatableFromXML(sWFDataId);


                    if (Session["UserNameList"] != null)
                    {
                        sNameList = (ArrayList)Session["UserNameList"];
                        //Session.Remove("UserNameList");
                    }

                    DataTable dtWorkOrderDetailsDeComm = new DataTable();
                    if (sWFDataId != "" && (sTaskType == "1" || sTaskType == "2" || sTaskType == "4"))
                        dtWorkOrderDetailsDeComm = objApproval.GetDatatableFromXML(sWFDataId);
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("WO_AMT_DECOM", typeof(int));
                        DataRow dtrow = dt.NewRow();
                        dtrow["WO_AMT_DECOM"] = 0000;
                        dt.Rows.Add(dtrow);
                        dtWorkOrderDetailsDeComm = dt;
                    }

                    //dtWorkOrderDetailsDeComm.Columns.Remove("WO_NO_DECOM");
                    //dtWorkOrderDetailsDeComm.Columns.Remove("WO_NO_OF");

                    //dtWorkOrderDetailsDeComm.Columns.Add("WO_NO_DECOM", typeof(string));
                    //dtWorkOrderDetailsDeComm.Columns.Add("WO_NO_OF", typeof(string));

                    //dtWorkOrderDetailsDeComm.Rows[0]["WO_NO_DECOM"] = sDivName + " / " + sSubDivName + " / " + WO_NO_DECOM;
                    //dtWorkOrderDetailsDeComm.Rows[0]["WO_NO_OF"] = sDivName + " / " + sSubDivName + " / " + WO_NO_OF;

                    crpWorkOrder.OpenSubreport("CrpWorkOrderComm.rpt").SetDataSource(dtWorkOrderDetailsComm);
                    crpWorkOrder.OpenSubreport("CrpWorkOrderDecomm.rpt").SetDataSource(dtWorkOrderDetailsDeComm);
                    //crpWorkOrder.OpenSubreport("CrpWorkOrderOil.rpt").SetDataSource(dtWorkOrderDetailsDeComm);
                    //crpWorkOrder.OpenSubreport("CrpWorkOrderCredit.rpt").SetDataSource(dtWorkOrderDetailsDeComm);
                    crpPrint.ReportSource = crpWorkOrder;

                    if (sTaskType == "3")
                    {

                        crpWorkOrder.SetParameterValue("TaskType", "3");
                    }
                    else if (sTaskType == "1" || sTaskType == "2" || sTaskType == "4" || sWoId == "")
                        crpWorkOrder.SetParameterValue("TaskType", "1");
                    if (sLevelOfApproval == "1")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", "");
                        crpWorkOrder.SetParameterValue("aousername", "");
                        crpWorkOrder.SetParameterValue("dousername", "");
                        crpWorkOrder.SetParameterValue("draft", "[DRAFT]");
                    }


                    if (sLevelOfApproval == "2")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", sNameList[1]);
                        crpWorkOrder.SetParameterValue("aousername", "");
                        crpWorkOrder.SetParameterValue("dousername", "");
                        crpWorkOrder.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "3")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", sNameList[1]);
                        crpWorkOrder.SetParameterValue("aousername", sNameList[2]);
                        crpWorkOrder.SetParameterValue("dousername", "");
                        crpWorkOrder.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "4")
                    {
                        crpWorkOrder.SetParameterValue("aetusername", sNameList[0]);
                        crpWorkOrder.SetParameterValue("stousername", sNameList[1]);
                        crpWorkOrder.SetParameterValue("aousername", sNameList[2]);
                        crpWorkOrder.SetParameterValue("dousername", sNameList[3]);
                        crpWorkOrder.SetParameterValue("draft", "");
                    }
                    crpWorkOrder.SetParameterValue("Officename", sOffCode);
                    crpPrint.DataBind();

                }

                if (Request.QueryString["id"].ToString().Equals("WorkOrder"))
                {

                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    clsReports objReport = new clsReports();
                    string sFailureId = Request.QueryString["FailureId"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    dtWorkOrderDetailsComm = objReport.PrintWorkOrderReport(sFailureId);

                    crpWorkOrder.OpenSubreport("CrpWorkOrderComm.rpt").SetDataSource(dtWorkOrderDetailsComm);
                    crpWorkOrder.OpenSubreport("CrpWorkOrderDecomm.rpt").SetDataSource(dtWorkOrderDetailsComm);
                    crpPrint.ReportSource = crpWorkOrder;
                    if (dtWorkOrderDetailsComm.Rows[0]["WO_NO_OF"].ToString() != "0")
                    {
                        crpWorkOrder.SetParameterValue("OilFilter", "1");
                    }
                    else
                        crpWorkOrder.SetParameterValue("OilFilter", "0");

                    if (dtWorkOrderDetailsComm.Rows[0]["WO_NO_CREDIT"].ToString() != "0")
                    {
                        crpWorkOrder.SetParameterValue("CreditWO", "1");
                    }
                    else
                        crpWorkOrder.SetParameterValue("CreditWO", "0");

                    crpWorkOrder.SetParameterValue("aetusername", "");
                    crpWorkOrder.SetParameterValue("stousername", "");
                    crpWorkOrder.SetParameterValue("aousername", "");
                    crpWorkOrder.SetParameterValue("dousername", "");
                    crpWorkOrder.SetParameterValue("Officename", sOffCode);
                    crpWorkOrder.SetParameterValue("draft", "");
                    crpPrint.DataBind();
                }

                #endregion


                if (Request.QueryString["id"] == "GatePass")
                {
                    DataTable dt = new DataTable();
                    DataTable dtSign = new DataTable();
                    DataSet ds = new DataSet();
                    clsReports objInvoice = new clsReports();

                    //ReportDocument cryRpt = (ReportDocument)HttpContext.Current.Session["ReportData"];

                    dt = objInvoice.LoadGatePass(Request.QueryString["InvoiceId"].ToString());


                    if (dt.Rows.Count > 0)
                    {
                        crpGatepassReport.SetDataSource(dt);
                        crpPrint.ReportSource = crpGatepassReport;
                        crpPrint.RefreshReport();
                    }
                    else
                    {
                        ShowMsgBox("NO Records Found!");
                    }

                }

                if (Request.QueryString["id"] == "CRReport")
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    string sLevel = Request.QueryString["iLevel"].ToString();

                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.CompletionReport(sDecommId);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["LEVEL"] = sLevel;
                    }

                    crpReportDetails.SetDataSource(dt);
                    crpPrint.ReportSource = crpReportDetails;
                    crpPrint.DataBind();
                }

                // RI Report
                if (Request.QueryString["id"] == "RIReport")
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.RIReport(sDecommId);

                    crpRIReport.SetDataSource(dt);
                    crpPrint.ReportSource = crpRIReport;
                    crpPrint.DataBind();
                }
                // Decom SO LEVEL
                if (Request.QueryString["id"] == "RIReportso")
                {

                    string swfoid = Request.QueryString["wfoID"].ToString();
                    string sDtrcode = Request.QueryString["sDtrcode"].ToString();
                    string sFailurId = Request.QueryString["sFailurId"].ToString();
                    DataTable dt = new DataTable();
                    clsApproval objApp = new clsApproval();
                    clsReports objreport = new clsReports();

                    DataTable dtfailureDecomm = new DataTable();
                    dtfailureDecomm = objApp.GetDatatableFromXML(swfoid);

                    DataTable dtaddrow = new DataTable();
                    DataRow dtrow = dtaddrow.NewRow();
                    DataTable dtRixml = new DataTable();
                    DataTable dtFinalResult = new DataTable();

                    dtaddrow.Columns.Add("TR_RI_NO", typeof(string));
                    dtaddrow.Columns.Add("TR_RI_DATE", typeof(string));
                    dtaddrow.Columns.Add("TC_CAPACITY", typeof(string));
                    dtaddrow.Columns.Add("SM_NAME", typeof(string));
                    dtaddrow.Columns.Add("EST_UNIT_PRICE", typeof(string));
                    dtaddrow.Columns.Add("WO_NO_DECOM", typeof(string));
                    dtaddrow.Columns.Add("EST_NO", typeof(string));
                    dtaddrow.Columns.Add("DF_DTC_CODE", typeof(string));
                    dtaddrow.Columns.Add("DTC_NAME", typeof(string));
                    dtaddrow.Columns.Add("DIVISION", typeof(string));
                    dtaddrow.Columns.Add("SUBDIVISION", typeof(string));

                    dtaddrow.Columns.Add("SECTION", typeof(string));
                    dtaddrow.Columns.Add("MAKE", typeof(string));
                    dtaddrow.Columns.Add("TC_SLNO", typeof(string));
                    dtaddrow.Columns.Add("TC_MANF_DATE", typeof(string));
                    dtaddrow.Columns.Add("TC_CODE", typeof(string));
                    dtaddrow.Columns.Add("TR_OIL_QUNTY", typeof(string));
                    dtaddrow.Columns.Add("DTRCOMMISIONDATE", typeof(string));
                    dtaddrow.Columns.Add("TR_OIL_QTY_BYSK", typeof(string));
                    dtaddrow.Columns.Add("TR_DECOMM_DATE", typeof(string));
                    dtaddrow.Columns.Add("ACK_NO", typeof(string));
                    dtaddrow.Columns.Add("ACK_DATE", typeof(string));
                    dtaddrow.Columns.Add("SDO_USERNAME", typeof(string));

                    dtaddrow.Columns.Add("SO_USERNAME", typeof(string));
                    dtaddrow.Columns.Add("STO_USERNAME", typeof(string));
                    dtaddrow.Columns.Add("SK_USERNAME", typeof(string));
                    dtaddrow.Columns.Add("TR_MANUAL_ACKRV_NO", typeof(string));
                    dtaddrow.Columns.Add("TR_NO_OF_BARRELS", typeof(string));


                    dtrow["TR_OIL_QUNTY"] = dtfailureDecomm.Rows[0]["TR_OIL_QUNTY"].ToString();
                    dtrow["TR_DECOMM_DATE"] = dtfailureDecomm.Rows[0]["TR_DECOMM_DATE"].ToString();
                    dtrow["TR_NO_OF_BARRELS"] = dtfailureDecomm.Rows[0]["TR_NO_OF_BARRELS"].ToString();

                    dtaddrow.Rows.Add(dtrow);
                    dtFinalResult = objreport.RIReportso(sDtrcode, sFailurId);

                    DataColumn dcOIL = dtaddrow.Columns["TR_OIL_QUNTY"];
                    DataColumn dcOILBrl = dtaddrow.Columns["TR_NO_OF_BARRELS"];
                    DataColumn dcDEDATE = dtaddrow.Columns["TR_DECOMM_DATE"];



                    dtFinalResult.Columns.Add(dcOIL.ColumnName, dcOIL.DataType);
                    dtFinalResult.Columns.Add(dcOILBrl.ColumnName, dcOIL.DataType);
                    dtFinalResult.Columns.Add(dcDEDATE.ColumnName, dcDEDATE.DataType);


                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {

                        dtFinalResult.Rows[i]["TR_OIL_QUNTY"] = dtaddrow.Rows[0]["TR_OIL_QUNTY"];
                        dtFinalResult.Rows[i]["TR_NO_OF_BARRELS"] = dtaddrow.Rows[0]["TR_NO_OF_BARRELS"];
                        dtFinalResult.Rows[i]["TR_DECOMM_DATE"] = dtaddrow.Rows[0]["TR_DECOMM_DATE"];

                    }

                    crpRIAck.SetDataSource(dtFinalResult);
                    crpPrint.ReportSource = crpRIAck;
                    crpPrint.DataBind();
                }

                //RI Acknoldgment Report
                if (Request.QueryString["id"] == "RIAckReport")
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.RIReport(sDecommId);

                    crpRIAck.SetDataSource(dt);
                    crpPrint.ReportSource = crpRIAck;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "IndentReport")
                {
                    //clsIndent objIndent = new clsIndent();
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sofficeCode = Request.QueryString["OffCode"].ToString();
                    DataTable dt = new DataTable();

                    clsApproval objApproval = new clsApproval();
                    clsReports objReport = new clsReports();
                    if (sWFDataId != "")
                    {
                        dt = objApproval.GetDatatableFromXML(sWFDataId);
                        string sWOSlno = Convert.ToString(dt.Rows[0]["TI_WO_SLNO"]);
                        string sstoreId = Convert.ToString(dt.Rows[0]["TI_STORE_ID"]);
                        dt = objReport.WO_IndentDetails(sWOSlno, sstoreId);

                    }
                    else
                    {
                        if (sIndentId != "")
                        {
                            dt = objReport.IndentDetails(sIndentId);
                        }
                    }

                    crpIndent.SetDataSource(dt);
                    crpPrint.ReportSource = crpIndent;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "InvoiceReport")
                {
                    string sInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sCapacity = Request.QueryString["Capacity"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.InvoiceReport(sInvoiceId, sOfficeCode, sCapacity);

                    crpInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpInvoice;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "FormNewReport")
                {
                    string WOANo = Request.QueryString["WOANo"].ToString();
                    string WOADate = Request.QueryString["WOADate"].ToString();
                    string actualoil = Request.QueryString["actualoil"].ToString();
                    string oil = Request.QueryString["oil"].ToString();
                    string barrel = Request.QueryString["barrel"].ToString();
                    string oiltype = Request.QueryString["oiltype"].ToString();
                    string issuedate = Request.QueryString["issuedate"].ToString();
                    string mtrialf0lio = Request.QueryString["mtrialf0lio"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.NewFormReport(WOANo, WOADate, actualoil, oil, barrel, oiltype, issuedate, mtrialf0lio);

                    TransilOilNewReport.SetDataSource(dt);
                    crpPrint.ReportSource = TransilOilNewReport;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "WorkAwardReport")
                {
                    string sWAId = Request.QueryString["WorkAwardId"].ToString();
                    DataTable dt = new DataTable();
                    clsWorkAward objAwrd = new clsWorkAward();
                    objAwrd.sWOAId = sWAId;
                    objAwrd.GetWorkAwardReport(objAwrd);
                    crpworkaward.SetDataSource(objAwrd.dtWODetails);
                    crpPrint.ReportSource = crpworkaward;
                    crpPrint.DataBind();
                }


                //if (Request.QueryString["id"] == "WorkAwardReportmajor")
                //{
                //    string sWAId = Request.QueryString["WorkAwardId"].ToString();
                //    DataTable dt = new DataTable();
                //    ClsMajorWorkAward objAwrd = new ClsMajorWorkAward();
                //    objAwrd.sWOAId = sWAId;
                //    objAwrd.GetWorkAwardReport(objAwrd);
                //    crpmajorwrkawrd.SetDataSource(objAwrd.dtWODetails);
                //    crpPrint.ReportSource = crpmajorwrkawrd;
                //    crpPrint.DataBind();
                //}
                if (Request.QueryString["id"] == "WorkAwardReportmajor")
                {
                    string sWAId = Request.QueryString["WorkAwardId"].ToString();
                    DataTable dt = new DataTable();
                    ClsMajorWorkAward objAwrd = new ClsMajorWorkAward();
                    ArrayList sNameList = new ArrayList();
                    string sLevelOfApproval = string.Empty;
                    string sOffCode = string.Empty;

                    if (Request.QueryString["OffCode"].ToString() != "" && Request.QueryString["OffCode"].ToString() != null)
                    {
                        sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                        sOffCode = Request.QueryString["OffCode"].ToString();

                        if (Session["UserNameList"] != null)
                        {
                            sNameList = (ArrayList)Session["UserNameList"];
                        }
                    }

                    //string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    //string sOffCode = Request.QueryString["OffCode"].ToString();

                    //if (Session["UserNameList"] != null)
                    //{
                    //    sNameList = (ArrayList)Session["UserNameList"];
                    //}

                    objAwrd.sWOAId = sWAId;
                    objAwrd.GetWorkAwardReport(objAwrd);
                    crpmajorwrkawrd.SetDataSource(objAwrd.dtWODetails);
                    if (Request.QueryString["OffCode"].ToString() == "" || Request.QueryString["OffCode"].ToString() == null)
                    {
                        crpmajorwrkawrd.SetParameterValue("aeeusername", "AEE");
                        crpmajorwrkawrd.SetParameterValue("eeusername", "EE");
                        crpmajorwrkawrd.SetParameterValue("seeusername", "SEE");

                    }

                    if (Request.QueryString["OffCode"].ToString() != "" && Request.QueryString["OffCode"].ToString() != null)
                    {
                        if (sLevelOfApproval == "1")
                        {

                            crpmajorwrkawrd.SetParameterValue("aeeusername", sNameList[0]);
                            crpmajorwrkawrd.SetParameterValue("eeusername", sNameList[1]);
                            crpmajorwrkawrd.SetParameterValue("seeusername", sNameList[2]);

                        }


                        if (sLevelOfApproval == "2")
                        {
                            crpmajorwrkawrd.SetParameterValue("aeeusername", sNameList[0]);
                            crpmajorwrkawrd.SetParameterValue("eeusername", sNameList[1]);
                            crpmajorwrkawrd.SetParameterValue("seeusername", sNameList[2]);

                        }
                        if (sLevelOfApproval == "3")
                        {
                            crpmajorwrkawrd.SetParameterValue("aeeusername", sNameList[0]);
                            crpmajorwrkawrd.SetParameterValue("eeusername", sNameList[1]);
                            crpmajorwrkawrd.SetParameterValue("seeusername", sNameList[2]);

                        }
                    }
                    //    if (sLevelOfApproval == "1")
                    //{

                    //    crpmajorwrkawrd.SetParameterValue("aeeusername", sNameList[0]);
                    //    crpmajorwrkawrd.SetParameterValue("eeusername", sNameList[1]);
                    //    crpmajorwrkawrd.SetParameterValue("seeusername", sNameList[2]);

                    //}


                    //if (sLevelOfApproval == "2")
                    //{
                    //    crpmajorwrkawrd.SetParameterValue("aeeusername", sNameList[0]);
                    //    crpmajorwrkawrd.SetParameterValue("eeusername", sNameList[1]);
                    //    crpmajorwrkawrd.SetParameterValue("seeusername", sNameList[2]);

                    //}
                    //if (sLevelOfApproval == "3")
                    //{
                    //    crpmajorwrkawrd.SetParameterValue("aeeusername", sNameList[0]);
                    //    crpmajorwrkawrd.SetParameterValue("eeusername", sNameList[1]);
                    //    crpmajorwrkawrd.SetParameterValue("seeusername", sNameList[2]);

                    //}
                    crpPrint.ReportSource = crpmajorwrkawrd;
                    crpPrint.DataBind();
                }
                #endregion

                #region Capacity Enhanacement Transaction Report




                #region EnhanceEstm Report For So
                if (Request.QueryString["id"].ToString().Equals("EnhanceEstimationSO"))
                {
                    #region Variable Declaration
                    clsApproval objApp = new clsApproval();
                    DataTable dt = new DataTable();
                    string sTCcode = Request.QueryString["TCcode"].ToString();
                    string sWoId = Request.QueryString["WOId"].ToString();
                    string sRes = string.Empty;
                    string sEnhncCap = string.Empty;
                    clsReports objRep = new clsReports();
                    clsEnhancement objEnhnc = new clsEnhancement();
                    #endregion
                    string sWfoID = objEnhnc.getWfoIDforEstimationSO(sWoId);
                    //Get Data from Xml
                    dt = objApp.GetDatatableFromXML(sWfoID);
                    //Get Reason and Enhance Capacity to Generate Report
                    if (dt.Columns.Contains("DF_REASON"))
                        sRes = dt.Rows[0]["DF_REASON"].ToString();
                    if (dt.Columns.Contains("DF_ENHANCE_CAPACITY"))
                        sEnhncCap = dt.Rows[0]["DF_ENHANCE_CAPACITY"].ToString();

                    DataTable dtCommEstimation;
                    dtCommEstimation = objRep.PrintEstimatedReportSO(sTCcode, sWoId, sEnhncCap);

                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimationReportSO(sTCcode, sWoId, sRes, sEnhncCap);


                    if (dtDecomEstimation.Rows.Count > 0)
                    {
                        crpEnhanceEst.OpenSubreport("crpEnhanceCommEst.rpt").SetDataSource(dtCommEstimation);
                        crpEnhanceEst.OpenSubreport("crpEnhanceDeCommEst.rpt").SetDataSource(dtDecomEstimation);
                        crpPrint.ReportSource = crpEnhanceEst;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("NO Records Found!");
                    }
                }
                #endregion

                if (Request.QueryString["id"] == "EnhanceCRReport")
                {

                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    string sLevel = Request.QueryString["iLevel"].ToString();

                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.CompletionReport(sDecommId);


                    crpReportDetails.SetDataSource(dt);
                    crpPrint.ReportSource = crpReportDetails;
                    crpPrint.DataBind();
                }


                if (Request.QueryString["id"] == "EnhanceIndentReport")
                {
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.IndentDetails(sIndentId);

                    crpEnhanceIndent.SetDataSource(dt);
                    crpPrint.ReportSource = crpEnhanceIndent;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "EnhanceInvoiceReport")
                {
                    string sInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sCapacity = Request.QueryString["Capacity"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.InvoiceReport(sInvoiceId, sOfficeCode, sCapacity);

                    crpEnhanceInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpEnhanceInvoice;
                    crpPrint.DataBind();
                }

                #endregion


                if (Request.QueryString["id"] == "repairerabstractyearwise")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();


                    dt = objReport.getrepaireryearwiseabstract();


                    objreyear.SetDataSource(dt);
                    crpPrint.ReportSource = objreyear;

                    crpPrint.DataBind();


                }
                if (Request.QueryString["id"] == "RolewiseCount")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string officecode = Request.QueryString["Officecode"].ToString();

                    dt = objReport.printRolewisependingCount(officecode);
                    if (dt.Rows.Count > 0)
                    {
                        crpRolewiseCount.SetDataSource(dt);
                        crpPrint.ReportSource = crpRolewiseCount;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("NO Records Found!");
                    }

                }


                if (Request.QueryString["id"] == "RepairGatepass")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string sTransId = Request.QueryString["TransId"].ToString();
                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    if (sTransId != "" && sTransId != null)
                    {

                        dt = objReport.PrintRepairGatePassReport(strInvoiceNo);
                        crpRepairGatepass.SetDataSource(dt);
                        crpPrint.ReportSource = crpRepairGatepass;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        dt = objReport.PrintRepairInvoiceReport(strInvoiceNo);
                        crpRepairGatepass.SetDataSource(dt);
                        crpPrint.ReportSource = crpRepairGatepass;
                        crpPrint.DataBind();
                    }

                    //crpPrint.ID = "RepairGatepass-" + stroffcode + "-" + strTodayDate;

                }

                if (Request.QueryString["id"] == "ScrapGatepass")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintScrapGatePass(strInvoiceNo);
                    crpRepairGatepass.SetDataSource(dt);
                    crpPrint.ReportSource = crpRepairGatepass;
                    crpPrint.DataBind();
                    //crpPrint.ID = "ScrapGatepass-" + stroffcode + "-" + strTodayDate;

                }

                if (Request.QueryString["id"] == "StoreGatepass")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintStoreInvoiceGatePass(strInvoiceNo);
                    if (dt.Rows.Count > 0)
                    {
                        crpGatePass.SetDataSource(dt);
                        crpPrint.ReportSource = crpGatePass;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                    //crpPrint.ID = "StoreGatepass-" + stroffcode + "-" + strTodayDate;

                }

                if (Request.QueryString["id"] == "BankGatepass")
                {
                    DataTable dt = new DataTable();
                    clsBankInvoice objReport = new clsBankInvoice();
                    string strInvoiceNo = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.GetGatepassDetails(strInvoiceNo);
                    crpRepairGatepass.SetDataSource(dt);
                    crpPrint.ReportSource = crpRepairGatepass;
                    crpPrint.DataBind();
                    //crpPrint.ID = "StoreGatepass-" + stroffcode + "-" + strTodayDate;

                }


                if (Request.QueryString["id"] == "ScrapInvoice")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    string strCsrapInvoiceId = Request.QueryString["scrapInvoice"].ToString();
                    dt = objReport.PrintScrapInvoicereport(strCsrapInvoiceId);

                    crpScrapInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpScrapInvoice;
                    crpPrint.DataBind();
                    //crpPrint.ID = "ScrapInvoice-" + stroffcode + "-" + strTodayDate;

                }

                if (Request.QueryString["id"] == "InterStoreInvoice")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    string sInvoiceNo = Request.QueryString["InvoiceNo"].ToString();
                    dt = objReport.PrintInterStoreInvoicereport(sInvoiceNo);

                    crpStoreInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpStoreInvoice;
                    crpPrint.DataBind();
                    //crpPrint.ID = "InterStoreInvoice-" + stroffcode + "-" + strTodayDate;

                }
                if (Request.QueryString["id"].ToString().Equals("WorkOrderNewDtcCommission"))
                {
                    clsApproval objApproval = new clsApproval();
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    ArrayList sNameList = new ArrayList();
                    //string WO_NO;
                    //string WO_NO_DECOM;
                    //string WO_NO_OF;
                    //string sDivName = string.Empty;

                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    if (sOffCode == null)
                    {
                        if (objSession.OfficeCode.Length > Constants.Division)
                        {
                            sOffCode = objSession.OfficeCode.Substring(1, Constants.Division);
                        }

                    }
                    string sTaskType = Request.QueryString["TaskType"].ToString();
                    string sSubDivName = Request.QueryString["sSubDivName"].ToString();
                    string sWoId = Request.QueryString["WoId"].ToString();
                    if (sWoId != "")
                    {
                        clsReports objReport = new clsReports();
                        dtWorkOrderDetailsComm = objReport.PrintWorkOrderDetailsForNewDTC(sWoId);
                    }
                    if (sWFDataId != "")
                        dtWorkOrderDetailsComm = objApproval.GetDatatableFromXML(sWFDataId);

                    //WO_NO = dtWorkOrderDetailsComm.Rows[0]["WO_NO"].ToString();
                    //WO_NO_DECOM = dtWorkOrderDetailsComm.Rows[0]["WO_NO_DECOM"].ToString();
                    //WO_NO_OF = dtWorkOrderDetailsComm.Rows[0]["WO_NO_OF"].ToString();

                    //dtWorkOrderDetailsComm.Columns.Remove("WO_NO");

                    //dtWorkOrderDetailsComm.Columns.Add("WO_NO", typeof(string));

                    //if (sOffCode != "")
                    //{
                    //     sDivName = sOffCode.Split(':')[1];
                    //}

                    //dtWorkOrderDetailsComm.Rows[0]["WO_NO"] = sDivName + " / " + sSubDivName + " / " + WO_NO;

                    if (Session["UserNameList"] != null)
                    {
                        sNameList = (ArrayList)Session["UserNameList"];
                        //Session.Remove("UserNameList");
                    }


                    //*-------------*//

                    //DataTable dtWorkOrderDetailsDeComm = new DataTable();
                    //if (sWFDataId != "" && (sTaskType == "1" || sTaskType == "2" || sTaskType == "4"))
                    //    dtWorkOrderDetailsDeComm = objApproval.GetDatatableFromXML(sWFDataId);
                    //else
                    //{
                    DataTable dt = new DataTable();
                    dtWorkOrderDetailsComm.Columns.Add("SCHEME_TYPE", typeof(string));
                    dtWorkOrderDetailsComm.Columns.Add("SECTION", typeof(string));
                    dtWorkOrderDetailsComm.Columns.Add("PROJECT_TYPE", typeof(string));


                    //DataRow dtrow = dtWorkOrderDetailsComm.NewRow();
                    //    dtrow["SCHEME_TYPE"] = Request.QueryString["scheme"].ToString();
                    //    dtrow["SECTION"] = Request.QueryString["section"].ToString();                  
                    //    dtrow["PROJECT_TYPE"] = Request.QueryString["sprojName"].ToString();


                    //dtWorkOrderDetailsComm.Rows.Add(dtrow);

                    dtWorkOrderDetailsComm.Rows[0]["SCHEME_TYPE"] = Request.QueryString["scheme"].ToString();
                    dtWorkOrderDetailsComm.Rows[0]["SECTION"] = Request.QueryString["section"].ToString();
                    dtWorkOrderDetailsComm.Rows[0]["PROJECT_TYPE"] = Request.QueryString["sprojName"].ToString();


                    if (dtWorkOrderDetailsComm.Columns.Contains("WO_RATING"))
                    {
                        string rating = Convert.ToString(dtWorkOrderDetailsComm.Rows[0]["WO_RATING"]);
                        clsWorkOrder obj = new clsWorkOrder();
                        dtWorkOrderDetailsComm.Rows[0]["WO_RATING"] = obj.GetRating(rating);
                    }


                    //  dtWorkOrderDetailsComm.Merge(dt);
                    //  }

                    //*-------------*//


                    //dtWorkOrderDetailsDeComm.Columns.Remove("WO_NO_DECOM");
                    //dtWorkOrderDetailsDeComm.Columns.Remove("WO_NO_OF");

                    //dtWorkOrderDetailsDeComm.Columns.Add("WO_NO_DECOM", typeof(string));
                    //dtWorkOrderDetailsDeComm.Columns.Add("WO_NO_OF", typeof(string));

                    //dtWorkOrderDetailsDeComm.Rows[0]["WO_NO_DECOM"] = sDivName + " / " + sSubDivName + " / " + WO_NO_DECOM;
                    //dtWorkOrderDetailsDeComm.Rows[0]["WO_NO_OF"] = sDivName + " / " + sSubDivName + " / " + WO_NO_OF;
                    crpNewdtcWo.SetDataSource(dtWorkOrderDetailsComm);

                    crpPrint.ReportSource = crpNewdtcWo;

                    crpNewdtcWo.SetParameterValue("projecttype", Request.QueryString["TaskType"].ToString());


                    if (sLevelOfApproval == "1")
                    {
                        crpNewdtcWo.SetParameterValue("aetusername", sNameList[0]);
                        crpNewdtcWo.SetParameterValue("stousername", "");
                        crpNewdtcWo.SetParameterValue("aousername", "");
                        crpNewdtcWo.SetParameterValue("dousername", "");
                        crpNewdtcWo.SetParameterValue("draft", "[DRAFT]");
                    }


                    if (sLevelOfApproval == "2")
                    {
                        crpNewdtcWo.SetParameterValue("aetusername", sNameList[0]);
                        crpNewdtcWo.SetParameterValue("stousername", sNameList[1]);
                        crpNewdtcWo.SetParameterValue("aousername", "");
                        crpNewdtcWo.SetParameterValue("dousername", "");
                        crpNewdtcWo.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "3")
                    {
                        crpNewdtcWo.SetParameterValue("aetusername", sNameList[0]);
                        crpNewdtcWo.SetParameterValue("stousername", sNameList[1]);
                        crpNewdtcWo.SetParameterValue("aousername", sNameList[2]);
                        crpNewdtcWo.SetParameterValue("dousername", "");
                        crpNewdtcWo.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "4")
                    {
                        crpNewdtcWo.SetParameterValue("aetusername", sNameList[0]);
                        crpNewdtcWo.SetParameterValue("stousername", sNameList[1]);
                        crpNewdtcWo.SetParameterValue("aousername", sNameList[2]);
                        crpNewdtcWo.SetParameterValue("dousername", sNameList[3]);
                        crpNewdtcWo.SetParameterValue("draft", "");
                    }
                    crpNewdtcWo.SetParameterValue("Officename", sOffCode);
                    crpPrint.DataBind();

                }

                if (Request.QueryString["id"] == "NewDtcIndentReport")
                {
                    //clsIndent objIndent = new clsIndent();
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sofficeCode = Request.QueryString["OffCode"].ToString();
                    DataTable dt = new DataTable();

                    clsApproval objApproval = new clsApproval();
                    clsReports objReport = new clsReports();
                    if (sWFDataId != "")
                    {
                        dt = objApproval.GetDatatableFromXML(sWFDataId);
                        string sWOSlno = Convert.ToString(dt.Rows[0]["TI_WO_SLNO"]);
                        string sstoreId = Convert.ToString(dt.Rows[0]["TI_STORE_ID"]);
                        string sindentNo = Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]);
                        string sIndentDate = Convert.ToString(dt.Rows[0]["TI_INDENT_DATE"]);
                        dt = objReport.NewDTC_WO_IndentDetails(sWOSlno, sstoreId);
                        dt.Rows[0]["TI_INDENT_DATE"] = sIndentDate.ToString();
                        dt.Rows[0]["TI_INDENT_NO"] = sindentNo.ToString();

                    }
                    else
                    {
                        if (sIndentId != "")
                        {
                            dt = objReport.IndentDetails(sIndentId);
                        }
                    }

                    crpnewdtcindent.SetDataSource(dt);
                    crpPrint.ReportSource = crpnewdtcindent;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "NewDtcCR_CommReport")
                {
                    //clsIndent objIndent = new clsIndent();              
                    string sWFDataId = Request.QueryString["DtcID"].ToString();
                    string sofficeCode = Request.QueryString["OffCode"].ToString();
                    DataTable dt = new DataTable();

                    clsApproval objApproval = new clsApproval();
                    clsReports objReport = new clsReports();
                    string sWO_ttk_status = string.Empty;
                    if (sWFDataId != "")
                    {
                        //dt = objApproval.GetDatatableFromXML(sWFDataId);
                        //string sWOSlno = Convert.ToString(dt.Rows[0]["TI_WO_SLNO"]);
                        //string sstoreId = Convert.ToString(dt.Rows[0]["TI_STORE_ID"]);
                        //string sindentNo = Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]);
                        //string sIndentDate = Convert.ToString(dt.Rows[0]["TI_INDENT_DATE"]);
                        dt = objReport.NewDTC_CR_CommiDetails(sWFDataId);
                        sWO_ttk_status = Convert.ToString(dt.Rows[0]["WO_TTK_STATUS"]);

                    }
                    crpnewDTCCR.SetDataSource(dt);

                    crpPrint.ReportSource = crpnewDTCCR;
                    if (sWO_ttk_status != "" && sWO_ttk_status != null)
                    {
                        crpnewDTCCR.SetParameterValue("projecttype", sWO_ttk_status);
                    }
                    else
                    {
                        crpnewDTCCR.SetParameterValue("projecttype", "1");
                    }
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"] == "NewDtcInvoiceReport")
                {
                    string sInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    string sOfficeCode = Request.QueryString["OfficeCode"].ToString();
                    string sCapacity = Request.QueryString["Capacity"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    dt = objReport.NewDtcInvoiceReport(sInvoiceId, sOfficeCode, sCapacity);

                    crpnewdtcInvoice.SetDataSource(dt);
                    crpPrint.ReportSource = crpnewdtcInvoice;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "IndentReport")
                {
                    //clsIndent objIndent = new clsIndent();
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sofficeCode = Request.QueryString["OffCode"].ToString();
                    DataTable dt = new DataTable();

                    clsApproval objApproval = new clsApproval();
                    clsReports objReport = new clsReports();
                    if (sWFDataId != "")
                    {
                        dt = objApproval.GetDatatableFromXML(sWFDataId);
                        string sWOSlno = Convert.ToString(dt.Rows[0]["TI_WO_SLNO"]);
                        string sstoreId = Convert.ToString(dt.Rows[0]["TI_STORE_ID"]);
                        dt = objReport.WO_IndentDetails(sWOSlno, sstoreId);

                    }
                    else
                    {
                        if (sIndentId != "")
                        {
                            dt = objReport.IndentDetails(sIndentId);
                        }
                    }

                    crpIndent.SetDataSource(dt);
                    crpPrint.ReportSource = crpIndent;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"].ToString().Equals("WorkOrderPreviewRepairer"))
                {
                    clsApproval objApproval = new clsApproval();
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    ArrayList sNameList = new ArrayList();

                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    string sIncurerdcost = Request.QueryString["sIncuredcost"].ToString();
                    string sScrapWono = "";
                    string sScrapAmt = "";
                    string sScrapCode = "";
                    if (sOffCode == null)
                    {
                        if (objSession.OfficeCode.Length > Constants.Division)
                        {
                            sOffCode = objSession.OfficeCode.Substring(1, Constants.Division);
                        }

                    }
                    DataTable dtWorkOrderDetailsDeComm = new DataTable();
                    string sTaskType = Request.QueryString["TaskType"].ToString();
                    string sSubDivName = Request.QueryString["sSubDivName"].ToString();
                    string sWoId = Request.QueryString["WoId"].ToString();

                    if (sWFDataId != "")
                    {
                        dtWorkOrderDetailsDeComm = objApproval.GetDatatableFromXML(sWFDataId);
                        DataColumnCollection columns = dtWorkOrderDetailsDeComm.Columns;
                        if (columns.Contains("RWO_SS_WO_NO"))
                        {
                            sScrapWono = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_WO_NO"]);
                        }
                        if (columns.Contains("RWO_SS_AMT"))
                        {
                            sScrapAmt = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_AMT"]);
                        }
                        if (columns.Contains("RWO_SS_ACCODE"))
                        {
                            sScrapCode = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_ACCODE"]);
                        }
                        //sScrapWono =Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_WO_NO"]);
                        // sScrapAmt = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_AMT"]);
                        //sScrapCode = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_ACCODE"]);
                    }


                    if (Session["UserNameList"] != null)
                    {
                        sNameList = (ArrayList)Session["UserNameList"];

                    }


                    objrepwo.SetDataSource(dtWorkOrderDetailsDeComm);
                    crpPrint.ReportSource = objrepwo;




                    if (sLevelOfApproval == "1")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", "");
                        objrepwo.SetParameterValue("aousername", "");
                        objrepwo.SetParameterValue("dousername", "");
                        objrepwo.SetParameterValue("draft", "[DRAFT]");
                    }


                    if (sLevelOfApproval == "2")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", sNameList[1]);
                        objrepwo.SetParameterValue("aousername", "");
                        objrepwo.SetParameterValue("dousername", "");
                        objrepwo.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "3")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", sNameList[1]);
                        objrepwo.SetParameterValue("aousername", sNameList[2]);
                        objrepwo.SetParameterValue("dousername", "");
                        objrepwo.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "4")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", sNameList[1]);
                        objrepwo.SetParameterValue("aousername", sNameList[2]);
                        objrepwo.SetParameterValue("dousername", sNameList[3]);
                        objrepwo.SetParameterValue("draft", "");
                    }
                    objrepwo.SetParameterValue("Officename", sOffCode);
                    if (sIncurerdcost != "")
                    {
                        objrepwo.SetParameterValue("Incuredcost", sIncurerdcost);
                    }
                    else
                    {
                        objrepwo.SetParameterValue("Incuredcost", "0");
                    }
                    if (sScrapWono != "" && sScrapAmt != "" && sScrapCode != "")
                    {
                        objrepwo.SetParameterValue("scrapwono", sScrapWono);
                        objrepwo.SetParameterValue("scrapamount", sScrapAmt);
                        objrepwo.SetParameterValue("scrapcode", sScrapCode);
                    }
                    else
                    {
                        objrepwo.SetParameterValue("scrapwono", "0");
                        objrepwo.SetParameterValue("scrapamount", "0");
                        objrepwo.SetParameterValue("scrapcode", "0");
                    }
                    crpPrint.DataBind();

                }

                if (Request.QueryString["id"] == "RecieveDTR")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    string strCsrapInvoiceId = Request.QueryString["InvoiceId"].ToString();
                    dt = objReport.PrintReceiveDTrReport(strCsrapInvoiceId);

                    crpRecieveDTr.SetDataSource(dt);
                    crpPrint.ReportSource = crpRecieveDTr;
                    crpPrint.DataBind();
                    //crpPrint.ID = "Recieve DTR-" + stroffcode + "-" + strTodayDate;

                }

                if (Request.QueryString["id"] == "InterStoreIndent")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    string sStoreIndentId = Request.QueryString["IndentId"].ToString();
                    dt = objReport.PrintStoreIndentReport(sStoreIndentId);
                    #region new code for show if data is not there for report
                    if (dt.Rows.Count == 0)
                    {
                        Response.Write("<script language=javascript>alert('Data Not Available')</script>");
                    }
                    else
                    {
                        crpStoreIndent.SetDataSource(dt);
                        crpPrint.ReportSource = crpStoreIndent;
                        crpPrint.DataBind();
                        //crpPrint.ID = "Inter StoreIndent-" + stroffcode + "-" + strTodayDate;

                    }
                    #endregion

                }

                #region Abstract Report
                if (Request.QueryString["id"] == "AbstractReport")
                {
                    clsReports objReport = new clsReports();

                    DataTable dt = new DataTable();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {

                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }


                    dt = objReport.PrintAbstractReport(objReport);


                    if (dt.Rows.Count > 0)
                    {

                        crpAbstractReport objRep = new crpAbstractReport();

                        objRep.SetDataSource(dt);

                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                        //crpPrint.ID = "AbstractReport-" + stroffcode + "-" + strTodayDate;

                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }


                if (Request.QueryString["id"] == "CRAbstract")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }


                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {

                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }

                    dt = objReport.CRAbstract(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        crpCRAbstract.SetDataSource(dt);
                        crpPrint.ReportSource = crpCRAbstract;
                        crpPrint.DataBind();
                        //crpPrint.ID = "CRAbstract-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }

                if (Request.QueryString["id"] == "AbstractRptTcFailed")
                {
                    clsReports objReport = new clsReports();

                    DataTable dt = new DataTable();
                    DataTable dtRepairCount = new DataTable();
                    DataTable dtcompletedRepairCount = new DataTable();

                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {

                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }

                    dt = objReport.PrintAbstractReportTcFailedAtFSR(objReport);
                    dtRepairCount = objReport.PrintRepairerTcCount(objReport);
                    dtcompletedRepairCount = objReport.PrintCompletedRepairerTcCount(objReport);

                    crpAbstractReportTcFailedReplaceAtFSR objRep = new crpAbstractReportTcFailedReplaceAtFSR();

                    if (dt.Rows.Count > 0)
                    {
                        objRep.SetDataSource(dt);
                        objRep.OpenSubreport("CrpRepairerTcCountSub.rpt").SetDataSource(dtRepairCount);
                        objRep.OpenSubreport("CrpRepairCompleted.rpt").SetDataSource(dtcompletedRepairCount);

                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                        //crpPrint.ID = "Abstract TcFailed-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }


                #endregion

                if (Request.QueryString["id"] == "DTrReportMake")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();

                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }

                    if (Request.QueryString["Make"].ToString() != null && Request.QueryString["Make"].ToString() != "")
                    {
                        objReport.sMake = Request.QueryString["Make"].ToString();
                    }
                    if (Request.QueryString["Capacity"].ToString() != null && Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                    }

                    if (Request.QueryString["Location"].ToString() != null && Request.QueryString["Location"].ToString() != "")
                    {
                        objReport.sLocType = Request.QueryString["Location"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();



                    // objReport.sOfficeCode = objSession.OfficeCode;
                    dt = objReport.PrintDTrReport(objReport);
                    crpDTrReport objRep = new crpDTrReport();
                    if (dt.Rows.Count > 0)
                    {
                        objRep.SetDataSource(dt);
                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                        //crpPrint.ID = "DTR Report-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }


                if (Request.QueryString["id"] == "DTCReportFeeder")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }

                    objReport.sOfficeCode = Request.QueryString["OfficeCode"].ToString();

                    if (Request.QueryString["SchemaType"].ToString() != null && Request.QueryString["SchemaType"].ToString() != "")
                    {
                        objReport.sSchemeType = Request.QueryString["SchemaType"].ToString();
                    }

                    if (Request.QueryString["Capacity"].ToString() != null && Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                    }

                    // objReport.sOfficeCode = objSession.OfficeCode;

                    dt = objReport.PrintDTCCReport(objReport);
                    //crpDTCreport objRep = new crpDTCreport();
                    if (dt.Rows.Count > 0)
                    {
                        crpDTCRep.SetDataSource(dt);
                        crpPrint.ReportSource = crpDTCRep;
                        crpPrint.DataBind();
                        //crpPrint.ID = "DTC Report-" + stroffcode + "-" + strTodayDate;
                    }

                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }

                if (Request.QueryString["id"] == "TCFail")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }
                    if (Request.QueryString["Type"].ToString() != null && Request.QueryString["Type"].ToString() != "")
                    {
                        objReport.sType = Request.QueryString["Type"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["FailType"].ToString() != null && Request.QueryString["FailType"].ToString() != "")
                    {
                        objReport.sFailureType = Request.QueryString["FailType"].ToString();
                    }
                    if (Request.QueryString["Make"].ToString() != null && Request.QueryString["Make"].ToString() != "")
                    {
                        objReport.sMake = Request.QueryString["Make"].ToString();
                    }

                    if (Request.QueryString["Capacity"].ToString() != null && Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                    }

                    if (Request.QueryString["GrntyType"].ToString() != null && Request.QueryString["GrntyType"].ToString() != "")
                    {
                        objReport.sGuranteeType = Request.QueryString["GrntyType"].ToString();
                    }
                    if (Request.QueryString["coilType"].ToString() != null && Request.QueryString["coilType"].ToString() != "")
                    {
                        objReport.sFailType = Request.QueryString["coilType"].ToString();
                    }

                    if (Request.QueryString["ReportType"].ToString() != null && Request.QueryString["ReportType"].ToString() != "")
                    {
                        objReport.sReportType = Request.QueryString["ReportType"].ToString();
                    }
                    if ((Request.QueryString["Stage"].ToString() ?? "").Length > 0) // added on 09-08-2023
                    {
                        objReport.SelectStage = Request.QueryString["Stage"].ToString();
                    }
                    // objReport.sOfficeCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objReport.sOfficeCode, objSession.RoleId);
                    objReport.sRoleID = objSession.RoleId;
                    dt = objReport.TCFailReport(objReport);
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();

                    if (dt.Rows.Count > 0)
                    {
                        objRep.SetDataSource(dt);
                        crpPrint.ReportSource = objRep;
                        crpPrint.DataBind();
                        //crpPrint.ID = "TC FailReport-" + stroffcode + "-" + strTodayDate;

                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }

                #region RegAbstract

                if (Request.QueryString["id"] == "RegAbstract")
                {
                    clsReports objReport = new clsReports();
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    ReportDocument rptDoc = new ReportDocument();

                    //objreg.Load(Server.MapPath("~/CregAbstract.rpt"));
                    //    Server.MapPath("~/CregAbstract.rpt");
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("dd-mm-yyyy");



                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("dd-mm-yyyy");


                    }

                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {

                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }

                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();

                    dt = objReport.PrintRegAbstact(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        objreg.SetDataSource(dt);
                        crpPrint.ReportSource = objreg;
                        crpPrint.DataBind();


                        //crpPrint.ID = "CregAbstract-" + stroffcode + "-" + strTodayDate;


                    }
                    //    ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                    //    if (Fromdate != string.Empty)
                    //    {
                    //        crParameterDiscreteValue4.Value = "From  " + Fromdate;
                    //    }
                    //    else
                    //    {
                    //        crParameterDiscreteValue4.Value = "";
                    //    }

                    //    crParameterFieldDefinitions = objreg.DataDefinition.ParameterFields;
                    //    crParameterFieldDefinition = crParameterFieldDefinitions["crpFromdate"];
                    //    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    //    crParameterValues.Add(crParameterDiscreteValue4);
                    //    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                    //    ParameterDiscreteValue crParameterDiscreteValue2 = new ParameterDiscreteValue();
                    //    if (Todate != string.Empty)
                    //    {
                    //        crParameterDiscreteValue2.Value = "To  " + Todate;
                    //    }
                    //    else
                    //    {
                    //        crParameterDiscreteValue2.Value = "";
                    //    }

                    //    crParameterFieldDefinitions = objreg.DataDefinition.ParameterFields;
                    //    crParameterFieldDefinition = crParameterFieldDefinitions["crpTodate"];
                    //    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    //    crParameterValues.Add(crParameterDiscreteValue2);
                    //    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    //}
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }
                #endregion


                if (Request.QueryString["id"] == "WorkOderReg")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sFailType = Request.QueryString["CoilType"].ToString();
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");

                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }

                    dt = objReport.WoRegDetails(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        crpWoRegDetails.SetDataSource(dt);
                        crpPrint.ReportSource = crpWoRegDetails;
                        crpPrint.DataBind();
                        //crpPrint.ID = "WorkOderReg-" + stroffcode + "-" + strTodayDate;

                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }



                if (Request.QueryString["id"] == "DTCAddDetails")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["FeederType"].ToString() != "")
                    {
                        objReport.sFeederType = Request.QueryString["FeederType"].ToString();
                    }
                    if (Request.QueryString["Capacity"].ToString() != "")
                    {
                        objReport.sCapacity = Request.QueryString["Capacity"].ToString();
                        objReport.sGreaterVal = "TRUE";
                    }
                    objReport.sType = Request.QueryString["Type"].ToString();

                    dt = objReport.DTCAddedReport(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        crpAddDTC.SetDataSource(dt);
                        crpPrint.ReportSource = crpAddDTC;
                        crpPrint.DataBind();
                        //crpPrint.ID = " DTC AddDetails report-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"] == "CRDetails")
                {
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    objReport.sFailId = Genaral.Decrypt(Request.QueryString["FailureId"].ToString());
                    objReport.sDtcCode = Genaral.Decrypt(Request.QueryString["DTCCODE"].ToString());
                    dt = objReport.CRDetails(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        crpReportDetails.SetDataSource(dt);
                        crpPrint.ReportSource = crpReportDetails;
                        crpPrint.DataBind();
                        //crpPrint.ID = "CRDetails-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                //TRANFORMER PERFORMANCE

                if (Request.QueryString["id"] == "Pending Analysis Report")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    DateTime DFromDate = new DateTime();
                    DateTime DToDate = new DateTime();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = Convert.ToDateTime(DFromDate).ToString("dd-MMM-yyyy");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DToDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = Convert.ToDateTime(DToDate).ToString("dd-MMM-yyyy");
                    }
                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }
                    if (Request.QueryString["StrReprierName"].ToString() != null && Request.QueryString["StrReprierName"].ToString() != "")
                    {
                        objReport.sRepriername = Request.QueryString["StrReprierName"].ToString();
                    }

                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    objReport.sTempFromDate = objReport.sFromDate;
                    objReport.sTempTodate = objReport.sTodate;
                    objReport.sFromDate = DFromDate.ToString("yyyy/MM/dd");
                    objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    dt = objReport.PENDINGWTREPARIER(objReport);
                    dt1 = objReport.TransformerWiseDetails(objReport);



                    if (dt1.Rows.Count > 0)
                    {

                        crpReprierPer.SetDataSource(dt);
                        crpReprierPer.OpenSubreport("Transformerwisedetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = crpReprierPer;
                        crpPrint.DataBind();
                        //crpPrint.ID = "Repairer Pending-" + stroffcode + "-" + strTodayDate;


                        //ParameterDiscreteValue crParameterDiscreteValue1 = new ParameterDiscreteValue();
                        //if (Fromdate != string.Empty)
                        //{
                        //    crParameterDiscreteValue1.Value = "FROM  " + Fromdate;
                        //}
                        //else
                        //{
                        //    crParameterDiscreteValue1.Value = " ";
                        //}

                        //crParameterFieldDefinitions = crpReprierPer.DataDefinition.ParameterFields;
                        //crParameterFieldDefinition = crParameterFieldDefinitions["fromdate"];
                        //crParameterValues = crParameterFieldDefinition.CurrentValues;
                        //crParameterValues.Add(crParameterDiscreteValue1);
                        //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                        //crParameterValues = new ParameterValues();
                        //if (Todate != string.Empty)
                        //{
                        //    crParameterDiscreteValue1.Value = "TO  " + Todate;
                        //}
                        //else
                        //{
                        //    crParameterDiscreteValue1.Value = " ";
                        //}
                        //crParameterDiscreteValue1.Value = Todate;
                        //crParameterFieldDefinitions = crpReprierPer.DataDefinition.ParameterFields;
                        //crParameterFieldDefinition = crParameterFieldDefinitions["todate"];
                        //crParameterValues = crParameterFieldDefinition.CurrentValues;
                        //crParameterValues.Add(crParameterDiscreteValue1);
                        //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }


                if (Request.QueryString["id"] == "Delivered Analysis Report")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DFromDate = DateTime.ParseExact(objReport.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = Convert.ToDateTime(DFromDate).ToString("dd-MMM-yyyy");

                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DtoDate = DateTime.ParseExact(objReport.sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = Convert.ToDateTime(DtoDate).ToString("dd-MMM-yyyy");

                    }
                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {
                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }
                    if (Request.QueryString["StrReprierName"].ToString() != null && Request.QueryString["StrReprierName"].ToString() != "")
                    {
                        objReport.sRepriername = Request.QueryString["StrReprierName"].ToString();
                    }


                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();

                    dt = objReport.ReperierCompleted(objReport);
                    dt1 = objReport.TransformerWiseDetailsCompleted(objReport);

                    if (dt1.Rows.Count > 0)
                    {
                        crpCompleted.SetDataSource(dt);
                        crpCompleted.OpenSubreport("TransformerwiseCompleteddetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = crpCompleted;
                        crpPrint.DataBind();
                        //crpPrint.ID = "Reperier Completed-" + stroffcode + "-" + strTodayDate;

                        //ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                        //if (Fromdate != string.Empty)
                        //{
                        //    crParameterDiscreteValue4.Value = "From  " + Fromdate;
                        //}
                        //else
                        //{
                        //    crParameterDiscreteValue4.Value = "";
                        //}

                        //crParameterFieldDefinitions = crpCompleted.DataDefinition.ParameterFields;
                        //crParameterFieldDefinition = crParameterFieldDefinitions["crpFromdate"];
                        //crParameterValues = crParameterFieldDefinition.CurrentValues;
                        //crParameterValues.Add(crParameterDiscreteValue4);
                        //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                        //ParameterDiscreteValue crParameterDiscreteValue2 = new ParameterDiscreteValue();
                        //if (Todate != string.Empty)
                        //{
                        //    crParameterDiscreteValue2.Value = "To  " + Todate;
                        //}
                        //else
                        //{
                        //    crParameterDiscreteValue2.Value = "";
                        //}

                        //crParameterFieldDefinitions = crpCompleted.DataDefinition.ParameterFields;
                        //crParameterFieldDefinition = crParameterFieldDefinitions["crpTodate"];
                        //crParameterValues = crParameterFieldDefinition.CurrentValues;
                        //crParameterValues.Add(crParameterDiscreteValue2);
                        //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }

                //dtrMakeView
                if (Request.QueryString["id"] == "DTr make wise Reports")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    objReport.sFailType = Request.QueryString["CoilType"].ToString();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }
                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");


                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");


                    }

                    if (Request.QueryString["MakeValue"].ToString() != null && Request.QueryString["MakeValue"].ToString() != "")
                    {
                        objReport.sMake = Request.QueryString["MakeValue"].ToString();

                    }

                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {

                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }


                    // Fromdate = Request.QueryString["FromDate"].ToString();
                    // Todate = Request.QueryString["ToDate"].ToString();
                    dt = objReport.Printdtrwise(objReport);
                    dt1 = objReport.PrintMakeWiseDetails(objReport);
                    if (dt.Rows.Count > 0)
                    {
                        objDtr.SetDataSource(dt);
                        objDtr.OpenSubreport("crpMakeWiseDetails.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = objDtr;
                        crpPrint.DataBind();
                        //crpPrint.ID = "MakeWise Details-" + stroffcode + "-" + strTodayDate;


                        //ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                        //if (Fromdate != string.Empty)
                        //{
                        //    crParameterDiscreteValue4.Value = "From  " + Fromdate;
                        //}
                        //else
                        //{
                        //    crParameterDiscreteValue4.Value = "";
                        //}

                        //crParameterFieldDefinitions = objDtr.DataDefinition.ParameterFields;
                        //crParameterFieldDefinition = crParameterFieldDefinitions["crpFromdate"];
                        //crParameterValues = crParameterFieldDefinition.CurrentValues;
                        //crParameterValues.Add(crParameterDiscreteValue4);
                        //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                        //ParameterDiscreteValue crParameterDiscreteValue2 = new ParameterDiscreteValue();
                        //if (Todate != string.Empty)
                        //{
                        //    crParameterDiscreteValue2.Value = "To  " + Todate;
                        //}
                        //else
                        //{
                        //    crParameterDiscreteValue2.Value = "";
                        //}

                        //crParameterFieldDefinitions = objDtr.DataDefinition.ParameterFields;
                        //crParameterFieldDefinition = crParameterFieldDefinitions["crpTodate"];
                        //crParameterValues = crParameterFieldDefinition.CurrentValues;
                        //crParameterValues.Add(crParameterDiscreteValue2);
                        //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }


                //dtr RepairerWise Report
                if (Request.QueryString["id"] == "DTr RepairerWise")
                {
                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    DataTable dtOthers = new DataTable();
                    DataTable dtAbstract = new DataTable();


                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");

                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");

                    }

                    if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                    {

                        objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                    }


                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();
                    dt = objReport.PrintRePairerwise(objReport);
                    dtOthers = objReport.PrintRePairerOtherswise(objReport);
                    DataTable dt2 = new DataTable();
                    string strOthers = string.Empty;
                    if (dtOthers.Rows.Count != 0)
                    {
                        strOthers = dtOthers.Rows[0]["FAILURE"].ToString();
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Rows.Add("NO REPAIRER NAME");
                        dt.Columns[1].ReadOnly = false;
                        dt.Rows[dt.Rows.Count - 1]["MORETHANONCE"] = "0";
                    }
                    if (strOthers != "")
                    {
                        dt.Rows.Add("FAILURE");
                        dt.Columns[1].ReadOnly = false;
                        dt.Rows[dt.Rows.Count - 1]["MORETHANONCE"] = strOthers;
                    }

                    DataTable dtFinal = new DataTable();
                    dtFinal = dt;
                    dtAbstract = objReport.PrintDtrRepairerWise(objReport);
                    if (dtFinal.Rows.Count > 0)
                    {
                        crpRepairerWise.SetDataSource(dtFinal);
                        crpRepairerWise.OpenSubreport("crpDtrRepairerWiseDetails.rpt").SetDataSource(dtAbstract);
                        crpPrint.ReportSource = crpRepairerWise;
                        crpPrint.DataBind();
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }

                if (Request.QueryString["id"] == "FailureAbstract")
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    clsReports objReport = new clsReports();
                    ReportDocument repDoc = new ReportDocument();

                    objReport.sOfficeCode = clsStoreOffice.GetZone_Circle_Div_Offcode(objSession.OfficeCode, objSession.RoleId);

                    if (Request.QueryString["Month"].ToString() != null && Request.QueryString["Month"].ToString() != "")
                    {
                        objReport.sMonth = Request.QueryString["Month"].ToString();
                        objReport.sCurrentMonth = DateTime.Now.ToString("yyyy-MM");
                    }
                    objReport.sReportType = Request.QueryString["ReportType"].ToString();

                    dt = objReport.FailureAbstract(objReport);
                    dt1 = objReport.FailMonthWiseAbstract(objReport);
                    if (dt.Rows.Count > 0)
                    {

                        crpFailureAbstract.SetDataSource(dt);

                        crpFailureAbstract.OpenSubreport("FailureAbstractMonthWise.rpt").SetDataSource(dt1);
                        crpPrint.ReportSource = crpFailureAbstract;
                        crpPrint.DataBind();
                        //crpFailureAbstract.SetParameterValue("ReportType", objReport.sReportType);
                        //string testpath = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;
                        //repDoc.Load(testpath + "\\IIITS.DTLMS.REPORTS\\FailureAbstract.rpt");//"C:\\Users\\jeevan.j\\Desktop\\SVN\\IIITS.DTLMS.REPORTS\\FailureAbstract.rpt"
                        //repDoc.SetDataSource(dt);
                        //repDoc.Subreports["FailureAbstractMonthWise.rpt"].SetDataSource(dt1);
                        //repDoc.SetParameterValue("ReportType", objReport.sReportType);

                        //repDoc.Subreports[0].SetParameterValue("ReportType", objReport.sReportType);

                        //crpPrint.DataBind();
                        //crpPrint.ID = "FailureAbstract-" + stroffcode + "-" + strTodayDate;
                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }

                }

                if (Request.QueryString["id"] == "FrequentTCFail")
                {

                    string Fromdate = string.Empty;
                    string Todate = string.Empty;
                    DataTable dt = new DataTable();
                    clsReports objReport = new clsReports();
                    if (Request.QueryString["FeederName"].ToString() != null && Request.QueryString["FeederName"].ToString() != "")
                    {
                        objReport.sFeeder = Request.QueryString["FeederName"].ToString();
                    }

                    if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                    {
                        objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sFromDate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sFromDate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                    {
                        objReport.sTodate = Request.QueryString["ToDate"].ToString();
                        DateTime DToDate = DateTime.ParseExact(objReport.sTodate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objReport.sTodate = DToDate.ToString("yyyy/MM/dd");
                    }
                    if (Request.QueryString["FailType"].ToString() != null && Request.QueryString["FailType"].ToString() != "")
                    {
                        objReport.sFailureType = Request.QueryString["FailType"].ToString();
                    }
                    if (Request.QueryString["GuranteeType"].ToString() != null && Request.QueryString["GuranteeType"].ToString() != "")
                    {
                        objReport.sGuranteeType = Request.QueryString["GuranteeType"].ToString();
                    }
                    if (Request.QueryString["DTCCode"].ToString() != null && Request.QueryString["DTCCode"].ToString() != "")
                    {
                        objReport.sDtcCode = Request.QueryString["DTCCode"].ToString();
                    }
                    if (Request.QueryString["DTRCode"].ToString() != null && Request.QueryString["DTRCode"].ToString() != "")
                    {
                        objReport.sDtrCode = Request.QueryString["DTRCode"].ToString();
                    }
                    objReport.sOfficeCode = Request.QueryString["Officecode"].ToString();
                    objReport.sFailType = Request.QueryString["coilType"].ToString();
                    dt = objReport.FrequentTcFail(objReport);
                    Fromdate = Request.QueryString["FromDate"].ToString();
                    Todate = Request.QueryString["ToDate"].ToString();

                    if (dt.Rows.Count > 0)
                    {
                        crpDtcFailFrequent.SetDataSource(dt);
                        crpPrint.ReportSource = crpDtcFailFrequent;
                        crpPrint.DataBind();

                    }
                    else
                    {
                        ShowMsgBox("No Records Found");
                    }
                }


                if (Request.QueryString["id"].ToString().Equals("RefinedpermanentEstimation"))
                {
                    string sEstId = Request.QueryString["EstimationId"].ToString();
                    clsReports objRep = new clsReports();

                    DataTable dtEstimation = new DataTable();
                    dtEstimation = objRep.GetpermanentEstimationDetails(sEstId);

                    DataTable dtEstimationTotal = new DataTable();
                    dtEstimationTotal = objRep.GetPermanentEstimationAmount(sEstId);
                    dtEstimation.Columns.Add("DTR_CODE", typeof(string));
                    DataRow drow = dtEstimation.NewRow();
                    for (int i = 0; i < dtEstimation.Rows.Count; i++)
                    {
                        dtEstimation.Rows[i]["DTR_CODE"] = dtEstimationTotal.Rows[0]["DTR_CODE"].ToString();
                    }
                    dtEstimationTotal.Columns.Remove("DTR_CODE");
                    crpperestimate.SetDataSource(dtEstimation);
                    crpPrint.ReportSource = crpperestimate;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"].ToString().Equals("RefinedpermanentEstimationSO"))
                {
                    string sWfo_ID = Request.QueryString["sWFOID"].ToString();
                    string sDtrcode = Request.QueryString["sDtrcode"].ToString();

                    clsApproval objApp = new clsApproval();
                    clsReports objRep = new clsReports();
                    clsPermanentEstimation objest = new clsPermanentEstimation();

                    DataSet dtfailureEst = new DataSet();
                    DataTable dtEstimationaldetails = new DataTable();
                    dtfailureEst = objApp.GetDatatableFromMultipleXML(sWfo_ID);

                    DataTable dtaddrow = new DataTable();
                    DataRow dtrow = dtaddrow.NewRow();
                    DataTable dtFinalResult = new DataTable();
                    DataTable dtEstimationmaterial = new DataTable();

                    dtEstimationaldetails = objest.gettcdetails(sDtrcode);

                    dtaddrow.Columns.Add("PEST_ID", typeof(string));
                    dtaddrow.Columns.Add("PEST_CAPACITY", typeof(string));
                    dtaddrow.Columns.Add("MRIM_REMARKS", typeof(string));
                    dtaddrow.Columns.Add("MRIM_ITEM_NAME", typeof(string));
                    dtaddrow.Columns.Add("MD_NAME", typeof(string));
                    dtaddrow.Columns.Add("PESTM_ITEM_QNTY", typeof(string));
                    dtaddrow.Columns.Add("PESTM_ITEM_RATE", typeof(string));
                    dtaddrow.Columns.Add("AMOUNT", typeof(string));
                    dtaddrow.Columns.Add("PESTM_ITEM_TAX", typeof(string));
                    dtaddrow.Columns.Add("PESTM_ITEM_TOTAL", typeof(string));
                    dtaddrow.Columns.Add("MRIM_ITEM_ID", typeof(string));

                    for (int i = 0; i < dtfailureEst.Tables.Count; i++)
                    {
                        if (dtfailureEst.Tables[i].Rows.Count > 0)
                        {

                            if (i == 0)
                            {

                                objest.sFailureId = Convert.ToString(dtfailureEst.Tables[0].Rows[0]["PEST_ID"]);
                                objest.sReplaceCapacity = Convert.ToString(dtfailureEst.Tables[0].Rows[0]["PEST_CAPACITY"]);

                                dtaddrow = CreateDatatable(objest);

                            }
                            else if (i == 1)
                            {


                                objest.sremarks = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRIM_REMARKS"]);
                                objest.sMaterialName = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRIM_ITEM_NAME"]).Replace("ç", ",");
                                objest.sMaterialQnty = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["PESTM_ITEM_QNTY"]);
                                objest.sMaterialRate = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRI_BASE_RATE"]);
                                objest.sAmount = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["AMOUNT"]);
                                objest.sMaterialTax = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["PESTM_ITEM_TAX"]);
                                objest.sMaterialTotal = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRI_TOTAL"]);
                                objest.sMaterialunitName = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MD_NAME"]);
                                objest.sMaterialItemId = Convert.ToString(dtfailureEst.Tables[1].Rows[0]["MRIM_ITEM_ID"]);
                                dtFinalResult = CreateDatatableFromString(objest);

                                DataColumn dcID = dtaddrow.Columns["PEST_ID"];
                                DataColumn dcCap = dtaddrow.Columns["PEST_CAPACITY"];
                                DataColumn dcOmname = dtEstimationaldetails.Columns["OM_NAME"];
                                DataColumn dcName = dtEstimationaldetails.Columns["TM_NAME"];
                                DataColumn dcDtrcode = dtEstimationaldetails.Columns["DTR_CODE"];

                                dtFinalResult.Columns.Add(dcID.ColumnName, dcID.DataType);
                                dtFinalResult.Columns.Add(dcCap.ColumnName, dcCap.DataType);
                                dtFinalResult.Columns.Add(dcOmname.ColumnName, dcOmname.DataType);
                                dtFinalResult.Columns.Add(dcName.ColumnName, dcName.DataType);
                                dtFinalResult.Columns.Add(dcDtrcode.ColumnName, dcDtrcode.DataType);

                                for (int j = 0; j < dtFinalResult.Rows.Count; j++)
                                {

                                    dtFinalResult.Rows[j]["PEST_ID"] = dtaddrow.Rows[0]["PEST_ID"];
                                    dtFinalResult.Rows[j]["PEST_CAPACITY"] = dtaddrow.Rows[0]["PEST_CAPACITY"];
                                    dtFinalResult.Rows[j]["OM_NAME"] = dtEstimationaldetails.Rows[0]["OM_NAME"];
                                    dtFinalResult.Rows[j]["TM_NAME"] = dtEstimationaldetails.Rows[0]["TM_NAME"];
                                    dtFinalResult.Rows[j]["DTR_CODE"] = dtEstimationaldetails.Rows[0]["DTR_CODE"];
                                }


                                dtFinalResult.Columns.Add("EstTotal", typeof(string));
                                dtFinalResult.Columns.Add("EstAmount", typeof(string));
                                dtFinalResult.Columns.Add("EstTax", typeof(string));


                                Double Mtotalamount = 0.0;
                                Double Mtotaltax = 0.0;
                                Double Mamount = 0.0;

                                Double Ltotalamount = 0.0;
                                Double Ltotaltax = 0.0;
                                Double Lamount = 0.0;

                                Double Stotalamount = 0.0;
                                Double Stotaltax = 0.0;
                                Double Samount = 0.0;

                                Double Totalamount = 0.0;
                                Double totaltax = 0.0;
                                Double amount = 0.0;

                                for (int j = 0; j < dtFinalResult.Rows.Count; j++)
                                {

                                    if (Convert.ToString(dtFinalResult.Rows[j]["MRIM_REMARKS"]).Trim() == "Labour Charges")
                                    {
                                        Lamount = Lamount + Convert.ToDouble(dtFinalResult.Rows[j]["AMOUNT"]);
                                        Ltotaltax = Ltotaltax + Convert.ToDouble(dtFinalResult.Rows[j]["PESTM_ITEM_TAX"]);
                                        Ltotalamount = Ltotalamount + Convert.ToDouble(dtFinalResult.Rows[j]["PESTM_ITEM_TOTAL"]);
                                    }




                                }
                                amount = (Mamount + Lamount) - Samount;
                                totaltax = (Mtotaltax + Ltotaltax) - Stotaltax;
                                Totalamount = (Mtotalamount + Ltotalamount) - Stotalamount;

                                for (int j = 0; j < dtFinalResult.Rows.Count; j++)
                                {

                                    dtFinalResult.Rows[j]["EstTotal"] = Totalamount;
                                    dtFinalResult.Rows[j]["EstAmount"] = amount;
                                    dtFinalResult.Rows[j]["EstTax"] = totaltax;

                                }

                                crpperestimateso.SetDataSource(dtFinalResult);
                                crpPrint.ReportSource = crpperestimateso;
                                crpPrint.DataBind();

                            }


                        }
                    }
                }

                if (Request.QueryString["id"].ToString().Equals("WorkOrderPreviewpermanent"))
                {
                    clsApproval objApproval = new clsApproval();
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    ArrayList sNameList = new ArrayList();

                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    if (sOffCode == null)
                    {
                        if (objSession.OfficeCode.Length > Constants.Division)
                        {
                            sOffCode = objSession.OfficeCode.Substring(1, Constants.Division);
                        }

                    }
                    DataTable dtWorkOrderDetailsDeComm = new DataTable();
                    string sTaskType = Request.QueryString["TaskType"].ToString();
                    string sSubDivName = Request.QueryString["sSubDivName"].ToString();
                    string sWoId = Request.QueryString["WoId"].ToString();

                    if (sWFDataId != "")
                        dtWorkOrderDetailsDeComm = objApproval.GetDatatableFromXML(sWFDataId);

                    if (Session["UserNameList"] != null)
                    {
                        sNameList = (ArrayList)Session["UserNameList"];

                    }


                    crpworkper.SetDataSource(dtWorkOrderDetailsDeComm);
                    crpPrint.ReportSource = crpworkper;




                    if (sLevelOfApproval == "1")
                    {
                        crpworkper.SetParameterValue("aetusername", sNameList[0]);
                        crpworkper.SetParameterValue("stousername", "");
                        crpworkper.SetParameterValue("aousername", "");
                        crpworkper.SetParameterValue("dousername", "");
                        crpworkper.SetParameterValue("draft", "[DRAFT]");
                    }


                    if (sLevelOfApproval == "2")
                    {
                        crpworkper.SetParameterValue("aetusername", sNameList[0]);
                        crpworkper.SetParameterValue("stousername", sNameList[1]);
                        crpworkper.SetParameterValue("aousername", "");
                        crpworkper.SetParameterValue("dousername", "");
                        crpworkper.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "3")
                    {
                        crpworkper.SetParameterValue("aetusername", sNameList[0]);
                        crpworkper.SetParameterValue("stousername", sNameList[1]);
                        crpworkper.SetParameterValue("aousername", sNameList[2]);
                        crpworkper.SetParameterValue("dousername", "");
                        crpworkper.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "4")
                    {
                        crpworkper.SetParameterValue("aetusername", sNameList[0]);
                        crpworkper.SetParameterValue("stousername", sNameList[1]);
                        crpworkper.SetParameterValue("aousername", sNameList[2]);
                        crpworkper.SetParameterValue("dousername", sNameList[3]);
                        crpworkper.SetParameterValue("draft", "");
                    }
                    crpworkper.SetParameterValue("Officename", sOffCode);
                    crpPrint.DataBind();

                }

                if (Request.QueryString["id"].ToString().Equals("WorkOrderPreviewRepairer"))
                {
                    clsApproval objApproval = new clsApproval();
                    DataTable dtWorkOrderDetailsComm = new DataTable();
                    ArrayList sNameList = new ArrayList();

                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sLevelOfApproval = Request.QueryString["LApprovel"].ToString();
                    string sOffCode = Request.QueryString["OffCode"].ToString();
                    string sIncurerdcost = Request.QueryString["sIncuredcost"].ToString();
                    string sScrapWono = "";
                    string sScrapAmt = "";
                    string sScrapCode = "";
                    if (sOffCode == null)
                    {
                        if (objSession.OfficeCode.Length > Constants.Division)
                        {
                            sOffCode = objSession.OfficeCode.Substring(1, Constants.Division);
                        }

                    }
                    DataTable dtWorkOrderDetailsDeComm = new DataTable();
                    string sTaskType = Request.QueryString["TaskType"].ToString();
                    string sSubDivName = Request.QueryString["sSubDivName"].ToString();
                    string sWoId = Request.QueryString["WoId"].ToString();

                    if (sWFDataId != "")
                    {
                        dtWorkOrderDetailsDeComm = objApproval.GetDatatableFromXML(sWFDataId);
                        DataColumnCollection columns = dtWorkOrderDetailsDeComm.Columns;
                        if (columns.Contains("RWO_SS_WO_NO"))
                        {
                            sScrapWono = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_WO_NO"]);
                        }
                        if (columns.Contains("RWO_SS_AMT"))
                        {
                            sScrapAmt = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_AMT"]);
                        }
                        if (columns.Contains("RWO_SS_ACCODE"))
                        {
                            sScrapCode = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_ACCODE"]);
                        }
                        //sScrapWono =Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_WO_NO"]);
                        // sScrapAmt = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_AMT"]);
                        //sScrapCode = Convert.ToString(dtWorkOrderDetailsDeComm.Rows[0]["RWO_SS_ACCODE"]);
                    }


                    if (Session["UserNameList"] != null)
                    {
                        sNameList = (ArrayList)Session["UserNameList"];

                    }


                    objrepwo.SetDataSource(dtWorkOrderDetailsDeComm);
                    crpPrint.ReportSource = objrepwo;




                    if (sLevelOfApproval == "1")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", "");
                        objrepwo.SetParameterValue("aousername", "");
                        objrepwo.SetParameterValue("dousername", "");
                        objrepwo.SetParameterValue("draft", "[DRAFT]");
                    }


                    if (sLevelOfApproval == "2")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", sNameList[1]);
                        objrepwo.SetParameterValue("aousername", "");
                        objrepwo.SetParameterValue("dousername", "");
                        objrepwo.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "3")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", sNameList[1]);
                        objrepwo.SetParameterValue("aousername", sNameList[2]);
                        objrepwo.SetParameterValue("dousername", "");
                        objrepwo.SetParameterValue("draft", "[DRAFT]");
                    }
                    if (sLevelOfApproval == "4")
                    {
                        objrepwo.SetParameterValue("aetusername", sNameList[0]);
                        objrepwo.SetParameterValue("stousername", sNameList[1]);
                        objrepwo.SetParameterValue("aousername", sNameList[2]);
                        objrepwo.SetParameterValue("dousername", sNameList[3]);
                        objrepwo.SetParameterValue("draft", "");
                    }
                    objrepwo.SetParameterValue("Officename", sOffCode);
                    if (sIncurerdcost != "")
                    {
                        objrepwo.SetParameterValue("Incuredcost", sIncurerdcost);
                    }
                    else
                    {
                        objrepwo.SetParameterValue("Incuredcost", "0");
                    }
                    if (sScrapWono != "" && sScrapAmt != "" && sScrapCode != "")
                    {
                        objrepwo.SetParameterValue("scrapwono", sScrapWono);
                        objrepwo.SetParameterValue("scrapamount", sScrapAmt);
                        objrepwo.SetParameterValue("scrapcode", sScrapCode);
                    }
                    else
                    {
                        objrepwo.SetParameterValue("scrapwono", "0");
                        objrepwo.SetParameterValue("scrapamount", "0");
                        objrepwo.SetParameterValue("scrapcode", "0");
                    }
                    crpPrint.DataBind();

                }


                if (Request.QueryString["id"] == "IndentReportPermanent")
                {
                    //clsIndent objIndent = new clsIndent();
                    string sIndentId = Request.QueryString["IndentId"].ToString();
                    string sWFDataId = Request.QueryString["WFDataId"].ToString();
                    string sofficeCode = Request.QueryString["OffCode"].ToString();
                    DataTable dt = new DataTable();

                    clsApproval objApproval = new clsApproval();
                    clsReports objReport = new clsReports();
                    if (sWFDataId != "")
                    {
                        dt = objApproval.GetDatatableFromXML(sWFDataId);
                        string sWOSlno = Convert.ToString(dt.Rows[0]["PTI_WO_SLNO"]);
                        string sstoreId = clsStoreOffice.GetStoreID(sofficeCode);
                        dt = objReport.WO_IndentDetailspermanent(sWOSlno, sstoreId);

                    }
                    else
                    {
                        if (sIndentId != "")
                        {
                            dt = objReport.IndentDetailspermanent(sIndentId);
                        }
                    }

                    crpindentper.SetDataSource(dt);
                    crpPrint.ReportSource = crpindentper;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "RIReportpermanent")
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.RIReportpermanent(sDecommId);

                    crpriper.SetDataSource(dt);
                    crpPrint.ReportSource = crpriper;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "RIReportpermanentso")
                {

                    string swfoid = Request.QueryString["wfoID"].ToString();
                    string sDtrcode = Request.QueryString["sDtrcode"].ToString();
                    string sFailurId = Request.QueryString["sFailurId"].ToString();
                    DataTable dt = new DataTable();
                    clsApproval objApp = new clsApproval();
                    clsReports objreport = new clsReports();

                    DataTable dtfailureDecomm = new DataTable();
                    dtfailureDecomm = objApp.GetDatatableFromXML(swfoid);

                    DataTable dtaddrow = new DataTable();
                    DataRow dtrow = dtaddrow.NewRow();
                    DataTable dtRixml = new DataTable();
                    DataTable dtFinalResult = new DataTable();

                    dtaddrow.Columns.Add("PTR_RI_NO", typeof(string));
                    dtaddrow.Columns.Add("PTR_RI_DATE", typeof(string));
                    dtaddrow.Columns.Add("TC_CAPACITY", typeof(string));
                    dtaddrow.Columns.Add("SM_NAME", typeof(string));
                    dtaddrow.Columns.Add("PEST_UNIT_PRICE", typeof(string));
                    dtaddrow.Columns.Add("PWO_NO_DECOM", typeof(string));
                    dtaddrow.Columns.Add("PEST_NO", typeof(string));
                    dtaddrow.Columns.Add("PEST_DTC_CODE", typeof(string));
                    dtaddrow.Columns.Add("DTC_NAME", typeof(string));
                    dtaddrow.Columns.Add("DIVISION", typeof(string));
                    dtaddrow.Columns.Add("SUBDIVISION", typeof(string));

                    dtaddrow.Columns.Add("SECTION", typeof(string));
                    dtaddrow.Columns.Add("MAKE", typeof(string));
                    dtaddrow.Columns.Add("TC_SLNO", typeof(string));
                    dtaddrow.Columns.Add("TC_MANF_DATE", typeof(string));
                    dtaddrow.Columns.Add("TC_CODE", typeof(string));
                    dtaddrow.Columns.Add("PTR_OIL_QUNTY", typeof(string));
                    dtaddrow.Columns.Add("DTRCOMMISIONDATE", typeof(string));
                    dtaddrow.Columns.Add("PTR_OIL_QTY_BYSK", typeof(string));
                    dtaddrow.Columns.Add("PTR_DECOMM_DATE", typeof(string));
                    dtaddrow.Columns.Add("ACK_NO", typeof(string));
                    dtaddrow.Columns.Add("ACK_DATE", typeof(string));
                    dtaddrow.Columns.Add("SDO_USERNAME", typeof(string));

                    dtaddrow.Columns.Add("SO_USERNAME", typeof(string));
                    dtaddrow.Columns.Add("STO_USERNAME", typeof(string));
                    dtaddrow.Columns.Add("SK_USERNAME", typeof(string));
                    dtaddrow.Columns.Add("PTR_MANUAL_ACKRV_NO", typeof(string));


                    dtrow["PTR_OIL_QUNTY"] = dtfailureDecomm.Rows[0]["PTR_OIL_QUNTY"].ToString();
                    dtrow["PTR_DECOMM_DATE"] = dtfailureDecomm.Rows[0]["PTR_DECOMM_DATE"].ToString();


                    dtaddrow.Rows.Add(dtrow);
                    dtFinalResult = objreport.RIReportpermanentso(sDtrcode, sFailurId);

                    DataColumn dcOIL = dtaddrow.Columns["PTR_OIL_QUNTY"];
                    DataColumn dcDEDATE = dtaddrow.Columns["PTR_DECOMM_DATE"];



                    dtFinalResult.Columns.Add(dcOIL.ColumnName, dcOIL.DataType);
                    dtFinalResult.Columns.Add(dcDEDATE.ColumnName, dcDEDATE.DataType);


                    for (int i = 0; i < dtFinalResult.Rows.Count; i++)
                    {

                        dtFinalResult.Rows[i]["PTR_OIL_QUNTY"] = dtaddrow.Rows[0]["PTR_OIL_QUNTY"];
                        dtFinalResult.Rows[i]["PTR_DECOMM_DATE"] = dtaddrow.Rows[0]["PTR_DECOMM_DATE"];

                    }

                    crpriper.SetDataSource(dtFinalResult);
                    crpPrint.ReportSource = crpriper;
                    crpPrint.DataBind();
                }
                if (Request.QueryString["id"].ToString().Equals("EnhanceEstimation"))
                {
                    string sEnhanceId = Request.QueryString["EnhanceId"].ToString();
                    clsReports objRep = new clsReports();
                    string sStatus = Convert.ToString(Request.QueryString["sStatus"]);
                    string sFailType = Convert.ToString(Request.QueryString["FailType"]);
                    string sInsulationType = Convert.ToString(Request.QueryString["Insulationtype"]);

                    string soilqnty = Convert.ToString(Request.QueryString["oilqnty"]);
                    string oiltotal = Convert.ToString(Request.QueryString["oiltotal"]);
                    string oiltype = Convert.ToString(Request.QueryString["oiltype"]);
                    string oilprice = Convert.ToString(Request.QueryString["oilprice"]);
                    string StatusFalg = Convert.ToString(Request.QueryString["statusFlag"]);
                    string starrating = Convert.ToString(Request.QueryString["starrating"]);

                    if (soilqnty == "" || soilqnty == string.Empty)
                    {
                        soilqnty = "0";
                    }

                    if (oiltotal == "" || oiltotal == string.Empty)
                    {
                        oiltotal = "0";
                    }

                    if (oiltype == "" || oiltype == string.Empty)
                    {
                        oiltype = "0";
                    }
                    if (oilprice == "" || oilprice == string.Empty)
                    {
                        oilprice = "123.2";
                    }

                    if (starrating == "" || starrating == string.Empty)
                    {
                        starrating = "0";
                    }

                    DataTable dtCommEstimation = new DataTable();


                    dtCommEstimation = objRep.PrintEstimatedReport(sEnhanceId, sStatus, sFailType, sInsulationType, starrating, StatusFalg);
                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.PrintDecomEstimatedReport(sEnhanceId, sStatus, sFailType, sInsulationType, starrating, StatusFalg);
                    //DataTable dtSurvey = new DataTable();
                    //dtSurvey = objRep.PrintSurveyReport(sEnhanceId);
                    crpEnhanceEst.OpenSubreport("CrpCommEstimationReport.rpt").SetDataSource(dtCommEstimation);
                    crpEnhanceEst.OpenSubreport("crpDeCommEstReport.rpt").SetDataSource(dtDecomEstimation);
                    // crpEstimationReport.OpenSubreport("crpServeyReport.rpt").SetDataSource(dtSurvey);
                    crpPrint.ReportSource = crpEnhanceEst;

                    crpEnhanceEst.SetParameterValue("oilqnty", soilqnty);
                    crpEnhanceEst.SetParameterValue("oiltotal", oiltotal);
                    crpEnhanceEst.SetParameterValue("oiltype", oiltype);
                    crpEnhanceEst.SetParameterValue("oilprice", oilprice);


                    crpPrint.DataBind();


                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    crParameterFieldDefinitions = crpEnhanceEst.DataDefinition.ParameterFields;
                    objparmfy.Value = soilqnty;
                    objparmfy.Value = oiltotal;
                    objparmfy.Value = oiltype;
                    objparmfy.Value = oilprice;


                    crParameterFieldDefinition = crParameterFieldDefinitions["oilqnty"];
                    crParameterFieldDefinition = crParameterFieldDefinitions["oiltotal"];
                    crParameterFieldDefinition = crParameterFieldDefinitions["oiltype"];
                    crParameterFieldDefinition = crParameterFieldDefinitions["oilprice"];


                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(objparmfy);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }



                if (Request.QueryString["id"].ToString().Equals("EstimationCalc"))
                {
                    string sFailId = Request.QueryString["FailId"].ToString();
                    clsReports objRep = new clsReports();

                    string sFailType = Convert.ToString(Request.QueryString["FailType"]);
                    string sInsulationType = Convert.ToString(Request.QueryString["Insulationtype"]);

                    string soilqnty = Convert.ToString(Request.QueryString["oilqnty"]);
                    string oiltotal = Convert.ToString(Request.QueryString["oiltotal"]);
                    string oiltype = Convert.ToString(Request.QueryString["oiltype"]);
                    string oilprice = Convert.ToString(Request.QueryString["oilprice"]);
                    string StatusFalg = Convert.ToString(Request.QueryString["statusFalg"]);
                    string starrating = Convert.ToString(Request.QueryString["starrating"]);


                    if (soilqnty == "" || soilqnty == string.Empty)
                    {
                        soilqnty = "0";
                    }

                    if (oiltotal == "" || oiltotal == string.Empty)
                    {
                        oiltotal = "0";
                    }

                    if (oiltype == "" || oiltype == string.Empty)
                    {
                        oiltype = "0";
                    }
                    if (oilprice == "" || oilprice == string.Empty)
                    {
                        oilprice = "123.2";
                    }

                    if (starrating == "" || starrating == string.Empty)
                    {
                        starrating = "0";
                    }


                    DataTable dtCommEstimation = new DataTable();
                    dtCommEstimation = objRep.CalcEstimatedReport(sFailId, sFailType, sInsulationType, starrating, StatusFalg);
                    DataTable dtDecomEstimation = new DataTable();
                    dtDecomEstimation = objRep.CalcDecomEstimatedReport(sFailId, sFailType, sInsulationType, starrating, StatusFalg);
                    //DataTable dtSurvey = new DataTable();
                    //dtSurvey = objRep.PrintSurveyReport(sEnhanceId); 
                    crpCalcEst.OpenSubreport("CrpCommEstimationReport.rpt").SetDataSource(dtCommEstimation);
                    crpCalcEst.OpenSubreport("crpDeCommEstReport.rpt").SetDataSource(dtDecomEstimation);
                    // crpEstimationReport.OpenSubreport("crpServeyReport.rpt").SetDataSource(dtSurvey);
                    // crpPrint.ReportSource = crpCalcEst;
                    //crpPrint.DataBind();


                    crpPrint.ReportSource = crpCalcEst;

                    crpCalcEst.SetParameterValue("oilqnty", soilqnty);
                    crpCalcEst.SetParameterValue("oiltotal", oiltotal);
                    crpCalcEst.SetParameterValue("oiltype", oiltype);
                    crpCalcEst.SetParameterValue("oilprice", oilprice);

                    crpPrint.DataBind();


                    ParameterDiscreteValue objparmfy = new ParameterDiscreteValue();

                    crParameterFieldDefinitions = crpCalcEst.DataDefinition.ParameterFields;
                    objparmfy.Value = soilqnty;
                    objparmfy.Value = oiltotal;
                    objparmfy.Value = oiltype;
                    objparmfy.Value = oilprice;

                    crParameterFieldDefinition = crParameterFieldDefinitions["oilqnty"];
                    crParameterFieldDefinition = crParameterFieldDefinitions["oiltotal"];
                    crParameterFieldDefinition = crParameterFieldDefinitions["oiltype"];
                    crParameterFieldDefinition = crParameterFieldDefinitions["oilprice"];
                    crParameterValues = crParameterFieldDefinition.CurrentValues;
                    crParameterValues.Add(objparmfy);
                    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                }



                if (Request.QueryString["id"] == "RIAckReportpermanent")
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();
                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.RIReportpermanentsk(sDecommId, objSession.OfficeCode);

                    crpRIAckper.SetDataSource(dt);
                    crpPrint.ReportSource = crpRIAckper;
                    crpPrint.DataBind();
                }

                if (Request.QueryString["id"] == "CRReportpermanent")
                {
                    string sDecommId = Request.QueryString["DecommId"].ToString();

                    DataTable dt = new DataTable();
                    clsReports objreport = new clsReports();
                    dt = objreport.CompletionReportpermanent(sDecommId);


                    crReportpermanent.SetDataSource(dt);
                    crpPrint.ReportSource = crReportpermanent;
                    crpPrint.DataBind();
                }


                //Cregister abstract report
                #region reg abstract
                //if (Request.QueryString["id"] == "RegAbstract")
                // {
                //     clsReports objReport = new clsReports();
                //     string Fromdate = string.Empty;
                //     string Todate = string.Empty;
                //     DataTable dt = new DataTable();
                //     if (Request.QueryString["FromDate"].ToString() != null && Request.QueryString["FromDate"].ToString() != "")
                //     {
                //         objReport.sFromDate = Request.QueryString["FromDate"].ToString();
                //     }
                //     if (Request.QueryString["ToDate"].ToString() != null && Request.QueryString["ToDate"].ToString() != "")
                //     {
                //         objReport.sTodate = Request.QueryString["ToDate"].ToString();
                //     }

                //     if (Request.QueryString["offcode"].ToString() != null && Request.QueryString["offcode"].ToString() != "")
                //     {

                //         objReport.sOfficeCode = Request.QueryString["offcode"].ToString();
                //     }

                //     Fromdate = Request.QueryString["FromDate"].ToString();
                //     Todate = Request.QueryString["ToDate"].ToString();

                //     dt = objReport.PrintRegAbstact(objReport);

                //     if (dt.Rows.Count > 0)
                //     {
                //         objreg.SetDataSource(dt);
                //         crpPrint.ReportSource = objreg;
                //         crpPrint.DataBind();
                //         crpPrint.ID = "RegAbstact-" + stroffcode + "-" + strTodayDate;

                //         ParameterDiscreteValue crParameterDiscreteValue4 = new ParameterDiscreteValue();
                //         if (Fromdate != string.Empty)
                //         {
                //             crParameterDiscreteValue4.Value = "From  " + Fromdate;
                //         }
                //         else
                //         {
                //             crParameterDiscreteValue4.Value = "";
                //         }

                //         crParameterFieldDefinitions = objreg.DataDefinition.ParameterFields;
                //         crParameterFieldDefinition = crParameterFieldDefinitions["crpFromdate"];
                //         crParameterValues = crParameterFieldDefinition.CurrentValues;
                //         crParameterValues.Add(crParameterDiscreteValue4);
                //         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                //         ParameterDiscreteValue crParameterDiscreteValue2 = new ParameterDiscreteValue();
                //         if (Todate != string.Empty)
                //         {
                //             crParameterDiscreteValue2.Value = "To  " + Todate;
                //         }
                //         else
                //         {
                //             crParameterDiscreteValue2.Value = "";
                //         }

                //         crParameterFieldDefinitions = objreg.DataDefinition.ParameterFields;
                //         crParameterFieldDefinition = crParameterFieldDefinitions["crpTodate"];
                //         crParameterValues = crParameterFieldDefinition.CurrentValues;
                //         crParameterValues.Add(crParameterDiscreteValue2);
                //         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                //     }
                //     else
                //     {
                //         ShowMsgBox("No Records Found");
                //     }


                // }
                #endregion

                if (Request.QueryString["id"] == "DtcFailDetails")
                {
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();
                    objReport.sOfficeCode = Convert.ToString(Request.QueryString["Officecode"]);
                    if (Convert.ToString(Request.QueryString["FeederName"]) != null && Convert.ToString(Request.QueryString["FeederName"]) != "")
                    {
                        objReport.sFeeder = Convert.ToString(Request.QueryString["FeederName"]);
                    }
                    if (Convert.ToString(Request.QueryString["FromDate"]) != null && Convert.ToString(Request.QueryString["FromDate"]) != "")
                    {
                        objReport.sFromDate = Convert.ToString(Request.QueryString["FromDate"]);
                    }
                    if (Convert.ToString(Request.QueryString["ToDate"]) != null && Convert.ToString(Request.QueryString["ToDate"]) != "")
                    {
                        objReport.sTodate = Convert.ToString(Request.QueryString["ToDate"]);
                    }

                    objReport.sFailureStatus = Convert.ToString(Request.QueryString["sFailureStatus"]);

                    dt = objReport.GetDtcFailDetail(objReport);

                    crpPgrsReport.SetDataSource(dt);
                    crpPrint.ReportSource = crpPgrsReport;
                    crpPrint.DataBind();
                }

                #region buffer stock 

                if (Request.QueryString["id"] == "BufferStock")
                {
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();


                    dt = objReport.GetBufferStock(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        objcrpBufferStock.SetDataSource(dt);
                        crpPrint.ReportSource = objcrpBufferStock;
                        crpPrint.DataBind();
                    }
                }

                if (Request.QueryString["id"] == "BankStock")
                {
                    clsReports objReport = new clsReports();
                    DataTable dt = new DataTable();


                    dt = objReport.GetBankStock(objReport);

                    if (dt.Rows.Count > 0)
                    {
                        objbank.SetDataSource(dt);
                        crpPrint.ReportSource = objbank;
                        crpPrint.DataBind();
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                StackFrame CallStack = new StackFrame(1, true);
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        public DataTable LoadSignImages(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                clsReports objreport = new clsReports();
                List<string> sImagePaths = new List<string>();
                DataTable dtRoles = new DataTable();

                sImagePaths = objreport.PrintBlob(sOfficeCode);
                dtRoles = objreport.GetRoles();
                DataRow drow = dt.NewRow();
                dt.Rows.Add(drow);
                foreach (string str in sImagePaths)
                {
                    DataColumnCollection columns = dt.Columns;
                    string[] strRoles = str.Split('~');
                    if (!columns.Contains(strRoles[1]))
                    {
                        dt.Columns.Add(strRoles[1]);
                        drow[strRoles[1]] = strRoles[0];
                    }
                }
                dt = ContainColumn(dt, dtRoles);
                return dt;

                //crpBlob crpBlob = new crpBlob();
                //crpBlob.SetDataSource(dt);
                //crpPrint.ReportSource = crpBlob;
                //crpPrint.DataBind();
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }
        }

        public DataTable ContainColumn(DataTable dt, DataTable dtRoles)
        {

            DataColumnCollection columns = dt.Columns;
            for (int i = 0; i < dtRoles.Rows.Count; i++)
            {
                string strColumn = Convert.ToString(dtRoles.Rows[i]["RO_NAME"]);
                if (!columns.Contains(strColumn))
                {
                    dt.Columns.Add(strColumn);
                }
            }
            return dt;
        }

        public DataTable CreateDatatableFromString(clsEstimation objEst)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("MRIM_REMARKS");
            dt.Columns.Add("MRIM_ITEM_NAME");
            dt.Columns.Add("ESTM_ITEM_QNTY");
            dt.Columns.Add("ESTM_ITEM_RATE");
            dt.Columns.Add("ESTM_ITEM_TAX");
            dt.Columns.Add("ESTM_ITEM_TOTAL");
            dt.Columns.Add("MD_NAME");
            dt.Columns.Add("AMOUNT");
            dt.Columns.Add("MRIM_ITEM_ID");


            string[] sRemarks = objEst.sremarks.Split('`');
            string[] sName = objEst.sMaterialName.Split('`');
            string[] sQnty = objEst.sMaterialQnty.Split('`');
            string[] sRate = objEst.sMaterialRate.Split('`');
            string[] sTax = objEst.sMaterialTax.Split('`');
            string[] sTotal = objEst.sMaterialTotal.Split('`');
            string[] sUnitName = objEst.sMaterialunitName.Split('`');
            string[] sAmounts = objEst.sAmount.Split('`');
            string[] sMaterialItemid = objEst.sMaterialItemId.Split('`');

            for (int i = 0; i < sRemarks.Length; i++)
            {

                for (int j = 0; j < sName.Length; j++)
                {
                    for (int k = 0; k < sQnty.Length; k++)
                    {
                        for (int m = 0; m < sRate.Length; m++)
                        {
                            for (int q = 0; q < sAmounts.Length; q++)
                            {
                                for (int n = 0; n < sTax.Length; n++)
                                {
                                    for (int o = 0; o < sTotal.Length; o++)
                                    {
                                        for (int r = 0; r < sMaterialItemid.Length; r++)
                                        {
                                            for (int p = 0; p < sUnitName.Length; p++)
                                            {
                                                if (sUnitName[p] != "" && sUnitName[p] != " ")
                                                {
                                                    DataRow dRow = dt.NewRow();
                                                    dRow["MRIM_REMARKS"] = sRemarks[i];
                                                    dRow["MRIM_ITEM_NAME"] = sName[j];
                                                    dRow["ESTM_ITEM_QNTY"] = sQnty[k];
                                                    dRow["ESTM_ITEM_RATE"] = sRate[m];
                                                    dRow["AMOUNT"] = sAmounts[q];
                                                    dRow["ESTM_ITEM_TAX"] = sTax[n];
                                                    dRow["ESTM_ITEM_TOTAL"] = sTotal[o];
                                                    dRow["MD_NAME"] = sUnitName[p];
                                                    dRow["MRIM_ITEM_ID"] = sMaterialItemid[r];
                                                    dt.Rows.Add(dRow);
                                                    dt.AcceptChanges();
                                                }
                                                i++;
                                                j++;
                                                k++;
                                                m++;
                                                q++;
                                                n++;
                                                o++;
                                                r++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public DataTable CreateDatatable(clsEstimation objEst)
        {




            DataTable dt = new DataTable();
            dt.Columns.Add("EST_FAILUREID");
            dt.Columns.Add("EST_CAPACITY");




            string[] sFailureid = objEst.sFailureId.Split('`');
            string[] sCapacity = objEst.sReplaceCapacity.Split('`');


            for (int i = 0; i < sFailureid.Length; i++)
            {

                for (int j = 0; j < sCapacity.Length; j++)
                {


                    DataRow dRow = dt.NewRow();
                    dRow["EST_FAILUREID"] = sFailureid[i];
                    dRow["EST_CAPACITY"] = sCapacity[j];

                    dt.Rows.Add(dRow);
                    dt.AcceptChanges();

                    i++;
                    j++;

                }
            }

            return dt;
        }




        public DataTable CreateDatatable(clsPermanentEstimation objEst)
        {




            DataTable dt = new DataTable();
            dt.Columns.Add("PEST_ID");
            dt.Columns.Add("PEST_CAPACITY");




            string[] sFailureid = objEst.sFailureId.Split('`');
            string[] sCapacity = objEst.sReplaceCapacity.Split('`');


            for (int i = 0; i < sFailureid.Length; i++)
            {

                for (int j = 0; j < sCapacity.Length; j++)
                {


                    DataRow dRow = dt.NewRow();
                    dRow["PEST_ID"] = sFailureid[i];
                    dRow["PEST_CAPACITY"] = sCapacity[j];

                    dt.Rows.Add(dRow);
                    dt.AcceptChanges();

                    i++;
                    j++;

                }
            }

            return dt;
        }

        public DataTable CreateDatatableFromString(clsPermanentEstimation objEst)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("MRIM_REMARKS");
            dt.Columns.Add("MRIM_ITEM_NAME");
            dt.Columns.Add("PESTM_ITEM_QNTY");
            dt.Columns.Add("PESTM_ITEM_RATE");
            dt.Columns.Add("PESTM_ITEM_TAX");
            dt.Columns.Add("PESTM_ITEM_TOTAL");
            dt.Columns.Add("MD_NAME");
            dt.Columns.Add("AMOUNT");
            dt.Columns.Add("MRIM_ITEM_ID");


            string[] sRemarks = objEst.sremarks.Split('`');
            string[] sName = objEst.sMaterialName.Split('`');
            string[] sQnty = objEst.sMaterialQnty.Split('`');
            string[] sRate = objEst.sMaterialRate.Split('`');
            string[] sTax = objEst.sMaterialTax.Split('`');
            string[] sTotal = objEst.sMaterialTotal.Split('`');
            string[] sUnitName = objEst.sMaterialunitName.Split('`');
            string[] sAmounts = objEst.sAmount.Split('`');
            string[] sMaterialItemid = objEst.sMaterialItemId.Split('`');

            for (int i = 0; i < sRemarks.Length; i++)
            {

                for (int j = 0; j < sName.Length; j++)
                {
                    for (int k = 0; k < sQnty.Length; k++)
                    {
                        for (int m = 0; m < sRate.Length; m++)
                        {
                            for (int q = 0; q < sAmounts.Length; q++)
                            {
                                for (int n = 0; n < sTax.Length; n++)
                                {
                                    for (int o = 0; o < sTotal.Length; o++)
                                    {
                                        for (int r = 0; r < sMaterialItemid.Length; r++)
                                        {
                                            for (int p = 0; p < sUnitName.Length; p++)
                                            {
                                                if (sUnitName[p] != "" && sUnitName[p] != " ")
                                                {
                                                    DataRow dRow = dt.NewRow();
                                                    dRow["MRIM_REMARKS"] = sRemarks[i];
                                                    dRow["MRIM_ITEM_NAME"] = sName[j];
                                                    dRow["PESTM_ITEM_QNTY"] = sQnty[k];
                                                    dRow["PESTM_ITEM_RATE"] = sRate[m];
                                                    dRow["AMOUNT"] = sAmounts[q];
                                                    dRow["PESTM_ITEM_TAX"] = sTax[n];
                                                    dRow["PESTM_ITEM_TOTAL"] = sTotal[o];
                                                    dRow["MD_NAME"] = sUnitName[p];
                                                    dRow["MRIM_ITEM_ID"] = sMaterialItemid[r];
                                                    dt.Rows.Add(dRow);
                                                    dt.AcceptChanges();
                                                }
                                                i++;
                                                j++;
                                                k++;
                                                m++;
                                                q++;
                                                n++;
                                                o++;
                                                r++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }


        public DataTable CreateDatatable(ClsRepairerEstimate objEst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RESTD_ID");
            dt.Columns.Add("RESTD_NO");
            dt.Columns.Add("RESTD_CRON");
            dt.Columns.Add("RESTD_CAPACITY");
            dt.Columns.Add("RESTD_COIL_TYPE");
            dt.Columns.Add("RESTD_PHASE");

            string[] sEstid = objEst.sEstid.Split('`');
            string[] sEstNo = objEst.sEstimationNo.Split('`');
            string[] sEstCron = objEst.sEstDate.Split('`');
            string[] sCapacity = objEst.sReplaceCapacity.Split('`');
            string[] coiltype = objEst.coiltype.Split('`');
            string[] sPhases = objEst.sPhases.Split(',');


            for (int l = 0; l < sPhases.Length; l++)
            {

                for (int k = 0; k < coiltype.Length; k++)
                {
                    for (int j = 0; j < sCapacity.Length; j++)
                    {

                        for (int i = 0; i < sEstid.Length; i++)
                        {
                            for (int h = 0; h < sEstCron.Length; h++)
                            {
                                for (int g = 0; g < sEstNo.Length; g++)
                                {

                                    DataRow dRow = dt.NewRow();
                                    if (coiltype[k] == "1")
                                    {
                                        coiltype[k] = "SINGLE COIL";

                                    }
                                    else
                                    {
                                        coiltype[k] = "MULTI COIL";
                                    }
                                    switch (sPhases[l])
                                    {
                                        case "1":
                                            sPhases[l] = "R-Phase";
                                            break;
                                        case "2":
                                            sPhases[l] = "Y-Phase";
                                            break;
                                        case "3":
                                            sPhases[l] = "B-Phase";
                                            break;

                                        case "1`2`3":
                                            sPhases[l] = "R-Phase,Y-Phase,B-Phase";
                                            break;
                                        case "1`2":
                                            sPhases[l] = "R-Phase,Y-Phase";
                                            break;
                                        case "2`3":
                                            sPhases[l] = "Y-Phase,B-Phase";
                                            break;
                                        case "1`3":
                                            sPhases[l] = "R-Phase,B-Phase";
                                            break;

                                        default:

                                            break;
                                    }

                                    //if (sPhases[l] == "1`2`3`")
                                    //{
                                    //    sPhases[l] = "R-Phase,Y-Phase,B-Phase";

                                    //}
                                    //else if (sPhases[l] == "1`2`")
                                    //{
                                    //    sPhases[l] = "R-Phase,Y-Phase";

                                    //}
                                    //else if (sPhases[l] == "2`3`")
                                    //{
                                    //    sPhases[l] = "Y-Phase,B-Phase";

                                    //}
                                    // else if (sPhases[l] == "1`3`")
                                    //{
                                    //    sPhases[l] = "R-Phase,B-Phase";

                                    //}

                                    //foreach (string str in sPhases)
                                    //{
                                    //    //rybPhase.SelectedValue = str;

                                    //    if (str == "1")
                                    //    {
                                    //        sPhases[l] = "R-Phase";
                                    //    }
                                    //    if (str == "2")
                                    //    {
                                    //        sPhases[l] = "Y-Phase";
                                    //    }
                                    //    if (str == "3")
                                    //    {
                                    //        sPhases[l] = "B-Phase";
                                    //    }

                                    //}



                                    dRow["RESTD_PHASE"] = sPhases[l];
                                    dRow["RESTD_COIL_TYPE"] = coiltype[k];
                                    dRow["RESTD_CAPACITY"] = sCapacity[j];
                                    dRow["RESTD_ID"] = sEstid[i];
                                    dRow["RESTD_CRON"] = sEstCron[h];
                                    dRow["RESTD_NO"] = sEstNo[g];



                                    dt.Rows.Add(dRow);
                                    dt.AcceptChanges();

                                    l++;
                                    k++;
                                    j++;

                                    i++;
                                    h++;
                                    g++;
                                }
                            }
                        }
                    }

                }
            }

            return dt;
        }


        public DataTable CreateDatatableFromString(ClsRepairerEstimate objEst)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("MRIM_REMARKS");
            dt.Columns.Add("MRIM_ITEM_NAME");
            dt.Columns.Add("RESTM_ITEM_QNTY");
            dt.Columns.Add("RESTM_ITEM_RATE");
            dt.Columns.Add("RESTM_ITEM_TAX");
            dt.Columns.Add("RESTM_ITEM_TOTAL");
            dt.Columns.Add("MD_NAME");
            dt.Columns.Add("AMOUNT");
            dt.Columns.Add("MRIM_ITEM_ID");


            string[] sRemarks = objEst.sremarks.Split('`');
            string[] sName = objEst.sMaterialName.Split('`');
            string[] sQnty = objEst.sMaterialQnty.Split('`');
            string[] sRate = objEst.sMaterialRate.Split('`');
            string[] sTax = objEst.sMaterialTax.Split('`');
            string[] sTotal = objEst.sMaterialTotal.Split('`');
            string[] sUnitName = objEst.sMaterialunitName.Split('`');
            string[] sAmounts = objEst.sAmount.Split('`');
            string[] sMaterialItemid = objEst.sMaterialItemId.Split('`');

            for (int i = 0; i < sRemarks.Length; i++)
            {

                for (int j = 0; j < sName.Length; j++)
                {
                    for (int k = 0; k < sQnty.Length; k++)
                    {
                        for (int m = 0; m < sRate.Length; m++)
                        {
                            for (int q = 0; q < sAmounts.Length; q++)
                            {
                                for (int n = 0; n < sTax.Length; n++)
                                {
                                    for (int o = 0; o < sTotal.Length; o++)
                                    {
                                        for (int r = 0; r < sMaterialItemid.Length; r++)
                                        {
                                            for (int p = 0; p < sUnitName.Length; p++)
                                            {
                                                if (sUnitName[p] != "" && sUnitName[p] != " ")
                                                {
                                                    DataRow dRow = dt.NewRow();
                                                    dRow["MRIM_REMARKS"] = sRemarks[i];
                                                    dRow["MRIM_ITEM_NAME"] = sName[j];
                                                    dRow["RESTM_ITEM_QNTY"] = sQnty[k];
                                                    dRow["RESTM_ITEM_RATE"] = sRate[m];
                                                    dRow["AMOUNT"] = sAmounts[q];
                                                    dRow["RESTM_ITEM_TAX"] = sTax[n];
                                                    dRow["RESTM_ITEM_TOTAL"] = sTotal[o];
                                                    dRow["MD_NAME"] = sUnitName[p];
                                                    dRow["MRIM_ITEM_ID"] = sMaterialItemid[r];
                                                    dt.Rows.Add(dRow);
                                                    dt.AcceptChanges();
                                                }
                                                i++;
                                                j++;
                                                k++;
                                                m++;
                                                q++;
                                                n++;
                                                o++;
                                                r++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }




        protected void Page_UnLoad(object sender, EventArgs e)
        {


            if (strReport == "RepairerIncurred")
            {
                objrepcost.Close();
                objrepcost.Dispose();
            }
            if (strReport == "FeederBifurcationSO")
            {
                objcrpFeederBifurcationSO.Close();
                objcrpFeederBifurcationSO.Dispose();
            }

            if (strReport == "WorkOrderPreviewRepairer")
            {
                objrepwo.Close();
                objrepwo.Dispose();
            }

            if (strReport == "FailedOb")
            {
                crpfailob.Close();
                crpfailob.Dispose();
            }



            if (strReport == "WorkAwardReportmajor")
            {
                crpmajorwrkawrd.Close();
                crpmajorwrkawrd.Dispose();
            }

            if (strReport == "EnumReport")
            {
                crpOper.Close();
                crpOper.Dispose();
            }
            if (strReport == "DetailedField")
            {
                crpDetailedFieldReport.Close();
                crpDetailedFieldReport.Dispose();
            }
            if (strReport == "DetailedStore")
            {
                crpDetailedStoreReport.Close();
                crpDetailedStoreReport.Dispose();
            }
            if (strReport == "LocOperator")
            {
                crpLocOperator.Close();
                crpLocOperator.Dispose();
            }
            if (strReport == "FieldLoc")
            {
                crpLocField.Close();
                crpLocField.Dispose();
            }
            if (strReport == "StoreLoc")
            {
                crpLocStore.Close();
                crpLocStore.Dispose();
            }
            if (strReport == "Estimation")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "RefinedEstimationrepairer")
            {
                crpestimationrepairer.Close();
                crpestimationrepairer.Dispose();
            }
            if (strReport == "EstimationSO")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "WorkOrderPreview")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "WorkOrder")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "GatePass")
            {
                crpGatepassReport.Close();
                crpGatepassReport.Dispose();
            }
            if (strReport == "CRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RIReport")
            {
                crpRIReport.Close();
                crpRIReport.Dispose();
            }
            if (strReport == "RIAckReport")
            {
                crpRIAck.Close();
                crpRIAck.Dispose();
            }
            if (strReport == "IndentReport")
            {
                crpIndent.Close();
                crpIndent.Dispose();
            }
            if (strReport == "InvoiceReport")
            {
                crpInvoice.Close();
                crpInvoice.Dispose();
            }
            if (strReport == "EnhanceEstimation")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceEstimationSO")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceCRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "EnhanceIndentReport")
            {
                crpEnhanceIndent.Close();
                crpEnhanceIndent.Dispose();
            }
            if (strReport == "EnhanceInvoiceReport")
            {
                crpEnhanceInvoice.Close();
                crpEnhanceInvoice.Dispose();
            }


            if (strReport == "RolewiseCount")
            {
                crpRolewiseCount.Close();
                crpRolewiseCount.Dispose();
            }
            if (strReport == "ScrapGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "StoreGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapInvoice")
            {
                crpScrapInvoice.Close();
                crpScrapInvoice.Dispose();
            }
            if (strReport == "InterStoreInvoice")
            {
                crpStoreInvoice.Close();
                crpStoreInvoice.Dispose();
            }
            if (strReport == "RecieveDTR")
            {
                crpRecieveDTr.Close();
                crpRecieveDTr.Dispose();
            }
            if (strReport == "InterStoreIndent")
            {
                crpStoreIndent.Close();
                crpStoreIndent.Dispose();
            }
            if (strReport == "AbstractReport")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "AbstractRptTcFailed")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTrReportMake")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTCReportFeeder")
            {
                crpDTCRep.Close();
                crpDTCRep.Dispose();
            }
            if (strReport == "TCFail")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "CRAbstract")
            {
                crpCRAbstract.Close();
                crpCRAbstract.Dispose();
            }
            if (strReport == "WorkOderReg")
            {
                crpWoRegDetails.Close();
                crpWoRegDetails.Dispose();
            }
            if (strReport == "DTCAddDetails")
            {
                crpAddDTC.Close();
                crpAddDTC.Dispose();
            }
            if (strReport == "CRDetails")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RegAbstract")
            {

                objreg.Close();
                objreg.Dispose();
            }

            if (strReport == "Pending Analysis Report")
            {
                crpReprierPer.Close();
                crpReprierPer.Dispose();
            }

            if (strReport == "Delivered Analysis Report")
            {
                crpCompleted.Close();
                crpCompleted.Dispose();
            }

            if (strReport == "DTr make wise Reports")
            {
                objDtr.Close();
                objDtr.Dispose();
            }


            if (strReport == "DTr RepairerWise")
            {
                crpRepairerWise.Close();
                crpRepairerWise.Dispose();
            }
            if (strReport == "FailureAbstract")
            {
                crpFailureAbstract.Close();
                crpFailureAbstract.Dispose();
            }

            if (strReport == "FrequentTCFail")
            {
                crpDtcFailFrequent.Close();
                crpDtcFailFrequent.Dispose();
            }
            if (strReport == "NewDtcIndentReport")
            {
                crpnewdtcindent.Close();
                crpnewdtcindent.Dispose();
            }
            if (strReport == "NewDtcInvoiceReport")
            {
                crpnewdtcInvoice.Close();
                crpnewdtcInvoice.Dispose();
            }
            if (strReport == "NewDtcCR_CommReport")
            {
                crpnewDTCCR.Close();
                crpnewDTCCR.Dispose();
            }
            if (strReport == "WorkOrderNewDtcCommission")
            {
                crpNewdtcWo.Close();
                crpNewdtcWo.Dispose();
            }
            if (strReport == "RefinedEstimation")
            {
                crpestimation.Close();
                crpestimation.Dispose();
            }
            if (strReport == "RefinedEstimationSO")
            {
                crpestimationso.Close();
                crpestimationso.Dispose();
            }

            if (strReport == "RefinedEstimationSOrepairer")
            {
                crpestimationsorepairer.Close();
                crpestimationsorepairer.Dispose();
            }

            if (strReport == "PgrsDocketSO")
            {
                crpPgrsFailure.Close();
                crpPgrsFailure.Dispose();
            }
            if (strReport == "PgrsDocket")
            {
                crpPgrsFailure.Close();
                crpPgrsFailure.Dispose();
            }

            if (strReport == "RefinedpermanentEstimation")
            {
                crpperestimate.Close();
                crpperestimate.Dispose();
            }

            if (strReport == "RefinedpermanentEstimationSO")
            {
                crpperestimateso.Close();
                crpperestimateso.Dispose();
            }

            if (strReport == "WorkOrderPreviewpermanent")
            {
                crpworkper.Close();
                crpworkper.Dispose();
            }

            if (strReport == "IndentReportPermanent")
            {
                crpindentper.Close();
                crpindentper.Dispose();
            }

            if (strReport == "RIReportpermanent")
            {
                crpriper.Close();
                crpriper.Dispose();
            }

            if (strReport == "RIReportpermanentso")
            {
                crpriper.Close();
                crpriper.Dispose();
            }


            if (strReport == "RIAckReportpermanent")
            {
                crpRIAckper.Close();
                crpRIAckper.Dispose();
            }

            if (strReport == "CRReportpermanent")
            {
                crReportpermanent.Close();
                crReportpermanent.Dispose();
            }


            if (strReport == "BufferStock")
            {
                objcrpBufferStock.Close();
                objcrpBufferStock.Dispose();
            }

            if (strReport == "RepairerOb")
            {
                objrepairerob.Close();
                objrepairerob.Dispose();
            }

            if (strReport == "SchemeDetails")
            {
                objscheme.Close();
                objscheme.Dispose();
            }

            if (strReport == "Detailsview")
            {
                objdetail.Close();
                objdetail.Dispose();
            }

            if (strReport == "ReplRepaireddetails")
            {
                objrplrep.Close();
                objrplrep.Dispose();
            }

            if (strReport == "ReplacedDetails")
            {
                objreplabs.Close();
                objreplabs.Dispose();
            }

            if (strReport == "Detailsviewaddedabstract")
            {
                objaaddabst.Close();
                objaaddabst.Dispose();
            }


            if (strReport == "SchemeDetailscapacitywise")
            {
                objschemecap.Close();
                objschemecap.Dispose();
            }

            if (strReport == "repairerabstractyearwise")
            {
                objreyear.Close();
                objreyear.Dispose();
            }

            if (strReport == "DTRFailureAbstract")
            {
                objDTRFailureAbstract.Close();
                objDTRFailureAbstract.Dispose();
            }

            if (strReport == "BankStock")
            {
                objbank.Close();
                objbank.Dispose();
            }

            if (strReport == "EstimationCalc")
            {
                crpCalcEst.Close();
                crpCalcEst.Dispose();
            }

        }

        protected void crpPrint_Unload(object sender, EventArgs e)
        {
            if (strReport == "RepairerIncurred")
            {
                objrepcost.Close();
                objrepcost.Dispose();
            }
            if (strReport == "FeederBifurcation")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "FailedOb")
            {
                crpfailob.Close();
                crpfailob.Dispose();
            }
            if (strReport == "WorkOrderPreviewRepairer")
            {
                objrepwo.Close();
                objrepwo.Dispose();
            }

            if (strReport == "WorkAwardReportmajor")
            {
                crpmajorwrkawrd.Close();
                crpmajorwrkawrd.Dispose();
            }

            if (strReport == "EnumReport")
            {
                crpOper.Close();
                crpOper.Dispose();
            }
            if (strReport == "DetailedField")
            {
                crpDetailedFieldReport.Close();
                crpDetailedFieldReport.Dispose();
            }
            if (strReport == "DetailedStore")
            {
                crpDetailedStoreReport.Close();
                crpDetailedStoreReport.Dispose();
            }
            if (strReport == "LocOperator")
            {
                crpLocOperator.Close();
                crpLocOperator.Dispose();
            }
            if (strReport == "FieldLoc")
            {
                crpLocField.Close();
                crpLocField.Dispose();
            }
            if (strReport == "StoreLoc")
            {
                crpLocStore.Close();
                crpLocStore.Dispose();
            }
            if (strReport == "Estimation")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "RefinedEstimationrepairer")
            {
                crpestimationrepairer.Close();
                crpestimationrepairer.Dispose();
            }
            if (strReport == "EstimationSO")
            {
                crpEstimationReport.Close();
                crpEstimationReport.Dispose();
            }
            if (strReport == "WorkOrderPreview")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "WorkOrder")
            {
                crpWorkOrder.Close();
                crpWorkOrder.Dispose();
            }
            if (strReport == "GatePass")
            {
                crpGatepassReport.Close();
                crpGatepassReport.Dispose();
            }
            if (strReport == "CRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RIReport")
            {
                crpRIReport.Close();
                crpRIReport.Dispose();
            }
            if (strReport == "RIAckReport")
            {
                crpRIAck.Close();
                crpRIAck.Dispose();
            }
            if (strReport == "IndentReport")
            {
                crpIndent.Close();
                crpIndent.Dispose();
            }
            if (strReport == "InvoiceReport")
            {
                crpInvoice.Close();
                crpInvoice.Dispose();
            }
            if (strReport == "EnhanceEstimation")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceEstimationSO")
            {
                crpEnhanceEst.Close();
                crpEnhanceEst.Dispose();
            }
            if (strReport == "EnhanceCRReport")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "EnhanceIndentReport")
            {
                crpEnhanceIndent.Close();
                crpEnhanceIndent.Dispose();
            }
            if (strReport == "EnhanceInvoiceReport")
            {
                crpEnhanceInvoice.Close();
                crpEnhanceInvoice.Dispose();
            }
            if (strReport == "RepairGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "StoreGatepass")
            {
                crpRepairGatepass.Close();
                crpRepairGatepass.Dispose();
            }
            if (strReport == "ScrapInvoice")
            {
                crpScrapInvoice.Close();
                crpScrapInvoice.Dispose();
            }
            if (strReport == "InterStoreInvoice")
            {
                crpStoreInvoice.Close();
                crpStoreInvoice.Dispose();
            }
            if (strReport == "RecieveDTR")
            {
                crpRecieveDTr.Close();
                crpRecieveDTr.Dispose();
            }
            if (strReport == "InterStoreIndent")
            {
                crpStoreIndent.Close();
                crpStoreIndent.Dispose();
            }
            if (strReport == "AbstractReport")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "AbstractRptTcFailed")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTrReportMake")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "DTCReportFeeder")
            {
                crpDTCRep.Close();
                crpDTCRep.Dispose();
            }
            if (strReport == "TCFail")
            {
                objRep.Close();
                objRep.Dispose();
            }
            if (strReport == "CRAbstract")
            {
                crpCRAbstract.Close();
                crpCRAbstract.Dispose();
            }
            if (strReport == "WorkOderReg")
            {
                crpWoRegDetails.Close();
                crpWoRegDetails.Dispose();
            }
            if (strReport == "DTCAddDetails")
            {
                crpAddDTC.Close();
                crpAddDTC.Dispose();
            }
            if (strReport == "CRDetails")
            {
                crpReportDetails.Close();
                crpReportDetails.Dispose();
            }
            if (strReport == "RegAbstract")
            {

                objreg.Close();
                objreg.Dispose();

            }

            if (strReport == "Pending Analysis Report")
            {
                crpReprierPer.Close();
                crpReprierPer.Dispose();
            }

            if (strReport == "Delivered Analysis Report")
            {
                crpCompleted.Close();
                crpCompleted.Dispose();
            }

            if (strReport == "DTr make wise Reports")
            {
                objDtr.Close();
                objDtr.Dispose();
            }

            if (strReport == "DTr RepairerWise")
            {
                crpRepairerWise.Close();
                crpRepairerWise.Dispose();
            }
            if (strReport == "FailureAbstract")
            {
                crpFailureAbstract.Close();
                crpFailureAbstract.Dispose();
            }
            if (strReport == "FrequentTCFail")
            {
                crpDtcFailFrequent.Close();
                crpDtcFailFrequent.Dispose();
            }
            if (strReport == "NewDtcIndentReport")
            {
                crpnewdtcindent.Close();
                crpnewdtcindent.Dispose();
            }
            if (strReport == "NewDtcInvoiceReport")
            {
                crpnewdtcInvoice.Close();
                crpnewdtcInvoice.Dispose();
            }
            if (strReport == "NewDtcCR_CommReport")
            {
                crpnewDTCCR.Close();
                crpnewDTCCR.Dispose();
            }
            if (strReport == "WorkOrderNewDtcCommission")
            {
                crpNewdtcWo.Close();
                crpNewdtcWo.Dispose();
            }
            if (strReport == "RefinedEstimation")
            {
                crpestimation.Close();
                crpestimation.Dispose();
            }
            if (strReport == "RefinedEstimationSO")
            {
                crpestimationso.Close();
                crpestimationso.Dispose();
            }

            if (strReport == "RefinedEstimationSOrepairer")
            {
                crpestimationsorepairer.Close();
                crpestimationsorepairer.Dispose();
            }

            if (strReport == "PgrsDocketSO")
            {
                crpPgrsFailure.Close();
                crpPgrsFailure.Dispose();
            }
            if (strReport == "PgrsDocket")
            {
                crpPgrsFailure.Close();
                crpPgrsFailure.Dispose();
            }

            if (strReport == "RefinedpermanentEstimation")
            {
                crpperestimate.Close();
                crpperestimate.Dispose();
            }

            if (strReport == "RefinedpermanentEstimationSO")
            {
                crpperestimateso.Close();
                crpperestimateso.Dispose();
            }

            if (strReport == "WorkOrderPreviewpermanent")
            {
                crpworkper.Close();
                crpworkper.Dispose();
            }

            if (strReport == "IndentReportPermanent")
            {
                crpindentper.Close();
                crpindentper.Dispose();
            }

            if (strReport == "RIReportpermanent")
            {
                crpriper.Close();
                crpriper.Dispose();
            }

            if (strReport == "RIReportpermanentso")
            {
                crpriper.Close();
                crpriper.Dispose();
            }


            if (strReport == "RIAckReportpermanent")
            {
                crpRIAckper.Close();
                crpRIAckper.Dispose();
            }

            if (strReport == "CRReportpermanent")
            {
                crReportpermanent.Close();
                crReportpermanent.Dispose();
            }

            if (strReport == "BufferStock")
            {
                objcrpBufferStock.Close();
                objcrpBufferStock.Dispose();
            }
            if (strReport == "RepairerOb")
            {
                objrepairerob.Close();
                objrepairerob.Dispose();
            }

            if (strReport == "SchemeDetails")
            {
                objscheme.Close();
                objscheme.Dispose();
            }

            if (strReport == "Detailsview")
            {
                objdetail.Close();
                objdetail.Dispose();
            }

            if (strReport == "ReplRepaireddetails")
            {
                objrplrep.Close();
                objrplrep.Dispose();
            }

            if (strReport == "ReplacedDetails")
            {
                objreplabs.Close();
                objreplabs.Dispose();
            }

            if (strReport == "Detailsviewaddedabstract")
            {
                objaaddabst.Close();
                objaaddabst.Dispose();
            }

            if (strReport == "SchemeDetailscapacitywise")
            {
                objschemecap.Close();
                objschemecap.Dispose();
            }

            if (strReport == "repairerabstractyearwise")
            {
                objreyear.Close();
                objreyear.Dispose();
            }

            if (strReport == "DTRFailureAbstract")
            {
                objDTRFailureAbstract.Close();
                objDTRFailureAbstract.Dispose();
            }

            if (strReport == "BankStock")
            {
                objbank.Close();
                objbank.Dispose();
            }

            if (strReport == "EstimationCalc")
            {
                crpCalcEst.Close();
                crpCalcEst.Dispose();
            }
            if (strReport == "FeederBifurcation")
            {
                crpobjFeederBifurcation.Close();
                crpobjFeederBifurcation.Dispose();
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
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}