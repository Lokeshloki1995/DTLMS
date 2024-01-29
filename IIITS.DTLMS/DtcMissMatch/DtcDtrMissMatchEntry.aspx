<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DtcDtrMissMatchEntry.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.DtcDtrMissMatchEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            white-space: nowrap !important;
        }
    </style>

    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdAllocate.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;
    </script>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= txtDtcCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTC Code')
                document.getElementById('<%= txtDtcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDtrCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTr Code')
                document.getElementById('<%= txtDtrCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%=cmdAllocate.ClientID %>').value.trim() == "Allocate") {
                return confirm("Are You Sure Want to Allocate DTC");
            }
        }

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        function ValidateDtcCode(DtcCode) {
            var reg = new RegExp("^[H][A-Z]+$");
            if (!reg.test(DtcCode.value)) {
                alert("Enter Valid DTC Code");
                DtcCode.value = '';
                return;
            }
        };

        function ValidateDtrCode(DtrCode) {
            var reg = new RegExp("^[H][0-9]+$");
            if (!reg.test(DtrCode.value)) {
                alert("Enter Valid DTr Code");
                DtrCode.value = '';
                return;
            }
        };

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">MisMatch  DTC DTr Allocation
                </h3>

                <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">
                    <i class="fa fa-exclamation-circle"
                        style="font-size: 36px"></i></a>
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
                                    <i class="fa fa-info-circle"></i>This Web Page Can Be Used To DTC DTR Allocation
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
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>MisMatch DTC DTr Allocation</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                        <asp:HiddenField ID="hdfDtrCode" runat="server" />
                                        <asp:HiddenField ID="hdfLocType" runat="server" />
                                        <asp:HiddenField ID="hdfOffCode" runat="server" />
                                        <asp:HiddenField ID="hdfNewLocType" runat="server" />
                                        <asp:HiddenField ID="hdfNewOffCode" runat="server" />
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcCode" runat="server" AutoPostBack="true"
                                                        OnTextChanged="btnDtcSearch_Click" onchange="ValidateDtcCode(this);"
                                                        MaxLength="6"></asp:TextBox>
                                                    <asp:Button ID="btnDtcSearch" runat="server" Text="S" TabIndex="2"
                                                        CssClass="btn btn-primary"
                                                        OnClick="btnDtcSearch_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Remarks</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" AutoPostBack="false">
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span4"></div>
                                        <div class="span4" id="checkboxdiv" runat="server">
                                            <div class="checkbox">
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text="" AutoPostBack="True"
                                                    OnCheckedChanged="CheckBox1_CheckedChanged" />
                                                <span class="label label-important">Select If DTC is Available For Above
                                            <asp:Label ID="lblDtrCode" runat="server" Text=""></asp:Label>
                                                </span>
                                            </div>
                                        </div>
                                        <div style="margin-left: 35px;" id="divOldTc" runat="server">
                                            <div class="span5">
                                                <div id="dtcold" class="span3" runat="server">
                                                    <div class="control-group">
                                                        <label class="control-label">
                                                            DTC Code
                                                        </label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtOldDtc" runat="server" AutoPostBack="true"
                                                                    OnTextChanged="btnSearchdtc2_Click"
                                                                    onchange="ValidateDtcCode(this);" MaxLength="6">
                                                                </asp:TextBox>
                                                                <asp:Button ID="btnSearchdtc2" runat="server" Text="S" TabIndex="2"
                                                                    CssClass="btn btn-primary" OnClick="btnSearchdtc2_Click" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5" runat="server">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" AutoPostBack="true"
                                                        OnTextChanged="btnDtrSearch_Click"
                                                        onchange="ValidateDtrCode(this);" MaxLength="8"></asp:TextBox>
                                                    <asp:Button ID="btnDtrSearch" runat="server" Text="S" TabIndex="2"
                                                        CssClass="btn btn-primary"
                                                        OnClick="btnDtrSearch_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="span12">
            <div class="widget blue">
                <div class="widget-title">
                    <h4>
                        <i class="icon-reorder"></i>Mapping Details</h4>
                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                        class="icon-remove"></a></span>
                </div>
                <div class="widget-body">
                    <div class="widget-body form">
                        <div class="form-horizontal">
                            <div class="row-fluid" style="width: 50%">

                                <div class="span8">
                                    <%--Displaying 1st dtc Details--%>
                                    <asp:GridView ID="grdDtcDetails" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        Width="300px" OnRowCreated="grdDtcDetails_RowCreated">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcCode" runat="server"
                                                        Text='<%# Bind("dt_code") %>'
                                                        Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="DTC Name"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcName" runat="server"
                                                        Text='<%# Bind("DT_NAME") %>'
                                                        Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Capacity" HeaderText="DTC Capacity"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity" runat="server"
                                                        Text='<%# Bind("TC_CAPACITY") %>'
                                                        Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_tc_id" HeaderText="DTr Code"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("DT_TC_ID") %>'
                                                        Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="span4">
                                    <%--Displaying 1st dtr Details--%>
                                    <asp:GridView ID="grdDtrDetails" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        OnRowCreated="grdDtrDetails_RowCreated">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCode" runat="server"
                                                        Text='<%# Bind("TC_CODE") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTr Capacity"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CURRENT_LOCATION"
                                                HeaderText="TC Current Location"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCurrentLocation" runat="server"
                                                        Text='<%# Bind("CURRENTLOCATION") %>'
                                                        Style="word-break: break-all;" Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="DTr Status"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'
                                                        Style="word-break: break-all;"
                                                        ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_Location" HeaderText="DTr Location"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOffNmae" runat="server" Text='<%# Bind("OFFNAME") %>'
                                                        Style="word-break: break-all;"
                                                        Width="150px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <br />
                            <div class="row-fluid" style="width: 50%">
                                <div class="span8">
                                    <%--Displaying 2nd dtc Details--%>
                                    <asp:GridView ID="grdSecondDtcDetails" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        OnRowCreated="grdSecondDtcDetails_RowCreated">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcCode2" runat="server" Text='<%# Bind("dt_code") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="DTC Name"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcName2" runat="server" Text='<%# Bind("DT_NAME") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Capacity" HeaderText="DTC Capacity"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity2" runat="server" Text='<%# Bind("TC_CAPACITY") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_tc_id" HeaderText="DTr Code"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtrCode2" runat="server" Text='<%# Bind("DT_TC_ID") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="span4">
                                    <%--Displaying 2nd dtr Details--%>
                                    <asp:GridView ID="grdDtrDetails2" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        OnRowCreated="grdDtrDetails2_RowCreated">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCode2" runat="server" Text='<%# Bind("TC_Code") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTr Capacity"
                                                Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity2" runat="server" Text='<%# Bind("TC_CAPACITY") %>'
                                                        Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CURRENT_LOCATION"
                                                HeaderText="DTr Current Location"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCurrentLocation2" runat="server"
                                                        Text='<%# Bind("CURRENTLOCATION") %>'
                                                        Style="word-break: break-all;" Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="DTr Status" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus2" runat="server" Text='<%# Bind("STATUS") %>'
                                                        Style="word-break: break-all;"
                                                        Width="150px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_Location" HeaderText="DTr Location"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOffNmae2" runat="server" Text='<%# Bind("OFFNAME") %>'
                                                        Style="word-break: break-all;"
                                                        Width="150px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="text-center" align="center">
        <asp:Button ID="cmdAllocate" runat="server" Text="Allocate" CssClass="btn btn-primary"
            OnClientClick="javascript:return ValidateMyForm()" onchange="javascript:preventMultipleSubmissions();"
            OnClick="cmdAllocate_Click" />
        <asp:Button ID="cmdReset" runat="server" Text="Reset"
            CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
