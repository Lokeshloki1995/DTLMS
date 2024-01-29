<%@ Page Language="C#" AutoEventWireup="true"    CodeBehind="ViewDetailsgrid.aspx.cs" Inherits="IIITS.DTLMS.ViewDetailsgrid" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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

        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <h3 class="page-title">View  Details 
                    </h3>
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
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>
                                <asp:Label ID="lblDetailsview" runat="server" Text="View Details Status"></asp:Label></h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span2">
                                    <asp:Button ID="cmdBack" runat="server" Text="Back"
                                        CssClass="btn btn-danger" OnClientClick="javascript:window.location.href='DashboardForm/MdDashboard.aspx'; return false;" /><br />
                                </div>
                                <asp:HiddenField ID="hdfFeederCode" runat="server" />
                                <asp:HiddenField ID="hdfFromDate" runat="server" />
                                <asp:HiddenField ID="hdfToDate" runat="server" />
                                <asp:HiddenField ID="hdfRefId" runat="server" />
                                <asp:HiddenField ID="hdfGridId" runat="server" />
                                <asp:HiddenField ID="hdfOfficeCode" runat="server" />
                            </div>

                            <div class="space20"></div>

                            <asp:GridView ID="grdlfailureDetailsview"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdlfailureDetailsview_RowCommand"
                                OnPageIndexChanging="grdlfailureDetailsview_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MONTHS" HeaderText="MONTHS" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmonths" runat="server" Text='<%# Bind("MONTHS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                          
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DT COUNT" HeaderText="TOTAL DT COUNT" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldtcount" runat="server" Text='<%# Bind("DT_COUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PRESENT YEAR" HeaderText="PRESENT YEAR" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresentyear" runat="server" Text='<%# Bind("PRESENTYEAR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PRESENT MONTH" HeaderText="PRESENT MONTH" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldpresentmonth" runat="server" Text='<%# Bind("PRESENTMONTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PRESENT COUNT" HeaderText="PRESENT COUNT" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresentcount" runat="server" Text='<%# Bind("PRESENTCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PREVIOUS YEAR" HeaderText="PREVIOUS YEAR" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviousyear" runat="server" Text='<%# Bind("PREVIOUSYEAR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PREVIOUS MONTH" HeaderText="PREVIOUS MONTH" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviousmonth" runat="server" Text='<%# Bind("PREVIOUSMONTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PREVIOUS COUNT" HeaderText="PREVIOUS COUNT" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldpreviouscount" runat="server" Text='<%# Bind("PREVIOUSCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   

                                </Columns>

                            </asp:GridView>




                              <asp:GridView ID="grdlTCDetailsview"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdlTCDetailsview_RowCommand"
                                OnPageIndexChanging="grdlTCDetailsview_PageIndexChanging">
                                <Columns>
                                 

                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="AGP" HeaderText="AGP" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatusagp" runat="server" Text='<%# Bind("AGP") %>'></asp:Label>
                                        </ItemTemplate>
                                            <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WGP" HeaderText="WGP" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatuswgp" runat="server" Text='<%# Bind("WGP") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="UNKNOWN" HeaderText="UNKNOWN" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcountunk" runat="server" Text='<%# Bind("UNKNOWN") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                   


                                </Columns>

                            </asp:GridView>


                             <asp:GridView ID="grdlTCStatusview"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdlTCStatusview_RowCommand"
                                OnPageIndexChanging="grdlTCStatusview_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="BRAND_NEW" HeaderText="BRAND_NEW" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblnew" runat="server" Text='<%# Bind("BRAND_NEW") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                 


                                    <asp:TemplateField AccessibleHeaderText="REPAIR_GOOD" HeaderText="REPAIR_GOOD" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrepairecnt" runat="server" Text='<%# Bind("REPAIR_GOOD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="FAULTY" HeaderText="FAULTY" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfaulty" runat="server" Text='<%# Bind("FAULTY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>

                            </asp:GridView>


                               <asp:GridView ID="grdlalternativeview"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdlalternativeview_RowCommand"
                                OnPageIndexChanging="grdlalternativeview_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ARRANGED" HeaderText="ARRANGED" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblarrange" runat="server" Text='<%# Bind("ARRANGE") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                 


                                    <asp:TemplateField AccessibleHeaderText="NOT_ARRANGED" HeaderText="NOT_ARRANGED" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblnotarrange" runat="server" Text='<%# Bind("NOT_ARRANGE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     


                                </Columns>

                            </asp:GridView>


                            <asp:GridView ID="grdRepairerPerformanceview"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdRepairerPerformanceview_RowCommand"
                                OnPageIndexChanging="grdRepairerPerformanceview_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFFICE_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFFICE_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PENDING" HeaderText="PENDING" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpending" runat="server" Text='<%# Bind("PENDING") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="COMPLETED" HeaderText="COMPLETED" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcompleted" runat="server" Text='<%# Bind("COMPLETED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>



                              <asp:GridView ID="grdpendingreplacementview"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdpendingreplacementview_RowCommand"
                                OnPageIndexChanging="grdpendingreplacementview_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFFICE_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFFICE_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="Less_than_30 days" HeaderText="Less_than_30 days" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1days" runat="server" Text='<%# Bind("[Less_than_30 days]") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="More_than_30 days" HeaderText="More_than_30 days" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1to3days" runat="server" Text='<%# Bind("[More_than_30 days]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%-- <asp:TemplateField AccessibleHeaderText=">3 Days" HeaderText=">3 Days" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmorethan3Days" runat="server" Text='<%# Bind("[>3 Days]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                </Columns>

                            </asp:GridView>



                             <asp:GridView ID="Ridetails"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdRidetails_RowCommand"
                                OnPageIndexChanging="grdRidetails_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFFICE_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFFICE_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="1-Day" HeaderText="1 Day" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1days" runat="server" Text='<%# Bind("[1-Day]") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="1-3 Days" HeaderText="1-3 Days" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1to3days" runat="server" Text='<%# Bind("[1-3 Days]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText=">3 Days" HeaderText=">3 Days" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmorethan3Days" runat="server" Text='<%# Bind("[>3 Days]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>

                             <asp:GridView ID="frequentlyfail"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdfrequentlyfail_RowCommand"
                                OnPageIndexChanging="grdfrequentlyfail_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="TOTAL" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>


                             <asp:GridView ID="frequentlyfaildtc"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" >
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                             <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_NAME") %>'></asp:Label>
                                            <%--<asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>--%>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DF_DTC_CODE" HeaderText="DF_DTC_CODE" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldtccode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                             <asp:GridView ID="frequentlyfaildtr"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server"  OnRowCommand="grdfrequentlyfaildtr_RowCommand"
                                OnPageIndexChanging="grdfrequentlyfaildtr_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                          
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12" ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="< 63" HeaderText="< 63" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl63cap" runat="server" Text='<%# Bind("[< 63]") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="63-100" HeaderText="63-100" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("[63-100]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField AccessibleHeaderText="100-250" HeaderText="100-250" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("[100-250]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="> 250" HeaderText="> 250" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl250cap" runat="server" Text='<%# Bind("[> 250]") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     

                                    
                                     
                                </Columns>

                            </asp:GridView>

                              <asp:GridView ID="Gridexpenditure"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" OnRowCommand="grdlexpenditureview_RowCommand"
                                OnPageIndexChanging="grdlexpenditureview_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="MONTHS" HeaderText="MONTHS" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmonths" runat="server" Text='<%# Bind("MONTHS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;"
                                                Width="200px" CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="off code" HeaderText="off code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                          
                                    </asp:TemplateField>

                                 <%--   <asp:TemplateField AccessibleHeaderText="DT COUNT" HeaderText="TOTAL DT COUNT" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldtcount" runat="server" Text='<%# Bind("DT_COUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>--%>

                                     <asp:TemplateField AccessibleHeaderText="PRESENT YEAR" HeaderText="PRESENT YEAR" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresentyear" runat="server" Text='<%# Bind("PRESENTYEAR") %>'></asp:Label>
                                        </ItemTemplate>

                                         <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PRESENT MONTH" HeaderText="PRESENT MONTH" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldpresentmonth" runat="server" Text='<%# Bind("PRESENTMONTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PRESENT COUNT" HeaderText="PRESENT COUNT" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresentcount" runat="server" Text='<%# Bind("PRESENTCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PREVIOUS YEAR" HeaderText="PREVIOUS YEAR" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviousyear" runat="server" Text='<%# Bind("PREVIOUSYEAR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PREVIOUS MONTH" HeaderText="PREVIOUS MONTH" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviousmonth" runat="server" Text='<%# Bind("PREVIOUSMONTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PREVIOUS COUNT" HeaderText="PREVIOUS COUNT" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldpreviouscount" runat="server" Text='<%# Bind("PREVIOUSCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   

                                </Columns>

                            </asp:GridView>


                            <asp:GridView ID="Gridpodetails"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                AllowPaging="true" ShowFooter="false"
                                runat="server" >
                                <Columns>
                                      <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PREVIOUSYEAR" HeaderText="PREVIOUSYEAR" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviousyear" runat="server" Text='<%# Bind("PREVIOUSYEAR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="PREVIOUS_PO_COUNT" HeaderText="PREVIOUS PO COUNT">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviouspocount" runat="server" Text='<%# Bind("PREVIOUS_PO_COUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                  
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PREVIOUS_TOTAL_QUANTITY" HeaderText="PREVIOUS TOTAL QUANTITY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblprevioustotalqnty" runat="server" Text='<%# Bind("PREVIOUS_TOTAL_QUANTITY") %>'></asp:Label>
                                        </ItemTemplate>
                                          
                                    </asp:TemplateField>


                                     <asp:TemplateField AccessibleHeaderText="PREVIOUS_PO_AMOUNT" HeaderText="PREVIOUS PO AMOUNT" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblpreviospoamnt" runat="server" Text='<%# Bind("PREVIOUS_PO_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PRESENTYEAR" HeaderText="PRESENTYEAR" >
                                        <ItemTemplate>
                                            <asp:Label ID="lbldpresentyear" runat="server" Text='<%# Bind("PRESENTYEAR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PRESENT_PO_COUNT" HeaderText="PRESENT PO COUNT" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresentpocount" runat="server" Text='<%# Bind("PRESENT_PO_COUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="PRESENT_TOTAL_QUANTITY" HeaderText="PRESENT TOTAL QUANTITY" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresenttotalqnty" runat="server" Text='<%# Bind("PRESENT_TOTAL_QUANTITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PRESENT_PO_AMOUNT" HeaderText="PRESENT PO AMOUNT" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblpresentpoamnt" runat="server" Text='<%# Bind("PRESENT_PO_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>

                        </div>
                    </div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

        </div>
    </div>


         </form>

    </body>
</html>