<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TCRepairIssue.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TCRepairIssue" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">





        function preventMultipleSubmissions() {
   $('#<%=cmdSave.ClientID %>').prop('disabled', true);

}

window.onbeforeunload = preventMultipleSubmissions;
        function ValidateMyForm() {

            if (document.getElementById('<%= cmbGuarantyType.ClientID %>').value == "-Select-") {
                alert('Select Guarantee Type')
                document.getElementById('<%= cmbGuarantyType.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= ddlType.ClientID %>').value == "-Select-") {
                alert('Select Type (Supplier/Repairer)')
                document.getElementById('<%= ddlType.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbRepairer.ClientID %>').value == "--Select--") {
                alert('Select Repairer / Supplier')
                document.getElementById('<%= cmbRepairer.ClientID %>').focus()
                return false
            }
        }



        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
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
                    <h3 class="page-title">Faulty Transformer Issue                    
                                      
                    </h3>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-primary" OnClick="cmdClose_Click" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Supplier / Repairer Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <asp:LinkButton ID="lnkRepairerDetails" runat="server"
                                        Style="font-size: 12px; margin-left: 311px; color: Blue" OnClick="cmdSearch_Click">View Repairer Rates</asp:LinkButton>
                                    <br />
                                    <br />
                                    <asp:Panel ID="Panel1" runat="server">
                                        <div class="row-fluid">
                                            <div class="span1"></div>
                                            <div class="span5">

                                                <div class="control-group">

                                                    <div class="control-group">
                                                        <label class="control-label">
                                                            <asp:Label ID="lblSuppRep" runat="server" Text="Supplier/Repairer"></asp:Label>


                                                            <span class="Mandotary">*</span>
                                                        </label>
                                                        <div class="controls">

                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtStoreId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfTccode" runat="server" />
                                                                <asp:HiddenField ID="hdftcQnty" runat="server" />
                                                                <asp:HiddenField ID="hdnwfoid" runat="server" />
                                                                <asp:HiddenField ID="hdnrepid" runat="server" />

                                                                <asp:DropDownList ID="cmbRepairer" runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="cmbRepairer_SelectedIndexChanged" TabIndex="1">
                                                                </asp:DropDownList><br />

                                                            </div>
                                                        </div>



                                                    </div>


                                                    <div class="control-group">
                                                        <label class="control-label">Estimation No</label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtEstimationNo" runat="server" ReadOnly="true"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label">Work Order No</label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtWorkorderNo" runat="server" ReadOnly="true"></asp:TextBox>

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
                                                        <label class="control-label">
                                                            Estimation Date
                                            
                                                <span class="Mandotary">*</span></label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtEstimationDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" TargetControlID="txtEstimationDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="control-group">
                                                        <label class="control-label">
                                                            Work Order Date
                                            
                                                <span class="Mandotary">*</span></label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtWorkOrderDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1" TargetControlID="txtWorkOrderDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="control-group">
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                    <%-- <asp:ListItem >--Select--</asp:ListItem>--%>
                                                                    <asp:ListItem Value="2">Repairer</asp:ListItem>
                                                                    <%-- <asp:ListItem Value="1">Supplier</asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbGuarantyType" runat="server" Visible="false">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                                <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                                <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>

                                                </div>
                                                <asp:DropDownList ID="cmbStarRated" runat="server" Visible="false"></asp:DropDownList>
                                            </div>

                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Name</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtName" runat="server" MaxLength="50" TabIndex="2" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Phone</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" TabIndex="3" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Address</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="50" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                                onkeyup="javascript:ValidateTextlimit(this,100)" ReadOnly="true"></asp:TextBox>

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

                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
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
                            <h4><i class="icon-reorder"></i>Purchase Order Details</h4>
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
                                            <div class="span1"></div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Purchase Order No
                                            
                                                <span class="Mandotary">*</span>
                                                    </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPONo" runat="server" MaxLength="25" TabIndex="6" onkeypress="javascript:return RestrictSpace(event)"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Purchase Order Date
                                             
                                                <span class="Mandotary">*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPODate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender_txtPODate" runat="server" CssClass="cal_Theme1" TargetControlID="txtPODate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                            <asp:TextBox ID="txtRepairMasterId" runat="server" MaxLength="20" TabIndex="8" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group" id="divrm" runat="server">
                                                    <label class="control-label">
                                                        Remarks <span class="Mandotary">*</span>
                                                    </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <%-- <asp:TextBox ID="txtRemarks" AutoPostBack="true" runat="server" MaxLength="10" onclick="cmdSearchPO_Click" TextMode="MultiLine" TabIndex="9"></asp:TextBox>--%>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="10" TextMode="MultiLine" TabIndex="9"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">


                                                <asp:Panel ID="pnlfile" runat="server">
                                                    <div class="control-group" id="file1" runat="server">
                                                        <label class="control-label">Budget Certificate<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fupdDoc" runat="server" accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                                <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="control-group" id="file2" runat="server">
                                                        <label class="control-label">Draft Purchase Order Document<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fuppodoc" runat="server" accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                                <asp:Label ID="lblpoFilename" runat="server" Text="Initial Text"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </div>
                                <div class="space20"></div>
                                <div class="container-fluid">
                                    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        OnPageIndexChanging="gvFiles_PageIndexChanging" OnSorting="gvFiles_Sorting" AllowSorting="true" Visible="true">
                                        <Columns>
                                            <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Budget Document" ItemStyle-Width="700" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile"
                                                        CommandArgument='<%# Eval("Name") %>'> 
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadFiledwnld"
                                                       CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                    <div class="space20"></div>
                                    <asp:GridView ID="gvFiles1" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        OnPageIndexChanging="gvFiles1_PageIndexChanging" OnSorting="gvFiles1_Sorting" AllowSorting="true" Visible="true">
                                        <Columns>
                                            <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Draft Purchase Order Document" ItemStyle-Width="700" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload21" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile1"
                                                        CommandArgument='<%# Eval("Name") %>'> 
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                   <asp:LinkButton ID="lnkDownload22" runat="server" ForeColor="Blue" Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadFiledwnld1"
                                                       CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
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
                        <h4><i class="icon-reorder"></i>Selected Transformer</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>

                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->

                        </div>
                        <!-- END FORM-->

                        <asp:GridView ID="grdFaultTC" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                            AutoGenerateColumns="false" PageSize="50" DataKeyNames="TC_ID"
                            CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" TabIndex="16" OnRowCommand="grdFaultTC_RowCommand">
                            <Columns>

                                <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="TC SlNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTCSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstarrate" runat="server" Text='<%# Bind("TC_STAR_RATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee " Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" Visible="false">

                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type">

                                    <ItemTemplate>
                                        <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" Visible="false">
                                    <ItemTemplate>
                                        <center>

                                            <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                CommandName="Submit" Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>

                        </asp:GridView>
                        <asp:GridView ID="grdFaultyTCAmt" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                            AutoGenerateColumns="false" PageSize="50" DataKeyNames="TC_ID"
                            CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" TabIndex="16" Visible="false">
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

                                <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTCSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstarrate" runat="server" Text='<%# Bind("TC_STAR_RATE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Warantee " Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" Visible="false">

                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type">

                                    <ItemTemplate>
                                        <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="REMARKS" HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>' Style="word-break: break-word; font-weight: bold" Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="REASON" HeaderText="Reason">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReason" runat="server" Text='<%# Bind("REASON") %>' Style="word-break: break-word" Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ESTIMATION_AMOUNT" HeaderText="Repairer Cost">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstimationAmount" runat="server" Text='<%# Bind("ESTIMATION_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>

                        </asp:GridView>


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

                                                        <asp:HiddenField ID="hdfRepairId" runat="server" />
                                                        <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" 
                                                            Width="550px" Height="40px"  onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
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


            <div class="space20"></div>
            <div class="form-horizontal" align="center">

                <div class="span3"></div>
                <div class="span5">
                    <asp:Button ID="btnCalEstAmt" runat="server" Text="Calculate Estimation Amount"
                        CssClass="btn btn-primary" OnClick="btnCalEstAmt_Click" TabIndex="4" Visible="false" />
                    <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                        CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                        CssClass="btn btn-primary" OnClick="cmdReset_Click" TabIndex="11" />
                </div>
                <div class="span5">
                    <%--  <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                        CssClass="btn btn-primary" OnClick="cmdReset_Click" TabIndex="11" />--%><br />
                </div>
                <%--  <div class="span1">
                                        <asp:Button ID="cmddownload" runat="server" Text="DOWNLOAD DOCUMENT" CausesValidation="false"
                                            CssClass="btn btn-primary" OnClick="DownloadFile1" TabIndex="11" /><br />
                                    </div>--%>
                <div class="span7"></div>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <div class="space20"></div>
            <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
            <asp:HiddenField ID="hdfEstimateNo" runat="server" />
            <asp:HiddenField ID="hdfWorkorderNo" runat="server" />
            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
            <asp:HiddenField ID="hdfWFOId" runat="server" />
            <asp:HiddenField ID="hdfWFDataId" runat="server" />
            <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
            <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtChallen" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtbudgetfilepath" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtpofilepath" runat="server" Visible="false"></asp:TextBox>


            <div class="form-horizontal">
                <div class="row-fluid">
                    <div class="span1"></div>
                    <div id="selectedtc" style="visibility: hidden">
                        <div class="span5">

                            <div id="DTrCODE" runat="server" class="control-group">
                                <label class="control-label">DTr Code</label>

                                <div class="controls">
                                    <div class="input-append">
                                        <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10" TabIndex="12"></asp:TextBox>
                                        <asp:Button ID="cmdSearchTC" Text="S" class="btn btn-primary" runat="server"
                                            CausesValidation="false" OnClick="cmdSearchTC_Click" TabIndex="13" />


                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span4">
                            <div id="MAKE" runat="server" class="control-group">
                                <label class="control-label">Make</label>
                                <div class="controls">
                                    <div class="input-append">
                                        <asp:TextBox ID="txtMake" runat="server" MaxLength="50" TabIndex="14"></asp:TextBox>
                                        <asp:TextBox ID="txtSelectedTcId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                        <asp:Button ID="cmdLoad" Text="Load" class="btn btn-primary" runat="server"
                                            OnClick="cmdLoad_Click" TabIndex="15" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--                    <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>GatePass</h4>
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
                        <label class="control-label">Vehicle No
                         
                           <span class="Mandotary"> *</span></label>
                      
                        <div class="controls">
                            <div class="input-append">
                                  <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>                   
                                <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                                <asp:HiddenField ID="hdfEstimateNo" runat="server" />
                                <asp:HiddenField ID="hdfWorkorderNo" runat="server" />
                                <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
         
                         <label class="control-label">Receipient Name
                         
                           <span class="Mandotary"> *</span></label>
                      
                        <div class="controls">
                            <div class="input-append">
                                 <asp:HiddenField ID="hdfWFOId" runat="server" />
                            <asp:HiddenField ID="hdfWFDataId" runat="server" />
                            <asp:HiddenField ID="hdfWFOAutoId" runat="server" />                      
                                <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50"></asp:TextBox>
                                                                              
                            </div>
                        </div>
                    </div>

                        <label class="control-label">Challen Number
                       
                           <span class="Mandotary"> *</span></label>
                      
                        <div class="controls">
                            <div class="input-append">
                                                   
                                <asp:TextBox ID="txtChallen" runat="server" MaxLength="50"></asp:TextBox>
                            
                            </div>
                        </div>
                    </div>
                            <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                       
                                        <div class="span1">
                                        <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass" 
                                      CssClass="btn btn-primary" onclick="cmdGatePass_Click" Enabled="false"
                                                />
                                         </div>

                                          <div class="space20"></div>
         
         </div>
         
      </div>



    </div>

     
    </div>
    </div>
    </div>


    
    </div>--%>
            <!-- END SAMPLE FORM PORTLET-->
        </div>


        <!-- END PAGE CONTENT-->

    </div>

</asp:Content>
