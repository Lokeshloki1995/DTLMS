<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="WorkOrderRangeAllocationView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.WorkOrderRangeAllocationView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>

    <style type="text/css">
        table {
            overflow: scroll;
        }

        td {
            border: 1px solid #ccc;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        table.dataTable thead th {
            border-bottom: 1px solid #111;
            font-size: 12px !important;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 10px 0px !important;
            text-align: center !important;
        }

        .table-advance tr td {
            border-left-width: 1px !important;
            border: 1px solid #d4d4d4;
            font-size: 12px !important;
        }

        th {
            white-space: nowrap;
        }
    </style>

    <script type="text/javascript">        //
        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdWO').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({

                "sPaginationType": "full_numbers"
            });
        });
    </script>


    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' User?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        table#ContentPlaceHolder1_grdWO {
            overflow: auto;
        }

        td {
            border: none;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        th {
            white-space: nowrap;
            text-align: center !important;
        }

        thead {
            text-align: center !important;
        }

        span {
            text-align: center;
        }

        select#ContentPlaceHolder1_cmbZone, select#ContentPlaceHolder1_cmbsubdivision, select#ContentPlaceHolder1_cmbCircle, select#ContentPlaceHolder1_cmbSection, select#ContentPlaceHolder1_cmbDivision {
            width: 216px !important;
        }

        select {
            width: 70px;
        }

        .gvPagerCss span {
            background-color: #f9f9f9;
            font-size: 18px;
        }

        .gvPagerCss td {
            padding-left: 5px;
            padding-right: 5px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

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

        div.pager {
            text-align: center;
            margin: 1em 0;
        }

        div.pager {
            text-align: center;
            margin: 1em 0;
        }

        div.pg-goto {
            color: #000000;
            font-size: 15px;
            cursor: pointer;
            background: #D0B389;
            padding: 2px 4px 2px 4px;
        }

        div.pg-selected {
            color: #fff;
            font-size: 15px;
            background: #000000;
            padding: 2px 4px 2px 4px;
        }

        div.pg-normal {
            color: #000000;
            font-size: 15px;
            cursor: pointer;
            background: #D0B389;
            padding: 2px 4px 2px 4px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Work Order Range Allocation View
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

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Work Order Range Allocation View</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div class="widget-body form">


                                <div style="float: right">
                                    <div class="span15">

                                        <asp:Button ID="cmdNew" runat="server" Text="Create New"
                                            CssClass="btn btn-success" OnClick="cmdNew_Click" />
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                            OnClick="Export_click" /><br />
                                        <br />
                                    </div>
                                </div>

                                <asp:GridView ID="grdWO"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server"
                                    ShowFooter="false">
                                    <PagerStyle CssClass="gvPagerCss " />
                                    <HeaderStyle CssClass="both" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WRA_DIV_NAME" HeaderText="Division Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("WRA_DIV_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WRA_DIV" HeaderText="Division ID " Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivID" runat="server" Text='<%# Bind("WRA_DIV") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WRA_FINANCIALYEAR" HeaderText="Financial Year" SortExpression="WRA_FINANCIALYEAR">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFinancialYear" runat="server" Text='<%# Bind("WRA_FINANCIALYEAR") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WRA_ACC_HEAD" HeaderText="Account Head">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccHead" runat="server" Text='<%# Bind("WRA_ACC_HEAD") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WRA_ALLOTMENT_DATE" HeaderText="Allotment Date">

                                            <ItemTemplate>
                                                <asp:Label ID="lblAllotmentDate" runat="server" Text='<%# Bind("WRA_ALLOTMENT_DATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WRA_QUANTITY" HeaderText="Quantity" SortExpression="WRA_QUANTITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("WRA_QUANTITY") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WRA_START_RANGE" HeaderText="Start Range" SortExpression="WRA_START_RANGE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartRange" runat="server" Text='<%# Bind("WRA_START_RANGE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WRA_END_RANGE" HeaderText="End Range" SortExpression="WRA_END_RANGE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndRange" runat="server" Text='<%# Bind("WRA_END_RANGE") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageText="first" LastPageText="last" Mode="NumericFirstLast" />
                                </asp:GridView>
                            </div>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->

        <!-- END PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can be used To View Work Order Range Allocated Details and 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New Work order Range allocation Click On New Button
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
