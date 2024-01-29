<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransBankInvoice.aspx.cs" Inherits="IIITS.DTLMS.StoreTransfer.TransBankInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">

        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Bank Invoice
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


        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Indent details</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>

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
                                            <label class="control-label">Indent Number<span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtIndentId" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtIndentNumber" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Indent date<span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtIndentDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">OM Number<span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtOMNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">OM Date<span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtOMDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtFilepath" runat="server" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="span3"></div>
                                    <div class="span5">
                                        <asp:GridView ID="grdTcRequest"
                                            AutoGenerateColumns="false" PageSize="5" DataKeyNames="BO_ID"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                            runat="server" TabIndex="16">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="BO_ID" HeaderText="BI ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSoId" runat="server" Text='<%# Bind("BO_ID") %>'></asp:Label>
                                                        <asp:TextBox ID="txtSoId" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="BO_CAPACITY" HeaderText="Capacity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("BO_CAPACITY") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="BO_QUANTITY" HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("BO_QUANTITY") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <asp:LinkButton ID="lnkIndent" runat="server" Visible="false" OnClick="lnkIndent_Click">  <img src="../img/Manual/Pdficon.png" style="width:20px" />Click Here to View Indent</asp:LinkButton>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>

        </div>

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Bank Indent</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">

                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <asp:Panel ID="pnlApprovalIndent" runat="server">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Invoice Number<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvNo" runat="server" MaxLength="20"></asp:TextBox>
                                                        <asp:TextBox ID="txtInvid" runat="server" Visible="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Select DTR<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTRCode" runat="server"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" /><br />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span6">

                                            <div class="control-group">
                                                <label class="control-label">Invoice date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvDate" runat="server"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtInvDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtInvDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTR SerialNo<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSlno" runat="server"></asp:TextBox>
                                                        <asp:Button ID="cmdAdd" Text="Add" class="btn btn-primary" runat="server" OnClick="cmdAdd_Click" /><br />
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span2"></div>
                                        <div class="span7">

                                            <div class="space20"></div>
                                            <div class="space20"></div>

                                            <asp:GridView ID="grdTcDetails"
                                                AutoGenerateColumns="false" PageSize="5" DataKeyNames="TC_ID"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" OnRowCommand="grdTcDetails_RowCommand">
                                                <Columns>

                                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC_ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                                            <%-- <asp:TextBox ID="txtTcId" runat="server" ></asp:TextBox>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="Transformer Sl No." Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="DTr Make Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>




                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                    CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <div class="space20"></div>



                                        </div>
                                </asp:Panel>


                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="row-fluid" runat="server" id="dvComments" style="display: none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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

                                            <label class="control-label">Comments<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtActionType" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
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

            <div class="span3"></div>
            <div class="span2">
                <asp:Button ID="cmdSave" runat="server" Text="Save"
                    OnClientClick="return ValidateMyForm();" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
            </div>
            <div class="span1">
                <asp:Button ID="cmdReset" runat="server" Text="Reset"
                    CssClass="btn btn-primary" />
            </div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <div class="row-fluid" id="dvgatepass" runat="server" style="display:none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>GatePass</h4>
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
                                                Vehicle No</label>
                                            <div class="span1">
                                                <span class="Mandotary">*</span>
                                            </div>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <label class="control-label">
                                            Receipient Name</label>
                                        <div class="span1">
                                            <span class="Mandotary">*</span>
                                        </div>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <label class="control-label">
                                        Challen Number</label>
                                    <div class="span1">
                                        <span class="Mandotary">*</span>
                                    </div>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:TextBox ID="txtChallen" runat="server" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="space20">
                                </div>
                                <div class="form-horizontal" align="center">
                                    <div class="span3">
                                    </div>
                                    <div class="span1">
                                    </div>
                                    <div class="span1">
                                        <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass" CssClass="btn btn-primary" OnClick="cmdGatePass_Click"
                                           />
                                    </div>
                                    <div class="space20">
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
