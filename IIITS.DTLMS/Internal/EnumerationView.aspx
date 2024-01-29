<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="EnumerationView.aspx.cs" Inherits="IIITS.DTLMS.Internal.EnumerationView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../assets/bootstrap-toggle-buttons/static/js/jquery.min.js"></script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('#cmdLoad').click(function () {
                $('#grdEnumerationDetails').empty();
                LoadEnumeration();
            })
        });


        function LoadEnumeration() {
            debugger;
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: 'EnumerationView.aspx/LoadEnumeration',
                data: "{}",
                dataType: 'JSON',
                success: function (response) {
                    $('#grdEnumerationDetails').append("<tr><th>ED_ID </th><th>TYPE </th><th>DTE_DTCCODE </th><th>DTE_NAME </th><th>DTE_TC_CODE </th><th>OPERATOR1 </th><th>MAKE </th><th>DTE_TC_SLNO </th><th>ED_LOCTYPE </th><th>ED_STATUS_FLAG </th></tr>")
                    for (var i = 0; i < response.d.length; i++) {
                        debugger;
                        $('#grdEnumerationDetails').append("<tr><td>" + response.d[i].ED_ID + "</td><td>" + response.d[i].TYPE + "</td><td>" + response.d[i].DTE_DTCCODE + "</td><td>" + response.d[i].DTE_NAME + "</td><td>" + response.d[i].DTE_TC_CODE + "</td><td>" + response.d[i].OPERATOR1 + "</td><td>" + response.d[i].MAKE + "</td><td>" + response.d[i].DTE_TC_SLNO + "</td><td>" + response.d[i].ED_LOCTYPE + "</td><td>" + response.d[i].ED_STATUS_FLAG + "</td></tr>")
                    };

                },
                error: function () {

                    alert("Error");
                }
            });
            return false;
        };

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
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Enumeration View
                    </h3>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
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
                            <h4><i class="icon-reorder"></i>Enumeration View </h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                           
                            <div class="container-fluid">
                                <div class="span2" style="float: left">
                                    <asp:RadioButtonList ID="rdbPendingQC" runat="server" CssClass="radio">
                                        <asp:ListItem Text="Pending QC" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="QC Done" Value="1"></asp:ListItem>
                                        <%--<asp:ListItem Text="Pending Clarification" Value="2"></asp:ListItem>--%>
                                        <asp:ListItem Text="Reject" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>



                                </div>

                                <div class="span2" style="float: left">
                                    <asp:RadioButtonList ID="rdbLocationType" runat="server" CssClass="radio">
                                        <asp:ListItem Text="Field" Value="2"></asp:ListItem>
                                       <%-- <asp:ListItem Text="Store/Bank" Value="1,5"></asp:ListItem>--%>
                                         <asp:ListItem Text="Store" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Bank" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="Repairer" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>

            <%--                    <div class="span2" style="float: left">
                                    <asp:Label ID="lblStatus" runat="server" Text="Operator" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbOperator" runat="server" TabIndex="9" Width="150px">
                                        </asp:DropDownList>
                                    <div class="space10"></div>
                                    <asp:Label ID="Label1" runat="server" Text="Supervisor" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbSupervisor" runat="server" TabIndex="9" Width="150px">
                                        </asp:DropDownList>
                                </div>--%>

                                <div class="span3" style="float: left">
                                    <asp:Label ID="Label2" runat="server" Text="Division" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbDivision" runat="server" Width="150px" TabIndex="9" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    <div class="space10"></div>
                                    <asp:Label ID="Label3" runat="server" Text="SubDivision" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;
                                        <asp:DropDownList ID="cmbSubdivision" runat="server" Width="150px" AutoPostBack="true"
                                            TabIndex="9" OnSelectedIndexChanged="cmbSubdivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    <div class="space10"></div>
                                    <asp:Label ID="Label4" runat="server" Text="Section" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbSection" runat="server" Width="150px" TabIndex="9">
                                        </asp:DropDownList>
                                    <div class="space10"></div>
                                    <%--<asp:Label ID="Label9" runat="server" Text="Re-work" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbRework" runat="server" Width="150px" TabIndex="9">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                     <div class="space10"></div>--%>
                                   
                                   
                                </div>

                                <div class="span3" style="float: left">
                                    <asp:Label ID="Label5" runat="server" Text="Feeder" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbFeeder" runat="server" Width="150px" TabIndex="9">
                                        </asp:DropDownList>
                                    <div class="space10"></div>
                                    <asp:Label ID="Label6" runat="server" Text="Store" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbStore" runat="server" Width="150px" TabIndex="9">
                                        </asp:DropDownList>
                                    <div class="space10"></div>
                                    <asp:Label ID="Label7" runat="server" Text="Repairer" Font-Bold="False"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="cmbRepairer" runat="server" Width="150px" TabIndex="9">
                                    </asp:DropDownList>
                                    <div class="space10"></div>

                                    <asp:Label ID="Label8" runat="server" Text="Status" Font-Bold="False"
                                        Font-Size="Medium" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="cmbMake" runat="server" Width="150px" TabIndex="10" Visible="false">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="space10"></div>

                                    <div style="float: right;display:flex!important">
                                        
