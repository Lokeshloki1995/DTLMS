<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DeliverTC.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.DeliverTC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <style type="text/css">
        .handPointer {
            cursor: pointer;
        }

        .blockpointer {
            cursor: not-allowed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Receive Transformers
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DeliverPendingSearch.aspx'; return false;"
                        CssClass="btn btn-primary" />
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
                                <i class="icon-reorder"></i>Receive Transformers</h4>
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
                                                    Store<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Enabled="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--    <div class="control-group">
                                                <label class="control-label">
                                                    Verified By<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbVerifiedBy" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Deliver Challen No.<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtChallenNo" runat="server" MaxLength="30"></asp:TextBox>
                                                        <asp:TextBox ID="txtRepairMasterId" runat="server" MaxLength="10" Visible="false"
                                                            Width="20px"> </asp:TextBox>
                                                        <asp:TextBox ID="txtInsResultId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRINo" runat="server" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--<div class="control-group">
                                                <label class="control-label">Guarantee Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGuarantyType" runat="server">
                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Warenty Period<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWarrentyPeriod" runat="server" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                        <div class="span5">



                                            <div class="control-group">
                                                <label class="control-label">
                                                    Deliver Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeliverdate" runat="server"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="DeliverCalender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDeliverdate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RV No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRVNo" runat="server" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RV Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRVDate" runat="server"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="RVDateCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtRVDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRIDate" runat="server"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender_txtRIDate" runat="server" CssClass="cal_Theme1" TargetControlID="txtRIDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--  <div class="space20">
                                    </div>--%>
                                    <%--<div class="space20">
                                    </div>
                                    <div class="form-horizontal" align="center">
                                        <div class="span3">
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdSave" runat="server" Text="Recieve" CssClass="btn btn-primary"
                                                OnClick="cmdSave_Click" />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-primary"
                                                OnClick="cmdReset_Click" />
                                        </div>
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>--%>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Receive Pending Transformers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <%--          style="overflow: scroll; height: 350px;"--%>
                                    <div id="divResult" runat="server">
                                        <asp:GridView ID="grdReceivePending" AutoGenerateColumns="false" PageSize="10"
                                            CssClass="table table-striped table-bordered table-advance table-hover"
                                            AllowPaging="false" runat="server" ShowHeaderWhenEmpty="True"
                                            EmptyDataText="No Records Found"
                                            OnRowDataBound="grdReceivePending_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInsResult" runat="server" Text='<%# Bind("STATE") %>' Style="word-break: break-all;"
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                            Width="120px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;"
                                                            Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                            Width="90px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="warranty Period" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWarentyPeriod" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>' Style="word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="GUARENTEE_TYPE" HeaderText="GUARENTEE TYPE">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="cmbGuarantyType" runat="server" Style="width: 100px">
                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                            <%--<asp:ListItem Value="AGP">AGP</asp:ListItem>--%>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:Label ID="lblWarrentyType" runat="server" Text='<%# Bind("WARRENTY_TYPE") %>' Style="word-break: break-all;"
                                                            ></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWarrenty" runat="server" Text='<%# Bind("STATUS") %>' Style="width: 100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="WARRANTY_TYPE" HeaderText="warranty Period (In Months)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTCWarrenty" runat="server" Text='<%# Bind("TC_WARRENTY") %>' Style="width: 100px" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Test Document" Visible="true">


                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW"
                                                            OnClick="ViewFile"
                                                            CommandArgument='<%# Eval("IND_DOC") %>'>                                                           
                                                        </asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                       <%-- <asp:LinkButton ID="lnkDwnld" runat="server" CommandName="Download">
                                                        <img src="../img/Manual/Pdficon.png" style="width:20px" />Download Test Report</asp:LinkButton>
                                                       --%>

                                                        <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" OnClick="DownloadFiledwnld"
                                                            CommandArgument='<%# Eval("IND_DOC") %>'>  <img alt="" src="../img/Manual/Pdficon.png" style="width:20px" />Download Test Report                                                         
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="lnkNodownload" runat="server">
                                                        <img alt="" src="../img/Manual/nodoc.png" style="width:20px" />Report Not Available</asp:LinkButton>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="text-center">
                                        <div class="span5">
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdSave" runat="server" Text="Recieve" CssClass="btn btn-primary"
                                                OnClick="cmdSave_Click"  />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-primary"
                                                OnClick="cmdReset_Click" />
                                        </div>
                                        <div class="span5">
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div class="space20">
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
                                    <i class="icon-reorder"></i>Recieved Transformers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <asp:GridView ID="grdRecievedDTr" AutoGenerateColumns="false" PageSize="10" CssClass="table table-striped table-bordered table-advance table-hover"
                                        AllowPaging="true" runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
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
            <!-- END PAGE CONTENT-->
        </div>
    </div>
</asp:Content>
