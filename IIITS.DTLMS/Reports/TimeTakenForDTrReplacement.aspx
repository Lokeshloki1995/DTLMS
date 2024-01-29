<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TimeTakenForDTrReplacement.aspx.cs" Inherits="IIITS.DTLMS.Reports.TimeTakenForDTrReplacement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .slNoHeader {
            min-width: 25px !important;
        }
    </style>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/jscript">

        function Validate() {
            if (document.getElementById('<%= cmbFinYear.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbFinYear.ClientID %>').value.trim() == "") {
                alert('Select the Financial Year')
                document.getElementById('<%= cmbFinYear.ClientID %>').focus()
                return false
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
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
                                <i class="icon-reorder"></i>Time Taken For DTr Replacement</h4>
                            <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px; color: white"></i></a>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Financial Year<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbFinYear" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="HedFinYear" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>




                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Circle<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>

                                    <div class="text-center" style="margin-bottom: 35px !important">
                                        <asp:Button ID="cmdReport" runat="server" Text="Generate Abstract" CssClass="btn btn-primary" OnClick="Abstract_DTrReplacmenttimelineReport" OnClientClick="javascript:return ValidateMyForm()" />
                                        <asp:Button ID="cmdExportReport" runat="server" Text="Export Report" CssClass="btn btn-info" OnClick="Export_DTrReplacmenttimelineReport" />
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="BtnReset_Click" />
                                    </div>

                                    <div style="overflow-y: auto!important;" class="">

                                        <asp:GridView ID="grdAbstractDtrReplacementDetails" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                            ShowFooter="true"
                                            CssClass="table table-striped table-bordered table-advance table-hover"
                                            OnRowDataBound="grdAbstractDtrReplacementDetails_RowDataBound"
                                            runat="server">
                                            <HeaderStyle CssClass="both" />
                                            <Columns>

                                                <asp:TemplateField AccessibleHeaderText="DIVISIONNAME" HeaderText="DIVISION NAME" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCircle" runat="server" Text='<%# Bind("DIVISIONNAME") %>' Width="180px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="PARTICULARS" HeaderText="PARTICULARS" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("PARTICULARS") %>' Width="220px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="10_KVA" HeaderText="10 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl10kva" runat="server" Text='<%# Bind("10_KVA") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="15_KVA" HeaderText="15 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lb15kva" runat="server" Text='<%# Bind("15_KVA") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="25_KVA" HeaderText="25 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl25kva" runat="server" Text='<%# Bind("25_KVA") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="50_KVA" HeaderText="50 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl63kva" runat="server" Text='<%# Bind("50_KVA") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="63_KVA" HeaderText="63 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl63kva" runat="server" Text='<%# Bind("63_KVA") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="100_KVA" HeaderText="100 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl100kva" runat="server" Text='<%# Bind("100_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField AccessibleHeaderText="125_KVA" HeaderText="125 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl125kva" runat="server" Text='<%# Bind("125_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField AccessibleHeaderText="150_KVA" HeaderText="150 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl150kva" runat="server" Text='<%# Bind("150_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField AccessibleHeaderText="160_KVA" HeaderText="160 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl160kva" runat="server" Text='<%# Bind("160_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="200_KVA" HeaderText="200 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl200kva" runat="server" Text='<%# Bind("200_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="250_KVA" HeaderText="250 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl250kva" runat="server" Text='<%# Bind("250_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="300_KVA" HeaderText="300 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl300kva" runat="server" Text='<%# Bind("300_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="315_KVA" HeaderText="315 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl315kva" runat="server" Text='<%# Bind("315_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="400_KVA" HeaderText="400 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl400kva" runat="server" Text='<%# Bind("400_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="500_KVA" HeaderText="500 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl500kva" runat="server" Text='<%# Bind("500_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="630_KVA" HeaderText="630 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl630kva" runat="server" Text='<%# Bind("630_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="750_KVA" HeaderText="750 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl750kva" runat="server" Text='<%# Bind("750_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="960_KVA" HeaderText="960 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl960kva" runat="server" Text='<%# Bind("960_KVA") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="1000_KVA" HeaderText="1000 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1000kva" runat="server" Text='<%# Bind("1000_KVA") %>' Width="60px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="1250_KVA" HeaderText="1250 KVA" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1250kva" runat="server" Text='<%# Bind("1250_KVA") %>' Width="60px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="TOTAL" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("TOTAL") %>' Width="57px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                            <RowStyle CssClass="gridViewRow" />
                                        </asp:GridView>
                                    </div>



                                    <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                        </div>
                    </div>
                    <div class="space20">
                    </div>
                    <!-- END FORM-->

                </div>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
</asp:Content>
