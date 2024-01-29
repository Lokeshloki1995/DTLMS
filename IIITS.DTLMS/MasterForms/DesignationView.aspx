<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DesignationView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DesignationView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css" />
    <style type="text/css">
        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        table#ContentPlaceHolder1_grdDesignationDetails td {
            white-space: nowrap;
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

        i.fa.fa-exclamation-circle {
            font-size: 23px !important;
            color: white !important;
            padding: 0px 0px 0px 3px !important;
        }
    </style>

    <script type="text/javascript">
        debugger;
        jQuery(function ($) {
            $(document).ready(function () {
                $('#grdDesignationDetails').dataTable({
                    "aaSorting": [],
                    "jQueryUI": true
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Designation View
                </h3>
                <%--  <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                <a style="float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; padding: 0px 0px 0px 3px!important"></i></a>

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
                        <h4><i class="icon-reorder"></i>Designation View
                            <%--                                  <a style="float:right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size:16px;padding: 0px 0px 0px 3px!important"></i></a>--%>
                        </h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">




                        <div style="float: right">
                            <div class="span7">
                                <asp:Button ID="cmdNew" runat="server" Text="New Designation"
                                    CssClass="btn btn-success" OnClick="cmdNew_Click" /><br />
                            </div>



                            <div class="span1">
                                <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                    OnClick="Export_clickDest" /><br />
                            </div>
                        </div>

                        <div class="space20"></div>
                        <!-- END FORM-->




                        <asp:GridView ID="grdDesignationDetails" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"  ShowFooter="True"
                            CssClass="table table-striped table-bordered table-advance table-hover" onrowcommand="grdDesignationDetails_RowCommand"
                            runat="server" OnPageIndexChanging="grdDesignationDetails_PageIndexChanging" OnSorting="grdDesignationDetails_Sorting" AllowSorting="true">
                            <HeaderStyle CssClass="both" />

                            <Columns>
                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="DM_DESGN_ID" HeaderText="Id" Visible="false">

                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("DM_DESGN_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="DM_NAME" HeaderText="Designation" Visible="true" SortExpression="DM_NAME">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("DM_NAME") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("DM_NAME") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                    </ItemTemplate>
                                       <FooterTemplate>
                                        <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="input_textSearch" Width="150px"
                                                placeholder="Enter Designation Name" ToolTip="Enter Designation Name to Search"></asp:TextBox>
                                        </asp:Panel>
                                    </FooterTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="DM_DESC" HeaderText="Description"
                                    Visible="true" SortExpression="DM_DESC">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%# Bind("DM_DESC") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("DM_DESC") %>' Style="word-break: break-all;" Width="350px"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                            </FooterTemplate>
                                </asp:TemplateField>




                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                OnClick="imgBtnEdit_Click" Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                CausesValidation="false" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                    </div>




                </div>
            </div>

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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All The Designation and To Add New Designation.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Existing Designation Details Can Be Edited By Clicking Edit Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New Designation  Can Be Added By Clicking New Designation Button
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
