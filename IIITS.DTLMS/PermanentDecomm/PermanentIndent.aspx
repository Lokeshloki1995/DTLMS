<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PermanentIndent.aspx.cs" Inherits="IIITS.DTLMS.PermanentDecomm.PermanentIndent" %>

<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

       


        function ValidateMyForm() {

            if (document.getElementById('<%= txtWONo.ClientID %>').value.trim() == "") {
                alert('Enter Valid Work Order No.')
                document.getElementById('<%= txtWONo.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtIndentNo.ClientID %>').value.trim() == "") {
                alert('Enter Valid Indent No.')
                document.getElementById('<%= txtIndentNo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtIndentDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Indent Date')
                document.getElementById('<%= txtIndentDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbStoreName.ClientID %>').value == " - Select - ") {
                alert('Select Store Name')
                document.getElementById('<%= cmbStoreName.ClientID %>').focus()
                return false
            }
           // if (document.getElementById('<%=cmbStoreType.ClientID%>')).value == "--Select--"){
              //  alert('Select Store Type')
               // document.getElementById('<%=cmbStoreType.ClientID%>').focus()
               // return false
           // }

       

            var FromdateInput = document.getElementById('<%= txtIndentDate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
            if (!FromdateInput.match(goodDate)) {
                alert("Please enter valid Indent date");
                document.getElementById('<%= txtIndentDate.ClientID %>').focus()
                return false;
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
                    <h3 class="page-title">Permanent Indent Creation
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
                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWONo" runat="server" MaxLength="17" TabIndex="1"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click"
                                                            TabIndex="2" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfLocCode" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfGuarenteeType" runat="server" />
                                                        <asp:TextBox ID="txtWODate" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO Issued By</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIssuedBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtDTCId" runat="server" Visible="false" Width="20px" MaxLength="100"></asp:TextBox>
                                                        <asp:TextBox ID="txtTCId" runat="server" Visible="false" Width="20px" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Requested Capacity(in KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfCron" runat="server" />
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:TextBox ID="txtNewCapacity" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                        </div>
                                        <div class="span5" id="dvFailure" runat="server">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Transfromer Centre Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTCCode" runat="server" ReadOnly="true"></asp:TextBox><br />
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                            OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Transfromer Centre Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtFailureDate" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display:none">
                                                <label class="control-label">
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure ID"></asp:Label>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFailureID" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfAvailQuantity" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTr Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                            OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
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
                                <i class="icon-reorder"></i>Permanent Indent Creation</h4>
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
                                                        Indent No. <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtIndentNo" runat="server" MaxLength="50" TabIndex="2"></asp:TextBox>
                                                            <asp:TextBox ID="txtIndentId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:TextBox ID="txtWOSlno" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfWOslno" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Indent Date <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtIndentDate" runat="server" ReadOnly="true" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtIndentDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtIndentDate" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group" style="display:none">
                                                    <label class="control-label">
                                                        DTR Drawn From <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbStoreType" runat="server" AutoPostBack="true" TabIndex="4" OnSelectedIndexChanged="cmbStoreType_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="1">STORE</asp:ListItem>
                                                                <asp:ListItem Value="2">BANK</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Store Name<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbStoreName" runat="server" Enabled="false" TabIndex="4">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Indent Description
                                                    </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtIndentDesc" runat="server" MaxLength="500" TabIndex="5" onkeyup="return ValidateTextlimit(this,500);"
                                                                Style="resize: none" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="form-horizontal" align="center">
                                    <div class="space20">
                                    </div>
                                    <div class="span1">
                                    </div>
                                    <div class="span3" style="display:none">
                                        <asp:CheckBox ID="chkAlert" runat="server" Text=""
                                            CssClass="checkbox" Checked="true" TabIndex="6" />
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="span6">
                                        <span class="Mandotary" runat="server" visible="false" id="spanQuant">*</span>
                                        <%--<asp:HyperLink ID="lblQuantityMsg" runat="server" ForeColor="Green" Font-Size="Medium"></asp:HyperLink>--%>
                                        <asp:LinkButton ID="lnkQuantityMsg" runat="server" Visible="false" OnClick="LinkButton1_Click" ForeColor="Green" Font-Size="Medium">HyperLink</asp:LinkButton>
                                        <%--<asp:HyperLink ID="HyperQuantityMsg" OnClick="GetStore_TcDetails" runat="server" ForeColor="Green" Font-Size="Medium" Text="">HyperLink</asp:HyperLink>--%>
                                        <%--<asp:Label ID="lblQuantityMsg" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <!-- END FORM-->
                    </div>
                </div>

                <uc1:ApprovalHistoryView ID="ApprovalHistoryView" runat="server" />
                <!-- END SAMPLE FORM PORTLET-->
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
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="8" TextMode="MultiLine"
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
                        <asp:Button ID="cmdSave" runat="server" Text="Save" TabIndex="7" OnClientClick="javascript:return AlertTCCount()"
                            CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                    </div>
                    <div class="span2">
                        <asp:Button ID="cmdViewWO" runat="server" Text="View Work Order" CssClass="btn btn-primary"
                            OnClick="cmdViewWO_Click" TabIndex="13" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <div class="space20">
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
</asp:Content>