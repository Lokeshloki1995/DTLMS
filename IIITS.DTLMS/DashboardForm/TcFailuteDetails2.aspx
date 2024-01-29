<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcFailuteDetails2.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.TcFailuteDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../assets/jquery-validation/lib/Jquery.min.js"></script>
   <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>--%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script src="../js/gridviewScroll.min.js" type="text/javascript"></script>
    <style type="text/css">
        .Normal
        {
            background-color: Lime;
            vertical-align: middle;
            text-align: center;
            position: absolute;
        }
        .Warning
        {
            font-weight: bold;
            text-align: center;
        }
        
        .headerstyle
        {
            position: absolute;
            font-weight: bold;
            text-align: center;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
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
    <script type="text/javascript">

        $(document).ready(function () {
            gridviewScroll();
        });

        function gridviewScroll() {
            debugger;
            gridView1 = $('#Gridview1').gridviewScroll({
                width: 600,
                height: 300,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 1,
                arrowsize: 30,
                varrowtopimg: "Images/arrowvt.png",
                varrowbottomimg: "Images/arrowvb.png",
                harrowleftimg: "Images/arrowhl.png",
                harrowrightimg: "Images/arrowhr.png",
                headerrowcount: 2,
                railsize: 16,
                barsize: 8
            });
        }
	</script>
</asp:Content>

