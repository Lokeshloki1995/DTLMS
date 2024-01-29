<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="Allocate.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.Allocate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= txtDtcCode.ClientID %>').value.trim() == "") {
                alert('Please Enter DTC Code')
                document.getElementById('<%= txtDtcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDtrCode.ClientID %>').value.trim() == "") {
                alert('Please Enter DTr Code')
                document.getElementById('<%= txtDtrCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%=cmdAllocate.ClientID %>').value.trim() == "Allocate") {
                return confirm("Are You Sure Want To Allocate DTC");
            }
        }

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
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
                <h3 class="page-title">Allocate UnMapped DTC
                </h3>
                <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
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
                                    <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Allocate UnMapped DTC
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
                            <i class="icon-reorder"></i>DTC Allocation</h4>
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
                                                    <asp:TextBox ID="txtDtcCode" runat="server" OnTextChanged="btnDtcSearch_Click" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                                    <asp:Button ID="btnDtcSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                        OnClick="btnDtcSearch_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Remarks</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div1" class="span5" runat="server">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" OnTextChanged="btnDtrSearch_Click" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                                    <asp:Button ID="btnDtrSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                        OnClick="btnDtrSearch_Click" />
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
                            <i class="icon-reorder"></i>UnMapped DTC Details</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <div class="form-horizontal">
                                <div class="row-fluid" style="width: 50%">
                                    <asp:GridView ID="grdDtcDetails" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        Width="300px" OnRowCreated="grdDtcDetails_RowCreated">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("dt_code") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="DTC Name" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Capacity" HeaderText="DTC Location"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("DT_OM_SLNO") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_tc_id" HeaderText="Dtr Code"
                                                Visible="false" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("DT_TC_ID") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div style="margin-left: 600px; margin-top: -115px;">

                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered table-advance table-hover"
                                            OnRowCreated="GridView1_RowCreated">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="TC Code" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DME_EXISTING_DTR_CODE") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="TC Capacity" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC CODE"
                                                    Visible="false" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DTCODE") %>'
                                                            Style="word-break: break-all;" Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CURRENT_LOCATION" HeaderText="Current Location" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrentLocation" runat="server" Text='<%# Bind("TC_CURRENT_LOCATION") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div style="margin-top: 140px;">
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
                OnClientClick="javascript:return ValidateMyForm()" OnClick="cmdAllocate_Click" />
            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                OnClick="cmdReset_Click" /><br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
</asp:Content>
