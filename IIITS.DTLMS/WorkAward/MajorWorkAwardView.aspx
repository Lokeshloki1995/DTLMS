<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MajorWorkAwardView.aspx.cs" Inherits="IIITS.DTLMS.WorkAward.MajorWorkAwardView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Major Work Award View
                </h3><%--                <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                                       <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>

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
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Major Work Award View</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div style="float: right">
                            <div class="span6">
                                <asp:Button ID="cmdNew" runat="server" Text="New WorkAward" CssClass="btn btn-success" OnClick="cmdNew_Click"
                                   /><br />
                            </div>
                            <div class="span1">
                                <asp:Button ID="cmdexport" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" /><br />
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <!-- END FORM-->
                        <asp:GridView ID="grdWorkAward" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="True" AllowSorting="true" CssClass="table table-striped table-bordered table-advance table-hover" runat="server" OnPageIndexChanging="grdWorkAward_PageIndexChanging" OnRowCommand="grdWorkAward_RowCommand" OnSorting="grdWorkAward_Sorting" >
                            <Columns>
                                <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RWAO_ID" HeaderText="Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("RWAO_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RWOA_NO" HeaderText="WORK AWARD NO" SortExpression="WOA_NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblwoano" runat="server" Text='<%# Bind("RWOA_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:TextBox ID="txtwoano" runat="server" CssClass="input_textSearch" Width="150px"
                                            placeholder="Enter WA Number" ToolTip="Enter WA No to Search"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="RWOA_TR_ID" HeaderText="REPAIRER" Visible="true" SortExpression="WOA_TR_ID">

                                    <ItemTemplate>
                                        <asp:Label ID="lblrepairer" runat="server" Text='<%# Bind("RWOA_TR_ID") %>' Style="word-break: break-all;"></asp:Label>
                                    </ItemTemplate>
                                   <FooterTemplate>
                                        <asp:TextBox ID="txtRepairer" runat="server" CssClass="input_textSearch" Width="150px"
                                            placeholder="Enter Repeirer" ToolTip="Enter Repairer to Search"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RWOA_AMOUNT" HeaderText="AMOUNT"
                                    Visible="true" SortExpression="RWOA_AMOUNT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("RWOA_AMOUNT") %>' Style="word-break: break-all;"></asp:Label>
                                    </ItemTemplate>
                                      <FooterTemplate>
                                        <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">

                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                        </asp:Panel>
                                    </FooterTemplate>
                                   
                                </asp:TemplateField>
                                

                                <asp:TemplateField AccessibleHeaderText="RWOA_DATE" HeaderText="WORK AWARD DATE" Visible="true" SortExpression="WOA_DATE">

                                    <ItemTemplate>
                                        <asp:Label ID="lbldate" runat="server" Text='<%# Bind("RWOA_DATE") %>' Style="word-break: break-all;"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                Width="12px" OnClick="imgBtnEdit_Click" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                        <div class="span7">
                        </div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
