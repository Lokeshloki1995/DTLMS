<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="clsDTrMappingToDiv.aspx.cs" 
    Inherits="IIITS.DTLMS.MasterForms.DTrMappingToDiv" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

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

        function AvoidSpaceandsplchar(event) {
            var k = event ? event.which : window.event.keyCode;
            if (k == 32) return false;
            if (k < 47 || k > 57 && k!=72 && k!=104) {
                return false;
            }          
        }

        function BlockCharSpace(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (code < 45 || code >57 ) {
                e.preventDefault();
            }
        }

        function AllowNumber(field, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var keychar = String.fromCharCode(charCode);
            var code = ('charCode' in e) ? e.charCode : e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57) && keychar != "-") {
                return false;
            }
            if (keychar == "." && field.value.indexOf(".") != -1) {
                return false;
            }
            if (charCode < 48 || charCode > 57)
            {
                return false;
            }
            if (keychar == "-") {
                if (field.value.indexOf("-") != -1) {
                    return false;
                }
                else {
                    //save caret position
                    var caretPos = getCaretPosition(field);
                    if (caretPos != 0) {
                        return false;
                    }
                }
            }
            return true;
        }

        function onlynumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode < 47 || charCode > 57) {
                return false;
            }
            return true;
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
                    <h3 class="page-title">DTR Mapping To Division
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
                </div>
                  <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="btnview" runat="server" Text="DTr Mapping View" 
                                      OnClientClick="javascript:window.location.href='DTrMappingToDivView.aspx'; return false;"
                            CssClass="btn btn-success" /></div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>DTR Mapping </h4>
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
                                                <label class="control-label">Division Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" MaxLength="25" ReadOnly="True"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Start Range<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTrStartRange" runat="server" MaxLength="7"  AutoComplete="Off"
                                                            onkeypress="return AvoidSpaceandsplchar(event)" AutoPostBack="true" 
                                                            OnTextChanged="txtQuantity_OnTextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- another span--%>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Quantity<span class="Mandotary"> *</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="5" onkeypress="return onlynumbers(event)" 
                                                         AutoComplete="Off"    AutoPostBack="true" OnTextChanged="txtQuantity_OnTextChanged"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr End Range </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTrEndRange" runat="server" MaxLength="20" ReadOnly="True"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="space20"></div>
                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">
                                        <div class="text-center">
                                            <asp:Button ID="btnSave" runat="server" Text="Save"
                                                CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="space20"></div>
                        <!-- END FORM-->
                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <div class="space20"></div>
    <div class="space20"></div>
    <div class="space20"></div>
    <div class="space20"></div>

    <asp:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup" CancelControlID="cmdClose"
        PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
    <div style="width: 100%; vertical-align: middle" align="center">

        <asp:Panel ID="pnlControls" runat="server" CssClass="modalPopup" align="center" Style="display: none">
            <div class="widget blue">
                <div class="widget-title">
                    <h4>Select Division Codes And Click On Proceed</h4>
                    <div class="space20"></div>

                    <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                        runat="server" OnPageIndexChanging="GrdOffices_PageIndexChanging" ShowHeaderWhenEmpty="True"
                        EmptyDataText="No Records Found" ShowFooter="true"
                        PageSize="6" Width="90%" OnRowDataBound="GrdOffices_RowDataBound"
                        AllowPaging="True" DataKeyNames="OFF_CODE" OnRowCommand="GrdOffices_RowCommand">
                        <Columns>
                            <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Division Code" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Division Code" Width="100px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Division Name" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaDesc" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all" Width="150px"> </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Division Name" Width="200px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select" Visible="true">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" />
<%--                                      <asp:RadioButton ID="cbSelect" runat="server" />--%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <div class="row-fluid">
                        <div class="span1"></div>
                        <div class="span2">
                            <div class="control-group">
                                <div class="controls">
                                    <div class="input-append">
                                        <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" Style="left: 0px; top: 0px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span2">
                            <div class="control-group">
                                <div class="controls">
                                    <div class="input-append">
                                        <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
