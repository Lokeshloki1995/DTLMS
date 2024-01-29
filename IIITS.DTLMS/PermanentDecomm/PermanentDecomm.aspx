<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PermanentDecomm.aspx.cs" Inherits="IIITS.DTLMS.PermanentDecomm.PermanentDecomm" %>
<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">


        function ValidateMyForm() {

          
            if (document.getElementById('<%= txtDecommDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Decommission Date')
                document.getElementById('<%= txtDecommDate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRINo.ClientID %>').value.trim() == "") {
                alert('Enter Valid RI Number')
                document.getElementById('<%= txtRINo.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtRIDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid RI Date')
                document.getElementById('<%= txtRIDate.ClientID %>').focus()
                return false

            }

            var FromdateInput = document.getElementById('<%= txtDecommDate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
            if (!FromdateInput.match(goodDate)) {
                alert("Please enter valid Decommission date");
                document.getElementById('<%= txtDecommDate.ClientID %>').focus()
                return false;
            }

            if (document.getElementById('<%= TxtCommDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid DTR Commission Date')
                document.getElementById('<%= TxtCommDate.ClientID %>').focus()
                return false
            }

            var FromdateInput = document.getElementById('<%= txtRIDate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
            if (!FromdateInput.match(goodDate)) {
                alert("Please enter valid RI date");
                document.getElementById('<%= txtRIDate.ClientID %>').focus()
                return false;
            }

            //          if (document.getElementById('<%= txtTrReading.ClientID %>').value.trim() == "") {
            //              alert('Enter Valid TR reading')
            //              document.getElementById('<%= txtTrReading.ClientID %>').focus()
            //              return false
            //          }
            if (document.getElementById('<%= cmbStore.ClientID %>').value == "-Select-") {
                alert('Select Store')
                document.getElementById('<%= cmbStore.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtOilQuantity.ClientID %>').value.trim() == "") {
                alert('Please enter oil qty')
                document.getElementById('<%= txtOilQuantity.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRemarks.ClientID %>').value.trim() == "") {
                alert('Enter Valid Remarks')
                document.getElementById('<%= txtRemarks.ClientID %>').focus()
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
                <h3 class="page-title">Permanent Decommissioning
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
                <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-danger"
                    OnClick="cmdClose_Click" />
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
                            <i class="icon-reorder"></i>Basic Details</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group" style="display:none">
                                            <label class="control-label">
                                                <asp:Label ID="lblIDText" runat="server" Text="Failure Id"></asp:Label>
                                                <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                    <asp:TextBox ID="txtFailureId" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                        MaxLength="10" TabIndex="1"></asp:TextBox>
                                                    <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click"
                                                        TabIndex="2" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Transformer Centre Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                    <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox><br />
                                                    <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTCDetails_Click">View Transformer Centre Details</asp:LinkButton>
                                                    <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Transformer Centre Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtInvoiceId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>                                        
                                    </div>
                                    <div class="span5">
                                        <div class="control-group" style="display:none">
                                            <label class="control-label">
                                                <asp:Label ID="lblDateText" runat="server" Text="Failure Date"></asp:Label>
                                            </label>
                                            <div class="controls" style="display:none">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfOfficeCode" runat="server" />
                                                    <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                    <asp:TextBox ID="txtFailureDate" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Make Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfDTCId" runat="server" />
                                                    <asp:HiddenField ID="hdfTCId" runat="server" />
                                                    <asp:TextBox ID="txtMake" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtReplaceId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtCapcity" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Permanent Decommissioning</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <asp:Panel ID="pnlApproval" runat="server">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Decommissioning Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDecommDate" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDecommDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Manual RI Number<span class="Mandotary"> </span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManualRINo" runat="server" MaxLength="25" TabIndex="4"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Number<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRINo" runat="server" MaxLength="10" TabIndex="4" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRIDate" runat="server"  MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtRIDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTR Reading
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTrReading" runat="server" MaxLength="10" TabIndex="6" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTR Commission Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="TxtCommDate" runat="server"  MaxLength="10" TabIndex="3"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="TxtCommDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Store Name <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Enabled="false" TabIndex="7">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Oil Quantity(in Litre)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilQuantity" runat="server" TabIndex="8" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="500" onkeyup="return ValidateTextlimit(this,500);"
                                                            Style="resize: none" TextMode="MultiLine" TabIndex="9"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1">
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="space20">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <uc1:ApprovalHistoryView ID="ApprovalHistoryView" runat="server" />

                <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
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
                                                            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
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
            </div>
            <div class="form-horizontal" align="center">
                <div class="span3">
                </div>
                <div class="span2">
                    <asp:Button ID="cmdSave" runat="server" Text="Submit" TabIndex="10" OnClientClick="javascript:return ValidateMyForm()"
                        CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                </div>
                <%-- <div class="span1"></div>--%>
                <div class="span1">
                    <asp:Button ID="cmdReset" runat="server" Text="Reset" TabIndex="11" CssClass="btn btn-primary"
                        OnClick="cmdReset_Click" /><br />
                </div>
               <%-- <div class="span2" >
                    <asp:Button ID="cmdViewInvoice" runat="server" Text="View Indent" CssClass="btn btn-primary"
                        OnClick="cmdViewIndent_Click" TabIndex="13" />
                    
                </div>--%>
                <%--<div class="span2">
                        <asp:Button ID="Button1" runat="server" Text="View Work Order" CssClass="btn btn-primary"
                            OnClick="cmdViewWO_Click" TabIndex="13" />
                    </div>--%>
                <div class="span7">
                </div>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <div class="space20">
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
        </div>
    </div>
    <!-- END PAGE CONTENT-->
</asp:Content>
