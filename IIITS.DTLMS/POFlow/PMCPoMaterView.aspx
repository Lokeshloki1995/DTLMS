<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PMCPoMaterView.aspx.cs" Inherits="IIITS.DTLMS.POFlow.PMCPoMaterView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }
        .descending th a {
            background: url(img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Purchase Order View
                    </h3>
                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                            <h4><i class="icon-reorder"></i>Purchase Order View</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

                        <div class="widget-body">
                            <div style="float: right">
                                <div class="span5">
                                    <asp:Button ID="cmdNew" runat="server" Text="New PO"
                                        CssClass="btn btn-success" OnClick="cmdNew_Click" /><br />
                                </div>

                                <div style="display: flex!important; padding-left: 0.5cm" class="span5">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_clickPOMaster" /><br />
                                </div>

                            </div>
                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdPoMasterView"
                                AutoGenerateColumns="false" PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdPoMasterView_PageIndexChanging"
                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                OnRowCommand="grdPoMasterView_RowCommand" ShowFooter="True" OnSorting="grdPOmaster_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PMC_PO_ID" HeaderText="PO Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoId" runat="server" Text='<%# Bind("PMC_PO_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DM_ID" HeaderText="dm Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldmId" runat="server" Text='<%# Bind("DM_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DM_NUMBER" HeaderText="DWA No" SortExpression="DM_NUMBER">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDmNo" runat="server" Text='<%# Bind("DM_NUMBER") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                <asp:TextBox ID="txtDWANumber" runat="server" Width="150px" placeholder="Enter DWA Number" autocomplete="off" ToolTip="Enter DWA Number to Search"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DM_DATE" HeaderText="DWA Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDWADate" runat="server" Text='<%# Bind("DM_DATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                        </FooterTemplate>--%>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DM_EXTENDED_UPTO" HeaderText="DWA Expiry Date" SortExpression="DM_EXTENDED_UPTO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDWAExpityDate" runat="server" Text='<%# Bind("DM_EXTENDED_UPTO") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DM_AMOUNT" HeaderText="DWA Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDWAAmount" runat="server" Text='<%# Bind("DM_AMOUNT") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PMC_PO_NO" HeaderText="PO No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("PMC_PO_NO") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel21" runat="server" DefaultButton="btnSearch">
                                                <asp:TextBox ID="txtPONumberSerch" runat="server" Width="150px" placeholder="Enter PO Number" autocomplete="off" ToolTip="Enter PO Number to Search"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PMC_PO_DATE" HeaderText="PO Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("PMC_PO_DATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PMC_PO_AMOUNT" HeaderText="PO Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoAmount" runat="server" Text='<%# Bind("PMC_PO_AMOUNT") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("TS_NAME") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PMC_PB_QUANTITY" HeaderText="PO Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoQuantity" runat="server" Text='<%# Bind("PMC_PB_QUANTITY") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                    CommandName="Submit" Width="12px" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>


            <!-- END PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All PMC PO Details,Existing PMC PO Details Can be Edited and New PO can Be Create.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit PO Details Click On Edit Button Enter Details And Click On Update Button To Update the Details
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Create New PO Click On New Button
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
