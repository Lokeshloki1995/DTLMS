<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Policy.aspx.cs" Inherits="IIITS.DTLMS.Policy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>Transformer Centre Life Cycle Management Software</title>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="Mosaddek" name="author" />
    <link href="assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
    <link href="assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />
    <link href="css/style-default.css" rel="stylesheet" id="style_color" />
    <link href="assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
    <link href="assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="Styles/calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/functions.js"></script>
    <%--<script src='<%= ResolveUrl("~/Scripts/functions.js") %>' type="text/javascript"></script>--%>
</head>
<body class="fixed-top">
    <form id="form1" runat="server">
    <div>
        <!-- BEGIN HEADER -->
        <div id="header" class="navbar navbar-inverse navbar-fixed-top">
            <!-- BEGIN TOP NAVIGATION BAR -->
            <div class="navbar-inner">
                <div class="container-fluid">
                    <!--BEGIN SIDEBAR TOGGLE-->
                    <div class="sidebar-toggle-box hidden-phone">
                        <div class="icon-reorder tooltips" data-placement="right" data-original-title="Toggle Navigation">
                        </div>
                    </div>
                    <!--END SIDEBAR TOGGLE-->
                    <!-- BEGIN LOGO -->
                    <a class="brand" style="width: 500px; color: White">Transformer Centre Life Cycle Management Software
                        </a><!-- END LOGO --><!-- BEGIN RESPONSIVE MENU TOGGLER --><a class="btn btn-navbar collapsed"
                            id="main_menu_trigger" data-toggle="collapse" data-target=".nav-collapse"><span class="icon-bar"></span><span
                                class="icon-bar"></span><span class="icon-bar"></span><span class="arrow"></span></a><!-- END RESPONSIVE MENU TOGGLER --><!-- END  NOTIFICATION --><div
                                    class="top-nav ">
                                    <ul class="nav pull-right top-menu">
                                        <!-- BEGIN SUPPORT -->
                                        <!-- END SUPPORT -->
                                        <!-- BEGIN USER LOGIN DROPDOWN -->
                                        <li style="margin-right: 80px !important; padding-top: 11px" runat="server" id="liOffDesg">
                                            <%-- <span style="font-weight:bold"> Login Page </span>
                                               <span> <asp:Label ID="lblOfficeName" style="font-size:12px;color:White"  runat="server"></asp:Label></span>
                                            --%>
                                            <asp:LinkButton ID="lknLoginPage" runat="server" Style="font-size: 16px; color: White;
                                                font-weight: bold" OnClick="lknLoginPage_Click">Home</asp:LinkButton>
                                            <br />
                                        </li>
                                    </ul>
                                    <!-- END TOP NAVIGATION MENU -->
                                </div>
                </div>
            </div>
            <!-- END TOP NAVIGATION BAR -->
        </div>
        <!-- END HEADER -->
    </div>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div>
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="space20">
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>

                                         <div class="span5">
                                            <div class="control-group">
                                                <label style="font-size: 24px; font-style: normal; font-weight: bold;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    Privacy Policy :&nbsp;&nbsp; 

                                                </label>
                                            </div>
                                        </div>

<%--                                        <div class=" pull-right">
                                                            <img src="../img/animated-hand-image-0010.gif" />
                                                                        <asp:LinkButton ID="lnkOSTicket" runat="server" Font-Underline="false" 
                                                                    style=" color:Blue;font-style: normal; font-size:20px; font-weight: bold;" 
                                                                    onclick="lnkOSTicket_Click">Click Here to Rise Ticket</asp:LinkButton>
                                                            </div>--%>

                                         <div class="span5">
                                         </div>
                                        <br />
                                        <br />
                                        <br />
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <asp:Label ID="lblNote" runat="server" Font-Bold="true" Text="" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        <div align="center">
                                            
                                        </div>
                                    </div>
                                  
                                    <div class="form-horizontal" align="center">
                                        <div class="span3">
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span1">
                                            <br />
                                        </div>
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->
        </div>
    </div>
    </form>
</body>
</html>
