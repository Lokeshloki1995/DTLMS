<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="FailurePendingDetails.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.FailurePendingDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>--%>
    <script type="text/javascript"  src="../assets/jquery-validation/lib/Jquery.min.js"></script>
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
                    <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="cmbExport_Click" /><br />
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
                                <i class="icon-reorder"></i>Failure Pending Details</h4>
                            <%--<span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>--%>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                            </div>
                            <!-- END FORM-->
                            <%--<div class="space20"></div>--%>
                            <div id="Gridview1" style="width: 1100px; height: 500px; overflow: auto">
                                <asp:GridView ID="grdFailurePending" HeaderStyle-CssClass="Warning" AutoGenerateColumns="false"
                                    PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnPageIndexChanging="grdFailurePending_PageIndexChanging" OnRowCreated="grdFailurePending_RowCreated"
                                    OnRowCommand="grdFailurePending_RowCommand" OnSorting="grdFailurePending_Sorting" AllowSorting="true">
                                    <%--<HeaderStyle HorizontalAlign="center" CssClass="FixedHeader" />--%>
                                     <HeaderStyle CssClass="both" />
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Transformer Centre Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="100px"
                                                        MaxLength="6"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="Transformer Centre Name" Visible="true" SortExpression="DT_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                    Width="250px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px"
                                                        MaxLength="6"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="OMSECTION" HeaderText="Section Name" Visible="true" SortExpression="OMSECTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmSectionName" runat="server" Text='<%# Bind("OMSECTION") %>' Style="word-break: break-all;"
                                                    Width="110px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="OM_CODE" HeaderText="OM Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmCode" runat="server" Text='<%# Bind("OM_CODE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure No" SortExpression="DF_ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailureNo" runat="server" Text='<%# Bind("DF_ID") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DF_DATE" HeaderText="Failure Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailureDate" runat="server" Text='<%# Bind("DF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="EST_NO" HeaderText="Estimation No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimationNo" runat="server" Text='<%# Bind("EST_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="EST_CRON" HeaderText="Estimation Date" SortExpression="EST_CRON">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimationDate" runat="server" Text='<%# Bind("EST_CRON") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="FL_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailStatus" runat="server" Text='<%# Bind("FL_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order NO" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOStatus" runat="server" Text='<%# Bind("WO_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Indent No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentNo" runat="server" Text='<%# Bind("TI_INDENT_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_DATE" HeaderText="Indent Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentDATE" runat="server" Text='<%# Bind("TI_INDENT_DATE") %>'
                                                    Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="INDT_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentStatus" runat="server" Text='<%# Bind("INDT_STATUS") %>'
                                                    Style="word-break: break-all;" Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="IN_INV_NO" HeaderText="Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("IN_INV_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="IN_DATE" HeaderText="Commission">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDATE" runat="server" Text='<%# Bind("IN_DATE") %>' Style="word-break: break-all;"
                                                    Width="90px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="INV_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvStatus" runat="server" Text='<%# Bind("INV_STATUS") %>' Style="word-break: break-all;"
                                                    Width="220px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblriNo" runat="server" Text='<%# Bind("TR_RI_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TR_RI_DATE" HeaderText="RI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRiDATE" runat="server" Text='<%# Bind("TR_RI_DATE") %>' Style="word-break: break-all;"
                                                    Width="90px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="RI_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRIStatus" runat="server" Text='<%# Bind("RI_STATUS") %>' Style="word-break: break-all;"
                                                    Width="220px" ForeColor="#77808a"></asp:Label>
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
