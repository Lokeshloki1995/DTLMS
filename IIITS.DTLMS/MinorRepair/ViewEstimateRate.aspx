<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ViewEstimateRate.aspx.cs" Inherits="IIITS.DTLMS.MinorRepair.ViewEstimateRate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
    </script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
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
    </style>


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
                    <h3 class="page-title">ItemWise Rate Contract Details 
                    </h3>
                    <%--<a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
                                </div>
                            </form>
                        </li>
                    </ul>--%>
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
                            <h4><i class="icon-reorder"></i>ItemWise Rate Contract Details </h4>
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

                            <div class="space20"></div>

                            <!-- END FORM-->


                            <asp:GridView ID="grdEstimation" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" AllowSorting="true" OnPageIndexChanging="grdEstimation_PageIndexChanging" OnRowCommand="grdEstimation_RowCommand" OnRowDataBound="grdEstimation_RowDataBound" OnSorting="grdEstimation_Sorting">
                                <HeaderStyle CssClass="both" />

                                <Columns>
                                    <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MRI_CAPACITY" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCAP" runat="server" Text='<%# Bind("MRI_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MRI_TR_ID" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltrID" runat="server" Text='<%# Bind("MRI_TR_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 

                                     <asp:TemplateField AccessibleHeaderText="MRI_WOUNDTYPE" HeaderText="RECORD ID"  Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblRecordId" runat="server" Text='<%# Bind("MRI_RECORD_ID") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="DIVISION" Visible="true" SortExpression="DIV_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV_NAME") %>'  Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="VENDOR_TYPE" HeaderText="VENDOR TYPE" SortExpression="VENDOR_TYPE">

                                        <ItemTemplate>
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("VENDOR_TYPE") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtventype" runat="server" Style="word-break: break-all;" Width="100px" placeholder="VENDOR TYPE"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="VENDOR_NAME" HeaderText="VENDOR NAME" SortExpression="VENDOR_NAME">

                                        <ItemTemplate>
                                            <asp:Label ID="lblvenname" runat="server" Text='<%# Bind("VENDOR_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:TextBox ID="txtvenname" runat="server" Style="word-break: break-all;" Width="100px" placeholder="Vendor Name"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="MD_NAME" HeaderText="CAPACITY" SortExpression="MD_NAME">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("MD_NAME") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:TextBox ID="txtcapacity" runat="server" Style="word-break: break-all;" Width="100px" placeholder="Capacity"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WOUND_TYPE" HeaderText="WOUNTD TYPE" SortExpression="WOUND_TYPE">

                                        <ItemTemplate>
                                            <asp:Label ID="lblwoundtype" runat="server" Text='<%# Bind("WOUND_TYPE") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="FROM" HeaderText="EFFECT FROM" SortExpression="FROM">

                                        <ItemTemplate>
                                            <asp:Label ID="lblFrom" runat="server" Text='<%# Bind("FROM") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>


                                    </asp:TemplateField>

                                     
                                    <asp:TemplateField AccessibleHeaderText="TO" HeaderText="EFFECT TO" SortExpression="TO">

                                        <ItemTemplate>
                                            <asp:Label ID="lblTo" runat="server" Text='<%# Bind("TO") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imgBtnEdit" title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                    Width="12px" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--  <asp:TemplateField HeaderText="Status"> 
                                     <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("TM_STATUS") %>' ></asp:Label>
                                         <center>
                                            <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                                           tooltip="Click to Activate Make" OnClientClick="return confirm ('Are you sure, you want to Activate Make');" width="10px" />        
                                            <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                                           tooltip="Click to DeActivate Make"  OnClientClick="return confirm ('Are you sure, you want to DeActivate Make');" />        
                                        </center>
                                    </ItemTemplate>
                               </asp:TemplateField>--%>
                                </Columns>

                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                            </asp:GridView>

                        </div>

                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>


            <!-- END PAGE CONTENT-->
        </div>
    </div>

</asp:Content>
