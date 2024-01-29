<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="LatestUpdates.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.LatestUpdates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function openTab(th) {
            window.open(th.name, '_blank');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        table#ContentPlaceHolder1_gvFiles {
            width: 100%;
            text-align: center;
            margin-top: 15px !important;
            margin-bottom: 15px !important;
        }
    </style>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Latest Updates
                </h3>
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
                        <h4><i class="icon-reorder"></i>Latest Updates</h4>
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
                                    <div class="span1"></div>
                                    <div class="span10">
                                        <asp:GridView ID="grdLatestUpdates" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                            CssClass="table table-striped table-bordered table-advance table-hover" runat="server" ShowFooter="false">
                                            <PagerStyle CssClass="gvPagerCss" />
                                            <HeaderStyle CssClass="both" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="UPDATEDESCRIPTION" HeaderText="What's New">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRecentUpdates" runat="server" Text='<%# Bind("UPDATEDESCRIPTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="EFFECTFROM" HeaderText="Last Updated Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastUpdateDate" runat="server" Text='<%# Bind("EFFECTFROM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <PagerSettings FirstPageText="first" LastPageText="last" Mode="NumericFirstLast" />


                                        </asp:GridView>

                                    </div>
                                </div>

                                <div class="span1"></div>
                                <div class="space20"></div>
                            </div>
                        </div>
                        <div class="space20"></div>
                        <!-- END FORM-->
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
</asp:Content>
