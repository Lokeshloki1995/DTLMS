<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="BudgetView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.BudgetView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
    </script>

    <style type="text/css">
        .table {
            width: 100%;
            /*table-layout: fixed;*/
            margin-bottom: 20px;
        }

        span#ContentPlaceHolder1_grdBudget_lblAccCode_0 {
            width: auto!important;
        }

        input#ContentPlaceHolder1_grdBudget_txtBMNO {
            width: 89%!important;
        }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <div class="container-fluid">

        <div class="row-fluid">
            <div class="span8">
                <h3 class="page-title">Budget Master View</h3>
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

        <div id="Div1" class="row-fluid" runat="server">
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Budget Details</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>

                    </span>
                </div>
                <div class="widget-body">
                    <div style="float: right">
                        <div class="span2">
                            <asp:Button ID="cmdNew" runat="server" Text="New" CssClass="btn btn-success"
                                OnClick="cmdNew_Click" />
                        </div>
                    </div>
                    <div class="widget-body form">
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal">

                            <div class="row-fluid">

                                <div class="control-group">
                                    <label class="control-label">Financial Year<span class="Mandotary"> *</span></label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbFinYear" runat="server">
                                            </asp:DropDownList>
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                        </div>
                                    </div>
                                </div>

                                <asp:GridView ID="grdBudget" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" OnRowDataBound="grdBudget_RowDataBound"
                                    AllowPaging="true" OnPageIndexChanging="grdBudget_PageIndexChanging" OnRowCommand="grdBudget_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BM_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetId" runat="server" Text='<%# Bind("BM_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BM_NO" HeaderText="OM No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetNO" runat="server" Text='<%# Bind("BM_NO") %>' Style="word-break: break-all"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtBMNO" runat="server" CssClass="input_textSearch" Width="150px"
                                                        placeholder="Enter Budget No" ToolTip="Enter Budget No to Search"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BM_ACC_CODE" HeaderText="Account Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccCode" runat="server" Text='<%# Bind("BM_ACC_CODE") %>'
                                                    Width="150px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BM_DIV_CODE" HeaderText="Division Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDIVCODE" runat="server" Text='<%# Bind("DIV_NAME") %>'
                                                    Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BM_FIN_YEAR" HeaderText="Financial Year">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfinyear" runat="server" Text='<%# Bind("BM_FIN_YEAR") %>' Width="200px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="BM_AMOUNT" HeaderText="Budget Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbmamnt" runat="server" Text='<%# Bind("BM_AMOUNT") %>' Width="200px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="BM_OB_AMNT" HeaderText="Budget OB Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbObmamnt" runat="server" Text='<%# Bind("BM_OB_AMNT") %>' Width="200px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                        CommandName="create" Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("FY_STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>
                                <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

