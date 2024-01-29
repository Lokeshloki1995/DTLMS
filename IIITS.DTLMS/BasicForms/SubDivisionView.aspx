<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="SubDivisionView.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.SubDivisionView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
                    <h3 class="page-title">SubDivision View
                    </h3>
<%--                    <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                     <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
                            <h4><i class="icon-reorder"></i>SubDivision View</h4>

                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                                <div class="span7">
                                    <asp:Button ID="cmdNewSubDivision" class="btn btn-success" Text="New SubDivision"
                                        runat="server" OnClick="cmdNewSubDivision_Click" />
                                    <br />
                                </div>
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_clicKSubDivision" /><br />
                                </div>
                            </div>
                        </div>
                        
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                         <asp:Button ID="cmdLoad" runat="server" Text="Load"
                                                    CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                               
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="space20"></div>


                            <asp:GridView ID="grdZoneOffice"
                                AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" ShowFooter="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                OnPageIndexChanging="grdZoneOffice_PageIndexChanging"
                                OnRowCommand="grdZoneOffice_RowCommand" OnSorting="grdZoneOffice_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="SD_ID" HeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGCorpid" runat="server" Text='<%# Bind("SD_ID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("SD_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="CM_CIRCLE_NAME" HeaderText="Circle Name" SortExpression="CM_CIRCLE_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpPhone" runat="server" Text='<%# Bind("CM_CIRCLE_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtcircleName" runat="server" placeholder="Enter Circle Name" Width="150px" CssClass="span12"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="Division Name" SortExpression="DIV_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpPhone1" runat="server" Text='<%# Bind("DIV_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtDivisionName" runat="server" placeholder="Enter Division Name" Width="150px" CssClass="span12"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SD_SUBDIV_NAME" HeaderText="Sub-Division Name" SortExpression="SD_SUBDIV_NAME">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("SD_ID") %>'></asp:HiddenField>
                                            <asp:Label ID="lblCorpName2" runat="server" Text='<%# Bind("SD_SUBDIV_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtsubDivisionName" runat="server" placeholder="Enter Sub Division Name" Width="150px" CssClass="span12"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="SD_SUBDIV_CODE" HeaderText="SubDivision Code" SortExpression="SD_SUBDIV_CODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpAddr1" runat="server" Text='<%# Bind("SD_SUBDIV_CODE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtSubDivCode" runat="server" placeholder="Enter Sub Div Code Code" Width="100px" CssClass="span12"></asp:TextBox>

                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="SD_MOBILE"
                                        HeaderText="Mobile No">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpAddr" runat="server" Text='<%# Bind("SD_MOBILE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>

                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />

                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="SD_HEAD_EMP" HeaderText="Office Head" SortExpression="SD_HEAD_EMP">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpHod" runat="server" Text='<%# Bind("SD_HEAD_EMP") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
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
                                                    OnClick="imbBtnDelete_Click" Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                    CausesValidation="false" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                            </asp:GridView>

                        </div>
                    </div>
                </div>

                <!-- END FORM-->

                <!-- END PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Sub Division Details and To Add New Division 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit Existing Details Click On <u>Edit</u> LiknkButton
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New Sub-Division Click On <u>New SubDivision</u> LiknkButton
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
