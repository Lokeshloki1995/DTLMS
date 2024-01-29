<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTRBilling.aspx.cs" Inherits="IIITS.DTLMS.Billing.DTRBilling" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

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
                    <h3 class="page-title">DTR Billing
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
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Bill View"
                        CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='BillingView.aspx'; return false;" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>DTR Billing</h4>
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
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Estimation No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfEstNo" runat="server" />
                                                         <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                         <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:TextBox ID="txtWoId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtEstId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtActionType" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtEstNo" runat="server" ></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" TabIndex="2" OnClick="cmdSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Estimation Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Estimation Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstAmount" runat="server"  ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="space20"></div>
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Work Order No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWoNo" runat="server"  ReadOnly="true"></asp:TextBox>
                                                         <asp:TextBox ID="txtWorkOrderID" runat="server"  Visible="false"  ReadOnly="true"></asp:TextBox>
                                                         <asp:TextBox ID="txtFailtype" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                         <asp:TextBox ID="txtGuranteetype" runat="server" Visible="false"  ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Work Order Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWoDate" runat="server"  ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">WO Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWoAmount" runat="server"  ReadOnly="true"></asp:TextBox>
                                                         <br />
                                                        <asp:LinkButton ID="lnkWoDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkWoDetails_Click">View WO Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span1"></div>
                                    </div>

                                    <div class="space20"></div>

                                    <div class="row-fluid" style="display: none">
                                        <div class="span3"></div>
                                        <div class="span5">
                                            <asp:GridView ID="grdCharges" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                                AutoGenerateColumns="false" PageSize="10"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" TabIndex="8">
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="TYPEOFCHARGES" HeaderText="TYPEOFCHARGES">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TYPEOFCHARGES") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="AMOUNT" HeaderText="AMOUNT">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("AMOUNT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                            <div class="space20"></div>
                                            <asp:LinkButton ID="lnkDetails" runat="server" Style="font-weight: bold;" ForeColor="Blue" Font-Underline="true">View Estimation Details</asp:LinkButton>

                                        </div>
                                    </div>
                                </div>
                            </div>



                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>

            <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div id="divMaterialCost" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Material Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>
                                                        <asp:GridView ID="grdMaterialMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"
                                                            AllowSorting="true">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtMaterialName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="200px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Estimated Quantity" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("ESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Actual Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtMQuantity" runat="server" Width="100px" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMqty" runat="server" Width="100px" MaxLength="7" Text='<%# Bind("ESTM_ITEM_QNTY") %>' onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBaserate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate(%)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>



                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Amount (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatAmount" runat="server" Text='<%# Bind("AMOUNT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Tax (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatTax" runat="server" Text='<%# Bind("TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblMatTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Total (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblMaterialTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>
                    <div id="divLabourCost" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Labour Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>
                                                        <asp:GridView ID="grdLabourMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"
                                                            AllowSorting="true">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabourId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtLabourName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabourName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="200px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Estimated Quantity" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabQuantity" runat="server" Text='<%# Bind("ESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Actual Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtLqty" runat="server" Width="100px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLqty" runat="server" Width="100px" MaxLength="7" Text='<%# Bind("ESTM_ITEM_QNTY") %>' onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabrate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate(%)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabtax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>


                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Amount (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabAmount" runat="server" Text='<%# Bind("AMOUNT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Tax (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinalLabTax" runat="server" Text='<%# Bind("TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLabTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Total (Rs)">



                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabtotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLabourTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>



                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>
                    <div id="divSalvages" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Salvages Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>


                                                        <asp:GridView ID="grdSalvageMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"
                                                            AllowSorting="true">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalvageId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtSalvageName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalvageName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="200px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Estimated Quantity" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalQuantity" runat="server" Text='<%# Bind("ESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Actual Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtSqty" runat="server" Width="100px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSqty" runat="server" Width="100px" MaxLength="7" Text='<%# Bind("ESTM_ITEM_QNTY") %>' onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsalunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsalrate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate(%)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsaltax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>



                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Amount (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalAmount" runat="server" Text='<%# Bind("AMOUNT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Tax (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalFinalTax" runat="server" Text='<%# Bind("TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSalTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Total (Rs)">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsaltotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSalvageTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all; font-weight: bold;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>


                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                    <div class="space20"></div>
                                                    <div>
                                                        <asp:Label ID="lblMessageDisplay" Font-Bold="true" runat="server" Text="Total Charges ( Material + Labour - Salvage ) =   "></asp:Label>
                                                        <asp:Label ID="lblTotalCharges" Font-Bold="true" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="space20"></div>
                                            <!-- END FORM-->




                                        </div>
                                    </div>

                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Invoice Details</h4>
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
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Invoice No<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" TabIndex="1" MaxLength="20" onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>
                                                        <asp:TextBox ID="txtInvId" runat="server" TabIndex="1" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtRate" runat="server" TabIndex="1" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtTax" runat="server" TabIndex="1" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtAmount" runat="server" TabIndex="1" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Invoice Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" TabIndex="2" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1_txtInvoiceDate" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">BR No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtBrNo" runat="server" TabIndex="3" MaxLength="15" onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Invoice Amount<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvAmount" runat="server" TabIndex="4" MaxLength="15" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Expenditure booked </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtbooked" runat="server" TabIndex="4" MaxLength="15" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Invoice Copy<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupInvoice" runat="server" TabIndex="5"/>
                                                        <asp:TextBox ID="txtFilepath" runat="server"  Visible="false"></asp:TextBox> 
                                                        
                                                        
                                                                <asp:HiddenField ID="HdfInvoicepath" runat="server" />                                                       
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:LinkButton ID="lnkDownloadFile" runat="server"  Visible="false" OnClick="lnkDownloadFile_Click">View Invoice</asp:LinkButton>

                                        </div>

                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                                        <div class="span12">
                                            <!-- BEGIN SAMPLE FORMPORTLET-->
                                            <div class="widget blue">
                                                <div class="widget-title">
                                                    <h4><i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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
                                                                <div class="span5">

                                                                    <div class="control-group">
                                                                        <label class="control-label">Comments<span class="Mandotary"> *</span></label>
                                                                        <div class="controls">
                                                                            <div class="input-append">
                                                                                <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine"
                                                                                    Width="550px" Height="125px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
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
                                    </div>

                                    <div class="text-center">
                                        
                                            <asp:Button ID="cmdCalc" runat="server" Text="Calculate"
                                                CssClass="btn btn-primary" OnClick="cmdCalc_Click" />
                                        
                                       
                                            <asp:Button ID="cmdSave" runat="server" Text="Save"
                                                OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                      
                                  
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="btnReset_Click" />
                                      
                                    </div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">

                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>

            <%--   <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Pending DTR Details</h4>
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
                                        <div class="space20"></div>
                                        <!-- END FORM-->
                                        <asp:GridView ID="grdPendingTc" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                            AutoGenerateColumns="false" PageSize="10"
                                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                            runat="server"
                                            TabIndex="8" OnPageIndexChanging="grdPendingTc_PageIndexChanging">
                                            <Columns>

                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField AccessibleHeaderText="DELIVERED" HeaderText="Deliver Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeliver" runat="server" Text='<%# Bind("DELIVERED") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="SM_NAME" HeaderText="Store">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStore" runat="server" Text='<%# Bind("SM_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="DELIVERY_DATE" HeaderText="Delivery Date">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Bind("DELIVERY_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI Number">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRINo" runat="server" Text='<%# Bind("TR_RI_NO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                        </asp:GridView>

                                        <div class="span1"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>--%>
        </div>


        <!-- END PAGE CONTENT-->
    </div>
    <style>
    .table {
    width: 100%;
    margin-bottom: 20px;
    display: -webkit-box;
    overflow-y: scroll;
    height: 500px;
}
</style>
</asp:Content>
