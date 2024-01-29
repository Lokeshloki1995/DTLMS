<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="EnumReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.EnumReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= cmbType.ClientID %>').value == "--Select--") {
              alert('Select Type')
              document.getElementById('<%= cmbType.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbDiv.ClientID %>').value == "--All--") {
              alert('Select Division')
              document.getElementById('<%= cmbDiv.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbSection.ClientID %>').value == "--All--") {
              alert('Select Section')
              document.getElementById('<%= cmbSection.ClientID %>').focus()
              return false
          }
          //          if (document.getElementById('<%= cmbFeeder.ClientID %>').value == "--Select--") {
          //              alert('Select Feeder')
          //              document.getElementById('<%= cmbFeeder.ClientID %>').focus()
          //              return false
          //          }

      }

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
                    <h3 class="page-title">Enumeration Report
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
                            <h4><i class="icon-reorder"></i>Enum Report</h4>
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
                                                <label class="control-label">Location Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbType_SelectedIndexChanged"
                                                            TabIndex="1">
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Store Report"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Field Report"></asp:ListItem>
                                                        </asp:DropDownList>
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
                                                <label class="control-label">Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Sub Division</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Section </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true"
                                                            TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Feeder </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeeder" runat="server"
                                                            TabIndex="1">
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
                                                <label class="control-label">From Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="15"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtFromDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">To Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="15"></asp:TextBox>

                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtToDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Report Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbdatewise" runat="server" AutoPostBack="true"
                                                            TabIndex="1">
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="5" Text="All"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="QCWise pending"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="QCWise Done"></asp:ListItem>
                                                            <asp:ListItem Value="4" Text="QCWise Reject"></asp:ListItem>

                                                            <%-- <asp:ListItem Value="3" Text="DTC Without TC Added Report"></asp:ListItem>  --%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center">




                                        <asp:Button ID="cmpReport" runat="server" Text="Generate Report"
                                            CssClass="btn btn-primary" OnClick="cmpReport_Click" />

                                        <asp:Button ID="cmbabstract" runat="server" Text="Generate Abstract"
                                            CssClass="btn btn-primary" OnClick="cmdDtrAbstract_Click" />

                                        <asp:Button ID="btnExport" runat="server" Text="Generate Excel"
                                            CssClass="btn btn-primary" OnClick="cmdDtrExport_Click" />


                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" OnClick="cmdReset_Click" />


                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                    <asp:Button ID="gridexcel" runat="server" Text="Export Excel"
                                        CssClass="btn btn-success" OnClick="gridexcel_Click" Visible="false" />

                                    <asp:GridView ID="grdAbstractDtrDetails" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        ShowFooter="true"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server">
                                        <HeaderStyle CssClass="both" />
                                        <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="DIVISION NAME" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblofficename" runat="server" Text='<%# Bind("DIV") %>' Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="SUB_DIV" HeaderText="SUB DIV NAME" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsubdiv" runat="server" Text='<%# Bind("SUB_DIV") %>' Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="SECTION" HeaderText="OM SECTION NAME" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblomsection" runat="server" Text='<%# Bind("SECTION") %>' Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="DTE_TC_CODE " HeaderText="DTC COUNT" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltccode" runat="server" Text='<%# Bind("DTE_TC_CODE") %>' Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                        </Columns>
                                    </asp:GridView>



                                    <asp:GridView ID="grdstoreabstract" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        ShowFooter="true"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server">
                                        <HeaderStyle CssClass="both" />
                                        <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="STORE NAME" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblofficename" runat="server" Text='<%# Bind("OFF_NAME") %>' Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="DTE_TC_CODE " HeaderText="TC COUNT" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltccode" runat="server" Text='<%# Bind("DTE_TC_CODE") %>' Width="150px"></asp:Label>
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
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>


            <!-- END PAGE CONTENT-->
        </div>

    </div>
</asp:Content>
