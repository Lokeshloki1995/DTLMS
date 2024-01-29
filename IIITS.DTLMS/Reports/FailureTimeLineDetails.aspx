<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FailureTimeLineDetails.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.FailureTimeLineDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
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
   <link href="/css/style-default.css" rel="stylesheet" id="Link1" />
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
                        <asp:Label ID="Label1" runat="server" Text="FailureTimeLine Details" ForeColor="White"></asp:Label>
                    </h3>
                   
                </div>
            </div>
             
            <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                       <%-- <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i><asp:Label ID="Label2" runat="server" Text="FailureTimeLine Details"></asp:Label></h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>--%>
                        <div class="widget-body">
                            <div style="float: right">
                            </div>
                            <!-- END FORM-->
                           
                            <div class="space20">
                            </div>
                           
                           <asp:Button ID="cmdExport"   runat="server" Text="Export"  class="space20"
                                             CssClass="btn btn-primary" onclick="cmdExport_Click"  />
                            <div style=" overflow:auto;">
                                
                                 <asp:GridView ID="grdFailuretc" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" onpageindexchanging="grdFailuretc_PageIndexChanging"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server">
                                    
                                    <HeaderStyle HorizontalAlign="center" CssClass="Warning" />
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Dtc Code" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("DT_CODE") %>' Style="word-break: break-all;"
                                                    Width="100px" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                    Width="100px" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="Tc Serial Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="FAILUREDATE" HeaderText="Failure Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("FAILUREDATE") %>' Style="word-break: break-all;"
                                                    Width="100px" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="MD_NAME" HeaderText="ALTERNATIVE POW SUPP" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAltSupp" runat="server" Text='<%# Bind("MD_NAME") %>' Style="word-break: break-all;" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                          <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                    Width="100px" ForeColor="Black"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                      
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