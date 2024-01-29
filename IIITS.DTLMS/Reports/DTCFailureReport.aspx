<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DTCFailureReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.DTCFailureReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .radio {
        }

        input#ContentPlaceHolder1_txtFromDate {
            z-index: inherit !important;
        }
            if (cmbFeederName1.SelectedIndex >= 0)
            {
                objReport.sFeeder = cmbFeederName1.SelectedValue;
            }
        .modal.fade.in {
            top: 10%;
            z-index: 9999;
        }

        .form-search input, .form-inline input, .form-horizontal input, .form-search textarea, .form-inline textarea, .form-horizontal textarea, .form-search select, .form-inline select, .form-horizontal select, .form-search .help-inline, .form-inline .help-inline, .form-horizontal .help-inline, .form-search .uneditable-input, .form-inline .uneditable-input, .form-horizontal .uneditable-input, .form-search .input-prepend, .form-inline .input-prepend, .form-horizontal .input-prepend, .form-search .input-append, .form-inline .input-append, .form-horizontal .input-append {
            z-index: inherit !important;
        }

        select#ContentPlaceHolder1_cmbZone, select#ContentPlaceHolder1_cmbCircle {
            z-index: 9999 !important;
        }

        #header.navbar {
            min-width: 0px;
            z-index: 10000;
        }

        i.fa.fa-exclamation-circle {
            font-size: 23px !important;
            color: white !important;
            padding: 0px 0px 0px 3px !important;
        }
    </style>
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= cmbReportType.ClientID %>').value.trim() == "1") {
                alert('Please Select Report Type')
                document.getElementById('<%= cmbReportType.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbStage.ClientID %>').value.trim() == "0") {
                alert('Please Select Stage')
                document.getElementById('<%= cmbStage.ClientID %>').focus()
                return false
            }
            <%--if (document.getElementById('<%= cmbCoil.ClientID %>').value.trim() == "0") { // Removed as mentioned in Jira Id : DTLMS-1598
                alert('Please Select Failure Type')
                document.getElementById('<%= cmbCoil.ClientID %>').focus()
                return false
            }--%>
        }



        <%--function ValidateMyForm2() {
            if (document.getElementById('<%= cmbCoil1.ClientID %>').value.trim() == "0") {  // Removed as mentioned in Jira Id : DTLMS-1598
                alert('Please Select Failure Type')
                document.getElementById('<%= cmbCoil1.ClientID %>').focus()
                return false
            }
        }--%>
        <%-- function ValidateMyForm3() {
           if (document.getElementById('<%= cmbCoil2.ClientID %>').value.trim() == "0") {  // Removed as mentioned in Jira Id : DTLMS-1598
                alert('Please Select Failure Type')
                document.getElementById('<%= cmbCoil2.ClientID %>').focus()
                return false
            }
        }--%>
       <%-- function ValidateMyForm1() {        // Removed as mentioned in Jira Id : DTLMS-1598
            if (document.getElementById('<%= txtFailMonth.ClientID %>').value.trim() == "0") {
                alert('Please Select Month')
                document.getElementById('<%= txtFailMonth.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbAbstract_ReportType.ClientID %>').value.trim() == "0") {
                alert('Please Select Failure Abstract Report Type')
                document.getElementById('<%= cmbAbstract_ReportType.ClientID %>').focus()
                return false
            }
        }--%>

    </script>

    <script type="text/javascript" language="javascript">
        function onCalendarShown(sender, args) {
            sender._switchMode("years", true);
            sender._switchMode("months", true);

            //sender._switchMode("day", false);            
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="">
            <!-- BEGIN PAGE HEADER-->
            <div class="">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        <%--        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>Transformer Center Failure Report
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <!--<div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClick="Back_Click" 
                        CssClass="btn btn-primary" />
                </div>-->
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Pending / Completed Failure Details
                                  <a style="float: right!important" href="#" data-toggle="modal" data-target="#myModal1" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px; padding: 0px 0px 0px 3px!important"></i></a>
                            </h4>
                            <%--<span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>--%>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="2"
                                                            OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTr Make</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:Label ID="lblFailuretype" class="control-label" runat="server" Text="Failure Type"></asp:Label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFailureType" runat="server" TabIndex="11">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:Label ID="Label3" class="control-label" runat="server">Report Type <span class="Mandotary">*</span></asp:Label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbReportType" runat="server" TabIndex="15">
                                                            <asp:ListItem Value="1">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="2">Failure DateWise</asp:ListItem>
                                                            <asp:ListItem Value="3">DateWise at each Stage </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
        
                                         <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true" TabIndex="4"
                                                            OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    O & M Section</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOMSection" runat="server" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Feeder Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeederName1" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Seclect Capacity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity1" runat="server" TabIndex="6">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Gurantee Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGrntyType" runat="server" TabIndex="13">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <%--START // Removed as mentioned in Jira Id : DTLMS-1598 --%>
                                           <%-- <div class="control-group">
                                                <asp:Label ID="Label7" class="control-label" runat="server"> Failure Type<span class="Mandotary">*</span></asp:Label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCoil" runat="server" TabIndex="17" AutoPostBack="true" OnSelectedIndexChanged="cmbCoil_SelectedIndexChanged">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Minor Repair</asp:ListItem>
                                                            <asp:ListItem Value="2">Major Repair</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <%--END--%>

                                            <div class="control-group">
                                                <asp:Label ID="Label4" class="control-label" runat="server">Select Stage<span class="Mandotary">*</span></asp:Label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStage" runat="server" TabIndex="10">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Total Failed Tc</asp:ListItem>
                                                            <asp:ListItem Value="2">Estimation Pending</asp:ListItem>
                                                            <asp:ListItem Value="3">WorkOrder Pending</asp:ListItem>
                                                            <%--<asp:ListItem Value="4" runat="server" ID="majorlist4" >Indent Pending</asp:ListItem>--%>
                                                            <asp:ListItem Value="5" runat="server" ID="majorlist5">Invoice Pending</asp:ListItem>
                                                            <%--<asp:ListItem Value="6" runat="server" ID="majorlist6" >Decommission Pending</asp:ListItem>--%>
                                                            <asp:ListItem Value="7" runat="server" ID="majorlist7">Return Invoice Pending</asp:ListItem>
                                                            <asp:ListItem Value="8" runat="server" ID="majorlist8">CR Pending</asp:ListItem>
                                                            <asp:ListItem Value="9" runat="server" ID="majorlist9">CR Completed</asp:ListItem>
                                                            <asp:ListItem Value="10" runat="server" ID="majorlist10">All</asp:ListItem>
                                                            <asp:ListItem Value="11" runat="server" ID="minorlist11" Enabled="false">Receive DTR Pending</asp:ListItem>
                                                            <asp:ListItem Value="12" runat="server" ID="minorlist12" Enabled="false">commission Pending</asp:ListItem>
                                                            <asp:ListItem Value="13" runat="server" ID="minorlist13" Enabled="false">commission Completed</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">

                            <div class="text-center">

                                <asp:Button ID="cmdGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm()"
                                    CssClass="btn btn-primary" TabIndex="10" OnClick="cmdGenerate_Click" />

                                <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                    TabIndex="11" OnClick="cmdReset_Click" /><%--<br />--%>


                                <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm()"
                                    TabIndex="12" OnClick="Export_click" /><br />



                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>

                        </div>
                        <br />
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->
        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->

       <%--  Hiding as mentioned in Jira Id : DTLMS-1598.   End  --%>
       
     <div class="row-fluid" id="dtrworkoderregdetails"  style="display:none">
            <div class="span12">
                <%-- BEGIN SAMPLE FORMPORTLET--%>
                  <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
                <div class="widget blue" >

                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DTR WORK ORDER REG DETAILS

                        </h4>
                        <a href="#" data-toggle="modal" data-target="#myModal2" title="Click For "><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white; padding: 4px 4px 3px 5px!important;"></i></a>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                           <%-- BEGIN FORM--%>
                          <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">
                                                Zone
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbZone1" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbZone1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDiv2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbDiv1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                      <div class="control-group">
                                            <label class="control-label">
                                                From Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate2" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFromDate2" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDiv2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbSubDiv1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                O & M Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSection" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Feeder Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFeederName2" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" style="display:none">
                                            <asp:Label ID="Label8" class="control-label" runat="server"> Coil Type<span class="Mandotary">*</span></asp:Label>

                                            <div class="controls" >
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCoil1" runat="server" TabIndex="1">
                                                        <%--<asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Minor Coil</asp:ListItem>--%>
                                                        <asp:ListItem Value="2">Major Coil</asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">  To Date </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate2" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate2" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center">

                                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm2()"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnGenerate_Click" />

                                        <asp:Button ID="BtnWOReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="BtnWOReset_Click" />

                                        <asp:Button ID="Button2" runat="server" Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm2()"
                                            TabIndex="12" OnClick="Export_clickworkorder" /><br />
                                        <br />

                                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <%-- END FORM--%>
                        </div>
                    </div>
                </div>
                <%-- END SAMPLE FORM PORTLET--%>
            </div>
        </div>
       
        <div class="row-fluid" style="display: none">
            <div class="span12">
               <%-- BEGIN SAMPLE FORMPORTLET--%>
                     <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Failure Abstract</h4>
                        <a href="#" data-toggle="modal" data-target="#myModal3" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                          <%--   BEGIN FORM--%>
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Select Month <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFailMonth" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="dtPickerFromDate" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFailMonth" DefaultView="Months" Format="yyyy-MM" OnClientShown="onCalendarShown">
                                                    </ajax:CalendarExtender>
                                                  <%--<ajax:CalendarExtender  runat="server" CssClass="calendarClass"  Enabled="true" Format="MMM-yy" 
                                                      PopupButtonID="imgcalendarFileDate" TargetControlID="txtFailMonth" DefaultView="Months">
                                                            </asp:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <asp:Label ID="lblReportType" class="control-label" runat="server">Select Report Type<span class="Mandotary">*</span></asp:Label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbAbstract_ReportType" runat="server" TabIndex="1">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Till Month Transaction</asp:ListItem>
                                                        <asp:ListItem Value="2"> On Month Transaction</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" align="center">

                                        <asp:Button ID="btnFailGenerate" runat="server" Text="Generate"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnFailGenerate_Click" OnClientClick="javascript:return ValidateMyForm1()" />

                                        <asp:Button ID="btnFailreset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="btnFailreset_Click" /><br />
                                        <br />

                                        <asp:Button ID="Button5" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                            TabIndex="12" OnClick="Export_clickFailure" /><br />
                                        <br />

                                        <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                           <%-- END FORM--%>
                        </div>
                    </div>
                </div>
                <%-- END SAMPLE FORM PORTLET--%>
            </div>
        </div>        
       
        <div class="row-fluid" >
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
               
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>FREQUENTLY FAILING DTC'S</h4>
                        <a href="#" data-toggle="modal" data-target="#myModal4" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Zone
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbZone2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbZone2_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDivision3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbDivision3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <asp:Label ID="Label5" class="control-label" runat="server" Text="Failure Type"></asp:Label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFailureType1" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                From Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate3" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFromDate3" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDivision3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbSubDivision3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                O & M Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbOMSection3" runat="server" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="cmbSection3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Feeder Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFeederName3" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTR Code <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTRCode" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Gurantee Type</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbguranteetype1" runat="server" TabIndex="1">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                        <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>

                                       <%-- <div class="control-group">
                                            <asp:Label ID="Label9" class="control-label" runat="server"> Coil Type<span class="Mandotary">*</span></asp:Label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCoil2" runat="server" TabIndex="1">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Minor Coil</asp:ListItem>
                                                        <asp:ListItem Value="2">Major Coil</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>--%>

                                        <div class="control-group">
                                            <label class="control-label">
                                                To Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate3" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender6" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate3" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" align="center">

                                        <asp:Button ID="Button3" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm3()"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnGenerate3_Click" />

                                        <asp:Button ID="Button4" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="BtnReset3_Click" /><%--<br />--%>
                                        <%--  <br />--%>

                                        <asp:Button ID="Button6" runat="server" Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm3()"
                                            TabIndex="12" OnClick="Export_click3" /><br />
                                        <br />

                                        <asp:Label ID="Label6" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Form include 4 Reports
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Click <span style="font-size: 20px; font-weight: bold">!</span> in each Section to know more
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

    <!-- MODAL-->
    <div class="modal fade" id="myModal1" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* This Report Will Display Pending / Completed Failure Details
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You Can Take Report by selecting FromDate or ToDate or Make or FailureType 
                        or Guarentee Type or Capacity or  Circle or Division or Subdivision or Section
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* But Report Type And Select Stage is Mandatory
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* In Select Stage you will find Different Stages , you can select any stage and can view the report.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* If you Select FailureDateWise in ReportType then It will Display Records that 
                        had got Failed between provided from date and to date and also completed the different stage in that date as well 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* If you Select DateWiseAt Eact Stage in ReportType then It will Display Records
                         between provided from date and to date in the different stage , it may be Failure in any date 
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

    <!-- MODAL-->
    <div class="modal fade" id="myModal2" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* This Report Will Display Pending Failure Details
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* It may be in Any stage but Work order has to be complete
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

    <!-- MODAL-->
    <div class="modal fade" id="myModal3" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* This Report Will Display DTC Failure Abstract for Selected Month
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You must Select any Month
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You must Select Report Type As well, there is 2 types in it
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* 1st Till Month Transaction will display the failure records till Selected month
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* 2nd On Month Transaction will display the failure records for only Selected month
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

    <!-- MODAL-->
    <div class="modal fade" id="myModal4" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* This Report Will Display Frequently Failed DTC's
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* In this youc can get report by selecting FromDate or ToDate or dtc code, tc code
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>
