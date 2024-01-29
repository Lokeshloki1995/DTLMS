<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="BankInvoice.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.BankInvoice" %>
<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateCapacity() {

            var oldCapacity = document.getElementById('<%= txtTcCapacity.ClientID %>').value.trim();
            var NewCapacity = document.getElementById('<%= txtNewtccapacity.ClientID %>').value.trim();
            var x = Number(oldCapacity);
            var y = Number(NewCapacity);
            if (x != y) {
                var result = confirm('Requested DTr Capacity is ' + NewCapacity + ' and Selected DTr Capacity is  ' + oldCapacity + ' Are you sure, Do you want to Continue?');
                if (result) {
                    return true;
                }
                else {

                    return false;
                }
            }
        }


        function ValidateMyForm() {

            if (document.getElementById('<%= txtIndentNo.ClientID %>').value.trim() == "") {
                alert('Select Valid Indent Number')
                document.getElementById('<%= txtIndentNo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtInvoiceNo.ClientID %>').value.trim() == "") {
                alert('Enter Valid Invoice No.')
                document.getElementById('<%= txtInvoiceNo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtInvoiceDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Invoice Date')
                document.getElementById('<%= txtInvoiceDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtAmount.ClientID %>').value.trim() == "") {
                alert('Enter Valid Amount')
                document.getElementById('<%= txtAmount.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtTCCode.ClientID %>').value.trim() == "") {
                alert('Enter Valid DTR Code')
                document.getElementById('<%= txtTCCode.ClientID %>').focus()
                return false
            }

        var FromdateInput = document.getElementById('<%= txtInvoiceDate.ClientID %>').value;
        var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
        if (!FromdateInput.match(goodDate)) {
            alert("Please enter valid Invoice date");
            document.getElementById('<%= txtInvoiceDate.ClientID %>').focus()
            return false;
        }
            
            //                 var sType = document.getElementById('<%= hdfType.ClientID %>').value;
            //                 if (sType != "3") {
            //}
            var sResult = ValidateCapacity();

            if (sResult == false) {
                return false
            }

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
                    <h3 class="page-title">
                        Invoice
                    </h3>
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClick="cmdClose_Click" CssClass="btn btn-primary" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>Transaction Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Indent No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIndentNo" runat="server" AutoPostBack="true" TabIndex="1" OnTextChanged="txtIndentNo_TextChanged"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfIndentId" runat="server" />
                                                        <asp:HiddenField ID="hdfTCCode" runat="server" />
                                                         <asp:HiddenField ID="hdfFailureDate" runat="server" />
                                                        <asp:Button ID="cmdSearchIndent" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                            OnClick="cmdSearchIndent_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Indent Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIndentdate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtIndentId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Indent Raised By</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfType" runat="server" />
                                                        <asp:TextBox ID="txtRaisedBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" id="dvFailure" runat="server">
                                                <label class="control-label">
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure ID"></asp:Label>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFailureId" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtType" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Transformer Centre Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       <%-- txtDtcCode--%>
                                                        <asp:TextBox ID="txtDTCCode1" runat="server" ReadOnly="true"></asp:TextBox>     
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5" runat="server" id="dvOld">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Transformer Centre Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                            OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Old DTr Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOldTcCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                            OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Old DTr Capacity(in KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtoldtccapacity" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtDTCId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtTCId" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    New DTr Capacity(in KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtNewtccapacity" runat="server" ReadOnly="true"></asp:TextBox>
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
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Invoice Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <asp:Panel ID="pnlApproval" runat="server">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div class="form-horizontal">
                                        <div class="row-fluid">
                                            <div class="span1">
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        DTr Code <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTCCode" runat="server" MaxLength="10" AutoPostBack="true" OnTextChanged="txtTCCode_TextChanged"
                                                               onkeypress="javascript:return OnlyNumber(event);"   TabIndex="3"></asp:TextBox>
                                                            <asp:Button ID="btnSearch" runat="server" Text="S" CssClass="btn btn-primary" OnClick="btnSearch_Click"
                                                                TabIndex="4" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        DTr Make</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTcMake" runat="server" ReadOnly="true" TabIndex="5"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        DTr Capacity(in KVA)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTcCapacity" runat="server" ReadOnly="true" TabIndex="6"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        DTR Serial NO <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtSLNo" runat="server" TabIndex="7" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                            </div>
                                            <div>
                                            </div>
                                            <%--  <div class="span1"></div>--%>
                                            <%-- another span--%>
                                            <div class="span5">
                                            <div class="control-group">
                                                    <label class="control-label">
                                                        Invoice No. <span class="Mandotary">*</span></label>
                                                    <asp:TextBox ID="txtInvoiceSlNo" runat="server" Visible="false"></asp:TextBox>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="25" TabIndex="7"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Manual Invoice No</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtManualInvNo" runat="server" MaxLength="25" TabIndex="6" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Invoice Date <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtInvoiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Amount(Rs) <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" TabIndex="9" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--    <div class="control-group">
                                            <label class="control-label">Drawing Description <span class="Mandotary"> *</span></label>
                        
                                            <div class="controls">
                                            <div class="input-append">
                                            <asp:TextBox ID="txtDrawingDescription" runat="server" MaxLength="500" TextMode="MultiLine" 
                                            style="resize:none" onkeyup="return ValidateTextlimit(this,500);"  ></asp:TextBox>
                                                       
                                            </div>
                                            </div>
                                            </div>--%>
                                            </div>
                                            <div class="space20">
                                            </div>
                                            <!-- END SAMPLE FORM PORTLET-->
                                        </div>
                                    </div>
                                    <!-- END PAGE CONTENT-->
                                    <div class="form-horizontal" align="center">
                                        <div class="row-fluid">
                                            <div class="span1">
                                            </div>
                                            <div class="span10">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Description</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtDrawingDescription" MaxLength="500" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);"
                                                                Style="resize: none; width: 650px;" runat="server" TabIndex="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span1">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>

            <uc1:ApprovalHistoryView ID="ApprovalHistoryView" runat="server" />

            <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Comments for Approve/Reject</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Comments<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="12" TextMode="MultiLine"
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
            <div class="form-horizontal" align="center">
                <div class="span3">
                </div>
                <div class="span2">
                    <asp:Button ID="cmdSave" runat="server" Text="Submit" OnClick="cmdSave_Click" TabIndex="11"
                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                </div>
                <div class="span2">
                    <asp:Button ID="cmdViewIndent" runat="server" Text="View Indent" CssClass="btn btn-primary"
                        OnClick="cmdViewIndent_Click" TabIndex="13" />
                </div>
                <div class="space20">
                </div>
                <!-- END FORM-->
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>GatePass</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Vehicle No</label>
                                                <div class="span1">
                                                    <span class="Mandotary">*</span>
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <label class="control-label">
                                                Receipient Name</label>
                                            <div class="span1">
                                                <span class="Mandotary">*</span>
                                            </div>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50"></asp:TextBox>
                                                    <asp:TextBox ID="txtDtcCode" runat="server" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <label class="control-label">
                                            Challen Number</label>
                                        <div class="span1">
                                            <span class="Mandotary">*</span>
                                        </div>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtChallen" runat="server" MaxLength="50"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="form-horizontal" align="center">
                                        <div class="span3">
                                        </div>
                                        <div class="span1">
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass" CssClass="btn btn-primary"
                                                OnClick="cmdGatePass_Click" Enabled="false" />
                                        </div>
                                        <div class="space20">
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
</asp:Content>
