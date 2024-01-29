<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FaultyTCInvoiceCreation.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.FaultyTCInvoiceCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">



          

function preventMultipleSubmissions() {
   $('#<%=cmdSave.ClientID %>').prop('disabled', true);

}

window.onbeforeunload = preventMultipleSubmissions;
 </script>   
        function ValidateMyForm() {


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


    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        ContentPlaceHolder1_grdFaultyTCAmt_lblRemarks {
    display: inline-block;
    width: 100px;
    word-break: break-all;
    font-weight: bold;
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
                                    <asp:Panel ID="Panel1" runat="server">
                                        <div class="row-fluid">
                                            <div class="span1"></div>
                                            <div class="span5">
                                                <div class="control-group">

                                                    
                                                    <div class="control-group">
                                                        <label class="control-label">
                                                            <asp:Label ID="lblSuppRep" runat="server" Text="Supplier/Repairer"></asp:Label>
                                                        </label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:DropDownList ID="cmbRepairer" runat="server" AutoPostBack="true"
                                                                     TabIndex="1">
                                                                </asp:DropDownList>
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
                                                        <label class="control-label">Purchase Order No</label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtPONo" runat="server" MaxLength="25" TabIndex="6" ReadOnly="true" onkeypress="javascript:return RestrictSpace(event)"></asp:TextBox>

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


                                                </div>
                                            </div>

                                            <div class="span5">


                                                <div class="control-group">
                                                    <label class="control-label">Estimation Date</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtEstimationDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" TargetControlID="txtEstimationDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Work Order Date</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtWorkOrderDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1" TargetControlID="txtWorkOrderDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Purchase Order Date</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPODate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender_txtPODate" runat="server" CssClass="cal_Theme1" TargetControlID="txtPODate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                            <asp:TextBox ID="txtRepairMasterId" runat="server" MaxLength="20" TabIndex="8" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Quantity</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtqnty" runat="server" MaxLength="10" TabIndex="3" ReadOnly="true"></asp:TextBox>
                                                            <asp:TextBox ID="txtRSMID" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfGuarenteeType" runat="server" />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="space20"></div>
                                    <asp:GridView ID="grdFaultyTCAmt" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        AutoGenerateColumns="false" PageSize="10" DataKeyNames="TC_ID"
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
                                                    <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
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
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>' Style="word-break: break-word;font-weight: bold" Width="100px" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="REASON" HeaderText="Reason">

                                            <ItemTemplate>
                                                <asp:Label ID="lblReason" runat="server" Text='<%# Bind("REASON") %>' Style="word-break: break-word" Width="120px"></asp:Label>
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

                            <div class="space20"></div>
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
                            <h4><i class="icon-reorder"></i>Issue Details</h4>
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
                                                        Indent Date
                                            
                                                <span class="Mandotary">*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtindentdate" runat="server" MaxLength="10" TabIndex="9"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender_txtindentdate" runat="server" CssClass="cal_Theme1" TargetControlID="txtindentdate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Invoice Date
                                            
                                                <span class="Mandotary">*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="10" TabIndex="9"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender_txtInvoiceDate" runat="server" CssClass="cal_Theme1" TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Indent No
                                             
                                                <span class="Mandotary">*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtIndentNO" runat="server" MaxLength="20" TabIndex="8" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Invoice No
                                             
                                                <span class="Mandotary">*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" TabIndex="8" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>



                                            </div>

                                        </div>
                                    </asp:Panel>
                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">

                                        <div class="span5"></div>
                                        <div class="span1">
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                                                CssClass="btn btn-primary" OnClick="cmdSave_Click" TabIndex="10" CausesValidation="false" />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                                CssClass="btn btn-primary" OnClick="cmdReset_Click" TabIndex="11" /><br />
                                        </div>
                                        <div class="span5"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>

                            </div>

                            <div class="space20"></div>
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

                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
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
                                                    <label class="control-label">
                                                        Vehicle No
                         
                           <span class="Mandotary">*</span></label>

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

                                                <label class="control-label">
                                                    Receipient Name
                         
                           <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <label class="control-label">
                                                Challen Number
                       
                           <span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtChallen" runat="server" MaxLength="50"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="space20"></div>

                                        <div class="form-horizontal" align="center">

                                            <div class="span3"></div>

                                            <div class="span1">
                                                <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass"
                                                    CssClass="btn btn-primary" OnClick="cmdGatePass_Click" Enabled="false" />
                                            </div>

                                            <div class="space20"></div>

                                        </div>





                                    </div>



                                </div>


                            </div>
                        </div>
                    </div>



                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>


            <!-- END PAGE CONTENT-->

        </div>
    </div>

</asp:Content>
