<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ScrapView.aspx.cs" Inherits="IIITS.DTLMS.ScrapEntry.ScrapView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <style>
        input#ContentPlaceHolder1_cmdNew {
            margin-right: 10px;
        }
        .widget-body{
            margin-left:150px;
        }
    </style>


<%--    <script type="text/javascript">

        function AllowOnlyAlphanumericNotAllowSpecial(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
           ((evt.which) ? evt.which : 0));

            if ((charCode > 47 && charCode < 58) ||
                (charCode > 64 && charCode < 91) ||
                (charCode > 96 && charCode < 123)) {
                return true;
            }

            else { return false; }
        }
        function DisableSplChars(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 92) {
                return false;
            } else {

                return true;
            }
        }
    </script>--%>

   
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <br />
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Scrap View
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
                            <h4><i class="icon-reorder"></i>Scrap View</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
 
                            <div class="space20"></div>
                            <!-- END FORM-->

                            <div class="space20"></div>
                            <div class="row-fluid">

                                <div class="span6">
                                    <asp:GridView ID="grdScrapTC" AutoGenerateColumns="false" PageSize="10"
                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        OnRowCommand="grdScrapTC_RowCommand" ShowFooter="True"
                                        runat="server" TabIndex="16" Style="width: 600px;" OnPageIndexChanging="grdScrapTC_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ST_ID" HeaderText="Scrap Dtails Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstdid" Width="100" runat="server" Text='<%# Bind("ST_ID") %>'></asp:Label>
                                                </ItemTemplate>

                 
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ST_OM_NO" HeaderText="OM No" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblomno"  Width="300px" runat="server" Text='<%# Bind("ST_OM_NO") %>'></asp:Label>
                                                </ItemTemplate>
                                               <FooterTemplate>
                                                    <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtdtrcode" runat="server" Width="110px" placeholder="Enter OM No" ToolTip="Enter OM No to Search"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ST_OM_DATE" HeaderText="OM Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblomdate" runat="server" Width="150" Text='<%# Bind("ST_OM_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="Search" TabIndex="9" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                  
                                            <asp:TemplateField AccessibleHeaderText="ST_CRBY" HeaderText="Cr_By" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcrby" runat="server" Text='<%# Bind("ST_CRBY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="STORE_NAME" HeaderText="Store Name" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstorename" runat="server" Width="150" Text='<%# Bind("STORE_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ST_QTY" HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstoreid" runat="server" Width="100" Text='<%# Bind("ST_QTY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkload" runat="server" ForeColor="Blue" Width="100" Text="View" OnClick="cmdLoad_click"
                                                        CommandArgument='<%# Eval("ST_ID") %>'> </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                            </div>
                        </div>
                    </div>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All DTr Scrap Details.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To View The Scrap Details  Click On View Button To Get The Details
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
