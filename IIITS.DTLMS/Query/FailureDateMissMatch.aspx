<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailureDateMissMatch.aspx.cs"
    Inherits="IIITS.DTLMS.Query.FailureDateMissMatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../Scripts/functions.js" type="text/javascript"></script>--%>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTC Code')
                document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTR Code')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDTrFailDate.ClientID %>').value.trim() == "") {
                alert('Select the DTR Failure Date')
                document.getElementById('<%= txtDTrFailDate.ClientID %>').focus()
                return false
            }
            else {
                var FromdateInput = document.getElementById('<%= txtDTrFailDate.ClientID %>').value;
                var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
                if (!FromdateInput.match(goodDate)) {
                    alert("Please Enter Valid Failure Date Format");
                    return false;
                }
            }
            if (document.getElementById('<%= txtTicketNumber.ClientID %>').value.trim() == "") {
                alert('Please Enter Valid Ticket Number')
                document.getElementById('<%= txtTicketNumber.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%=cmdUpdate.ClientID %>').value.trim() == "Update") {
                return confirm("Are You Sure, You Want To Update Failure Date");
            }
        }

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        function ValidateDtcCode(DtcCode) {
            var reg = new RegExp("^[H][A-Z]+$");
            if (!reg.test(DtcCode.value)) {
                alert("Enter Valid DTC Code");
                DtcCode.value = '';
                return;
            }
        };
        function ValidateTicketNum(TicketNum) {
            var reg = new RegExp("^[H][A-Z0-9]+$");
            if (!reg.test(TicketNum.value)) {
                alert("Enter Valid Ticket Number");
                TicketNum.value = '';
                return;
            }
        };

        function ValidateAlphaNoSplChr(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 39) {
                return true;
            }
            else if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32) {
                return false;
            }
            else
                return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Failure Date Change
                    </h3>
                    <div class="span1">
                        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">
                            <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                    </div>
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Failure Date Change</h4>
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
                                                <label class="control-label">DTC Code <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfDTCcode" runat="server" />
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="6" AutoPostBack="true"
                                                            onchange="ValidateDtcCode(this);" OnTextChanged="btnDtcSearch_Click" 
                                                            onkeypress="javascript:return ValidateAlphaNoSplChr(event);"></asp:TextBox>
                                                        <asp:Button ID="btnDtcSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                            OnClick="btnDtcSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">DTR Code  <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcCode" runat="server" MaxLength="8" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">DTR Failure Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTrFailDate" runat="server" MaxLength="11"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDTrFailDate" Format="dd-MM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Ticket Number <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTicketNumber" runat="server" MaxLength="15"
                                                            onchange="ValidateTicketNum(this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span1"></div>
                                </div>
                            </div>
                        </div>
                        <!-- END FORM-->
                    </div>
                    <div class="form-horizontal" align="center">
                        <div class="span3"></div>
                        <div class="span2">
                            <asp:Button ID="cmdUpdate" runat="server" Text="Update"
                                OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary"
                                OnClick="cmdUpdate_Click" />
                        </div>
                        <div class="span1">
                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                CssClass="btn btn-primary" OnClick="cmdReset_Click" />
                        </div>
                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>

            <div id="grdTable" style="display: none" runat="server">
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue ">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>Failure Date Change Details</h4>
                                <span class="tools">
                                    <a href="javascript:;" class="icon-chevron-down"></a>
                                    <a href="javascript:;" class="icon-remove"></a>
                                </span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div class="form-horizontal">
                                        <asp:GridView ID="grdDtrCommDateDetails" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered table-advance table-hover">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DTR_CODE" HeaderText="DTR Code" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("DF_EQUIPMENT_ID") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DF_DTR_COMMISSION_DATE" HeaderText="DTr Commission Date"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrfaildate1" runat="server" Text='<%# Bind("DF_DTR_COMMISSION_DATE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black" Enable="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DF_DATE" HeaderText="DTr Failure Date"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrfaildate2" runat="server" Text='<%# Bind("DF_DATE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black" Enable="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="EST_DATE" HeaderText="Estimation Date"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrfaildate3" runat="server" Text='<%# Bind("EST_DATE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black" Enable="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
