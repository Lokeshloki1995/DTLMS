<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTRMissmatchEntry.aspx.cs"
    Inherits="IIITS.DTLMS.DtcMissMatch.DTRMissmatchEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        table#ContentPlaceHolder1_grdDtrDetails {
            overflow: scroll !important;
            table-layout: fixed;
        }
    </style>
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ValidateMyForm() {
            debugger;
            if (document.getElementById('<%= txtDtrCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTr Code')
                document.getElementById('<%= txtDtrCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbStore.ClientID %>').value.trim() == "--Select--") {
                alert('Select Store')
                document.getElementById('<%= cmbStore.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbStatus.ClientID %>').value.trim() == "--Select--") {
                alert('Select Condition of the DTr ')
                document.getElementById('<%= cmbStatus.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%=BtnSend.ClientID %>').value.trim() == "BtnSend") {
                return confirm("Are You Sure Want To Send DTr To Store");
            }
        }
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

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
                <h3 class="page-title">DTr UnMapping
                </h3>

                <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle"
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
                                    <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Send DTR To Store
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
                            <i class="icon-reorder"></i>Send DTr To Store</h4>
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
                                    <div id="Div1" class="span5" runat="server">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" AutoPostBack="true"
                                                        OnTextChanged="btnDtrSearch_Click" onchange="ValidateDtrCode(this);"
                                                        MaxLength="8"></asp:TextBox>
                                                    <asp:Button ID="btnDtrSearch" runat="server" Text="S" TabIndex="2"
                                                        CssClass="btn btn-primary"
                                                        OnClick="btnDtrSearch_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Remarks <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div2" class="span5" runat="server">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Select Store <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbStore" runat="server" Width="150px" TabIndex="9">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Condition of DTr <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbStatus" runat="server" Width="150px" TabIndex="9">
                                                        <asp:ListItem Text="--Select--" Value="0" />
                                                        <asp:ListItem Text="Brand New" Value="1" />
                                                        <asp:ListItem Text="Repair Good" Value="2" />
                                                        <asp:ListItem Text="Released Good" Value="11" />
                                                        <%--<asp:ListItem Text="Faulty" Value="3" />
                                                        <asp:ListItem Text="Scrap" Value="4" />--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span3">
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
            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>DTr Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <asp:GridView ID="grdDtrDetails" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered table-advance table-hover">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC CODE" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DT_CODE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="150px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="TC Slno" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_SLNO") %>'
                                                            Style="word-break: break-all;"
                                                            Width="150px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="TC Capacity"
                                                    Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'
                                                            Style="word-break: break-all;"
                                                            Width="150px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CURRENT_LOCATION"
                                                    HeaderText="TC Current Location"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCurrentLocation" runat="server"
                                                            Text='<%# Bind("TC_CURRENT_LOCATION") %>'
                                                            Style="word-break: break-all;" Width="200px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="TC Status"
                                                    Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'
                                                            Style="word-break: break-all;"
                                                            Width="200px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_Location" HeaderText="TC Location"
                                                    Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffNmae" runat="server" Text='<%# Bind("OFFNAME") %>'
                                                            Style="word-break: break-all;"
                                                            Width="200px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="space20">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="text-center" align="center">
                    <asp:Button ID="BtnSend" runat="server" Text="Send To Store" CssClass="btn btn-primary"
                        OnClientClick="javascript:return ValidateMyForm()" OnClick="BtnSend_Click" />
                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                        CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
