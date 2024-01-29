<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeviceRegister.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DeviceRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function ConfirmStatus(status) {
            debugger;
            var result = confirm('Are you sure,Do you want to ' + status + ' Device?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }


    </script>
    <style type="text/css">
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

        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Mobile Register View
                    </h3>
                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
            </div>

            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Mobile Register View</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <%--------------------new implementation on 15-05-2023--------------------%>
                            <div style="float: left">
                                <%--<asp:TextBox ID="txtbuttonValue" runat="server" Visible="false"></asp:TextBox>--%>
                                <asp:TextBox ID="hidbuttonVal" runat="server" MaxLength="10" Visible="false"></asp:TextBox>

                                <%--UseSubmitBehavior="false"--%>

                                <div class="span3">
                                    <asp:Button ID="btnPendingRecords" Style="margin-left: 5px;" class="btn btn-primary" runat="server" Text="Pending" OnClick="LoadPending_clickDeviceRegister" />
                                </div>

                                <div class="span3">
                                    <asp:Button ID="btnApprovedRecords" runat="server" class="btn btn-primary" Style="margin-left: 15px;" Text="Approved" OnClick="LoadApproval_clickDeviceRegister" />
                                </div>

                                <div class="span3">
                                    <asp:Button ID="btnDesabeldRecords" runat="server" class="btn btn-primary" Text="Disabled" Style="margin-left: 35px;" OnClick="LoadDisabled_clickDeviceRegister" />
                                </div>
                            </div>
                            <%-----------------end-----------------------%>

                            <div style="float: right">
                                <%--          ----  commnted on 15-05-2023 ----
                                <div class="span3">
                                    <asp:Button ID="Button2" runat="server" Text="View" CssClass="btn btn-primary"
                                        OnClick="LoadView_clickDeviceRegister" />
                                </div>
                                <div class="span5">
                                    <asp:Button ID="Button1" runat="server" Text="Load pending" CssClass="btn btn-success"
                                        OnClick="LoadPending_clickDeviceRegister" />
                                </div>
                                --- End -----%>
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_clickDeviceRegister" Style="margin-right: 75px;" /><br />
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdDeviceRegister"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdDeviceRegister_PageIndexChanging"
                                OnRowCommand="grdDeviceRegister_RowCommand" OnRowDataBound="grdDeviceRegister_RowDataBound"
                                ShowFooter="True" OnSorting="grdDeviceRegister_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MR_REQUEST_BY" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("MR_REQUEST_BY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MR_ID" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMrId" runat="server" Text='<%# Bind("MR_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MU_DEVICE_ID" HeaderText="Device Id" SortExpression="MR_DEVICE_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIvDeviceId" runat="server" Text='<%# Bind("MR_DEVICE_ID") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtIvDeviceId" runat="server" Width="150px" placeholder="Enter Device Id" ToolTip="Enter Device Id to Search"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="US_FULL_NAME" HeaderText="Name" SortExpression="US_FULL_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullName" runat="server" Text='<%# Bind("US_FULL_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtsFullName" runat="server" Width="150px" placeholder="Enter Name" ToolTip="Enter Name to Search"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MU_APPROVE_STATUS" HeaderText="Approval Status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("MR_APPROVE_STATUS") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MU_CRON" HeaderText="Created On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Bind("MR_CRON") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("MR_APPROVE_STATUS") %>'></asp:Label>
                                            <center>
                                                <asp:ImageButton Visible="false" ID="imgBtnApproval" runat="server" ImageUrl="../img/Manual/Approve.png" Width="15px" Height="15px" CommandName="status"
                                                    ToolTip="Click to Approve Device" OnClientClick="return ConfirmStatus('Enable');" />

                                                <asp:ImageButton Visible="false" ID="imgBtnReject" runat="server" ImageUrl="../img/Manual/Reject.png" Width="15px" Height="15px" CommandName="status"
                                                    ToolTip="Click to Disable The Device" OnClientClick="return ConfirmStatus('Disable');" />

                                                <asp:ImageButton Visible="false" ID="imgDeactive" runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="activate"
                                                    ToolTip="Click to Enable User" OnClientClick="return ConfirmStatus('Enable');" Width="10px" />
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
                        </div>

                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>


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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Mobile Register Details and To Approve Mobile Users
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Approve Mobile User Click On Approval Button
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
