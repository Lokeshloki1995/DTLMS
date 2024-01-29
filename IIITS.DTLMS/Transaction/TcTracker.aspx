<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcTracker.aspx.cs" Inherits="IIITS.DTLMS.Transaction.TcTracker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTr Code')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                 return false
             }
         }
    </script>
    <style type="text/css">
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">DTR Tracker
                    </h3>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <%-- <div style="float:right;margin-top:20px;margin-right:12px" >
                    <asp:Button ID="cmdClose" runat="server" Text="Close"  OnClientClick="javascript:window.location.href='FaultTCSearch.aspx'; return false;"
                                    CssClass="btn btn-primary" />
            </div>--%>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>DTR Tracker
                                                <a style="float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px; padding: 0px 0px 0px 3px!important"></i></a>
                            </h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>

                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span5">
                                            <div class="control-group">

                                                <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10" autocomplete="off"></asp:TextBox>
                                                        <asp:Button ID="btnSearchId" Text="S" class="btn btn-primary" runat="server" /><br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkDTrDetails_Click" OnClientClick="javascript:return ValidateMyForm()">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">From Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtFromDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">

                                                <label class="control-label">Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbType" runat="server">
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Commissioning"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Failure"></asp:ListItem>
                                                            <asp:ListItem Value="4" Text="Dispatch"></asp:ListItem>
                                                            <asp:ListItem Value="5" Text="Receive"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="RI"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">To Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                    </div>
                                    <div class="text-center">
                                        <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                            OnClick="cmdLoad_Click" Width="116px" />


                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                            OnClick="Export_clickTCtracker" OnClientClick="javascript:return ValidateMyForm()" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>DTR Basic Details</h4>
                                <span class="tools">
                                    <a href="javascript:;" class="icon-chevron-down"></a>
                                    <a href="javascript:;" class="icon-remove"></a>

                                </span>
                            </div>

                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div class="form-horizontal">
                                        <div class="row-fluid">
                                            <div class="span12">
                                                <asp:Label ID="lbl1" runat="server" Text="DTr SlNo"></asp:Label>
                                                <asp:TextBox ID="txtTcSlNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                &nbsp&nbsp&nbsp&nbsp 
                                   <asp:Label ID="lbl2" runat="server" Text="DTr Make"></asp:Label>
                                                <asp:TextBox ID="txtTcMake" runat="server" ReadOnly="true"></asp:TextBox>
                                                &nbsp&nbsp&nbsp&nbsp 
                                   <asp:Label ID="lbl3" runat="server" Text="Capacity(in KVA)"></asp:Label>
                                                <asp:TextBox ID="txtCapacity" runat="server" ReadOnly="true"></asp:TextBox>
                                                &nbsp&nbsp&nbsp&nbsp 
                                                <asp:Label ID="lblRepCount" runat="server" Text="Repair Count"></asp:Label>
                                                <asp:TextBox ID="txtRepairCount" runat="server" ReadOnly="true" Width="160px"></asp:TextBox>
                                            </div>
                                            <div class="space20"></div>
                                            <asp:GridView ID="grdTcDetails"
                                                AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" OnPageIndexChanging="grdTcDetails_PageIndexChanging"
                                                OnRowCommand="grdTcDetails_RowCommand" OnSorting="grdTcDetails_Sorting" AllowSorting="true">
                                                <HeaderStyle CssClass="both" />
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="DRT_ACT_REFTYPE" HeaderText="Ref Type" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRefType" runat="server" Text='<%# Bind("DRT_ACT_REFTYPE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="DRT_DTR_STATUS" HeaderText="DTR Status" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDTrStatus" runat="server" Text='<%# Bind("DRT_DTR_STATUS") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="DRT_ACT_REFNO" HeaderText="Ref No" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRefNo" runat="server" Text='<%# Bind("DRT_ACT_REFNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="DATE" HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("TRANSDATE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location" SortExpression="LOCATION">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LOCATION") %>' Style="word-break: break-all" Width="450px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" SortExpression="STATUS">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all" Width="450px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action" Visible="false">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:ImageButton ID="imgView" runat="server" Height="15px" ImageUrl="~/img/Manual/view.png"
                                                                    Width="15px" CommandName="View" />
                                                            </center>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <center>
                                                                <asp:Label ID="lblHead" runat="server" Text="View Details"></asp:Label>
                                                            </center>
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
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
    </div>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <div class="span3"></div>
    <asp:Label ID="lblTcDetails" runat="server" ForeColor="Blue"></asp:Label>

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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Track DTR 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Can Enter Or Search Dtr Code in Dtr Code TextBox
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>After Entering Dtr Code Click On Load Button To Get The Details
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

    </div>
</asp:Content>
