<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PaymentDetails.aspx.cs" Inherits="IIITS.DTLMS.Billing.PaymentDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
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
                    <h3 class="page-title">Payment Details
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="cmdClose_Click"/>
                </div>
            </div>
            <!-- END PAGE HEADER-->

            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Bill Details</h4>
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
                                                    Invoice No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvNo" runat="server" TabIndex="1"></asp:TextBox>
                                                        <asp:TextBox ID="txtInvId" runat="server" TabIndex="1" Visible="false"></asp:TextBox>      
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                         <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOAutoId" runat="server" />                                         
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Invoice Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvdate" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                        </div>




                                        <div class="span5" runat="server" id="dvOld">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Invoice Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvAmount" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkInvDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkInvDetails_Click">View Bill Details</asp:LinkButton>
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
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Payment Details</h4>
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
                                                        Payment Mode <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbPaymode" runat="server"  TabIndex="1">
                                                                <asp:ListItem Value="0">--SELECT--</asp:ListItem>
                                                                <asp:ListItem Value="1">CHEQUE</asp:ListItem>
                                                                <asp:ListItem Value="2">DD</asp:ListItem>
                                                                <asp:ListItem Value="3">RTGS</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Reference No</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtRefno" runat="server" TabIndex="2"  MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Transaction Date</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtDate" runat="server" TabIndex="3" MaxLength="10"></asp:TextBox>
                                                             <asp:CalendarExtender ID="CalendarExtender1_txtDate" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Bill No <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtBillNo" runat="server" TabIndex="4"  MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                            </div>
                                            <div>
                                            </div>
                                         
                                            <div class="space20">
                                            </div>
                                            <!-- END SAMPLE FORM PORTLET-->
                                        </div>
                                    </div>
                                    <!-- END PAGE CONTENT-->
                                   
                                    <div class="space20">
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>


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
                    <asp:Button ID="cmdSave" runat="server" Text="Submit"  TabIndex="11"
                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                </div>
               
                <div class="space20">
                </div>
                <!-- END FORM-->
            </div>

        </div>
    </div>

    </div>
</asp:Content>