<%--                                        <asp:CheckBox ID="chkViewAll" runat="server" style="margin-right:80px!important" CssClass="checkbox" Text="View All" />
                                    
                                        <asp:CheckBox ID="chkMobileApp" runat="server"  CssClass="checkbox" Text="Mobile Entry" />--%>
                                            <asp:RadioButton ID="ViewAll" runat="server"  Text=" &nbsp; View All"  GroupName="view" Checked="true"  />  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                 <asp:RadioButton ID="Mob_entry" runat="server" Text="  &nbsp; Mobile Entry"  GroupName="view" />
                                    </div>


                                </div>
                            </div>
                           

                             <div style="margin-top:102px!important" class="text-center">

                              
                                    <asp:Button ID="cmdLoad" CssClass="btn btn-primary" runat="server" Text="Load"
                                        OnClick="cmdLoad_Click" />
                              

                            </div>
                            <div class="space10"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdEnumerationDetails" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnRowCommand="grdEnumerationDetails_RowCommand"
                                OnPageIndexChanging="grdEnumerationDetails_PageIndexChanging">
                                <Columns>

                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnumDetailsId" runat="server" Text='<%# Bind("ED_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TYPE" HeaderText="Enum. Type" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnuType" runat="server" Text='<%# Bind("TYPE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DTC CODE" HeaderText="DTC Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("DTE_DTCCODE") %>' Style="word-break: break-all" Width="130px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDTCcode" runat="server" placeholder="Enter DTC Code " Width="130px" MaxLength="10"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DTC NAME" HeaderText="DTC Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDTCName" runat="server" Text='<%# Bind("DTE_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px" MaxLength="25"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC Code" HeaderText="DTr Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DTE_TC_CODE") %>' Style="word-break: break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                     <%--       <asp:TextBox ID="txtTCcode" runat="server" placeholder="Enter DTr Code " Width="100px" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtTCcode" runat="server" placeholder="Enter DTr Code " Width="100px" MaxLength="10"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

<%--                                    <asp:TemplateField AccessibleHeaderText="OPERATOR1" HeaderText="Operator 1" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="Operator1" runat="server" Text='<%# Bind("OPERATOR1") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtOperator1" runat="server" placeholder="Enter Operator1 " Width="120px" MaxLength="25"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField AccessibleHeaderText="TCSLNO" HeaderText="Serial NO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCSLNO" runat="server" Text='<%# Bind("DTE_TC_SLNO") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSlNo" runat="server" placeholder="Enter SRL No " Width="120px" MaxLength="25"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit" Visible="true">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton CommandName="Submit" ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                    Width="12px" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="View"  Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton CommandName="View" ID="imgView" runat="server" Height="12px" ImageUrl="~/img/Manual/view.png"
                                                    Width="12px" ToolTip="View Comments" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ED_LOCTYPE" HeaderText="ED_LOCTYPE" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnumType" runat="server" Text='<%# Bind("ED_LOCTYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ED_ENUM_TYPE" HeaderText="ED_STATUS_FLAG" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusFlag" runat="server" Text='<%# Bind("ED_STATUS_FLAG") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ED_IS_FEEDER_BIFURCATION" HeaderText="ED_IS_FEEDER_BIFURCATION" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFeederBifrcation" runat="server" Text='<%# Bind("ED_IS_FEEDER_BIFURCATION") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>

                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>

                <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdPopClose"
                    PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                    <div style="display: none">
                        <asp:Button ID="btnshow" runat="server" Text="Button" />
                    </div>
                    <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="362px" Width="434px">
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <asp:Label ID="lblHeader" runat="server" Text="Comments"></asp:Label>
                                </h4>
                                <div class="space20"></div>
                                <%--<div class="row-fluid">--%>
                                <div class="span1"></div>

                                <div class="space20">

                                    <div class="span1"></div>

                                    <div class="span5">
                                        <div class="control-group" style="font-weight: bold">
                                            <label class="control-label">Comments</label>
                                            <div class="controls">
                                                <div class="input-append" align="center">
                                                    <div class="span3"></div>
                                                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="500" TabIndex="4" TextMode="MultiLine"
                                                        Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,500)" Width="250px" Height="165px"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div>

                                            <div class="control-group" style="font-weight: bold">
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <div class="span3"></div>
                                                        <div class="span7">
                                                        </div>
                                                        <div class="span1">
                                                            <asp:Button ID="cmdPopClose" runat="server" CssClass="btn btn-primary"
                                                                TabIndex="10" Text="Close" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="space20" align="center">
                                            <div class="form-horizontal" align="center">
                                                <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <!-- END PAGE CONTENT-->
        </div>
    </div>
</asp:Content>
