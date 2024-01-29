<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeliveryInstView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DeliveryInstView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <style>
        input#ContentPlaceHolder1_cmdNew {
            margin-right: 10px;
        }
    </style>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <br />
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Dispatch Instructions View
                        <%--                   <a style="float:right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                            <h4><i class="icon-reorder"></i>Dispatch Instructions View</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                                <div style="display: flex!important" class="span5">
                                    <asp:Button ID="cmdNew" runat="server" Text="New Dispatch"
                                        CssClass="btn btn-success" OnClick="cmdNew_Click" />
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_clickPOMaster" />
                                </div>


                            </div>


                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdDeliveryInstView" AutoGenerateColumns="false" PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                OnSorting="grdPOmaster_Sorting" OnRowCommand="grdPoMasterView_RowCommand"
                                runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnPageIndexChanging="grdPoMasterView_PageIndexChanging"
                                ShowFooter="True" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_ID" HeaderText="DI Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDIid" runat="server" Text='<%# Bind("DI_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PO_ID" HeaderText="PO Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoId" runat="server" Text='<%# Bind("DI_PO_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DI_NO" HeaderText="DI No" SortExpression="DI_NO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiNo" runat="server" Text='<%# Bind("DIM_DI_NO") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                <asp:TextBox ID="txtDINumber" runat="server" Width="110px" placeholder="Enter DI Number" ToolTip="Enter DI Number to Search"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="DI_DATE" HeaderText="Delivery Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiDate" runat="server" Text='<%# Bind("DI_DATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="STORE_NAME" HeaderText="Store Name" SortExpression="PO_SUPPLIER_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStore" runat="server" Text='<%# Bind("DI_STORE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_MAKE" HeaderText="Make Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmake" runat="server" Text='<%# Bind("DI_MAKE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="STAR_RATE" HeaderText="RatingId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRatingId" runat="server" Text='<%# Bind("DI_STARRATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="STAR_RATE" HeaderText="Rating">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRating" runat="server" Text='<%# Bind("DI_STARRATENAME") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_CAPACITY" HeaderText="Capacity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("DI_CAPACITY") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_QUANTITY" HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDIQuantity" runat="server" Text='<%# Bind("DI_QUANTITY") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_DUEDATE" HeaderText="Due Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("DI_DUEDATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_START_RANGE" HeaderText="DTr Start Range">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDIStartRange" runat="server" Text='<%# Bind("DI_START_RANGE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_END_RANGE" HeaderText="DTr End Range">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDIEndrange" runat="server" Text='<%# Bind("DI_END_RANGE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <center>

                                                <asp:ImageButton ID="imgBtnEdit" Title="Click To View" runat="server" Height="25px"
                                                    ImageUrl="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRCLCe9EV-Y4YfmEHP-2KavMYznWAk_IMGRmg&usqp=CAU"
                                                    CommandName="Submit" Width="20px" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                    <%--    ImageUrl="~/Styles/images/edit64x64.png" --%>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Dispatch Instruction Details,Existing Rate Contract Details Can be Edited and New Dispatch Instruction can Be Added.
                    </p>
                    <%-- <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit Dispatch Instruction Details Click On Edit Button Enter Details And Click On Update Button To Update the Details
                        </p>--%>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New Dispatch Instruction Click On  New Button
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
