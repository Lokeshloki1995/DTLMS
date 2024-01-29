<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PODetails.aspx.cs" Inherits="IIITS.DTLMS.Transaction.PODetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <div class="container-fluid">

        <div class="row-fluid">
            <div class="span8">
                <h3 class="page-title">DTR Delivery Status</h3>
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
                <asp:Button ID="cmdClose" runat="server" Text="PO Details"
                    CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='POTracker.aspx'; return false;" />
            </div>
            <a href="PODetails.aspx">PODetails.aspx</a>
        </div>

        <div id="Div1" class="row-fluid" runat="server">
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Delivery Instruction Details</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>

                    </span>
                </div>
                <div class="widget-body">

                    <div class="widget-body form">
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal">
                            <div class="row-fluid">

                                <div class="span1"></div>
                                <div class="span5">
                                    <div class="control-group">
                                        <label class="control-label">PO Number</label>

                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtPONumber" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span5">
                                    <div class="control-group">
                                        <label class="control-label">PO date</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtPODate" runat="server" ReadOnly="true"></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="space20"></div>
                                <div class="space20"></div>

                                <asp:TextBox ID="txtPOId" runat="server" Visible="false"></asp:TextBox>
                                <asp:GridView ID="grdPODetails" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AllowSorting="true"
                                    AllowPaging="true" OnPageIndexChanging="grdPODetails_PageIndexChanging" OnSorting="grdPODetails_Sorting" onrowcommand="grdPODetails_RowCommand_For_PODetails">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="PO_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDelId" runat="server" Text='<%# Bind("DI_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DI_NO" HeaderText="DI No" SortExpression="DI_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDelNO" runat="server" Text='<%# Bind("DI_NO") %>' Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtDiNO" runat="server" CssClass="input_textSearch" Width="150px"
                                                    placeholder="Enter DI No" ToolTip="Enter DI No to Search"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SM_NAME" HeaderText="Store" SortExpression="SM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("SM_NAME") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtStore" runat="server" CssClass="input_textSearch" Width="150px"
                                                    placeholder="Enter Store" ToolTip="Enter Store to Search"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" SortExpression="TM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtMake" runat="server" CssClass="input_textSearch" Width="150px"
                                                    placeholder="Enter Make" ToolTip="Enter Make to Search"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="DI_CAPACITY" HeaderText="Capacity" SortExpression="DI_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("DI_CAPACITY") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="DI_DATE" HeaderText="DI Date" SortExpression="DI_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupName" runat="server" Text='<%# Bind("DI_DATE") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                           <%--             <asp:TemplateField AccessibleHeaderText="DI_CONSIGNEE" HeaderText="Consignee" SortExpression="DI_CONSIGNEE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DI_CONSIGNEE") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>--%>

                                        <asp:TemplateField AccessibleHeaderText="DI_QUANTITY" HeaderText="QUANTITY" SortExpression="DI_QUANTITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("DI_QUANTITY") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="INWARD" HeaderText="INWARD" SortExpression="INWARD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoDTR" runat="server" Text='<%# Bind("INWARD") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>



                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="PENDING" HeaderText="PENDING" SortExpression="PENDING">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPending" runat="server" Text='<%# Bind("PENDING") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton runat="server" CommandName="View" ID="lnkView" Visible="true" OnClick="lnkBtnEdit_Click"> <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>

                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="dvDTR" class="row-fluid" runat="server" visible="false">
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>DTR Details</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>

                    </span>
                </div>
                <div class="widget-body">

                    <div class="widget-body form">
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal">
                            <div class="row-fluid">



                                <asp:GridView ID="grdTCDetails" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AllowSorting="true"
                                    AllowPaging="true" OnPageIndexChanging="grdTCDetails_PageIndexChanging" OnSorting="grdTCDetails_Sorting">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltccode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR Slno" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblslno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf Date" SortExpression="TC_MANF_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmanfdate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_RATING" HeaderText="Rating" SortExpression="TC_RATING">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrating" runat="server" Text='<%# Bind("TC_RATING") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>



                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
