<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" 
    CodeBehind="WorkOrderView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.WorkOrderView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function ConfirmWO() {
            var sTypeValue = document.getElementById('<%= cmbType.ClientID %>');
            var selectedText = sTypeValue.options[sTypeValue.selectedIndex].innerHTML;
            var result = confirm('Are you sure,Do you want to declare Work Order for ' + selectedText + ' ?');
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
        button.btn, input[type="submit"].btn {
    padding-top: 3px;
    padding-bottom: 3px;
    width: 130px;
}
         a:hover {
    text-shadow: none !important;
    color: blue;
}
         .widget-body {
    padding: 15px 15px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Work Order View
                </h3>
                <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" 
                    data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" 
                        style="font-size: 16px"></i></a>
                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="WorkOrderView.aspx.cs" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
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
                        <h4><i class="icon-reorder"></i>Work Order View</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span1"> </div>
                                <div class="span5">
                                     <div class="control-group">
                                        <label class="control-label"> Type   </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbType_SelectedIndexChanged">
                                                    <asp:ListItem Text="Failure" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Failure With Enhancement" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Enhancement" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">
                                            Circle
                                        </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1"
                                                    OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">
                                            Sub Division</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbSubDivision" runat="server" AutoPostBack="true" TabIndex="1"
                                                    OnSelectedIndexChanged="cmbSubDivision_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">
                                            From Date </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                    TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                </ajax:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <%-- another span--%>
                                <div class="span5">
                                    <div class="control-group">
                                        <label class="control-label">
                                            Zone
                                        </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1"
                                                    OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">
                                            Division</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1"
                                                     OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="control-group">
                                        <label class="control-label">
                                            O & M Section</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbOMSection" runat="server" TabIndex="1">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">
                                            To Date </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                    TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                </ajax:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="span1">  </div>
                                <div class="span1">  </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="text-center">
                                            <asp:Button ID="cmdGenerate" runat="server" Text="Load"
                                                CssClass="btn btn-primary" TabIndex="10" OnClick="cmdLoad_Click" />
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                TabIndex="11" OnClick="cmdReset_Click" />
                                            <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"
                                                OnClick="Export_ClickWorkorder" /><br />

                                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <br />

                        <%--        <div class="span2">
                                    <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" Visible="false"
                                        Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="span1">
                                    <asp:RadioButton ID="rdbViewAll" runat="server" Text="View All" CssClass="radio"
                                        GroupName="a" AutoPostBack="true" Style="display: none"
                                        OnCheckedChanged="rdbViewAll_CheckedChanged" />
                                </div>
                                <div class="span4">
                                    <asp:RadioButton ID="rdbAlready" runat="server" Text="Already Created"
                                        CssClass="radio" GroupName="a" AutoPostBack="true" Checked="true" Style="display: none"
                                        OnCheckedChanged="rdbAlready_CheckedChanged" />
                                </div>
                                <div style="float: right;">
                                    <asp:Button ID="cmdNew" runat="server" Text="New" OnClientClick="return ConfirmWO();"
                                        CssClass="btn btn-primary" OnClick="cmdNew_Click" Visible="false" />
                                </div>--%>
                            </div>
                        </div>
                    </div>

                    <div class="widget-body">
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <asp:Label ID="lblGridType" runat="server" Font-Bold="true" ForeColor="#4A8BC2"
                                    Font-Size="Medium"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div style="width: 100%; height: 560px; overflow: scroll">
                    <asp:GridView ID="grdWorkOrder" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        CssClass="table table-striped table-bordered table-advance table-hover" ShowFooter="true"
                        runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                        OnRowCommand="grdWorkOrder_RowCommand"
                        OnPageIndexChanging="grdWorkOrder_PageIndexChanging"
                        OnRowDataBound="grdWorkOrder_RowDataBound" OnSorting="grdWorkOrder_Sorting" AllowSorting="true">
                        <HeaderStyle CssClass="both" />

                        <Columns>
                            <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>    <%--    <%#Container.DataItemIndex+1 %>--%>
                                    <asp:Label ID="lblslnoId" runat="server" width="50" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFailureId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                        <asp:TextBox ID="txtFailureId" runat="server" placeholder="Enter Failure Id "
                                             Width="150px" MaxLength="10"></asp:TextBox>
                                    </asp:Panel>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- Both Columns are same but adding for User Interface Purpose--%>
                            <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Enhancement ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEnhanceId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                        <asp:TextBox ID="txtEnhanceId" runat="server" placeholder="Enter Enhance Id " 
                                            Width="150px" MaxLength="10"></asp:TextBox>
                                    </asp:Panel>
                                </FooterTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division" SortExpression="DIVISION">
                                <ItemTemplate>
                                    <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SUBDIVISION" HeaderText="SubDivision" SortExpression="SUBDIVISION">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubDivision" runat="server" Text='<%# Bind("SUBDIVISION") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="SECTION" HeaderText="Section" SortExpression="SECTION">
                                <ItemTemplate>
                                    <asp:Label ID="lblSection" runat="server" Text='<%# Bind("SECTION") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" SortExpression="DT_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblDtCode" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch">
                                        <asp:TextBox ID="txtDtcCode" runat="server" placeholder="Enter DTC Code " 
                                            Width="150px" MaxLength="6"></asp:TextBox>
                                    </asp:Panel>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name" SortExpression="DT_NAME">
                                <ItemTemplate>
                                    <asp:Label ID="lblDtName" runat="server" Text='<%# Bind("DT_NAME") %>' 
                                        Style="word-break: break-all;" Width="200px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch">
                                        <asp:TextBox ID="txtdtcName" runat="server" placeholder="Enter DTC Name " 
                                            Width="150px" MaxLength="50"></asp:TextBox>
                                    </asp:Panel>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" SortExpression="TC_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' 
                                        Style="word-break: break-all;" Width="100px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Panel ID="panel5" runat="server" DefaultButton="imgBtnSearch">
                                        <asp:TextBox ID="txtDtrCode" runat="server" placeholder="Enter DTr Code "
                                             Width="150px" MaxLength="8"></asp:TextBox>
                                    </asp:Panel>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="" HeaderText="WO Issued" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Comm WO No" SortExpression="WO_NO">
                                <ItemTemplate>
                                    <asp:Label ID="lblWoNo" runat="server" Text='<%# Bind("WO_NO") %>' 
                                        Style="word-break: break-all;" Width="200px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" 
                                        CommandName="search" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="Comm WO Date" SortExpression="WO_DATE">
                                <ItemTemplate>
                                    <asp:Label ID="lblWoDate" runat="server" Text='<%# Bind("WO_DATE") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_NO_DECOM" HeaderText="DeComm WO No" SortExpression="WO_NO_DECOM">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeCommWoNo" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' 
                                        Style="word-break: break-all;" Width="200px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_DATE_DECOM" HeaderText="DeComm WO Date" 
                                SortExpression="WO_DATE_DECOM">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeCommWoDate" runat="server" Text='<%# Bind("WO_DATE_DECOM") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <center>
                                        <asp:LinkButton runat="server" CommandName="CreateNew" ID="lnkCreate">
                                             <img src="../Styles/images/Create.png" style="width:20px" />Issue WO</asp:LinkButton>
                                        <asp:LinkButton runat="server" CommandName="Create" ID="lnkUpdate" Visible="false">
                                          View</asp:LinkButton>
                                    </center>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="lblHeader" runat="server" Text="Action"></asp:Label>
                                    </center>
                                </HeaderTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                        <div />

                    <asp:GridView ID="grdNewDTC" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        CssClass="table table-striped table-bordered table-advance table-hover" Visible="false"
                        runat="server" ShowHeaderWhenEmpty="True" ShowFooter="true"
                        EmptyDataText="No Records Found"
                        OnPageIndexChanging="grdNewDTC_PageIndexChanging"
                        OnRowCommand="grdNewDTC_RowCommand" OnSorting="grdNewDTC_Sorting" AllowSorting="true">
                        <HeaderStyle CssClass="both" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_SLNO" HeaderText="WO Slno" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWOSlno" runat="server" Text='<%# Bind("WO_SLNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WO No" SortExpression="WO_NO">
                                <ItemTemplate>
                                    <asp:Label ID="lblWoNo" runat="server" Text='<%# Bind("WO_NO") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWoNo" runat="server" placeholder="Enter WO No" Width="150px" MaxLength="17"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblWoDate" runat="server" Text='<%# Bind("WO_DATE") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="WO_ACC_CODE" HeaderText="WO Account Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblWoAccountCode" runat="server" Text='<%# Bind("WO_ACC_CODE") %>' 
                                        Style="word-break: break-all;" Width="150px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <center>
                                        <asp:LinkButton runat="server" CommandName="Create" ID="lnkUpdate">
                                             <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                    </center>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="lblHeader" runat="server" Text="Action"></asp:Label>
                                    </center>
                                </HeaderTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                <div class="span2">
                                    <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" Visible="false"
                                        Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="span1">
                                    <asp:RadioButton ID="rdbViewAll" runat="server" Text="View All" CssClass="radio"
                                        GroupName="a" AutoPostBack="true" Style="display: none"
                                        OnCheckedChanged="rdbViewAll_CheckedChanged" />
                                </div>
                                <div class="span4">
                                    <asp:RadioButton ID="rdbAlready" runat="server" Text="Already Created"
                                        CssClass="radio" GroupName="a" AutoPostBack="true" Checked="true" Style="display: none"
                                        OnCheckedChanged="rdbAlready_CheckedChanged" />
                                </div>
                                <div style="float: right;">
                                    <asp:Button ID="cmdNew" runat="server" Text="New" OnClientClick="return ConfirmWO();"
                                        CssClass="btn btn-primary" OnClick="cmdNew_Click" Visible="false" />
                                </div>
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
        </div>
    </div>
    <!-- END PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Type Of WorkOrder Based On Location Filter.
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
