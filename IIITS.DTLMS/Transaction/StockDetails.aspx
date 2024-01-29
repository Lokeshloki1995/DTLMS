<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="StockDetails.aspx.cs" Inherits="IIITS.DTLMS.Transaction.StockDetails" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta charset="utf-8" />
   <meta content="width=device-width, initial-scale=1.0" name="viewport" />
   <meta content="" name="description" />
   <meta content="Mosaddek" name="author" />
   <link href="/assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
   <link href="/assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
   <link href="/assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
   <link href="/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
   <link href="/css/style.css" rel="stylesheet" />
   <link href="/css/style-responsive.css" rel="stylesheet" />
   <link href="/css/style-default.css" rel="stylesheet" id="style_color" />
   <link href="/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
   <link href="/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="/Styles/calendar.css" rel="stylesheet" type="text/css" />
   <script type="text/javascript" src="Scripts/functions.js"></script>
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
         .ascending th a {
        background:url(/img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(/img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
         background: url(/img/sort_both.png) no-repeat;
         display: block;
          padding: 0 4px 0 20px;
     }
    </style>
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
    <div>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Stock Details
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
                            <h4><i class="icon-reorder"></i>Stock Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <!-- END FORM-->
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Store</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" TabIndex="9"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                         <asp:TextBox ID="txtStoreId" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Make Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" TabIndex="9"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="9" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtCapacity" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Rating</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbrating" runat="server" TabIndex="9"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">

                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                                Width="116px" OnClick="cmdLoad_Click" />
                                        </div>


                                    </div>
                                    <asp:Label ID="lblTotalDTr" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>
                                </div>
                            </div>

                            <asp:GridView ID="grdTcMaster" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdTcMaster_PageIndexChanging" OnSorting="grdTcMaster_Sorting" >
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText=" Transformer ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" Visible ="false">
                                                <asp:TextBox ID="txtTCCode" runat="server" placeholder="Enter DTr Code"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SlNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" Visible ="false">
                                                <asp:TextBox ID="txtTCSlno" runat="server" placeholder="Enter DTr Slno"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="TC_MAKE_ID" HeaderText="Make">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcMake" runat="server" Text='<%# Bind("TC_MAKE_ID") %>' Style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" Visible ="false">
                                                <asp:TextBox ID="txtMake" runat="server" placeholder="Enter Make"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" Visible ="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_LIFE_SPAN" HeaderText="Life Span" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcLifeSpan" runat="server" Text='<%# Bind("TC_LIFE_SPAN") %>' Style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TC_RATING" HeaderText="Rating" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRating" runat="server" Text='<%# Bind("TC_RATING") %>' Style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Edit" Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                    Width="12px" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                    Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                    CausesValidation="false" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

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
    </div>
    </form>


</body>
</html>
