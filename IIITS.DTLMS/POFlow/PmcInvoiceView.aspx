<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PMCInvoiceView.aspx.cs" Inherits="IIITS.DTLMS.POFlow.PMCInvoiceView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            overflow: scroll;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(/img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .widget-body {
            padding: 10px 10px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <div style="float: left;">
                    <div class="span12">
                        <h3 class="page-title">Invoice View Details</h3>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Invoice View</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <div class="widget-body">
                                <div style="float: right!important; margin-bottom: 15px !important; margin-top: -10px !important;">

                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info tab-button" OnClick="Export_ClickFailureEntry" />



                                </div>
                                <div style="overflow-x: auto!important; width: 100%!important;" class="">
                                    <asp:GridView ID="grdPmc" 
                                        AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="True" 
                                        PageSize="10" AllowPaging="true"
                                        EmptyDataText="No Records Found"
                                        OnRowCommand="grdPmc_RowCommand"
                                        OnPageIndexChanging="grdPmc_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-advance table-hover" 
                                        ShowFooter="true"
                                        runat="server">
                                        <PagerStyle CssClass="gvPagerCss" />
                                        <HeaderStyle CssClass="both" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PMC_ID" HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Bind("PMC_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PMC_PI_ID" HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentId" runat="server" Text='<%# Bind("PMC_PI_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PI_DTCCODE" HeaderText="DTC Code" SortExpression="PI_DTCCODE">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CommandName="CreateNew" Width="80px" ID="lnkCreateDTC">
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("PI_DTCCODE") %>'></asp:Label>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtsDtcCode" runat="server" Width="110px" placeholder="Enter DTC Code" ToolTip="Enter DTC Code"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PI_TC_CODE" HeaderText="DTr Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("PI_TC_CODE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtstcCode" runat="server" Width="110px" placeholder="Enter DTr Code" ToolTip="Enter DTr Code"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PI_CAPACITY" HeaderText="Capacity(in KVA)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("PI_CAPACITY") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel3" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtCapacity" runat="server" Width="110px" placeholder="Enter Capacity(in KVA)" ToolTip="Enter Capacity(in KVA)"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TPIE_PO_NO" HeaderText="PO No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPono" runat="server" Text='<%# Bind("TPIE_PO_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel4" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtPOno" runat="server" Width="110px" placeholder="Enter PO No" ToolTip="Enter PO No"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TPIE_INDENT_NO" HeaderText="Indent No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdentNo" runat="server" Text='<%# Bind("TPIE_INDENT_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel5" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtIndentno" runat="server" Width="110px" placeholder="Enter Indent No" ToolTip="Enter Indent No"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="TPIE_INDENT_DATE" HeaderText="Indent Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdentdate" runat="server" Text='<%# Bind("TPIE_INDENT_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PMC_INVOICE_NO" HeaderText="Invoice No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("PMC_INVOICE_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel6" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtsInvoiceNo" runat="server" Width="110px" placeholder="Enter Invoice No" ToolTip="Enter Invoice No"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="PMC_INVOICE_DATE" HeaderText="Invoice Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("PMC_INVOICE_DATE") %>'></asp:Label>
                                                </ItemTemplate>

                                                <FooterTemplate>
                                                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:LinkButton runat="server" CommandName="create" ID="lnkupdate" Visible="true">
                                                        <img src="../img/manual/view.png" style="width:20px" alt="" />view</asp:LinkButton>
                                                    </center>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <center>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Action"></asp:Label>
                                                    </center>
                                                </HeaderTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="5" PreviousPageText="Last" />
                                    </asp:GridView>
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
</asp:Content>


