<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="EstimationView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.EstimationView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Estimation View
                </h3>
                <%--                <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
                            </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>
        <!-- END PAGE HEADER-->

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Estimation View</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <%-- <div style="float:left" >--%>
                                <%--  <div class="span8">--%>


                                <div style="float: right;">

                                    <div class="span1">
                                        <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"   OnClick="Export_Clic" /><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="widget-body">
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <asp:Label ID="lblGridType" runat="server" Font-Bold="true" ForeColor="#4A8BC2"
                                    Font-Size="Medium"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <asp:GridView ID="grdEstimation" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        CssClass="table table-striped table-bordered table-advance table-hover" ShowFooter="true"
                        runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                        AllowSorting="true" OnRowCommand="grdEstimation_RowCommand" OnPageIndexChanging="grdEstimation_PageIndexChanging" OnSorting="grdEstimation_Sorting">
                        <HeaderStyle CssClass="both" />



                        <Columns>


                            <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFailureId" runat="server" Text='<%# Bind("EST_FAILUREID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Both Columns are same but adding for User Interface Purpose--%>
                            <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="EstimationID ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstId" runat="server" Text='<%# Bind("EST_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Transformer Centre Code" SortExpression="DT_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblDtCode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>

                                    <asp:TextBox ID="txtDtcCode" runat="server" placeholder="Enter DTC Code " Width="150px" MaxLength="6"></asp:TextBox>

                                </FooterTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DF_EQUIPMENT_ID") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>

                                    <asp:TextBox ID="txtDtrCode" runat="server" placeholder="Enter DTr Code" Width="150px" MaxLength="8" ></asp:TextBox>

                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="EST_NO" HeaderText="EST NO" SortExpression="DT_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstNo" runat="server" Text='<%# Bind("EST_NO") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>

                                    <asp:TextBox ID="txtEstNo" runat="server" placeholder="Enter EST No" Width="150px" MaxLength="10"></asp:TextBox>

                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="EST_CAPACITY" HeaderText="CAPACITY" SortExpression="DT_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("EST_CAPACITY") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>

                                    <asp:TextBox ID="txtCapacity" runat="server" placeholder="Enter Capacity" Width="150px" MaxLength="3"></asp:TextBox>

                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE" SortExpression="DT_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblFailuretype" runat="server" Text='<%# Bind("FAILURETYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>

                                    <asp:TextBox ID="txtfailuretype" runat="server" placeholder="Enter DTC Code " Width="150px" MaxLength="6"></asp:TextBox>

                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="EST_CRON" HeaderText="EST DATE" SortExpression="DT_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstDate" runat="server" Text='<%# Bind("EST_CRON") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                </FooterTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <center>
                                        <asp:LinkButton runat="server" CommandName="View" ID="lnkUpdate">
                                             <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                    </center>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="lblHeader" runat="server" Text="Action"></asp:Label>
                                    </center>
                                </HeaderTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                    </asp:GridView>

                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
