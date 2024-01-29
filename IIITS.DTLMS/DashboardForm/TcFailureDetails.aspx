<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TcFailureDetails.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.TcFailureDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                    <h3 class="page-title">
                        Failure Pending Details
                    </h3>
                    <ul class="breadcrumb" style="display: none;">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="Text1" type="text">
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>
                                </button>
                            </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
            </div>
            <div style="float: right; margin-top: -20px">
                <div class="span2">
                   <%-- <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="cmbExport_Click" /><br />--%>
                </div>
                <asp:HiddenField ID="hdfOffCode" runat="server" />
            </div>
            <div class="space10">
            </div>
            <div class="row-fluid" style="width: 1340px;">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>Failure DTR Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                            </div>
                            <!-- END FORM-->
                            <%--                                            <div class="space20"></div>--%>
                            <div id="Gridview1" style="width: 1100px; height: 500px; overflow: auto">
                                <asp:GridView ID="grdFailureDtrDetails" HeaderStyle-CssClass="Warning" AutoGenerateColumns="false"
                                    PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server">
                                    <%--  <HeaderStyle HorizontalAlign="center" CssClass="FixedHeader" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DTR_CODE" HeaderText="DTR Code" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" placeholder="Enter DTR Code " Width="100px"
                                                        MaxLength="6"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="TC SLNO" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="250px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtTcSlNo" runat="server" placeholder="Enter Tc Sl No " Width="150px"
                                                        MaxLength="6"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TCCAPACITY" HeaderText="TC CAPACITY" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="110px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="OFFICE NAME" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffName" runat="server" Text='<%# Bind("OFFNAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <%--<asp:TemplateField AccessibleHeaderText="CR_STATUS" HeaderText="Status" >
            <ItemTemplate>
                <asp:Label ID="lblCRStatus" runat="server" Text='<%# Bind("CR_STATUS") %>' style="word-break: break-all;"  width="200px" ForeColor="#ab8465" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
