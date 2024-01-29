<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DTrAllocation1.aspx.cs" Inherits="IIITS.DTLMS.Transaction.DTrAllocation1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                alert('Select Valid First DTr Code')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtSecondDtrCode.ClientID %>').value.trim() == "") {
                alert('Select Valid Second DTr Code')
                document.getElementById('<%= txtSecondDtrCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtfirstDtcCode.ClientID %>').value.trim() == "") {
                alert('DTC Code should not be empty')
                document.getElementById('<%= txtfirstDtcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtSecondDtcCode.ClientID %>').value.trim() == "") {
                alert('DTC Code should not be empty')
                document.getElementById('<%= txtSecondDtcCode.ClientID %>').focus()
                return false
            }
        }
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
                <h3 class="page-title">
                    DTR Swapping
                </h3>
                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                        <div class="input-append search-input-area">
                            <input class="" id="appendedInputButton" type="text" />
                            <button class="btn" type="button">
                                <i class="icon-search"></i>
                            </button>
                        </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
            <%-- <div style="float:right;margin-top:20px;margin-right:12px" >
                    <asp:Button ID="cmdClose" runat="server" Text="Close"  OnClientClick="javascript:window.location.href='FaultTCSearch.aspx'; return false;"
                                    CssClass="btn btn-primary" />
            </div>--%>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DTR Swapping</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <%-- <div class="span1"></div>--%>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                First DTr Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10"></asp:TextBox>
                                                    <asp:Button ID="cmdSearchId" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearchId_Click" /><br />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Sl No</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtfirstDtrSlNo" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFirstCapacity" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Make Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFirstDtrName" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtfirstDtcCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Second DTr Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtSecondDtrCode" runat="server" MaxLength="10"></asp:TextBox>
                                                    <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" /><br />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Sl No</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtSecondDtrSlNo" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtSecondCapacity" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Make Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtSecondDtrName" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtSecondDtcCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <div class="form-horizontal">
                                <div class="span3">
                                </div>
                                <div class="span2">
                                    <asp:Button ID="cmdAllocate" runat="server" Text="Swap" CssClass="btn btn-primary"
                                        Width="116px" OnClick="cmdAllocate_Click" OnClientClick="javascript:return ValidateMyForm()" />
                                </div>
                                <div class="span2">
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-primary"
                                        Width="116px" OnClick="cmdReset_Click" />
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
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <div class="span3">
    </div>
    <asp:Label ID="lblTcDetails" runat="server" ForeColor="Blue"></asp:Label>
</asp:Content>
