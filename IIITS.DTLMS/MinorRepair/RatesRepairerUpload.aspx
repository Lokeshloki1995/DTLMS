<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RatesRepairerUpload.aspx.cs" Inherits="IIITS.DTLMS.MinorRepair.RatesRepairerUpload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style>
        .widget-body {
            padding: 20px 15px;
        }
    </style>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">

                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Repairer Rates Upload
                    </h3>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>


                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Repairer - Division Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span6">
                                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                                        OnClientClick="javascript:window.location.href='ViewEstimateRate.aspx'; return false;"
                                        CssClass="btn btn-primary" /><br />
                                </div>
                            </div>
                            <div style="float: left" class="control-group">
                                <label class="control-label">Repairer<span class="Mandotary"> *</span></label>
                                <div class="controls">
                                    <div class="input-append">
                                        <asp:DropDownList ID="cmbRepairer" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div style="float: left; margin-left:10%;margin-right: 10%;"class="control-group">
                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                <div class="controls">
                                    <div class="input-append">
                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="control-group" id="exceldownload">
                             
                            </div>




                        </div>
                    </div>

                    <div class="form-horizontal" align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

