<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DtcDtrSwapping2.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.DtcDtrSwapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        DTC DTR SWAPPING
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
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
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>DTC DTR Swapping</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTC Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDtcCode" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnDtcSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                            OnClick="btnDtcSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5" runat="server" id="dvOld">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTR Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDtrCode" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnDtrSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                            OnClick="btnDtrSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1">
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="form-horizontal" align="center">
                                        <div class="span5">
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdLoadDetails" runat="server" Text="Load" CssClass="btn btn-primary"
                                                OnClientClick="javascript:return ValidateMyForm1()" />
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-primary" /><br />
                                        </div>
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>DTC Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <asp:GridView ID="grdDtcDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("dt_code") %>' Style="word-break: break-all;"
                                                            Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="DTC Name" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                            Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>DTR Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <asp:GridView ID="grdDtrDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="TC Code" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("Tc_code") %>' Style="word-break: break-all;"
                                                            Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="TC Name" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("Tc_Name") %>' Style="word-break: break-all;"
                                                            Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="space20">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
