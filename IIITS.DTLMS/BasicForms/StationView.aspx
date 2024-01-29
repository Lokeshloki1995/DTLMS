<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StationView.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.StationView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
    <style type="text/css">
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
                    <h3 class="page-title">Station View
                    </h3>

          <%--          <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                 <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
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
                            <h4><i class="icon-reorder"></i>StatStation View</h4>

                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                                <div class="span6">
                                    <asp:Button ID="cmdNewStation" class="btn btn-success" Text="New Station"
                                        runat="server" OnClick="cmdNewStation_Click" />
                                    <br />
                                </div>
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_clickStation" /><br />
                                </div>
                            </div>


                           
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged" >
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
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubdiv" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
<%--                                                        <asp:Button ID="cmdLoad" runat="server" Text="Load"
                                                            CssClass="btn btn-primary" OnClick="cmdLoad_Click" />--%>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <center>
                                                   <div class="span11">
                                                   <asp:Button ID="cmdLoad" runat="server" Text="Load"
                                                            CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                                  </div>
                                            </center>
                                    </div>
                                </div>
                            </div>


                            <asp:GridView ID="grdStation" AutoGenerateColumns="false" PageSize="10"
                                AllowPaging="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" OnPageIndexChanging="grdStation_PageIndexChanging"
                                ShowFooter="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnRowCommand="grdStation_RowCommand"
                                OnSorting="grdStation_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ST_ID" HeaderText="Station name" Visible="false">

                                        <ItemTemplate>

                                            <asp:Label ID="lblstid" runat="server" Text='<%# Bind("ST_ID") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>



                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ST_STATION_CODE" HeaderText="Station CODE" Visible="true">

                                        <ItemTemplate>
                                            <asp:Label ID="lblStationCODE" runat="server" Text='<%# Bind("ST_STATION_CODE") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox ID="txtStationCode" Width="80px" runat="server"></asp:TextBox>
                                        </FooterTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ST_NAME" HeaderText="Station name" Visible="true" SortExpression="ST_NAME">

                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ST_ID") %>'></asp:HiddenField>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("ST_NAME") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtStationName" runat="server"></asp:TextBox>
                                        </FooterTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="STC_CAP_VALUE" HeaderText="Voltage Class">

                                        <ItemTemplate>
                                            <asp:Label ID="lblStaReq" runat="server" Text='<%# Bind("STC_CAP_VALUE") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:ImageButton ID="imbBtnDelete" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px"
                                                CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ST_DESCRIPTION" HeaderText="Description" SortExpression="ST_DESCRIPTION">

                                        <ItemTemplate>
                                            <asp:Label ID="lblStaDesc" runat="server" Text='<%# Bind("ST_DESCRIPTION") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>




                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                    OnClick="imgBtnEdit_Click" Width="12px" />
                                            </center>
                                        </ItemTemplate>


                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                    OnClick="imbBtnDelete_Click" Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                    CausesValidation="false" />
                                            </center>
                                        </ItemTemplate>


                                    </asp:TemplateField>

                                </Columns>



                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />



                            </asp:GridView>
                            <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>


                        </div>
                    </div>
                </div>

                <!-- END FORM-->

                <!-- END PAGE CONTENT-->
            </div>
        </div>
    </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;tton>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Station  Details and To Add New Station  
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit Existing Details Click On <u>Edit</u> LiknkButton
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New Station  Click On <u>New Station</u> LiknkButton
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
