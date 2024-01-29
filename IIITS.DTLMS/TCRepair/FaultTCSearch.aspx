<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FaultTCSearch.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.FaultTCSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= cmbStore.ClientID %>').value == "--Select--") {
                alert('Select Store to Search')
                document.getElementById('<%= cmbStore.ClientID %>').focus()
                return false
            }
        }
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(/img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Faulty Transformer Search
                    </h3>
                    <%--        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="FaultyTCSerch" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
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
                            <h4><i class="icon-reorder"></i>Faulty Transformer Search</h4>
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
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="lblSuppRep" runat="server" Text="Supplier/Repairer"></asp:Label>
                                                    <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbRepairer_SelectedIndexChanged" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Estimation Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstimationAmount" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Quantity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtqnty" runat="server" MaxLength="10" TabIndex="3" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" TabIndex="3" Enabled="false" Visible="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
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
                                                <label class="control-label">Star Rated</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStarRated" runat="server" TabIndex="17">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" id="divremarks" runat="server">
                                                <label class="control-label">
                                                    Remarks <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="500"  TabIndex="9" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnmrep" runat="server"  />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="text-center">
                                        <asp:Button ID="cmdLoad" runat="server" Text="Load Faulty DTr" OnClientClick="javascript:return ValidateMyForm()"
                                            CssClass="btn btn-primary" OnClick="cmdLoad_Click" TabIndex="4" />

                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="5" OnClick="cmdReset_Click" />

                                        <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"
                                            OnClick="Export_ClickTcsearch" />

                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->
                            <div style="width: 100%!important; height: 100%!important; overflow: scroll!important">
                                <asp:GridView ID="grdFaultTC" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                    AutoGenerateColumns="false" PageSize="10" DataKeyNames="TC_ID" ShowFooter="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnRowCommand="grdFaultTC_RowCommand"
                                    OnPageIndexChanging="grdFaultTC_PageIndexChanging" TabIndex="6" OnSorting="grdFaultTC_Sorting">
                                    <HeaderStyle CssClass="both" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtDTRCode" runat="server" placeholder="Enter DTr CODE" Width="80px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtSlNo" runat="server" placeholder="Enter DTR SLNO" Width="120px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">

                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstarrate" runat="server" Text='<%# Bind("TC_STAR_RATE") %>' Style="word-break: break-all" Width="110px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee Period" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="RepSentcount" HeaderText="Repaired Count" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRepSentcount" runat="server" Text='<%# Bind("RCOUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblguarentee" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="REMARKS" HeaderText="Remarks">

                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>' Style="word-break: break-word; font-weight: bold" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="REASON" HeaderText="Reason">

                                            <ItemTemplate>
                                                <asp:Label ID="lblReason" runat="server" Text='<%# Bind("REASON") %>' Style="word-break: break-word" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Status" HeaderText="Status">

                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all" Width="70px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                        CommandName="Edit" Width="12px" title="Click To Edit" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnView" runat="server" Height="15px" ImageUrl="~/img/Manual/View1.jpg"
                                                        CommandName="View" Width="15px" title="Click To View" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">
                                        <div class="span3"></div>
                                        <div class="span3">
                                            <asp:Button ID="cmdLoadItemCode" runat="server" Text="Add DTr"
                                                CssClass="btn btn-primary" OnClick="cmdLoadItemCode_Click" Visible="false" TabIndex="7" />
                                        </div>
                                        <div class="space20"></div>
                                    </div>

                                </div>
                            </div>
                            <div id="divgrdItem" runat="server" visible="false">
                                <div style="width: 100%!important; height: 100%!important; overflow: scroll!important">
                                    <asp:GridView ID="grdItemCode" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        AutoGenerateColumns="false" OnRowCommand="grdItemCode_RowCommand" DataKeyNames="TC_ID"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server"
                                        TabIndex="6">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstarrate" runat="server" Text='<%# Bind("TC_STAR_RATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblguarentee" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ESTIMATION_AMOUNT" HeaderText="Repairer Cost">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEstimationAmount" runat="server" Text='<%# Bind("ESTIMATION_AMOUNT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="REMARKS" HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>' Style="word-break: break-word; font-weight: bold" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="REASON" HeaderText="Reason">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReason" runat="server" Text='<%# Bind("REASON") %>' Style="word-break: break-word;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="imgBtnDelete" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                        CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="space20"></div>
                            <div class="space20"></div>
                            <div class="space20"></div>
                            <div class="form-horizontal" align="center">

                                <div class="span3"></div>
                                <div class="span3">
                                    <asp:Button ID="cmdSend" runat="server" Text="Send for Repair"
                                        CssClass="btn btn-primary" OnClick="cmdSend_Click" Visible="false" TabIndex="7" />
                                </div>

                                <div class="space20"></div>
                            </div>

                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>


        <!-- END PAGE CONTENT-->
    </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Search Faulty Transformer By Clicking <u>Load Fault Transformer</u> Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Transformer Can Be Searched By Using Make, Capacity and Guarantee Type Filter Option
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>select Repairer and Click to Add TC Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once The Faulty Transformer List Is Loaded User Can Select The Transformer By Clicking CheckBoxes
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>After Selection Process User Can Click On <u>Click to send Supplier/Repairer</u> Button
                    </p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>
