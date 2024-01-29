<%@ Page Language="C#"      CodeBehind="UnAllocateDetails.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.UnAllocateDetails3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="/assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
    <link href="/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/style-responsive.css" rel="stylesheet" />
    <link href="/css/style-default.css" rel="stylesheet" id="style_color" />
    <link href="/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
    <link href="/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="Styles/calendar.css" rel="stylesheet" type="text/css" />
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
    
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <%--<div class="span12">
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
                </div>--%>
            </div>
            <form id="form1" runat="server">
            
            <div style="float: right; margin-top: 2px;">
                
                <asp:HiddenField ID="hdfOffCode" runat="server" />
            </div>
            <div class="space10">
            </div>
            <div class="row-fluid" style="width: 1305px;">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>UnAllocated DTC Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="span2">
                    <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="cmbExport_Click" /><br />
                </div>
                            <div style="float: right">
                            </div>
                            <!-- END FORM-->
                            <%--                                            <div class="space20"></div>--%>
                            <div id="Gridview1" class="row-fluid" style="width: 1280px; height: 500px; overflow: auto">
                                <asp:GridView ID="grdUnAllocateDetails" AutoGenerateColumns="false"
                                    PageSize="9" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnPageIndexChanging="grdUnAllocateDetails_PageIndexChanging"
                                     OnRowCommand="grdUnAllocateDetails_RowCommand"   OnSorting="grdUnAllocateDetails_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 

                                     <%-- <HeaderStyle HorizontalAlign="center" CssClass="FixedHeader" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
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
                                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name" Visible="true" SortExpression="DT_NAME">
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
                                        <asp:TemplateField AccessibleHeaderText="OMSECTION" HeaderText="TC Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmSectionName" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                    Width="180px"></asp:Label>
                                            </ItemTemplate>
                                             
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="UnAllocateDate" HeaderText="UnAllocated Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnAllocateDate" runat="server" Text='<%# Bind("DM_ENTRY_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="UnAllocateOffcode" HeaderText="Location Code" Visible="true" SortExpression="DT_OM_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnAllocateOffcode" runat="server" Text='<%# Bind("DT_OM_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="UnAllocateSection" HeaderText="Section" Visible="true" SortExpression="SECTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnAllocatesection" runat="server" Text='<%# Bind("SECTION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                            <%--<HeaderStyle CssClass="Warning" Height="5px" HorizontalAlign="Center"></HeaderStyle>--%>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                     <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          </form>
        </div>
    </div>

</body>
</html>
