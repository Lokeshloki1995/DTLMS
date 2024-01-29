<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTrReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.DTrReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>


    <script type="text/javascript">

        function ValidateMyForm() {

            if (document.getElementById('<%= cmbLocation.ClientID %>').value.trim() == "--Select--") {
                alert('Please Select the location Type')
                document.getElementById('<%= cmbLocation.ClientID %>').focus()
                return false
            }
        }
    </script>
    <style type="text/css">
        table#ContentPlaceHolder1_grdAbstractDtrDetails {
            table-layout: fixed;
            overflow: scroll;
            display: -webkit-inline-box;
            width: 100% !important;
        }
        /*i.fa.fa-exclamation-circle{
            padding: 0px 7px 10px 11px!important;
        }*/
        i.fa.fa-exclamation-circle {
            font-size: 23px !important;
            color: white !important;
            padding: 0px 0px 0px 3px !important;
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
                    <h3 class="page-title">
                        <%--     <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>   DTR Report 
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
                <%--           <div style="float:right;margin-top:20px;margin-right:12px" >--%>
                <%-- <asp:Button ID="Button1" runat="server" Text="Store View" 
                  OnClientClick="javascript:window.location.href='StoreView.aspx'; return false;"
                  CssClass="btn btn-primary" />--%>
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>DTR Report
                       

                          <a style="float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 16px; padding: 0px 0px 0px 3px !important"></i></a>


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
                                    <div class="span1"></div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Location Type <span class="Mandotary">*</span>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbLocation" runat="server" TabIndex="1" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbLocation_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                  <%--  <form id="form1" runat="server">
                                                        <asp:DropDownList ID="myDropdown" CssClass="multiselect" runat="server" Multiple="Multiple"></asp:DropDownList>
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                                    </form>--%>

                                                </div>
                                            </div>
                                        </div>




                                        <div class="control-group">
                                            <label class="control-label">
                                                Zone
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="2"
                                                        OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="4"
                                                        OnSelectedIndexChanged="cmbLocation_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label ID="lblSubDiv" runat="server" class="control-label">
                                    SubDivision</asp:Label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true" TabIndex="5"
                                                        OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <asp:Label class="control-label" runat="server" ID="lblSec">
                                    Section</asp:Label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSec" runat="server" TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="cmbsec_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <%-- another span--%>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <%--<asp:TextBox ID="txtTcCapacity" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                             MaxLength="10" ></asp:TextBox>--%>
                                                    <asp:DropDownList ID="cmbCapacity" runat="server">
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
                                                    <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="3"
                                                        OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">DTr Make</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbMake" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <asp:Label ID="lblfed" runat="server" class="control-label">
                                    Feeder Name</asp:Label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFeederName" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>
                                <div class="space20"></div>
                                <div class="text-center">

                                    <asp:Button ID="cmdReport" runat="server" Text="Generate Report"
                                        CssClass="btn btn-primary" OnClick="cmdReport_Click" OnClientClick="javascript:return ValidateMyForm()" />

                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        CssClass="btn btn-danger" OnClick="cmdReset_Click" />


                                    <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        TabIndex="12" OnClick="Export_click" OnClientClick="javascript:return ValidateMyForm()" />
                                    <br />
                                </div>
                                <%-- <div class="span1"></div>--%>
                                <div class="span7"></div>
                                <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="space20"></div>
                    <!-- END FORM-->
                </div>
            </div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <!-- END SAMPLE FORM PORTLET-->
        </div>
    </div>


    <div class="row-fluid">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>DTR Abstract Report</h4>
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
                                        <label class="control-label">
                                            Zone
                                        </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbAbstractZone" runat="server" AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="cmbAbstractZone_SelectedIndexChanged">
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
                                                <asp:DropDownList ID="cmbAbstractCircle" runat="server" AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="cmbAbstractCircle_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">
                                            Division</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbAbstractDivision" runat="server" AutoPostBack="true" TabIndex="4" OnSelectedIndexChanged="cmbAbstractDivision_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <%-- another span--%>
                                <div class="span5">

                                    <div class="control-group">
                                        <asp:Label ID="Label1" runat="server" class="control-label">
                                    SubDivision</asp:Label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbAbstractSubDiv" runat="server" AutoPostBack="true" TabIndex="5" OnSelectedIndexChanged="cmbAbstractSubDiv_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <asp:Label class="control-label" runat="server" ID="Label2">
                                    Section</asp:Label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbAbstractSection" runat="server" TabIndex="6" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span1"></div>
                            </div>
                            <div class="space20"></div>
                            <div class="space20"></div>
                            <div class="text-center" align="center">

                                <asp:Button ID="cmdDtrAbstract" runat="server" Text="Generate Report"
                                    CssClass="btn btn-primary" OnClick="cmdDtrAbstract_Click" />

                                <asp:Button ID="cmdAbstractReset" runat="server" Text="Reset"
                                    CssClass="btn btn-danger" OnClick="cmdAbstractReset_Click" />

                            </div>
                            <div class="span1" align="center">
                                <%--<asp:Button ID="cmdAbstractExport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                    TabIndex="12" OnClick="cmdAbstractExport_Click" />--%>
                                <br />
                            </div>
                            <%-- <div class="span1"></div>--%>
                            <div class="span7"></div>
                            <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                        </div>

                        <asp:GridView ID="grdAbstractDtrDetails" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                            ShowFooter="true"
                            CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server">
                            <HeaderStyle CssClass="both" />
                            <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ZONE NAME" HeaderText="ZONE NAME" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZoneName" runat="server" Text='<%# Bind("ZONE_NAME") %>' Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="CIRCLE NAME" HeaderText="CIRCLE NAME" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcirclename" runat="server" Text='<%# Bind("CIRCLE_NAME") %>' Style="word-break: break-all;"
                                            Width="150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="DIVISION NAME" HeaderText="DIVISION NAME" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lnlDivisionName" runat="server" Text='<%# Bind("DIVISION_NAME") %>' Style="word-break: break-all;"
                                            Width="150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SUBDIVISION NAME" HeaderText="SUBDIVISION NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVISION_NAME") %>' Style="word-break: break-all;"
                                            Width="150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SECTION NAME" HeaderText="SECTION NAME" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOmSectionName" runat="server" Text='<%# Bind("SECTION_NAME") %>' Style="word-break: break-all;"
                                            Width="150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="TOTAL COUNT" HeaderText="TOTAL COUNT" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCount" runat="server" Text='<%# Bind("TOTAL_COUNT") %>' Style="word-break: break-all;" Width="150px">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SUBDIVISION TOTAL COUNT" HeaderText="SUBDIVISIONTOTAL COUNT" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubdivCount" runat="server" Text='<%# Bind("SUBDIV_TOTAL_COUNT") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="space20"></div>
                <!-- END FORM-->
            </div>
        </div>
        <asp:Label ID="Label5" runat="server" ForeColor="Red"></asp:Label>
        <!-- END SAMPLE FORM PORTLET-->
    </div>
    </div>



         <!-- END PAGE CONTENT-->
    </div>
   </div>
   <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help
                    </h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Report is used to View the DTR present in Selected location
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>You Can Take Report by selecting DTRMake or Capacity or Circle or Division 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>you can Take Full Report by seclecting nothing in the form 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>By Clicking Generate Report Button crystal Report will be Generate After that you can download it in to PDF file
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>By Clicking ExportExcel Button Excel will be downloaded
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
