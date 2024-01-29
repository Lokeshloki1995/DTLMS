<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailureTransactionDelete.aspx.cs" Inherits="IIITS.DTLMS.Query.FailureTransactionDelete" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="https://cdn.rawgit.com/t4t5/sweetalert/v0.2.0/lib/sweet-alert.css" />
    <script type="text/javascript" src="https://cdn.rawgit.com/t4t5/sweetalert/v0.2.0/lib/sweet-alert.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/7.2.0/sweetalert2.all.min.js"></script>
    <script src="../scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ValidateMyFormLoadTime() {
            if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                swal({
                    text: "Please Enter Valid DTC Code "
                });
                document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                return false;
            }
        };

        function onlyAlphabets(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }

        function validatedeletetime(event) {
            debugger;
            var dtccode = document.getElementById('<%= txtDTCCode.ClientID %>').value.trim();
            var dtcCode = document.getElementById('<%= hdfDtc.ClientID %>').value.trim();
            var ticketnumber = document.getElementById('<%= txtTicketNumber.ClientID %>').value.trim();
            var rv = document.getElementById('<%= hdftrRvNo.ClientID %>').value.trim();
            var inv = document.getElementById('<%= hdfinIno.ClientID %>').value.trim();

            var Url = "FailureTransactionDelete/DeleteFailureTransaction_Click";

            if (dtccode == "") {
                swal({
                    text: "Please Enter Valid DTC Code "
                });
                document.getelementbyid('<%= txtDTCCode.ClientID %>').focus()
                 return false;
             } else if (ticketnumber == "") {
                 swal({
                     text: "Please Enter Valid Ticket Number "
                 });
                 document.getElementById('<%= txtTicketNumber.ClientID %>').focus()
                return false;
            }

        if (dtcCode == "") {
            swal({
                text: "Load Failure Details Before Delete "
            });
            document.getelementbyid('<%= txtDTCCode.ClientID %>').focus()
            return false;
        }

        swal({
            title: "are you sure?",
            text: "Do You Want To Continue",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#00b4b4",
            confirmButtonText: "yes",
            confirmButtonValue: true,
            cancelButtonText: "no",
            cancelButtonValue: true,
            closeOnConfirm: false,
            closeOnCancel: false
        }).then(function (isConfirm) {
            var val = isConfirm;
            if (isConfirm['value'] == true) {

                if (rv != null && inv != null && rv != "" && inv != "") {
                    swal({
                        text: "Rv and Invoice Completed are You Sure Want To Delete This",
                        showCancelButton: true,
                        confirmButtonColor: "#00b4b4",
                        confirmButtonText: "yes",
                        confirmButtonValue: true,
                        cancelButtonText: "no",
                        cancelButtonValue: true,
                        closeOnConfirm: false,
                        closeOnCancel: false
                    }).then(function (isConfirm) {
                        var val = isConfirm;
                        if (isConfirm['value'] == true) {
                            var dtccode = document.getElementById('<%= txtDTCCode.ClientID %>').value.trim();
                            var ticketnumber = document.getElementById('<%= txtTicketNumber.ClientID %>').value.trim();
                            var dtrCode = document.getElementById('<%= hdfDtrCode.ClientID %>').value.trim();
                            window.location = 'FailureTransactionDelete.aspx?DTCCOde=' + dtccode + '&TicketNumber=' + ticketnumber + '&DTRCODE=' + dtrCode;
                        }
                        else {
                            swal("Cancelled");
                            return false;
                        }
                    });;
                }

                else if (rv != "" && rv != null) {
                    swal({
                        text: "RV Completed Are You Sure Want To Delete",
                        showCancelButton: true,
                        confirmButtonColor: "#00b4b4",
                        confirmButtonText: "yes",
                        confirmButtonValue: true,
                        cancelButtonText: "no",
                        cancelButtonValue: true,
                        closeOnConfirm: false,
                        closeOnCancel: false
                    }).then(function (isConfirm) {
                        var val = isConfirm;
                        if (isConfirm['value'] == true) {
                            var dtccode = document.getElementById('<%= txtDTCCode.ClientID %>').value.trim();
                                var ticketnumber = document.getElementById('<%= txtTicketNumber.ClientID %>').value.trim();
                                var dtrCode = document.getElementById('<%= hdfDtrCode.ClientID %>').value.trim();
                                window.location = 'FailureTransactionDelete.aspx?DTCCOde=' + dtccode + '&TicketNumber=' + ticketnumber + '&DTRCODE=' + dtrCode;
                            }
                            else {
                                swal("Cancelled");
                                return false;
                            }
                        });;
                    }
                    else if (inv != "" && inv != null) {
                        swal({
                            text: "Invoice Completed Are You Sure Want To Delete",
                            showCancelButton: true,
                            confirmButtonColor: "#00b4b4",
                            confirmButtonText: "yes",
                            confirmButtonValue: true,
                            cancelButtonText: "no",
                            cancelButtonValue: true,
                            closeOnConfirm: false,
                            closeOnCancel: false
                        }).then(function (isConfirm) {
                            var val = isConfirm;
                            if (isConfirm['value'] == true) {
                                var dtccode = document.getElementById('<%= txtDTCCode.ClientID %>').value.trim();
                            var ticketnumber = document.getElementById('<%= txtTicketNumber.ClientID %>').value.trim();
                            var dtrCode = document.getElementById('<%= hdfDtrCode.ClientID %>').value.trim();
                            window.location = 'FailureTransactionDelete.aspx?DTCCOde=' + dtccode + '&TicketNumber=' + ticketnumber + '&DTRCODE=' + dtrCode;
                        }
                        else {
                            swal("Cancelled");
                            return false;
                        }
                    });;
                }
                else {
                    swal({
                        text: "RV And Invoice Not Completed Are You Sure Want To Delete",
                        showCancelButton: true,
                        confirmButtonColor: "#00b4b4",
                        confirmButtonText: "yes",
                        confirmButtonValue: true,
                        cancelButtonText: "no",
                        cancelButtonValue: true,
                        closeOnConfirm: false,
                        closeOnCancel: false
                    }).then(function (isConfirm) {
                        var val = isConfirm;
                        if (isConfirm['value'] == true) {
                            var dtccode = document.getElementById('<%= txtDTCCode.ClientID %>').value.trim();
                            var ticketnumber = document.getElementById('<%= txtTicketNumber.ClientID %>').value.trim();
                            var dtrCode = document.getElementById('<%= hdfDtrCode.ClientID %>').value.trim();
                            window.location = 'FailureTransactionDelete.aspx?DTCCOde=' + dtccode + '&TicketNumber=' + ticketnumber + '&DTRCODE=' + dtrCode;
                        }
                        else {
                            swal("Cancelled");
                            return false;
                        }
                    });;
                }
    }
    else {
        swal("Cancelled");
        return false;
    }
        });
}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <h3 class="page-title">DELETE FAILURE/ENHANCEMENT RECORD
                    </h3>
                    <div>
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
                            <h4><i class="icon-reorder"></i>DELETE FAILURE/ENHANCEMENT RECORD</h4>
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
                                        <div>
                                            <asp:HiddenField ID="hdfinIno" runat="server" />
                                            <asp:HiddenField ID="hdftrRvNo" runat="server" />
                                            <asp:HiddenField ID="hdfDtc" runat="server" />
                                            <asp:HiddenField ID="hdfDtrCode" runat="server" />

                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">DTC Code <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfDTCcode" runat="server" />
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="6"
                                                            onkeypress="return onlyAlphabets(event,this);" autocomplete="off"></asp:TextBox>
                                                        <asp:Button ID="btnFailDtcLoad" runat="server" Text="Load" AutoPostBack="True"
                                                            TabIndex="2" CssClass="btn btn-primary"
                                                            OnClientClick="javascript:return ValidateMyFormLoadTime()"
                                                            OnClick="LoadFailureDtc_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Ticket Number <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTicketNumber" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>

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
                </div>
            </div>

            <div id="grdTable" style="display: none" runat="server">
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue ">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>DELETE FAILURE/ENHANCEMENT RECORD</h4>
                                <span class="tools">
                                    <a href="javascript:;" class="icon-chevron-down"></a>
                                    <a href="javascript:;" class="icon-remove"></a>
                                </span>
                            </div>
                            <div class="widget-body">

                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div class="form-horizontal">
                                        <asp:GridView ID="grdDtcFailDetails" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered table-advance table-hover">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="Div Name" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DIV_NAME") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="SD_SUBDIV_NAME" HeaderText="SubDiv Name"
                                                    Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("SD_SUBDIV_NAME") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="OM_NAME" HeaderText="Section Name"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate1" runat="server" Text='<%# Bind("OM_NAME") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black" Enable="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTc Code"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate2" runat="server" Text='<%# Bind("DTC_CODE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate3" runat="server" Text='<%# Bind("TC_CODE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure Id"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate3" runat="server" Text='<%# Bind("DF_ID") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="DF_DATE" HeaderText="Fialure Date"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate3" runat="server" Text='<%# Bind("DF_DATE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="FAILURE_TYPE" HeaderText="Failure Type"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate3" runat="server" Text='<%# Bind("FAILURE_TYPE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="100px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="Pending With Stage"
                                                    HeaderText="Pending With Stage"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate3" runat="server" Text='<%# Bind("PENDING_WITH_STAGE") %>'
                                                            Style="word-break: break-all;"
                                                            Width="140px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="PENDING_WITH_USER" HeaderText="Pending With User"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtrCommdate3" runat="server" Text='<%# Bind("PENDING_WITH_USER") %>'
                                                            Style="word-break: break-all;"
                                                            Width="140px" ForeColor="Black"></asp:Label>
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

        <div class="form-horizontal" align="center">
            <div class="span4"></div>
            <div class="span2">
                <asp:Button ID="cmdDelete" runat="server" UseSubmitBehavior="false" Text="Delete" OnClientClick="javascript:return validatedeletetime(event)" CssClass="btn btn-primary" />
            </div>
            <div class="span1">
                <asp:Button ID="cmdReset" runat="server" Text="Reset"
                    CssClass="btn btn-primary" OnClick="cmdReset_Click" />
            </div>
            <div class="span7"></div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
