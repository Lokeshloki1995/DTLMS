<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="RIApprove.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.RIApprove" %>

<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtCommentFromStoreKeeper.ClientID %>').value.trim() == "") {
                alert('Enter Remarks for RI Approval')
                document.getElementById('<%= txtCommentFromStoreKeeper.ClientID %>').focus()
                return false
            }

<%--        var FromdateInput = document.getElementById('<%= txtAckDate.ClientID %>').value;--%>
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
<%--        if (!FromdateInput.match(goodDate)) {
            alert("Please enter valid ACK date");
            document.getElementById('<%= txtAckDate.ClientID %>').focus()
            return false;
        }--%>



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
                    <h3 class="page-title">RI Acknowledgement
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary"
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
                                <i class="icon-reorder"></i>RI Acknowledgement</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    <asp:HiddenField ID="hdfDecommDate" runat="server" />

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Slno</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:TextBox ID="txtDTrSlno" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtDTrId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDTCCode" runat="server" />
                                                    <asp:TextBox ID="txtssOfficeCode" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Make</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMake" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" style="display: none">
                                            <label class="control-label">
                                                Failure Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFailureDate" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--                                        <div class="control-group">
                                            <label class="control-label">
                                               Oil Quantity(In Litre) Entered By SO</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtOilQuantity" runat="server" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Decommission RI Number
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtRIno" runat="server" ReadOnly="true" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Decommission RI Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDecommDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Ack Number
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                    <asp:TextBox ID="txtAckNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>

                                        <%--                                        <div class="control-group">
                                                <label class="control-label">
                                                    Work Order Number
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append"> 
                                                        <asp:TextBox ID="TextBox1" runat="server"  MaxLength="500" TabIndex="2" TextMode="MultiLine"
                                                             ReadOnly="true"  Height="80px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,250)" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Work Order Number
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDeCommWO" runat="server" ReadOnly="true" Style="resize: none" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <%--                                         <div class="control-group">
                                                <label class="control-label">
                                                    Work Order Credit Number
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append"> 
                                                        <asp:TextBox ID="txtWoCreditNo" runat="server"  MaxLength="500" TabIndex="2" TextMode="MultiLine"
                                                             ReadOnly="true"  Height="80px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,250)" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                    </div>
                                    <div class="span5">
                                        <asp:Panel ID="pnlApproval" runat="server">

                                            <div class="control-group">
                                                <label class="control-label">Quantity Of Oil(as per Tank Capacity)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantityOfOil" runat="server" MaxLength="6" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Oil Level<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="oiltankcapacity" runat="server" MaxLength="6" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Oil Quantity(In Litre) <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilQtySK" runat="server" MaxLength="10"  onpaste="return false" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--                                            <div class="control-group">
                                <label class="control-label">
                                                    No Of Empty Barrels <span class="Mandotary">*</span></label>
                                                   <div class="controls">
                                                <div class="input-append">
                                                 <asp:TextBox ID="txtNoOfBarrels" runat="server" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                            </div>
                                                </div>
                                                </div>--%>

                                            <%--   <div class="control-group">
                                                <label class="control-label">
                                                    No Of Barrels <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtNoOfBarrels" runat="server" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Manual Ack Number
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManualAckNo" runat="server" MaxLength="10" autocomplete="off" onpaste="return false" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--<div class="control-group">
                                                <label class="control-label">
                                                    Ack Date
                                                <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAckDate1" runat="server" ReadOnly="true" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server"  autocomplete="off" CssClass="cal_Theme1"
                                                            TargetControlID="txtAckDate1" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <div class="control-group">
                                                <label class="control-label">Ack Date<span class="Mandotary">*</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAcknoledgeDate" runat="server" autocomplete="off" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtAcknoledgeDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfGuarenteeType" runat="server" />
                                                        <asp:TextBox ID="txtDecommId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtFailureId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtCommentFromStoreKeeper" runat="server" TextMode="MultiLine" MaxLength="500"
                                                            Height="50px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,250)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="space20"></div>


                                        </asp:Panel>

                                        <div class="container-fluid">
                                            <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                OnPageIndexChanging="gvFiles_PageIndexChanging" OnSorting="gvFiles_Sorting" AllowSorting="true">
                                                <Columns>

                                                    <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Transformer Survey Report" ItemStyle-Width="300" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Width="150" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile"
                                                                CommandArgument='<%# Eval("Name") %>'> 
                                                            </asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                  
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Width="150" Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadFiledwnld"
                                                       CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                    </div>

                                </div>
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
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine"
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
                        <asp:Button ID="cmdApprove" runat="server" Text="Approve" CssClass="btn btn-primary"
                            OnClick="cmdApprove_Click" OnClientClick="javascript:return ValidateMyForm()" />
                    </div>
                    <div class="span2">
                        <asp:Button ID="cmdViewWO" runat="server" Text="View Work Order" CssClass="btn btn-primary"
                            OnClick="cmdViewWO_Click" TabIndex="13" />
                    </div>
                    <%--  <div class="span2">
                        <asp:Button ID="cmdViewDecomm" runat="server" Text="View Decommission" CssClass="btn btn-primary"
                            OnClick="cmdViewDecomm_Click" TabIndex="13" />
                    </div>--%>
                    <div class="space20">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="span7">
    </div>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
