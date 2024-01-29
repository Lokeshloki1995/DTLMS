<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FeederViewApp.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.FeederViewApp" %>

<%@ Register Src="/ReportFilterControl.ascx" TagName="ReportFilterControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Feeder View
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

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Feeder View</h4>

                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span2">
                                    <asp:Button ID="cmdNew" runat="server" Text="New Feeder"
                                        CssClass="btn btn-primary" OnClick="cmdNew_Click" />
                                </div>

                            </div>


                            <div class="widget-body form">

                                <uc1:ReportFilterControl ID="ReportFilterControl1" runat="server" />
                                <div class="widget-body form">
                                    <div class="form-horizontal">
                                        <div class="row-fluid">
                                            <div class="span3"></div>
                                            <div class="span5">
                                                <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                                    OnClick="cmdLoad_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:GridView ID="grdFeeder"
                                    AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server"
                                    ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                                    AllowPaging="true" OnPageIndexChanging="grdFeeder_PageIndexChanging"
                                    OnRowCommand="grdFeeder_RowCommand">

                                    <Columns>

                                        <asp:TemplateField AccessibleHeaderText="SD_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFeederId" runat="server" Text='<%# Bind("FD_FEEDER_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="ST_NAME" HeaderText="Station Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStation" runat="server" Text='<%# Bind("ST_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtStation" runat="server" CssClass="input_textSearch" Width="150px" placeholder="Enter Station Name" ToolTip="Enter Station Name to Search"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="FD_FEEDER_NAME" HeaderText="Feeder Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCorpPhone" runat="server" Text='<%# Bind("FD_FEEDER_NAME") %>' Width="150px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtFeederName" runat="server" CssClass="input_textSearch" Width="150px" placeholder="Enter Feeder Name" ToolTip="Enter Feeder Name to Search"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="FD_FEEDER_CODE" HeaderText="Feeder Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCorpName2" runat="server" Text='<%# Bind("FD_FEEDER_CODE") %>' Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel3" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtFeederCode" runat="server" CssClass="input_textSearch" Width="150px" placeholder="Enter Feeder Code" ToolTip="Enter Feeder Code to Search"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Width="200px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px"
                                                    ToolTip="Search" TabIndex="9" CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="FD_TYPE" HeaderText="Feeder Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("FD_TYPE") %>' Width="150px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                        Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                                </asp:GridView>

                                <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                            </div>

                        </div>
                    </div>
                </div>

                <!-- END FORM-->

                <!-- END PAGE CONTENT-->
            </div>



        </div>



    </div>
</asp:Content>
