<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="CircleWiseRepairerReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.CircleWiseRepairerReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

    </script>
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
                    <h3 class="page-title">CIRCLE WISE REPAIRER TRANSFORMER DETAILS
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                </div>
            </div>
            <!-- END PAGE HEADER-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>CIRCLE WISE REPAIRER TRANSFORMER DETAILS
                                   <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px; padding: 0px 0px 0px 0px!important"></i></a>
                            </h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">



                                            <div class="control-group">
                                                <label class="control-label">
                                                    Financial Year<span class="Mandotary"> *</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFinancialyear" runat="server" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span1">
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="text-center">


                                        <asp:Button ID="cmdReport" runat="server" Text="Generate Report"
                                            CssClass="btn btn-primary" OnClick="Export_ClickCircleWiseRepairer" />
                                        <asp:Button ID="cmbabstract" runat="server" Text="Generate Abstract"
                                            CssClass="btn btn-primary" OnClick="cmdDtrAbstract_Click" />

                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" OnClick="cmdReset_Click" />




                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                    <br />



                                    <asp:GridView ID="grdAbstractDtrDetails" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        ShowFooter="true"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        OnRowDataBound="grdAbstractDtrDetails_RowDataBound"
                                        runat="server">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="ISSUED_TRNAME" HeaderText="NAME OF THE REPAIR AGENCY" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairername" runat="server" Text='<%# Bind("ISSUED_TRNAME") %>' Width="370px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ISSUED_25_KVA" HeaderText="25 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssued25kva" runat="server" Text='<%# Bind("ISSUED_25_KVA") %>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ISSUED_63_KVA" HeaderText="63 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssued63kva" runat="server" Text='<%# Bind("ISSUED_63_KVA") %>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="ISSUED_100_KVA " HeaderText="100 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssued100kva" runat="server" Text='<%# Bind("ISSUED_100_KVA") %>' Width="55px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="ISSUED_250_KVA" HeaderText="250 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssued250kva" runat="server" Text='<%# Bind("ISSUED_250_KVA") %>' Width="55px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ISSUED_Grand_Total_KVA" HeaderText="TOTAL" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssuedTotal" runat="server" Text='<%# Bind("ISSUED_Grand_Total_KVA") %>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="RECEIVED_25_KVA " HeaderText="25 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceived25kva" runat="server" Text='<%# Bind("RECEIVED_25_KVA") %>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="RECEIVED_63_KVA" HeaderText="63 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceived63kva" runat="server" Text='<%# Bind("RECEIVED_63_KVA") %>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="RECEIVED_100_KVA" HeaderText="100 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceived100kva" runat="server" Text='<%# Bind("RECEIVED_100_KVA") %>' Width="55px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="RECEIVED_250_KVA" HeaderText="250 KVA" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceived250kva" runat="server" Text='<%# Bind("RECEIVED_250_KVA") %>' Width="55px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RECEIVED_Grand_Total_KVA " HeaderText="TOTAL" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceivedTotal" runat="server" Text='<%# Bind("RECEIVED_Grand_Total_KVA") %>' Width="55px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="Total_RSD_REP_COST" HeaderText="TOTAL COST OF REPAIR" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalcost" runat="server" Text='<%# Bind("Total_RSD_REP_COST") %>' Width="100px" ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="gridViewRow" />                                       
                                    </asp:GridView>

                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- BEGIN PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Report is used to View the central wise repairer report
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>You Can Take Report by selecting Circle
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>By Clicking Generate Report Button excel Report will be Generate
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>By Clicking Generate Abstract Button Circle Wise Abstract Details will display in grid 
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
