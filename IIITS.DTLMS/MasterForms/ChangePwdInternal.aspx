<%@ Page Title="" Language="C#" AutoEventWireup="true"  CodeBehind="ChangePwdInternal.aspx.cs" Inherits="IIITS.DTLMS.ChangePwdInternal" %>

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
    <link href="/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="/Styles/calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/functions.js"></script>

    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtOldPwd.ClientID %>').value.trim() == "") {
             alert('Enter  Old Password')
             document.getElementById('<%= txtOldPwd.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtNewPwd.ClientID %>').value.trim() == "") {
             alert('Enter New Password')
             document.getElementById('<%= txtNewPwd.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtNewPwd.ClientID %>').value.trim() != "") {
             var pass = document.getElementById('<%= txtNewPwd.ClientID %>').value
             ///^(?=.{8,})(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[#!*()$%^&+-={}@@]).*$/;
             
             //if (!pass.match(/^(?=.{8,12})(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[#!*()$%^&+-={}@@]).*$/)) {
             if (!pass.match(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,12}$/)) {
                 alert("Password Length Should 8 Character and  contains at least 1 Capital Letter or  1 Small Letter,1 Digit, 1 Special Character")
                 document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                 return false;
             }

         }
         if (document.getElementById('<%= txtConfirmPwd.ClientID %>').value.trim() == "") {
             alert('Enter Confirm Password')
             document.getElementById('<%= txtConfirmPwd.ClientID %>').focus()
             return false
         }

     }
     function ResetForm() {

         document.getElementById('<%= txtOldPwd.ClientID %>').value = "";
         document.getElementById('<%= txtNewPwd.ClientID %>').value = "";
         document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";

         return false
     }

     function AvoidSpace(event) {
         var k = event ? event.which : window.event.keyCode;
         if (k == 32) return false;
     }

    </script>

    </head>
<body>
     <form id="form1" runat="server">

    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Change Password
                    </h3>
                    <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-info-circle" style="font-size: 36px"></i></a>
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
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Change Password</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group">

                                                <label class="control-label">Old Password <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdnpkid" runat="server" />
                                                        <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password" CssClass="input-text"
                                                            MaxLength="12" TabIndex="1" AutoCompleteType="Disabled"></asp:TextBox>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtOldPwd" ErrorMessage="Enter old password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>                                                        

                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">New Password <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"
                                                            CssClass="input-text" MaxLength="12" TabIndex="2" onkeypress="return AvoidSpace(event)"></asp:TextBox>


                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="txtNewPwd" ErrorMessage="Enter new password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                            </div>






                                            <div class="control-group">
                                                <label class="control-label">Confirm Password <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password"
                                                            CssClass="input-text" MaxLength="12" TabIndex="3" onkeypress="return AvoidSpace(event)"></asp:TextBox>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="txtConfirmPwd" ErrorMessage="Enter confirm password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="control-group">
                                                <label style="font-size: small">
                                                    <span class="Mandotary">*Password should be greater than or equal to 8 digit (It should Contain at least 1 Capital Letter,1 Digit, 1 Special Character)</span></label>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                                <div class="space20"></div>

                                <div class="form-horizontal" align="center">

                                    <div class="span3"></div>
                                    <div class="span1">
                                        <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="btn btn-primary"
                                            ValidationGroup="reg" Height="30px" Width="80px" OnClientClick="javascript:return ValidateMyForm();"
                                            OnClick="btnsubmit_Click" TabIndex="4" />

                                    </div>
                                    <%-- <div class="span1"></div>--%>
                                    <div class="span1">
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" OnClientClick="javascript:return ResetForm();"
                                            CssClass="btn btn-primary" TabIndex="5" /><br />
                                    </div>
                                    <div class="span1">
                                        <asp:Button ID="cmdclose" runat="server" Text="Close" 
                                            CssClass="btn btn-primary" TabIndex="6" OnClientClick="location.href = '../Login.aspx'; return false;"/><br />
                                    </div>
                                    <div class="span7"></div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                            </div>
                        </div>

                        <div class="space20"></div>
                        <!-- END FORM-->


                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Change The Old Password 
                    </p>

                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Enter Old Password in first Textbox and New Password in Second and Third Textbox and Click Submit To change Password
                    </p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->


 </form>
</body>
</html>