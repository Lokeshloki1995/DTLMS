<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="PMCTcAllotment.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.PMCTcAllotment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">

        function AllowOnlyAlphanumericNotAllowSpecial(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
           ((evt.which) ? evt.which : 0));

            if ((charCode > 47 && charCode < 58) ||
                (charCode > 64 && charCode < 91) ||
                (charCode > 96 && charCode < 123)) {
                return true;
            }

            else { return false; }
        }
        function DisableSplChars(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 92) {
                return false;
            } else {

                return true;
            }
        }

        function ConfirmDelete() {

            var result = confirm('Are you sure you want to Delete?');
            if (result) {
                return true;
            }
            else {

                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function checkRadioBtn(id) {
            var gv = document.getElementById('<%=grdDIPendingTC.ClientID %>');

            for (var i = 1; i < gv.rows.length; i++) {
                var radioBtn = gv.rows[i].cells[0].getElementsByTagName("input");

                // Check if the id not same
                if (radioBtn[0].id != id.id) {
                    radioBtn[0].checked = false;
                }
            }
        }
    </script>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create">DTr Allocation
               <%-- <div style="float: right">

                    <asp:Button ID="cmdClose" runat="server" Text="PO View"
                        CssClass="btn btn-primary" /><br />
                </div>--%>
                    <div style="float: right">

                        <asp:Button ID="cmdClose" runat="server" Style="margin-top: 14px; margin-right: 1px;" Text="DTr Allotment View"
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
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateDI" runat="server"><i class="icon-reorder"></i>Create DTr Allotment</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                            </span>
                        </div>

                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">DI Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDiId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtDINumber" runat="server" MaxLength="20" autocomplete="off" onkeypress="javascript:return AllowOnlyAlphanumericNotAllowSpecial(event);"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" /><br />
                                                        <asp:HiddenField ID="hdfIndentstatus" runat="server" />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span2"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Quantity <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTotalQuantity" ReadOnly="true" runat="server" MaxLength="8" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="row-fluid">

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">PO Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoNumber" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span2"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    PO Amount <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoamount" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row-fluid">

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">PO Available Amount<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAvailableAmt" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="space20"></div>
                                    <div class="row-fluid">

                                        <div class="span6">
                                            <asp:GridView ID="grdDIPendingTC" AutoGenerateColumns="false" PageSize="5"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" TabIndex="16" Style="width: 600px;" OnPageIndexChanging="grdDIPendingTC_PageIndexChanging"
                                                OnRowDataBound="grdDIPendingTC_RowDataBound">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="chkSelect" runat="server" onclick="checkRadioBtn(this);" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="DI_ID" HeaderText="DI_ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDI" runat="server" Text='<%# Bind("pmc_di_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="DIM_DI_NO" HeaderText="Dispatch No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDINO" runat="server" Text='<%# Bind("pmc_dim_di_no") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="PB_MAKE" HeaderText="Make" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("pmc_di_make_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMake" runat="server" Width="150" Text='<%# Bind("tm_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="Store" HeaderText="Store Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstore" runat="server" Width="100" Text='<%# Bind("sm_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("pmc_di_capacity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="PO_RATING" HeaderText="Rating" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRatingId" runat="server" Text='<%# Bind("pmc_di_starttype") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="RATING" HeaderText="Rating">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRating" runat="server" Width="150" Text='<%# Bind("md_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="Total Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("pmc_di_quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="Start Range" HeaderText="Start Range">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstartrange" runat="server" Width="100" Text='<%# Bind("pmc_di_tc_start_range") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="End Range" HeaderText="End Range">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblendrange" runat="server" Width="100" Text='<%# Bind("pmc_di_tc_end_range") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="DIM_ID" HeaderText="DIM_ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldimid" runat="server" Text='<%# Bind("pmc_dim_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="DIM_PO_ID" HeaderText="DIM_PO_ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblpoid" runat="server" Text='<%# Bind("pmc_dim_po_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                           <%--         <asp:TemplateField AccessibleHeaderText="DIM_PO_ID" HeaderText="DIM_PO_ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblindentstatus" runat="server" Text='<%# Bind("pdra_indent_status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Width="150" Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadDiRecords"
                                                                CommandArgument='<%# Eval("pmc_dim_di_no") %>'> </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>


                                    </div>
                                    <br />
                                    <div class="span6">

                                        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false"
                                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                            OnPageIndexChanging="gvFiles_PageIndexChanging" AllowSorting="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Download Delivery Documents" ItemStyle-Width="350" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile"
                                                            CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Width="150" Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadFiledwnld"
                                                       CommandArgument='<%# Eval("Name") %>'> </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                        </asp:GridView>


                                        <%-- <asp:LinkButton ID="lnkPoDownload" runat="server" Visible="false" OnClick="lnkPoDownload_Click">  <img src="../img/Manual/Pdficon.png" style="width:20px" />Click Here to Download PO</asp:LinkButton>--%>
                                    </div>


                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">

                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div class="control-group" id="DivUpload" runat="server">
                                        <label class="control-label">Upload Document <span class="Mandotary">*</span></label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" accept=".xlsx,.xls" />
                                                <asp:Label ID="Label1" runat="server" Text="Initial Text"></asp:Label>
                                                <asp:Button ID="cmdupload" class="btn btn-primary" runat="server" Text="Upload" OnClick="cmdUpload_click" />
                                            </div>
                                            <div class="form-horizontal" align="center">
                                                <asp:Label ID="lblmsg" runat="server" ForeColor="Green"></asp:Label>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="form-horizontal" align="center">
                                        <div class="input-append">
                                            <asp:Button ID="cmdsave" class="btn btn-primary" runat="server" Text="Save" OnClick="cmdsave_click" />
                                            <asp:Button ID="cmdReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="cmdreset_click" />
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <!-- END SAMPLE FORM PORTLET-->
                        </div>
                    </div>
                </div>

                <!-- BEGIN PAGE CONTENT-->

                <!-- END PAGE CONTENT-->

            </div>

        </div>
        <style>
            a#ContentPlaceHolder1_gvFiles_lnkDownload_0 {
                white-space: nowrap;
            }
        </style>
</asp:Content>
