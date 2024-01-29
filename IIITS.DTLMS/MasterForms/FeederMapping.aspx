<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="FeederMapping.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.FeederMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>--%>
    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
    rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>--%>

    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>

    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })


            $('[id*=lblMultiSelect]').multiselect({
                includeSelectAllOption: true
            });
        });

    </script>


    <script type="text/javascript">
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>

    <script type="text/javascript">
        function onlyAlphabets(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code == 32) &&
                !(code > 39 && code < 42) &&
                !(code > 45 && code < 48) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }
    </script>
    <%--<script type="text/javascript">
        $(function () {
            $('[id*=lblMultiSelect]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>--%>
    <style type="text/css">
        .modalBackground {
            /* background-color: Gray; */
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background: #fff;
            width: 600px;
            height: 500px;
        }
    </style>

    <script type="text/javascript">
        function Validate() {
            if (document.getElementById('<%= cmbdistrict.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbdistrict.ClientID %>').value.trim() == "") {
                alert('Please Select District')
                document.getElementById('<%= cmbdistrict.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%=cmbTaluk.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%=cmbTaluk.ClientID %>').value.trim() == "") {
                alert('Please Select Taluk')
                document.getElementById('<%= cmbTaluk.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbStation.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbStation.ClientID %>').value.trim() == "") {
                alert('Please Select Station')
                document.getElementById('<%= cmbStation.ClientID %>').focus()
                return false
            }

        }
    </script>
    <script type="text/javascript">        //
        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdFeeder').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({

                "sPaginationType": "full_numbers"
            });
        });
    </script>
    <style type="text/css">
        .multiselect-container .multiselect-option .form-check, .multiselect-container .multiselect-group .form-check, .multiselect-container .multiselect-all .form-check {
            padding: 0 5px 0 20px;
            display: flex !important;
        }

        button.multiselect.dropdown-toggle.custom-select.text-center {
            height: 27px !important;
            white-space: nowrap;
            width: 232px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .multiselect-container .multiselect-option.dropdown-item, .multiselect-container .multiselect-group.dropdown-item, .multiselect-container .multiselect-all.dropdown-item, .multiselect-container .multiselect-option.dropdown-toggle, .multiselect-container .multiselect-group.dropdown-toggle, .multiselect-container .multiselect-all.dropdown-toggle {
            cursor: pointer;
            display: contents;
        }

        .btn-group > .btn, .btn-group > .dropdown-menu, .btn-group > .popover {
            font-size: 14px;
            top: 27px;
        }

        table {
            overflow: scroll;
        }

        td {
            border: 1px solid #ccc;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        table.dataTable thead th {
            border-bottom: 1px solid #111;
            font-size: 12px !important;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 10px 0px !important;
            text-align: center !important;
        }

        .table-advance tr td {
            border-left-width: 1px !important;
            border: 1px solid #d4d4d4;
            font-size: 12px !important;
        }

        .multiselect-container.dropdown-menu {
            max-height: 122px;
            overflow-y: scroll;
        }

        th {
            white-space: nowrap;
        }

        table#ContentPlaceHolder1_grdFeeder th:nth-child(7), table#ContentPlaceHolder1_grdFeeder th:nth-child(8), table#ContentPlaceHolder1_grdFeeder th:nth-child(9), table#ContentPlaceHolder1_grdFeeder th:nth-child(10), table#ContentPlaceHolder1_grdFeeder th:nth-child(11), table#ContentPlaceHolder1_grdFeeder th:nth-child(12), table#ContentPlaceHolder1_grdFeeder th:nth-child(13) {
            display: none !important;
        }

        table#ContentPlaceHolder1_grdFeeder td:nth-child(7), table#ContentPlaceHolder1_grdFeeder td:nth-child(8), table#ContentPlaceHolder1_grdFeeder td:nth-child(9), table#ContentPlaceHolder1_grdFeeder td:nth-child(10), table#ContentPlaceHolder1_grdFeeder td:nth-child(11), table#ContentPlaceHolder1_grdFeeder td:nth-child(12), table#ContentPlaceHolder1_grdFeeder td:nth-child(13) {
            display: none !important;
        }
    </style>
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
                <h3 class="page-title" runat="server" id="Create">Feeder Mapping
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
            <%-- <div style="float: right; margin-top: 20px; margin-right: 12px">
                <asp:Button ID="cmdFeederView" class="btn btn-primary" Text="Feeder View"
                    OnClientClick="javascript:window.location.href='FeederView.aspx'; return false;" runat="server" />
            </div>--%>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4 id="CreateFeeder" runat="server"><i class="icon-reorder"></i>Feeder Mapping</h4>

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
                                            <label class="control-label">District Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbdistrict" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbDistrict_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Taluk Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbTaluk" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbTaluk_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <!--AFTER DIST TALUK -->
                                        <div class="control-group">
                                            <label class="control-label">Station Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbStation" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStation_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtFeederId" runat="server" MaxLength="25" Visible="false">0</asp:TextBox>
                                                    <asp:HiddenField ID="hdfTaluk" runat="server" />
                                                    <asp:HiddenField ID="hdfStation" runat="server" />
                                                    <asp:HiddenField ID="hdfBank" runat="server" />
                                                    <asp:HiddenField ID="hdfBus" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:ListBox runat="server" ID="lblMultiSelect" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="cmdAdd_Click" OnClientClick="javascript:return Validate()" />
                                            <asp:Button ID="Button1" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="cmdReset_Click" />

                                        </div>
                                    </div>
                                    <br />

                                    <%-- OnPageIndexChanging="grdFeeder_PageIndexChanging" AllowPaging="true"  ShowFooter="True" ShowHeaderWhenEmpty="True"--%>
                                    <asp:GridView ID="grdFeeder" CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server" EmptyDataText="No records Found"
                                        OnRowCommand="grdFeeder_RowCommand" OnRowDataBound="grdFeeder_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="SD_ID" HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFeederId" runat="server" Text='<%# Bind("FD_FEEDER_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ST_NAME" HeaderText="Station Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStation" runat="server" Text='<%# Bind("ST_NAME") %>' Style="word-break: break-all"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                                <%--   <FooterTemplate>
                                                    <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtStation" runat="server" CssClass="input_textSearch" Width="150px"
                                                            placeholder="Enter Station Name" ToolTip="Enter Station Name to Search"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="FD_FEEDER_NAME" HeaderText="Feeder Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorpPhone" runat="server" Text='<%# Bind("FD_FEEDER_NAME") %>'
                                                        Width="150px" Style="word-break: break-all"></asp:Label>
                                                </ItemTemplate>
                                                <%--  <FooterTemplate>
                                                    <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtFeederName" runat="server" CssClass="input_textSearch" Width="150px"
                                                            placeholder="Enter Feeder Name" ToolTip="Enter Feeder Name to Search"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="FD_FEEDER_CODE" HeaderText="Feeder Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorpName2" runat="server" Text='<%# Bind("FD_FEEDER_CODE") %>'
                                                        Width="80px" Style="word-break: break-all"></asp:Label>
                                                </ItemTemplate>
                                                <%--  <FooterTemplate>
                                                    <asp:Panel ID="panel3" runat="server" DefaultButton="btnSearch">
                                                        <asp:TextBox ID="txtFeederCode" runat="server" CssClass="input_textSearch" Width="150px"
                                                            placeholder="Enter Feeder Code" ToolTip="Enter Feeder Code to Search"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Width="200px"
                                                        Style="word-break: break-all"></asp:Label>
                                                </ItemTemplate>
                                                <%--<FooterTemplate>
                                                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                        Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                                </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="FD_TYPE" HeaderText="Office Code" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbloffcode" runat="server" Text='<%# Bind("OFF_CODE") %>' Width="150px"
                                                        Style="word-break: break-all"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="status" HeaderText="status" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("status") %>' Width="150px"
                                                        Style="word-break: break-all"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:LinkButton runat="server" CommandName="remove" ID="imgdelete" ToolTip="delete" Visible="true" Height="12px">
                                                    <img src="../Styles/images/delete64x64.png" style="width:12px" /></asp:LinkButton>
                                                        <%--<asp:ImageButton ID="imgdelete" Title="Click To Delete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                        CommandName="remove" Width="12px" />--%>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                    </asp:GridView>
                                    <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                    <div class="space20"></div>


                                </div>
                                <div class="text-center">


                                    <asp:Button ID="cmdSave" runat="server" Text="Save"
                                        OnClientClick="javascript:return Validate()" CssClass="btn btn-success"
                                        OnClick="cmdSave_Click" />

                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
</asp:Content>
