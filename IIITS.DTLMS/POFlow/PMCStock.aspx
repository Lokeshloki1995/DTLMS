<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PMCStock.aspx.cs" Inherits="IIITS.DTLMS.POFlow.PMCStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <div style="float: left;">
                    <div class="span12">
                        <h3 class="page-title">Stock Details</h3>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Stock Details</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
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
                                            <label class="control-label">From Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="txtFromDate_CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Zone</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="11" ReadOnly="true" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" TabIndex="11" ReadOnly="true" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">TO Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Circle</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="11" ReadOnly="true" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Store</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbStore" runat="server" AutoPostBack="true" TabIndex="11" ReadOnly="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="col-md-4 text-center" style="margin-bottom: 15px !important; margin-top: 20px !important;">
                                        <asp:Button ID="btnExport" runat="server" Text="Generate Report" CssClass="btn btn-info tab-button" OnClick="Export_Click" />
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-danger tab-button" OnClick="Reset_Click" />
                                    </div>
                                    <div class="span7"></div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
