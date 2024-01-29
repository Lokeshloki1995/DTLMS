<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="POTracker.aspx.cs" Inherits="IIITS.DTLMS.Transaction.POTracker" %>

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



        </div>

        <div id="Div1" class="row-fluid" runat="server">
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>PO Details</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>

                    </span>
                </div>
                <div class="widget-body">
                    <div style="float: right">
                        <div class="span2">
                            <asp:Button ID="cmdNew" runat="server" Text="New" CssClass="btn btn-success" />
                        </div>
                    </div>
                    <div class="widget-body form">
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal">

                            <div class="row-fluid">

                                <div class="control-group">
                                    <label class="control-label">Financial Year<span class="Mandotary"> *</span></label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbFinYear" runat="server">
                                            </asp:DropDownList>
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                        </div>
                                    </div>
                                </div>

                                <asp:GridView ID="grdPODetails" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AllowSorting="true" 
                                    AllowPaging="true" OnPageIndexChanging="grdPODetails_PageIndexChanging" OnSorting="grdPODetails_Sorting" onrowcommand="grdPODetails_RowCommand_for_POTrack">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="PO_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoId" runat="server" Text='<%# Bind("PO_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PO_NO" HeaderText="PO No" SortExpression="PO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNO" runat="server" Text='<%# Bind("PO_NO") %>' Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtPONO" runat="server" CssClass="input_textSearch" Width="150px" autocomplete="off"
                                                        placeholder="Enter PO No" ToolTip="Enter PO No to Search"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" SortExpression="TS_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupName" runat="server" Text='<%# Bind("TS_NAME") %>'
                                                 Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>

                                                <asp:TextBox ID="txtSupName" runat="server" CssClass="input_textSearch" Width="150px" autocomplete="off"
                                                    placeholder="Enter Supplier" ToolTip="Enter Supplier to Search"></asp:TextBox>

                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PO_RATE" HeaderText="PO Amount" SortExpression="PO_RATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("PO_RATE") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>

                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="input_textSearch" Width="150px" autocomplete="off"
                                                    placeholder="Enter Amount" ToolTip="Enter Amount to Search"></asp:TextBox>

                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="PO_DATE" HeaderText="PO Date" SortExpression="PO_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("PO_DATE") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="NOOFDTR" HeaderText="NO OF DTR" SortExpression="NOOFDTR">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNoDTR" runat="server" Text='<%# Bind("NOOFDTR") %>'
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                          
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <center>
                                                    <%--<asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                       Width="12px" />--%>

                                                    <asp:LinkButton runat="server"  CommandName="View" ID="lnkView"  visible="true" OnClick="imgBtnEdit_Click"> <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>


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
    </div>
</asp:Content>
