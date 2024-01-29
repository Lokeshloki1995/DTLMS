<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DTCAddDetails.aspx.cs" Inherits="IIITS.DTLMS.Reports.DTCAddDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
             <style>
             i.fa.fa-exclamation-circle {
                 font-size: 23px !important;
                 color: white !important;
                 padding: 0px 0px 0px 3px !important;
             }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                     DTC Added Report
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
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DTCAddDetails.aspx'; return false;"
                        CssClass="btn btn-danger" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>DTC Added Report
                                   <a style="float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px;padding:0px 0px 0px 3px!important"></i></a>
                            </h4>
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
                                             <div style="padding-left: 108px;">
                                            <asp:RadioButtonList ID="rdbReportType" runat="server" CssClass="radio" RepeatDirection="Horizontal"
                                                AutoPostBack="true" Width="679px">
                                                <asp:ListItem Value="1" Selected="True">Division Wise</asp:ListItem>
                                                <asp:ListItem Value="2">SUB Division Wise</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                            <div class="space20">
                                        </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    From Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Select Feeder Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeederType" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                             <div class="space20">
                                        </div>
                                            <div class="space20">
                                        </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    To Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Select Capacity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        
                                       
                                    </div>
                                </div><br />
                                            <div class="row-fluid">
                                          <div class="span12">
                                      <div class="text-center" >
                             
                                
                                         <asp:Button ID="cmdGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" TabIndex="10" OnClick="cmdGenerate_Click" />
                                 
                                   
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" TabIndex="11" OnClick="cmdReset_Click" />
                            
                                    
                                          <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info" TabIndex="12" OnClick="Export_click" />
                                         
                       
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
                            </div>
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
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* This Report Will Display DTC Records Added based on FromDate, ToDate, Feeder Type, Capacity</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Select Radio button Division Wise or Sub Division Wise</p>

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
