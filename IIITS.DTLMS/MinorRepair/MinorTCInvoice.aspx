<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MinorTCInvoice.aspx.cs" Inherits="IIITS.DTLMS.MinorRepair.MinorTCInvoice" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        left: -4px;
        top: -5px;
    }
        </style>

    <script type="text/javascript">

        function ValidateMyForm() {

            if (document.getElementById('<%= txtTansDate.ClientID %>').value.trim() == "") {
                alert('Enter Replacement Date')
                document.getElementById('<%= txtTansDate.ClientID %>').focus()
                return false
            }            
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        Acknowledgement
                    </h3>

                    <div class="span5">
                    <asp:Button ID="button" runat="server" Text="Close" CssClass="btn btn-primary"
                        OnClick="cmdClose_Click" TabIndex="13" />
                </div>

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
                <%--<div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClick="cmdClose_Click" CssClass="btn btn-primary" />
                </div>--%>
            </div>
            <!-- END PAGE HEADER-->
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
                                                               onkeypress="javascript:return OnlyNumber(event);" ReadOnly="true"   TabIndex="3"></asp:TextBox>
                                                            <%--<asp:Button ID="btnSearch" runat="server" Visible="false" Text="S" CssClass="btn btn-primary" OnClick="btnSearch_Click"
                                                                TabIndex="4" style="left: -4px; top: -5px" />--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <%--<label class="control-label">
                                                        DTr Make</label>--%>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <%--<asp:TextBox ID="txtTcMake" runat="server" ReadOnly="true" TabIndex="5"></asp:TextBox>--%>
                                                            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:TextBox ID="txtwrkId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        DTr Capacity(in KVA)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTcCapacity" runat="server" ReadOnly="true" TabIndex="6" CssClass="auto-style1"></asp:TextBox>
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

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        WorkOrder No. <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtWorkOrderNo" runat="server" TabIndex="7" ReadOnly="true"></asp:TextBox>
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
                                                        Transformer Centre Code <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="TxtDtcCode" runat="server" MaxLength="10" AutoPostBack="true" ReadOnly="true"
                                                               onkeypress="javascript:return OnlyNumber(event);"   TabIndex="3" CssClass="auto-style1"></asp:TextBox>
                                                            <%--<asp:Button ID="btnDtcSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                                TabIndex="4" OnClick="btnDtcSearch_Click" />--%>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Failure ID </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtFailureId" runat="server" MaxLength="25" ReadOnly="true" TabIndex="6"></asp:TextBox>
                                                        </div>
                                                        <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtType" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                    </div>
                                                </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Receive DTR Date </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtReceiveDate" runat="server" ReadOnly="true" MaxLength="11" TabIndex="6"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <%--<div class="control-group">
                                                    <label class="auto-style2">
                                                        Manual Invoice NO </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="TxtManualInvoice" runat="server" MaxLength="11" TabIndex="6"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>--%>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Replacement Date <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTansDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtTansDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtTansDate" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="control-group">
                                                    <label class="control-label">
                                                        Amount(Rs) <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" TabIndex="9" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>--%>
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
                                                            <asp:TextBox ID="txtComment" MaxLength="500" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);"
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

             <div class="form-horizontal" align="center">
                <div class="span3">
                </div>
                <div class="span2">
                    <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click" TabIndex="11"
                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                </div>
                 <div class="span2">
                    <asp:Button ID="cmdViewDTR" runat="server" Text="View Receive DTR" CssClass="btn btn-primary"
                      TabIndex="13" OnClick="cmdViewDTR_Click" />
                </div>
                <div class="span2">
                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-primary"
                        OnClick="cmdReset_Click" TabIndex="13" />
                </div>
                <div class="space20">
                </div>
                <!-- END FORM-->
            </div>
</asp:Content>
