<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="IIITS.DTLMS.ContactUs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>Transformer Centre Life Cycle Management Software</title>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="Mosaddek" name="author" />
    <%-- <link href="assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
  <%--  <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>
<%--    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>--%>
   
     <script type="text/javascript" src="js/bootstrap.min.js"></script>
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
    <link href="https://fontawesome.com/v4/assets/font-awesome/css/font-awesome.css" rel="Stylesheet" />
    <%--<script src='<%= ResolveUrl("~/Scripts/functions.js") %>' type="text/javascript"></script>--%>
    <style>
        .fixed-top{
            background-color:#fff!important
        }
        table#grdFirstContactDetails{
            width:100%!important;
        }
        table#grdFirstContactDetails td{
           text-align:center!important
        }
        table#grdFirstContactDetails th{
           text-align:center!important
        }

        table#grdSecondContactDetails{
            width:100%!important;
        }
        table#grdSecondContactDetails td{
           text-align:center!important
        }
        table#grdSecondContactDetails th{
           text-align:center!important
        }

        table#grdThirdGrid{
            width:100%!important;
        }
        table#grdThirdGrid td{
           text-align:center!important
        }
        table#grdThirdGrid th{
           text-align:center!important
        }
    </style>
</head>
<body class="fixed-top">
    <form id="form1" runat="server">
 <nav style="background-color:#438eb9!important" class="navbar navbar-inverse">
  <div class="container-fluid">
    <div class="navbar-header">
      <a class="navbar-brand" style="color:#fff!important" href="#">Transformer Life Cycle Management Software</a>
    </div>
    
 <a><asp:LinkButton ID="lknLoginPage" runat="server" Style="font-size: 16px; color: #fff!important;text-decoration:none!important"
                                        OnClick="lknLoginPage_Click"><i class="fa fa-home"></i>Home</asp:LinkButton></a>
  </div>
</nav>

  <div class="collapse navbar-collapse" id="navbarText">
    <ul class="navbar-nav mr-auto">
      <li class="nav-item active">
        <a class="nav-link" href="#">Transformer LifeCycle Management Software </a>
      </li>
     
    </ul>
    <span class="navbar-text">
     
    </span>
  </div>
</nav>
  

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




                                    <div class="container-fluid">
                                        <div class="row">
                                            <div class="col-md-9">
                                                <label style="font-size: 24px; font-style: normal; font-weight: bold;">
                                                    Contact for Support :

                                                </label>
                                            </div>
                                            <div class="col-md-3">

                                                <asp:LinkButton ID="lnkOSTicket" runat="server" Font-Underline="false"
                                                    Style="color: Blue; font-style: normal; font-size: 20px; font-weight: bold; float: right!important"
                                                    OnClick="lnkOSTicket_Click"><i class="fa fa-hand-o-right fa-1x"></i> Click Here to Raise Ticket</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="accordion" id="accordionExample">
                                        <div class="accordion-item">
                                            <h2 class="accordion-header" id="headingOne">
                                                <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                                    <b>First Level</b>
                                                </button>
                                            </h2>
                                            <div id="collapseOne" class="accordion-collapse collapse show" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                                                <div class="accordion-body">
                                                    <asp:GridView ID="grdFirstContactDetails" AutoGenerateColumns="false" PageSize="15"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        AllowPaging="true" runat="server" TabIndex="16" Width="800px">
                                                        <Columns>
                                                            <asp:TemplateField AccessibleHeaderText="circle" HeaderText="Circle" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCircle" runat="server" Text='<%# Bind("circle") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="Div" HeaderText="Division" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDiv" runat="server" Text='<%# Bind("Div") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_NAME" HeaderText="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("CC_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_MOBILENO" HeaderText="Phone Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFirstPhNo" runat="server" Text='<%# Bind("CC_MOBILENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_EMAI" HeaderText="Email Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFirstEmailId" runat="server" Text='<%# Bind("CC_EMAIL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="accordion-item">
                                            <h2 class="accordion-header" id="headingTwo">
                                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                                    <b>Second Level
                                                    </b>
                                                </button>
                                            </h2>
                                            <div id="collapseTwo" class="accordion-collapse collapse" aria-labelledby="headingTwo" data-bs-parent="#accordionExample">
                                                <div class="accordion-body">
                                                    <asp:GridView ID="grdSecondContactDetails" AutoGenerateColumns="false" PageSize="15"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        AllowPaging="true" runat="server" TabIndex="16" Width="800px">

                                                        <Columns>
                                                            <asp:TemplateField AccessibleHeaderText="CC_NAME" HeaderText="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSecondName" runat="server" Text='<%# Bind("CC_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_MOBILENO" HeaderText="Phone Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSecondPhNo" runat="server" Text='<%# Bind("CC_MOBILENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_EMAI" HeaderText="Email Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSecEmailId" runat="server" Text='<%# Bind("CC_EMAIL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="accordion-item">
                                            <h2 class="accordion-header" id="headingThree">
                                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                                    <b>Third Level
                                                    </b>
                                                </button>
                                            </h2>
                                            <div id="collapseThree" class="accordion-collapse collapse" aria-labelledby="headingThree" data-bs-parent="#accordionExample">
                                                <div class="accordion-body">
                                                    <asp:GridView ID="grdThirdGrid" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="true"
                                                        EmptyDataText="No records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                        AllowPaging="true" runat="server" TabIndex="16" Width="800px">
                                                        <Columns>
                                                            <asp:TemplateField AccessibleHeaderText="CC_NAME" HeaderText="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblThirdName" runat="server" Text='<%# Bind("CC_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_MOBILENO" HeaderText="Phone Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblThirdPhNo" runat="server" Text='<%# Bind("CC_MOBILENO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="CC_EMAI" HeaderText="Email Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblThirdEmailId" runat="server" Text='<%# Bind("CC_EMAIL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                    <!-- END FORM-->
                                </div>
                            </div>
                            <!-- END SAMPLE FORM PORTLET-->
                               <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <!-- END PAGE CONTENT-->
                </div>
            </div>
    </form>
</body>
</html>
