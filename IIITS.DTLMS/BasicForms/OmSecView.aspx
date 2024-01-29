<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="OmSecView.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.OmSecView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
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
            <%--BEGIN PAGE HEADER--%>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">OM UNIT View
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
<%--                <div style="float: right; margin-right: 10px; margin-top: 10px" class="span2">
                </div>--%>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>OM UNIT View</h4>

                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span7">
                                    <asp:Button ID="cmdNewOmSec" class="btn btn-success" Text="New OmSection"
                                        runat="server" OnClick="cmdNewOmSec_Click" />

                                    <br />
                                </div>
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_clicksection" /><br />
                                </div>
                            </div>

                            <div class="space3"></div>
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
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged" >
                                                        </asp:DropDownList>
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
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubdiv" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
<%--                                                        <asp:Button ID="cmdLoad" runat="server" Text="Load"
                                                    CssClass="btn btn-primary" OnClick="cmdLoad_Click" />--%>
                                                    </div>
                                                </div>
                                            </div>
     
                                                 <asp:Button ID="Button1" runat="server" Text="Load" 
                                                 CssClass="btn btn-primary" OnClick="cmdLoad_Click" />

                                        </div>
                                    </div>
                                </div>
                            </div>


                            <asp:GridView ID="grdOmSection"
                                AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" ShowFooter="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                OnPageIndexChanging="grdOmSection_PageIndexChanging" OnRowCommand="grdOmSection_RowCommand" OnSorting="grdOmSection_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="OM_SLNO" HeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGCorpid" runat="server" Text='<%# Bind("OM_SLNO") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>

                                            <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("OM_SLNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="SD_SUBDIV_NAME" HeaderText="SubDivision Name" SortExpression="SD_SUBDIV_NAME">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSubDivName" runat="server" Text='<%# Bind("SD_SUBDIV_NAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubName" runat="server" Text='<%# Bind("SD_SUBDIV_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtsubDivisionName" runat="server" placeholder="Enter Sub Division Name" Width="180px" CssClass="span12"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OM_NAME" HeaderText="OMU Name" SortExpression="OM_NAME">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOMname" runat="server" Text='<%# Bind("OM_NAME") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("OM_SLNO") %>'></asp:HiddenField>
                                            <asp:Label ID="lblSecName" runat="server" Text='<%# Bind("OM_NAME") %>' Style="word-break: break-all" Width="200"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtOmName" runat="server" placeholder="Enter OM Name" Width="150px" CssClass="span12"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="OM_CODE" HeaderText="OMU Code" SortExpression="OM_CODE">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpAddr1" runat="server" Text='<%# Bind("OM_CODE") %>' Style="word-break: break-all" Width="100"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtOmCode" runat="server" placeholder="Enter OM Code" Width="150px" CssClass="span12"></asp:TextBox>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OM_MOBILE_NO"
                                        HeaderText="Mobile No">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpAddr" runat="server" Text='<%# Bind("OM_MOBILE_NO") %>' Style="word-break: break-all" Width="150"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="OM_HEAD_EMP" HeaderText="Office Head" SortExpression="OM_HEAD_EMP">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpHod" runat="server" Text='<%# Bind("OM_HEAD_EMP") %>' Style="word-break: break-all" Width="150"></asp:Label>
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

                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Section Details and To Add New Section 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit Existing Details Click On <u>Edit</u> LiknkButton
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New Section Click On <u>New OmSection</u> LiknkButton
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
