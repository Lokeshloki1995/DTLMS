<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ReceiveTransBank.aspx.cs" Inherits="IIITS.DTLMS.StoreTransfer.ReceiveTransBank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Delete?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        function ValidateForm() {
            if (document.getElementById('<%= txtRVNo.ClientID %>').value == "") {
                alert('Enter Reciept Voucher No.(RV No)')
                document.getElementById('<%= txtRVNo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRemarks.ClientID %>').value == "") {
                alert('Enter Remarks')
                document.getElementById('<%= txtRemarks.ClientID %>').focus()
                return false
            }

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Receive Transformer
              <div style="float: right">

                  <asp:Button ID="cmdClose" runat="server" Text="Close"
                      CssClass="btn btn-primary" OnClick="cmdClose_Click" /><br />
              </div>

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
                <%-- <div style="float:right" >
                             
                                   <asp:Button ID="cmdClose" runat="server" Text="Indent View" 
                                              CssClass="btn btn-primary" onclick="cmdClose_Click" /><br /></div>
                --%>
            </div>


            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Indent Details</h4>
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
                                                <label class="control-label">Indent Number</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                          <asp:TextBox ID="txtActionType" runat="server" Visible="false"></asp:TextBox>
                                                          <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:TextBox ID="txtIndentId" runat="server" MaxLength="20" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtIndentNumber" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">OM Number</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOMNo" runat="server" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Indent date</label>
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtIndentDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">OM Date</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOmdate" runat="server" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span3"></div>
                                        <div class="span5">
                                            <div class="space20"></div>
                                            <div class="space20"></div>

                                            <asp:GridView ID="grdIndentDetails"
                                                AutoGenerateColumns="false" PageSize="5"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" TabIndex="16">

                                                <Columns>


                                                    <asp:TemplateField AccessibleHeaderText="SO_CAPACITY" HeaderText="Capacity(in KVA)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("BO_CAPACITY") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("BO_QUANTITY") %>'></asp:Label>

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
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Receive Transformer</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

                            </span>
                        </div>
                        <div class="widget-body">

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <asp:Panel ID="pnlApproval" runat="server">
                                        <div class="row-fluid">
                                            <div class="span1">
                                                <asp:TextBox ID="txtInvoiceId" runat="server" Width="50" Visible="false"></asp:TextBox>
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">Invoice Number </label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtInvoiceNumber" Enabled="false" runat="server" MaxLength="20"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">Invoice Date</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoicetDate" runat="server" Enabled="False"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="span2"></div>

                                            <div class="space20"></div>
                                            <div class="space20"></div>
                                    </asp:Panel>


                                    <asp:GridView ID="grdTcTransfer"
                                        AutoGenerateColumns="false" PageSize="5"
                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" Width="100%"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        runat="server">
                                        <Columns>

                                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="TC_SL_NO" HeaderText="DTr SlNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="Tc_make" HeaderText="DTr Make">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcMake" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity(in KVA)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="row-fluid">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <div class="space20"></div>
                            <div class="row-fluid">
                                <div class="span1">
                                    <asp:TextBox ID="TextBox1" runat="server" Width="50" Visible="false"></asp:TextBox>
                                </div>
                                <div class="span5">
                                    <div class="control-group">
                                        <label class="control-label">RV No<span class="Mandotary">*</span></label>
                                        <div class="controls">
                                            <div class="input-append" align="center">
                                                <asp:TextBox ID="txtRVNo" runat="server" MaxLength="25"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">Remarks<span class="Mandotary">*</span></label>
                                        <div class="controls">
                                            <div class="input-append" align="center">
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="500px" Style="resize: none"
                                                    TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row-fluid">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>

    <div class="form-horizontal" align="center">
        <div class="span3"></div>
        <div class="span2">
            <asp:Button ID="cmdSave" runat="server" Text="RECEIVE"
                OnClientClick="return ValidateForm();" CssClass="btn btn-primary" OnClick="cmdSave_Click" />

        </div>

    </div>

</asp:Content>
