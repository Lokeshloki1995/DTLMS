<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RepairerRatesView.aspx.cs" Inherits="IIITS.DTLMS.MinorRepair.RepairerRatesView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.9.1/font/bootstrap-icons.css"/>--%>
    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' User?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function isNumberKey(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
                return false;
            else {
                var len = $(element).val().length;
                var index = $(element).val().indexOf('.');
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 3) {
                        return false;
                    }
                }

            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function ValidateMyForm() {

            debugger;
            if (document.getElementById('<%= cmbDivision1.ClientID %>').value == "--Select--") {
                alert('Please Select Division')
                document.getElementById('<%= cmbDivision1.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmdRepairer1.ClientID %>').value == "--Select--") {
                alert('Please Select Repairer')
                document.getElementById('<%= cmdRepairer1.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtEffectFrom.ClientID %>').value == "") {
                alert('Please Select Effective From Date')
                document.getElementById('<%= txtEffectFrom.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmdCapacity1.ClientID %>').value == "--Select--") {
                alert('Please Select Capacity')
                document.getElementById('<%= cmdCapacity1.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmdStarrate1.ClientID %>').value == "--Select--") {
                alert('Please Select Star Rate')
                document.getElementById('<%= cmdStarrate1.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtEffectTo.ClientID %>').value == "") {
                alert('Please Select Effective To Date')
                document.getElementById('<%= txtEffectTo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtCost.ClientID %>').value == "") {
                alert('Please Enter Cost Of Repairer')
                document.getElementById('<%= txtCost.ClientID %>').focus()
                return false
            }
        }
    </script>



    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdRatesView').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({

                "sPaginationType": "full_numbers"
            });
        });
    </script>

    <style type="text/css">
        table#ContentPlaceHolder1_grdRatesView {
            overflow: auto;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        th {
            white-space: nowrap;
            text-align: center !important;
        }

        thead {
            text-align: center !important;
        }

        span {
            text-align: center;
        }

        .modalBackground {
            background-color: blueviolet;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .asc {
        }

        .modalBackground {
            background-color: #b4b4ec !important;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <%-- <style type="text/css">
        table {
            overflow: scroll;
        }

        td {
            border: 1px solid #ccc;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        table.dataTable thead th {
            border-bottom: 1px solid #111;
            font-size: 12px !important;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 10px 0px !important;
            text-align: center !important;
        }

        .table-advance tr td {
            border-left-width: 1px !important;
            border: 1px solid #d4d4d4;
            font-size: 12px !important;
        }

        th {
            white-space: nowrap;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Rate Contract Details 
                    </h3>

                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Rate Contract Details </h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span6">
                                    <asp:Button ID="cmdNew" runat="server" Text="New Rate"
                                        CssClass="btn btn-success" OnClick="cmdNew_Click" /><br />
                                </div>

                            </div>

                            <div class="span5">

                                <div class="control-group">
                                    <label class="control-label">Division Name</label>

                                    <div class="input-append">
                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmdDivChangeClick">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                <div class="control-group">
                                    <label class="control-label">Repairer</label>

                                    <div class="input-append">
                                        <asp:DropDownList ID="cmdRepairer" runat="server">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdfdivid" runat="server" />
                                        <asp:HiddenField ID="hdfrepid" runat="server" />
                                    </div>

                                </div>
                            </div>

                            <div class="span5">
                                <div class="control-group">
                                    <label class="control-label">Capacity</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmdCapacity" runat="server">
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Star Rate</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList runat="server" ID="cmdStarrate">
                                                <asp:ListItem Value="--Select--" Text="--Select--">  </asp:ListItem>
                                                <asp:ListItem Value="1" Text="Conventional">  </asp:ListItem>
                                                <asp:ListItem Value="2">Star Rated</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span11">
                                <div class="text-center">
                                    <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                        OnClick="cmdLoad_Click" />
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" TabIndex="11"
                                        CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                </div>
                            </div>

                            <div class="space20"></div>

                            <!-- END FORM-->
                             <div style="width: 100%!important; height: 100%!important; overflow: scroll!important">
                            <asp:GridView ID="grdRatesView" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" OnPageIndexChanging="grdRatesView_PageIndexChanging" OnRowCommand="grdRatesView_RowCommand" OnRowDataBound="grdRatesView_RowDataBound" OnSorting="grdRatesView_Sorting">
                                <HeaderStyle CssClass="both" />

                                <Columns>
                                    <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="srr_id" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%# Bind("srr_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MRI_CAPACITY" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCAP" runat="server" Text='<%# Bind("srr_cap_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MRI_TR_ID" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltrID" runat="server" Text='<%# Bind("srr_rep_id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="sdiv_id" HeaderText="sdiv_idE" SortExpression="sdiv_id" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lbldivID" runat="server" Text='<%# Bind("sdiv_id") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="sstar_id" HeaderText="sstar_id" SortExpression="sstar_id" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblstarID" runat="server" Text='<%# Bind("sstar_id") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="sDIV_NAME" HeaderText="DIVISION" Visible="true" SortExpression="sDIV_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("sDIV_NAME") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="VENDOR_NAME" HeaderText="REPAIRER NAME" SortExpression="sVENDOR_NAME">

                                        <ItemTemplate>
                                            <asp:Label ID="lblvenname" runat="server" Text='<%# Bind("sVENDOR_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MD_NAME" HeaderText="CAPACITY" SortExpression="sMD_NAME">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("sMD_NAME") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="star_rate" HeaderText="STAR RATE" SortExpression="star_rate">

                                        <ItemTemplate>
                                            <asp:Label ID="lblstar_rate" runat="server" Text='<%# Bind("star_rate") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="spo_no" HeaderText="PO NO" SortExpression="spo_no">

                                        <ItemTemplate>
                                            <asp:Label ID="lblPono" runat="server" Text='<%# Bind("spo_no") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="spo_date" HeaderText="PO DATE" SortExpression="spo_date">

                                        <ItemTemplate>
                                            <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("spo_date") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="FROM" HeaderText="EFFECT FROM" SortExpression="sFROM">

                                        <ItemTemplate>
                                            <asp:Label ID="lblFrom" runat="server" Text='<%# Bind("sFROM") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TO" HeaderText="EFFECT TO" SortExpression="sTO">

                                        <ItemTemplate>
                                            <asp:Label ID="lblTo" runat="server" Text='<%# Bind("sTO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="srr_amount" HeaderText="REPAIRER COST" SortExpression="srr_amount">

                                        <ItemTemplate>
                                            <asp:Label ID="lblamount" runat="server" Text='<%# Bind("srr_amount") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Repairer Award Doc">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnImageSomething" runat="server" CommandName="DownloadClick" ImageUrl="../Styles/images/Download.png" Style="width: 12px; height: 13px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <center>
                                                     <asp:LinkButton  runat="server" CommandName="DownloadClick" ID="imgBtnDlt" ToolTip="Click To Download" ">
                                                         <img src="https://cdn-icons.flaticon.com/png/128/3121/premium/3121602.png?token=exp=1660226102~hmac=17504c18423750e84edf7130fe895e6a" style="width:12px;Height:13px" /></asp:LinkButton>
                                                </center>
                                            </ItemTemplate><img src="https://cdn-icons.flaticon.com/png/128/3121/premium/3121602.png?token=exp=1660226102~hmac=17504c18423750e84edf7130fe895e6a" style="width:12px;Height:13px" /></asp:LinkButton>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton runat="server" CommandName="EditClick" ID="imgBtnEdit" ToolTip="Click To Edit" OnClientClick="return confirm ('Are you sure,Do you want to Edit?');">
                                         <img src="../Styles/images/edit64x64.png" style="width:12px;Height:13px" /></asp:LinkButton>
                                                &nbsp&nbsp&nbsp&nbsp&nbsp;
                                                <asp:LinkButton runat="server" CommandName="DeleteClick" ID="imbBtnDelete" ToolTip="Click To Delete" OnClientClick="return confirm ('Are you sure,Do you want to delete?');">
                                         <img src="../Styles/images/delete64x64.png" style="width:12px;Height:13px"  /></asp:LinkButton>

                                                <%-- <asp:ImageButton ID="imgBtnEdit1" title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                    Width="12px" />--%>
                                            </center>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                </Columns>

                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                            </asp:GridView>
                                 </div>
                        </div>

                    </div>

                    <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                        PopupControlID="pnlControls" BackgroundCssClass="modalBackground" Drag="true" />
                    <div style="width: 100%; vertical-align: middle; height: 469px;" align="center">
                        <div style="display: none">
                            <asp:Button ID="btnshow" runat="server" Text="Button" />
                        </div>
                        <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="550px" Width="850px">
                            <div class="widget blue">
                                <div class="widget-title">
                                    <h4>Repairer Details </h4>
                                    <%-- <i class="bi bi-x-circle-fill fa-6px"></i>--%>
                                    <div class="space20"></div>
                                    <%--<div class="row-fluid">--%>
                                    <div class="span1"></div>
                                    <div class="space20">
                                        <div class="span1"></div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Division Name<span class="Mandotary"> *</span></label>

                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdnID" runat="server" />
                                                    <asp:DropDownList ID="cmbDivision1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmdDiv1ChangeClick">
                                                    </asp:DropDownList>
                                                </div>

                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Repairer<span class="Mandotary"> *</span></label>

                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdRepairer1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmdRepairer1ChangeClick" >
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="control-group" style="font-weight: bold">
                                                <label class="control-label">Effect From<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append" align="center">
                                                        <asp:TextBox ID="txtEffectFrom" runat="server" MaxLength="10" TabIndex="3" autocomplete="off"> </asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server"
                                                            CssClass="cal_Theme1" TargetControlID="txtEffectFrom" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="font-weight: bold">
                                                <label class="control-label">PO Number </label>
                                                <div class="controls">
                                                    <div class="input-append" align="center">
                                                        <asp:TextBox ID="txtPoNo" runat="server" autocomplete="off" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="font-weight: bold">
                                                <label class="control-label">Repairer Cost<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append" align="center">
                                                        <asp:TextBox ID="txtCost" runat="server" autocomplete="off" MaxLength="9" TabIndex="3" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Capacity<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmdCapacity1" runat="server">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Star Rate<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList runat="server" ID="cmdStarrate1">
                                                            <asp:ListItem Value="--Select--" Text="--Select--">  </asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Conventional">  </asp:ListItem>
                                                            <asp:ListItem Value="2">Star Rated</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="font-weight: bold">
                                                <label class="control-label">Effect To<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append" align="center">

                                                        <asp:TextBox ID="txtEffectTo" runat="server" MaxLength="10" TabIndex="3" autocomplete="off"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server"
                                                            CssClass="cal_Theme1" TargetControlID="txtEffectTo" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="font-weight: bold">
                                                <label class="control-label">PO Date</label>
                                                <div class="controls">
                                                    <div class="input-append" align="center">
                                                        <asp:TextBox ID="txtPoDate" runat="server" MaxLength="10" TabIndex="3" autocomplete="off"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server"
                                                            CssClass="cal_Theme1" TargetControlID="txtPoDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <%--   <div class="control-group" id="fupBudget" runat="server">
                                                <label class="control-label">Repairer Award<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append" align="center">
                                                        <asp:FileUpload ID="fupdDoc" runat="server" accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                        <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>


                                        <div class="space20"></div>
                                        <div class="control-group" style="font-weight: bold">

                                            <div class="controls">
                                                <div class="input-append">

                                                    <div class="span11">
                                                        <div class="text-center">
                                                            <asp:Button ID="cmdSubmit" runat="server" CssClass="btn btn-primary"
                                                                OnClick="cmdSubmit_Click" OnClientClick="javascript:return ValidateMyForm()" TabIndex="10" Text="Submit" />
                                                            <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-danger"
                                                                TabIndex="10" Text="Close" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--  <div class="span5">
                                                <div class="control-group" style="font-weight: bold">
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <div class="span1">
                                                                
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        <div class="space20" align="center">

                                            <div class="form-horizontal" align="center">
                                                <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="space20"></div>
                            <div class="space20"></div>

                        </asp:Panel>
                    </div>

                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>


            <!-- END PAGE CONTENT-->
        </div>
</asp:Content>
