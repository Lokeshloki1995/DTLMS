<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreInvoiceCreation.aspx.cs" Inherits="IIITS.DTLMS.StoreTransfer.StoreInvoiceCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
     <script type="text/javascript">

         function preventMultipleSubmissions() {
             $('#<%=cmdSave.ClientID %>').prop('disabled', true);
             $('#<%=cmdApprove.ClientID %>').prop('disabled', true);
  <%-- $('#<%=cmdSave.ClientID %>').prop('disabled', false);--%>
}
 
window.onbeforeunload = preventMultipleSubmissions;
 
    </script>

    <script type="text/javascript">


        function ConfirmDelete() {
            System.Data.SQLite

            var result = confirm('Are you sure you want to Delete?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
        function ValidateMyForm() {
            if (document.getElementById('<%= txtInvoiceNumber.ClientID %>').value == "") {
                     alert('Enter Invoice Number')
                     document.getElementById('<%= txtInvoiceNumber.ClientID %>').focus()
                     return false
                 }

                 if (document.getElementById('<%= txtInvoiceDate.ClientID %>').value == "") {
                     alert('Enter Invoice Date')
                     document.getElementById('<%= txtInvoiceDate.ClientID %>').focus()
                     return false
                 }


                 if (document.getElementById('<%= txtRemarks.ClientID %>').value == "") {
                     alert('Enter Remarks')
                     document.getElementById('<%= txtRemarks.ClientID %>').focus()
                     return false
                 }
                 var outputstr = $("#txtInvoiceNumber").val().replace(/'/g, '');
                 alert(outputstr);

             }
             function ValidateForm() {
                 if (document.getElementById('<%= txtTcCode.ClientID %>').value == "") {
                     alert('Search DTr Sl No.')
                     document.getElementById('<%= txtTcCode.ClientID %>').focus()
                     return false
                 }
             }
    </script>
      <script language="Javascript" type="text/javascript">


                    function onlyAlphabets(e, t) {
                        var code = ('charCode' in e) ? e.charCode : e.keyCode;
                        if ( // space
                           !(code == 32) &&// Special characters
                            !(code == 35) &&
                            !(code == 64) &&
                            !(code > 43 && code < 60) &&
                            !(code > 37 && code < 42) &&
                             !(code == 47) &&
                          !(code > 64 && code < 94) && // upper alpha (A-Z)
                          !(code > 96 && code < 126)) { // lower alpha (a-z)
                            e.preventDefault();
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
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Store Invoice 
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
                            <h4><i class="icon-reorder"></i>Indent Information</h4>
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
                                                <label class="control-label">Indent Number <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIndentId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtIndentNumber" runat="server" ReadOnly="true" MaxLength="20"></asp:TextBox>
                                                        <asp:Button ID="btnIndentSearch" runat="server" Text="S" Visible="false"
                                                            CssClass="btn btn-primary" OnClick="btnIndentSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Request From Store</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlFromStore" runat="server" Enabled="False"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Indent Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIndentDate" runat="server" Enabled="False"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Quantity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantity" runat="server" Enabled="False"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span2"></div>
                                        <div class="span9">
                                            <div class="space20"></div>
                                            <div class="space20"></div>

                                            <asp:GridView ID="grdTcTransfer"
                                                AutoGenerateColumns="false" PageSize="5" DataKeyNames="SI_ID" align="center"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" Width="100%"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" OnPageIndexChanging="grdTcTransfer_PageIndexChanging">
                                                <Columns>

                                                    <asp:TemplateField AccessibleHeaderText="SI_ID" HeaderText="SI ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSiId" runat="server" Text='<%# Bind("SI_ID") %>'></asp:Label>
                                                            <asp:TextBox ID="txtSiId" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="SO_CAPACITY" HeaderText="Capacity(in KVA)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("CAPACITY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Requested No. of Transformers">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("REQ_QNTY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="IO_CAPACITY" HeaderText="Pending No. Of Transformers">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPendingQuantity" runat="server" Text='<%# Bind("PENDINGCOUNT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                    </div>



                                    <br />
                                    <br />


                                    <div class="span9" runat="server" id="Matdiv">
                                        <div class="control-group">
                                            <label class="control-label">Material allotment Form<span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:FileUpload ID="fupMaf" runat="server" />

                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                   <%--  OnClick="lnkDwnld_Click"--%>
                                    <div class="space5" runat="server" id="DivDownload">
                                        <asp:LinkButton ID="lnkDwnld" runat="server" OnClick="DownloadFiledwnld">
                                            <img alt="" src="../img/Manual/Pdficon.png" style="width:20px" />View Material allotment Form</asp:LinkButton>

                                        <asp:HiddenField ID="hdfMafPath" runat="server" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <%-- <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                        <asp:Button ID="btnIndentReset" runat="server" Text="Reset" 
                                       CssClass="btn btn-primary"/>
                                         </div>                   
                         </div>                  
                    </div>
                 </div>--%>
                <!-- END SAMPLE FORM PORTLET-->

                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <div class="space20"></div>
        <div class="row-fluid">

            <div class="span12">

                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue" runat="server" id="dvInvoiceCreate" style="display: none">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Invoice Generation</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>

                    <div class="widget-body">

                        <div class="widget-body form">

                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <asp:Panel ID="pnlApprovalInVoice" runat="server">
                                    <div class="row-fluid">


                                        <div class="span1"></div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Invoice Number<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvoiceId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtInvoiceNumber" runat="server" MaxLength="20"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Remarks<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="250" TextMode="MultiLine" Style="resize: none;" onclick="return Validate()" onkeyup="return ValidateTextlimit(this,250);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Invoice Date<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtInvoiceDate" runat="server"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">DTr Code<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcCode" runat="server" MaxLength="20" ReadOnly="false"></asp:TextBox>
                                                        <asp:Button ID="btnTcSearch" runat="server" Text="S"
                                                            CssClass="btn btn-primary" />
                                                        <div class="space15"></div>
                                                        <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-primary"
                                                            OnClick="cmdAdd_Click" OnClientClick="return ValidateForm();" />
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                        <div class="span2"></div>
                                        <div class="span9">

                                            <div class="space20"></div>
                                            <div class="space20"></div>

                                            <asp:GridView ID="grdTcDetails"
                                                AutoGenerateColumns="false" PageSize="5" DataKeyNames="TC_ID"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" OnRowCommand="grdTcDetails_RowCommand"
                                                OnPageIndexChanging="grdTcDetails_PageIndexChanging">
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
                                            <div class="space15"></div>
                                            <div class="space5"></div>


                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </div>


                                    </div>
                                </asp:Panel>

                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="text-center" align="center">


                <asp:Button ID="cmdSave" runat="server" Text="Save" onchange="javascript:preventMultipleSubmissions();" 
                    OnClientClick="return ValidateMyForm();" CssClass="btn btn-primary" OnClick="cmdSave_Click" />

                <asp:Button ID="btnReset" runat="server" Text="Reset"
                    CssClass="btn btn-primary" OnClick="btnReset_Click" />


                <div class="row-fluid" id="dvDTRDetails" runat="server" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>Issued DTR Details</h4>
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


                                            <asp:GridView ID="grdDtrDetails"
                                                AutoGenerateColumns="false" PageSize="5"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server">
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="DTr Make Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcMakeName" runat="server" Text='<%# Bind("MAKE") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTr Capacity(in KVA)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField AccessibleHeaderText="IS_NO" HeaderText="Invoice No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("IS_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="IS_DATE" HeaderText="Invoice Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("IS_DATE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                </Columns>
                                            </asp:GridView>

                                            <div class="space20"></div>


                                        </div>



                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>

                <div class="row-fluid" id="dvGatePass" runat="server" style="display: none">
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
                                                    <label class="control-label">Vehicle No</label>
                                                    <div class="span1">
                                                        <span class="Mandotary">*</span>
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" AutoComplete="Off"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                                                            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <label class="control-label">Receipient Name</label>
                                                <div class="span1">
                                                    <span class="Mandotary">*</span>
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtReciepient" runat="server" AutoComplete="Off" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <label class="control-label">Challen Number</label>
                                                <div class="span1">
                                                    <span class="Mandotary">*</span>
                                                </div>
                                                <div class="control-group">
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtChallen" runat="server" AutoComplete="Off" MaxLength="50"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <label class="control-label">Quantity Issued</label>
                                                <div class="span1">
                                                    <span class="Mandotary">*</span>
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="Textissueqty" runat="server" MaxLength="50" AutoComplete="Off" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>

                                                    </div>
                                                </div>


                                            </div>
                                        </div>
                                        <div class="space20"></div>

                                        <div class="text-center" align="center">


                                            <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass"
                                                CssClass="btn btn-primary" OnClick="cmdGatePass_Click" />


                                            <div class="space20"></div>

                                        </div>

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
                                                            <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                            <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                            <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                            <asp:HiddenField ID="hdfRecordId" runat="server" />
                                                            <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                            <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                            <asp:HiddenField ID="hdffileupload" runat="server" />
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


                <div class="text-center" align="center" id="dvApproveButton" runat="server">

                    <asp:Button ID="cmdApprove" runat="server" Text="Approve" Visible="false"
                        CssClass="btn btn-primary" OnClick="cmdApprove_Click" onchange="javascript:preventMultipleSubmissions();"  />

                </div>

            </div>
    </div>

    </div>
   
 
</asp:Content>
