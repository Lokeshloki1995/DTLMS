<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailureReplacementTimeLine.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.FailureReplacementTimeLine" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
      <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>  --%>
    <script language="javascript" type="text/javascript">
        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "img/Manual/Expand.png";
            } else {
                div.style.display = "none";
                img.src = "img/Manual/Expand.png";
            }
        }
        function divexpandcollapseChild(divname) {
            // alert("v");
            var div1 = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div1.style.display == "none") {
                div1.style.display = "inline";
                img.src = "img/Manual/collapse.png";
            } else {
                div1.style.display = "none";
                img.src = "img/Manual/Expand.png";
            }
        }

    </script>
    <style type="text/css">
        .hidden-column {
            display: none;
        }

        .table tr th {
            background-color: white;
            color: darkblue;
            text-align: center;
        }

        .table tr td {
            word-break: keep-all;
        }

        .form-horizontal .control-label {
            text-align: start !important;
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
                    <h3 class="page-title">Failure Replacement TimeLine
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Location
                      <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px; padding: 0px 0px 0px 0px!important"></i></a>
                            </h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div>
                                            <div class="span1"></div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        From Date
                                                    </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="1"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                            </ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        To Date
                                                    </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="2"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                            </ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--<div class="span1">
                              <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                 OnClick="cmdLoad_Click" visible="false" />
                           </div>--%>
                                        <div class="space20"></div>
                                        <div class="space20"></div>
                                        <div class="text-center">
                                            <asp:Button ID="Button1" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                                OnClick="cmdExport_Click" />
                                            <br />
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span7"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
                        <i class="fa fa-info-circle"></i>* This Report Will Display Pending And Completed invoice for DTC Failure
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* It will show Pending And Completed counts within 1 day, between 1 to 7 days, 
                  between 7 to 15 days, between 15 to 30 days, above 30 days 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* If you select from date and to date you will get only records between from date and to date
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Completed Indicate Invoice done and pending with RI or CR
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Pending Indicate Pending For invoice
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* If you click on circle view button you will get expand and will display Division Views and so on
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
