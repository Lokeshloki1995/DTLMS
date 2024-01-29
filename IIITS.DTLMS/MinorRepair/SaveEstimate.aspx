<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="SaveEstimate.aspx.cs" Inherits="IIITS.DTLMS.MinorRepair.SaveEstimate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                else
                    return false;
            }

            if (txt.value.indexOf(".") > 0) {
                var txtlen = txt.value.length;
                var dotpos = txt.value.indexOf(".");
                //Change the number here to allow more decimal points than 2
                if ((txtlen - dotpos) > 2)
                    return false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">

                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Repairer Rates Entry
                    </h3>
                           <div style="float: right">
                                <div class="span12">
                                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                                        OnClientClick="javascript:window.location.href='ViewEstimateRate.aspx'; return false;"
                                        CssClass="btn btn-danger" /><br />
                                </div>
                            </div>
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->



            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Repairer - Division Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

                        <div class="widget-body">

                     <%--       <div style="float: right">
                                <div class="span6">
                                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                                        OnClientClick="javascript:window.location.href='ViewEstimateRate.aspx'; return false;"
                                        CssClass="btn btn-danger" /><br />
                                </div>
                            </div>--%>

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Select Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSelectType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbSelectType_SelectedIndexChanged">
                                                            <asp:ListItem Enabled="true" Text="Select Type" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Repairer" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Supplier" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtRepId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtCapacity" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtWndType" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Repairer<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                               <div class="control-group">
                                                <label class="control-label">Repair Center Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtRpCenterName" runat="server" AutoComplete="off" MaxLength="100" onkeypress="javascript:return AllowSpecialchar(event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Supplier<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSupplier" runat="server" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Effect From Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtFromDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Effect To Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">


                                            <div class="control-group">
                                                <label class="control-label">Capacity<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Winding Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbwound" runat="server" AutoPostBack="false">
                                                            <asp:ListItem Enabled="true" Text="Select Type" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Aluminium" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Copper" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Rating Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRate" runat="server" AutoPostBack="false">
                                                            <asp:ListItem Enabled="true" Text="Select Type" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Star Rate" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Conventional" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Rate Contract Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoNo" runat="server" MaxLength="20"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Rate Contract Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoDate" runat="server" MaxLength="20"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtPoDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <%-- <div class="control-group">
                                                <label class="control-label">Upload File<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                          <asp:FileUpload ID="FileUpload1" runat="server"   Font-Size="Medium" Height="28px" Width="301px" />  
                                                          <p>  
                                                          <asp:Button ID="btnUpload" runat="server"  class="btn btn-success" BorderStyle="Solid" Font-Bold="True" Font-Italic="False" 
                                                           Font-Size="large" Height="38px" OnClick="FTPUpload" Text="Upload" Width="150px" />             
                                                           </p>  
                                                    </div>
                                                </div>
                                            </div>--%>





                                        </div>
                                    </div>
                                </div>
                                <div class="space20"></div>

                                <div class="form-horizontal" align="center">

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>

                                <div class="space20"></div>
                                <!-- END FORM-->
                                <div class="form-horizontal" align="center">

                                    <div class="span3"></div>


                                    <div class="span7"></div>
                                    <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                                <div class="space5"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END PAGE CONTENT-->
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Material Cost Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div>
                                            <asp:GridView ID="grdMaterialMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                runat="server" ShowHeaderWhenEmpty="True" OnPageIndexChanging="grdMaterial_PageIndexChanging"
                                                AllowSorting="true" OnRowDataBound="grdMaterialMast_RowDataBound">
                                                <HeaderStyle CssClass="both" />
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMaterialId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="ITEMTYPE" HeaderText="ITEMTYPE" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemType" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                        Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtMaterialName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMaterialName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                Style="word-break: break-all;" Width="200px"></asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtMQuantity" runat="server" Width="100px" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMqty" ReadOnly="true" runat="server" Text='<%# Bind("MRI_QUANTITY") %>' Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Rate">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtMBaseRate" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMBRate" runat="server" Width="100px" Text='<%# Bind("MRI_BASE_RATE") %>' MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Rate(%)">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtMTaxRate" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMTRate" runat="server" Width="100px" Text='<%# Bind("MRI_TAX") %>' MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" Visible="false">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtMTotal" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMTtal" runat="server" Text='0' Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="Measurement" HeaderText="Measurement">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cmbMeasurement" runat="server" Style="width: 100px">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem Value="Litre">Litre</asp:ListItem>
                                                                <asp:ListItem Value="KG">KG</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Select Data">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBx1" runat="server" />
                                                            </center>
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
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Labour Cost Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div>
                                            <asp:GridView ID="grdLabourMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                runat="server" ShowHeaderWhenEmpty="True" OnPageIndexChanging="grdLabour_PageIndexChanging"
                                                AllowSorting="true" OnRowDataBound="grdLabourMast_RowDataBound">
                                                <HeaderStyle CssClass="both" />
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabourId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="ITEMTYPE" HeaderText="ITEMTYPE" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabItemType" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                        Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLabourName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabourName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                Style="word-break: break-all;" Width="200px"></asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtLqty" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLqty" Text='<%# Bind("MRI_QUANTITY") %>' ReadOnly="true" runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Rate">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtLBaseRate" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLBRate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>' Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Rate(%)">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtLTaxRate" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLTRate" runat="server" Text='<%# Bind("MRI_TAX") %>' Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" Visible="false">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtLTotal" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLTotal" runat="server" Text='0' Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="Measurement" HeaderText="Measurement">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cmbLabMeasurement" runat="server" Style="width: 100px">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem Value="Litre">Litre</asp:ListItem>
                                                                <asp:ListItem Value="KG">KG</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select Data">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                                            </center>
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
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Salvages Cost Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div>
                                            <asp:GridView ID="grdSalvageMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                runat="server" ShowHeaderWhenEmpty="True" OnPageIndexChanging="grdSalvage_PageIndexChanging"
                                                AllowSorting="true" OnRowDataBound="grdSalvageMast_RowDataBound">
                                                <HeaderStyle CssClass="both" />
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalvageId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="ITEMTYPE" HeaderText="ITEMTYPE" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsalItemType" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                        Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtSalvageName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalvageName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                Style="word-break: break-all;" Width="200px"></asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtSqty" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSqty" Text='<%# Bind("MRI_QUANTITY") %>' ReadOnly="true" runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Rate">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtSBaseRate" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSBRate" Text='<%# Bind("MRI_BASE_RATE") %>' runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Rate(%)">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtSTaxRate" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSTRate" runat="server" Text='<%# Bind("MRI_TAX") %>' Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" Visible="false">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtLSTotal" runat="server" Width="100px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLSTotal" Text='0' runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="Measurement" HeaderText="Measurement">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cmbSalMeasurement" runat="server" Style="width: 100px">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem Value="Litre">Litre</asp:ListItem>
                                                                <asp:ListItem Value="KG">KG</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select Data">
                                                        <EditItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="CheckBox3" runat="server" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox3" runat="server" />
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="space20"></div>
                                <!-- END FORM-->
                                <div class="text-center" >

                                    
                                   
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave1_Click"
                                            CssClass="btn btn-primary" />
                                   
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                  
                                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                            </div>
                        </div>

                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->

        </div>
    </div>

</asp:Content>
