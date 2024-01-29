<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DWAView.aspx.cs" Inherits="IIITS.DTLMS.POFlow.DWAView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--      <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>

    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>

    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" />
<%--    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>


    <style type="text/css">

        .chosen-container-multi .chosen-choices {
            max-height: 110px !important;
            overflow: auto !important;
        }

            .chosen-container-multi .chosen-choices li.search-choice {
                font-size: 11px;
            }

        #ContentPlaceHolder1_grdDWA {
            overflow: scroll !important;
            width: 100% !important;
        }
 
        .main {
            border: 1px solid #c5c5c5 !important;
        }

        .tabs {
            margin: 0;
            padding: 0.2em 0.2em 0 !important;
            color: #333333 !important;
            font-weight: bold !important;
        }

        .tab-button {
            float: left !important;
            padding: 0.5em 1em !important;
            text-decoration: none !important;
            font-size: 14px !important;
            border: 1px solid #c5c5c5 !important;
        }

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

        table#ContentPlaceHolder1_grdUser {
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

        .chosen-container-multi .chosen-drop .result-selected {
            display: none !important;
        }

        .chosen-container-multi .chosen-choices li.search-choice span {
            word-wrap: break-word !important;
            font-size: 9px !important;
        }

        .chosen-container .chosen-results li.no-results {
            display: none !important;
        }
        input#ContentPlaceHolder1_cmdexport:focus{
            background: #22c0cb!important;
        }

    
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdDWA').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "sPaginationType": "full_numbers"
            });
        });
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip(); // added css in font-awesome.min.css line 43 and 405
        });



        function ConfirmStatus(status) {
            var result = confirm('Are you sure,Do you want to ' + status + ' DWA?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    </asp:Content>
        
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="container-fluid">
                <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <div style="float: left;">
                    <div class="span12">
                        <h3 class="page-title">Detailed WorkAward Details</h3>
                    </div>
                </div>

                  <div style="float: right !important; margin-top: 20px !important;">
                    <div class="span2">
                        <asp:Button ID="cmdNewLEC" runat="server" type="button" Text="Create New DWA" CssClass="btn btn-success" OnClick="CreateNewDWA_click" Style="float: left!important" />
                    </div>
                </div>
                 </div>
              </div>


               <%-- <h3 class="page-title">DWA View</h3>--%>
                <%--  <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
              <%--  <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>--%>
               <%-- <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button"><i class="icon-search"></i></button>
                            </div>
                        </form>
                    </li>
                </ul>--%>
                <!-- END PAGE TITLE & BREADCRUMB-->
            <%--row-flui--%>
          <div class="row-fluid">
            <div class="span12">
                <div class="widget blue">
          <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Detailed Work Award</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>


                     <div class="widget-body">
                        <div class="widget-body form">

             <div class="widget blue" >
              <div class="widget-body">
                    <div style="background-color: #d4d4d4; width: 100%;margin-bottom: 20px;margin-left:0px" class="span25">
                            <div class="tabs" style="width: 100% !important;">
                                <div style="float: left!important">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:Button ID="cmdActiveUser" CssClass="btn btn-danger tab-button" Text="Active DWA" runat="server" OnClick="cmdActiveUser_click" />
                                    <asp:Button ID="cmdInActiveUser" CssClass="btn btn-danger tab-button" Text="In-Active DWA" runat="server" OnClick="cmdInActiveUser_click" />
                                </div>
                                <div style="float: right!important">
                                     <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info tab-button" OnClick="Export_DWAClick" style="" />
                                </div>
                            </div>
                           </div>

                       <div style="overflow-x: auto!important; width: 100%!important;" class="">

                             <asp:GridView ID="grdDWA"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found"
                                    OnRowDataBound="grdDWA_RowDataBound"
                                    OnRowCommand="grdDWA_RowCommand"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server"
                                    ShowFooter="false">
                                  <PagerStyle CssClass="gvPagerCss" />
                                    <HeaderStyle CssClass="both" />
                                 <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="DM_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDmId" runat="server" Text='<%# Bind("DM_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="DM_LM_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDmLmId" runat="server" Text='<%# Bind("DM_LM_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="LM_CONTRACTOR_NAME" HeaderText="Contractor/Firm Name" SortExpression="LM_CONTRACTOR_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContractorName" runat="server" Text='<%# Bind("LM_CONTRACTOR_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                         </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="DM_NUMBER" HeaderText="DWA Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDWANo" runat="server" Text='<%# Bind("DM_NUMBER") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="DM_DATE" HeaderText="Award Date" SortExpression="DM_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDWDate" runat="server" Text='<%# Bind("DM_DATE") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField AccessibleHeaderText="DM_PERIOD" HeaderText="Valid Upto" SortExpression="DM_PERIOD">
                                            <ItemTemplate>
                                                <asp:Label ID="LabelValidUpto" runat="server" Text='<%# Bind("DM_EXTENDED_UPTO") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                     <asp:TemplateField AccessibleHeaderText="DM_PRJTYP" HeaderText="Project" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblProjectname" runat="server" Text='<%# Bind("MD_NAME") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                      <asp:TemplateField AccessibleHeaderText="DM_PROJECTNAME" HeaderText="Project Name" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherProject" runat="server" Text='<%# Bind("DM_PROJECTNAME") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                      <asp:TemplateField AccessibleHeaderText="DM_AMOUNT" HeaderText="Award Amount" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblAwardAmount" runat="server" Text='<%# Bind("DM_AMOUNT") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="LM_ADDRESS" HeaderText="Contractor Address" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("LM_ADDRESS") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="LM_CONTACT_NUMBER" HeaderText="Mobile No">

                                            <ItemTemplate>
                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("LM_CONTACT_NUMBER") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="edit"
                                                        Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("DIM_STATUS") %>'></asp:Label>
                                                <center>
                                                    <asp:ImageButton Visible="false" ID="imgDeactive" runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status"
                                                        ToolTip="Click to Active DWA" OnClientClick="return ConfirmStatus('Enable');" Width="10px" />
                                                    <asp:ImageButton Visible="false" ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif" CommandName="status"
                                                        ToolTip="Click to In-Active DWA" OnClientClick="return ConfirmStatus('Disable');" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     </Columns>
                                 <PagerSettings FirstPageText="first" LastPageText="last" Mode="NumericFirstLast" />
                                  </asp:GridView>
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
