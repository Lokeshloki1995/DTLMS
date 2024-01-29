<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PermanentCR.aspx.cs" Inherits="IIITS.DTLMS.PermanentDecomm.PermanentCR" %>
<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ValidateMyForm() {
           // i//f (document.getElementById('<%= txtInvQty.ClientID %>').value.trim() == "") {
                //alert('Enter Valid  Commissioning Inventory Quantity')
               // document.getElementById('<%= txtInvQty.ClientID %>').focus()
               //return false
          // }
           if (document.getElementById('<%= txtDecommInventry.ClientID %>').value.trim() == "") {
                alert('Enter Valid  DeCommissioning Inventory Quantity')
                document.getElementById('<%= txtDecommInventry.ClientID %>').focus()
               return false
           }
           if (document.getElementById("<%=txtcertify.ClientID %>").checked != true) {
                alert('You must agree to the terms first ')
                document.getElementById('<%= txtDecommInventry.ClientID %>').focus()
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
                <h3 class="page-title">Permanent CR Report
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
                            <i class="icon-reorder"></i>Permanent CR Report</h4>
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
                                                Transformer Centre Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:TextBox ID="txtDTCId" runat="server" ReadOnly="true" Width="20px" Visible="false"></asp:TextBox>
                                                    <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTCDetails_Click">View Transformer Centre Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                <asp:Label ID="lblFailDTr" runat="server" Text="DTr Code"></asp:Label>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:TextBox ID="txtFailureDTr" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:TextBox ID="txtFailDTrId" runat="server" ReadOnly="true" Width="20px" Visible="false"></asp:TextBox>
                                                    <asp:LinkButton ID="lnkFailDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkFailDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" style="display:none">
                                            <label class="control-label">
                                                 DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtNewDTr" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:TextBox ID="txtNewDTrId" runat="server" ReadOnly="true" Width="20px" Visible="false"></asp:TextBox>
                                                    <asp:LinkButton ID="lnkNewDTr" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkNewDTr_Click">View DTr Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" style="display: none">
                                            <label class="control-label">
                                                Store keeper Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtType" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    <asp:TextBox ID="txtStoreKeepName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                RI No</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtRINo" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Work Order Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtWrkOrderDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group" style="display:none">
                                            <label class="control-label">
                                                Commissioning Work Order NO</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtcomWO" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                DeCommissioning Work Order NO</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDEcomWO" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="control-group" id="histroy" runat="server">
                                            <label class="control-label">View Histroy</label>
                                            <div class="controls">
                                                <asp:LinkButton runat="server" ID="lnkHistory" ToolTip="View History" OnClick="lnkHistory_Click">
                                         <img src="../img/Manual/View1.jpg" style="width:20px" alt="view" /></asp:LinkButton>
                                            </div>
                                        </div>--%>
                                    </div>
                                    <div class="span5">
                                        <asp:Panel ID="pnlApproval" runat="server">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRIDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks From Store keeper
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemarksStoreKeeper" runat="server" MaxLength="500" onkeyup="return ValidateTextlimit(this,500);"
                                                            Style="resize: none" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Acceptence Date
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAcceptDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtWFOAuto" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Oil capacity(in Litre)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilCapacity" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtDecommId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtWFOId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: none">
                                                <label class="control-label">
                                                    Store Officer Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtStoreOffName" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks from Store Officer</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemStoreOfficer" runat="server" MaxLength="500" onkeyup="return ValidateTextlimit(this,500);"
                                                            Style="resize: none" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" style="display:none">
                                                <label class="control-label">
                                                    Commissiong Inventory Qty<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvQty" runat="server" MaxLength="5" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    DeCommissioning Inventory Qty<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDecommInventry" runat="server" MaxLength="5" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </asp:Panel>
                                    </div>
                                    <div class="span1">
                                    </div>
                                </div>
                                <asp:CheckBox ID="txtcertify"  runat="server"  Checked="true" Enabled="false" />
                                <asp:Label ID="Lbl" runat="server"
                                    Text="I certify that all the items that have been issued are used for this workorder."></asp:Label>
                                <div class="space20">
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- END FORM-->
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
                        <asp:Button ID="cmdCR" runat="server" Text="CR Report" CssClass="btn btn-primary"
                            OnClick="cmdCR_Click" OnClientClick="javascript:return ValidateMyForm()" />
                    </div>
                    <div class="span2">
                        <asp:Button ID="cmdViewRI" runat="server" Text="View RI" CssClass="btn btn-primary"
                            OnClick="cmdViewRI_Click" TabIndex="13" />
                    </div>
                    <%-- <div class="span1"></div>--%>
                    <div class="span7">
                    </div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
        </div>
    </div>
</asp:Content>
