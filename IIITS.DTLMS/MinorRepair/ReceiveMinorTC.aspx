<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ReceiveMinorTC.aspx.cs" Inherits="IIITS.DTLMS.MinorRepair.ReceiveTC" %>

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
                    <h3 class="page-title">Receive DTR
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="cmdClose_Click" />
                </div>
            </div>
            <!-- END PAGE HEADER-->

            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
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
                                                    Work Order No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWoNo" runat="server" AutoPostBack="true" TabIndex="1"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfWorkorderId" runat="server" />
                                                        <asp:HiddenField ID="hdfTCCode" runat="server" />
                                                        <asp:HiddenField ID="hdfFailureDate" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWrkdate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtwrkId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO Raised By</label>
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
                                                        <asp:HiddenField ID="hdfTCType" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Estimation No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstimationNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Estimation Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstDate" runat="server" ReadOnly="true"></asp:TextBox>
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
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue">View DTC Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Old Transformer Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOldTcCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Old Transformer Capacity(in KVA)</label>
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
                                                    Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Replacement Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTransDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtTransDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtTransDate" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
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
                                <i class="icon-reorder"></i>DTR Details</h4>
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
                                                            <asp:TextBox ID="txtTCCode" runat="server" MaxLength="10" AutoPostBack="true"
                                                                onkeypress="javascript:return OnlyNumber(event);" TabIndex="3" OnTextChanged="txtTCCode_TextChanged" ></asp:TextBox>
                                                            <asp:Button ID="btnSearch" runat="server" Text="S" CssClass="btn btn-primary"
                                                                TabIndex="4" OnClick="btnSearch_Click" />
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

                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        DTr Capacity(in KVA)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtCapacity" runat="server" ReadOnly="true" TabIndex="6"></asp:TextBox>
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
                                            <div class="space20">
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
                                                        Remarks</label>
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
                <div class="span2">
                    <asp:Button ID="cmdViewWO" runat="server" Text="View Work Order" CssClass="btn btn-primary"
                      TabIndex="13" OnClick="cmdViewWO_Click" />
                </div>
                <div class="space20">
                </div>
                <!-- END FORM-->
            </div>

        </div>
    </div>
</asp:Content>
