<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="IIITS.DTLMS.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" rel="stylesheet" />
    <style type="text/css">
        i.fa.fa-eye, .fa-eye-slash:before {
            font-size: 16px;
        }

        .input-group-append {
            float: right;
            margin-right: 76px;
            margin-top: 8px;
        }

        a.icon-view {
            position: absolute;
            margin-left: -26px;
            z-index: 999 !important;
        }

        .progress-bar {
            float: left;
            height: 100%;
            font-size: 12px;
            line-height: 20px;
            color: #fff;
            text-align: center;
            background-color: #5cb85c;
        }

        .progress {
            height: 7px;
            width: 220px;
            margin-left: -4px !important;
        }

        .form-horizontal .control-group {
            margin-bottom: 0px !important;
        }

        .btn-success {
            background: #449d44;
        }

        .btn-danger {
            background: #d9534f !important;
        }

        .form-horizontal .control-label {
            margin-left: -4px !important;
        }

        ul.list-unstyled {
            list-style: none;
        }

        ul, ol {
            padding: 0;
            margin: 0 0 10px 0px;
        }
    </style>

     <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=btnsubmit.ClientID %>').prop('disabled', true);
        }
        window.onbeforeunload = preventMultipleSubmissions;

    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            // target the link
            $(".toggle_hide_password").on('click', function (e) {
                e.preventDefault()

                // get input group of clicked link
                var input_group = $(this).closest('.input-group')

                // find the input, within the input group
                var input = input_group.find('.input-text')

                // find the icon, within the input group
                var icon = input_group.find('i')

                // toggle field type
                input.attr('type', input.attr("type") === "text" ? 'password' : 'text')

                // toggle icon class
                icon.toggleClass('fa-eye-slash fa-eye')
            })
        })
    </script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            $('#ContentPlaceHolder1_txtNewPwd').keyup(function () {

                var password = $('#ContentPlaceHolder1_txtNewPwd').val();

                if (checkStrength(password) == false) {

                    $(".progress-bar.progress-bar-success.progress-bar-danger").css("background-color", "transparent")
                    $("#result").hide();
                }
            });

            function checkStrength(password) {
                var strength = 0;
                debugger;
                if (password.match(/([a-zA-Z])/)) {
                    strength += 1;
                }

                //If password contains both lower and uppercase characters, increase strength value.
                if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) {

                    $('.low-upper-case').addClass('text-success');
                    $('.low-upper-case i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');

                } else {
                    $('.low-upper-case').removeClass('text-success');
                    $('.low-upper-case i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');

                }

                //If it has numbers and characters, increase strength value.
                if (password.match(/([0-9])/)) {
                    strength += 1;
                    $('.one-number').addClass('text-success');
                    $('.one-number i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');

                } else {
                    $('.one-number').removeClass('text-success');
                    $('.one-number i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }

                //If it has one special character, increase strength value.
                if (password.match(/([\[,!,%,&,@@,#,-,$,^,*,?,_,~,+,(,),`,{,},\-,',.,",<,>,/,=,:,\;,\,, ,|,\]])/)) {
                    strength += 1;
                    $('.one-special-char').addClass('text-success');
                    $('.one-special-char i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');
                }
                else {
                    $('.one-special-char').removeClass('text-success');
                    $('.one-special-char i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }

                if (password.length > 7) {
                    strength += 1;
                    $('.eight-character').addClass('text-success');
                    $('.eight-character i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');

                } else {
                    $('.eight-character').removeClass('text-success');
                    $('.eight-character i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }
                // If value is less than 2
                if (strength < 1) {
                    $('#result').removeClass()
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-danger').text('');
                    $('#password-strength').css('width', '0%');
                }
                else if (strength < 2) {
                    $('#result').addClass('good');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-danger').text('Very Weak');
                    $('#password-strength').css('width', '10%');
                }
                else if (strength < 4) {
                    $('#result').addClass('good');
                    $('#password-strength').removeClass('progress-bar-success');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-warning').text('Weak')
                    $('#password-strength').css('width', '60%');
                    return 'Weak'
                } else if (strength >= 4 && password.match(/([a-z])/) && password.match(/([A-Z])/)) {
                    $('#result').removeClass()
                    $('#result').addClass('strong');
                    $('#password-strength').removeClass('progress-bar-warning');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-success').text('Strong');
                    $('#password-strength').css('width', '100%');
                    return 'Strong'
                }
                else if (strength >= 4) {
                    $('#result').addClass('good');
                    $('#password-strength').removeClass('progress-bar-danger');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-warning').text('Weak')
                    $('#password-strength').css('width', '60%');
                    return 'Weak'
                }
            }
        });


        <%--function ValidateMyForm() {
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
                    alert("Password should be greater than or equal to 8 character (Must Contain at least 1 Capital Letter,1 Digit,1 Small Letter, 1 Special Character)")

                    document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                    return false;
                }

            }
            if (document.getElementById('<%= txtConfirmPwd.ClientID %>').value.trim() == "") {
                alert('Enter Confirm Password')
                document.getElementById('<%= txtConfirmPwd.ClientID %>').focus()
                return false
            }

        }--%>

        function ValidateMyForm() {
            if (document.getElementById('<%= txtOldPwd.ClientID %>').value == "") {
                alert('Please Enter  Current Password')
                document.getElementById('<%= txtOldPwd.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtNewPwd.ClientID %>').value == "") {
                alert('Please Enter New Password')
                document.getElementById('<%= txtNewPwd.ClientID %>').focus()
                document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                return false
            }
            if (document.getElementById('<%= txtNewPwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewPwd.ClientID %>').value

                if (!pass.match(/^(?=.*?[a-z])/)) {
                    alert("Password should contain At least one lower case letter")
                    document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                    document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                    return false;
                }
            }

            if (document.getElementById('<%= txtNewPwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewPwd.ClientID %>').value

                if (!pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Password should contain At least one Upper case letter")
                    document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                    document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                    return false;
                }
            }
            if (document.getElementById('<%= txtNewPwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewPwd.ClientID %>').value
                if (!pass.match(/^(?=.*?[0-9])/)) {
                    alert("Password should contain At least one number")
                    document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                    document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                    return false;
                }
            }
            if (document.getElementById('<%= txtNewPwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewPwd.ClientID %>').value
                   if (!pass.match(/^(?=.*?[!@#~`><;:'",. {}$%^?/=+&*()_-])/)) {
                       alert("Password should contain At least one Special Character")
                       document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                    document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                    return false;
                }
            }
            if (document.getElementById('<%= txtNewPwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewPwd.ClientID %>').value
                               if (!pass.match(/^(?=.*?).{8,12}$/)) {
                                   alert("Passwords Length Should be greater than 7 characters")
                                   document.getElementById('<%=txtNewPwd.ClientID %>').focus()
                    document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                    return false;
                }

            }

            if (document.getElementById('<%= txtOldPwd.ClientID %>').value == document.getElementById('<%= txtNewPwd.ClientID %>').value) {
                alert('New Password & Current password should not be same')
                document.getElementById('<%= txtNewPwd.ClientID %>').focus()
                document.getElementById('<%= txtNewPwd.ClientID %>').value = "";
                            document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";

                            $(".progress-bar").css("width", "0%");

                            $('.low-upper-case').removeClass('text-success');
                            $('.low-upper-case i').addClass('fa-file-text').removeClass('fa-check');

                            $('.one-number').removeClass('text-success');
                            $('.one-number i').addClass('fa-file-text').removeClass('fa-check');

                            $('.one-special-char').removeClass('text-success');
                            $('.one-special-char i').addClass('fa-file-text').removeClass('fa-check');

                            $('.eight-character').removeClass('text-success');
                            $('.eight-character i').addClass('fa-file-text').removeClass('fa-check');


                            $('#popover-password-top').removeClass('hide');
                            $("#result").hide();
                            return false
                        }
                        if (document.getElementById('<%= txtConfirmPwd.ClientID %>').value == "") {
                alert('Please Enter Confirm Password')
                document.getElementById('<%= txtConfirmPwd.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtConfirmPwd.ClientID %>').value != document.getElementById('<%= txtNewPwd.ClientID %>').value) {
                alert('New Password & Confirm Password do not match')
                document.getElementById('<%= txtConfirmPwd.ClientID %>').focus()
                document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
                return false
            }

        }

        function ResetForm() {

            document.getElementById('<%= txtOldPwd.ClientID %>').value = "";
            document.getElementById('<%= txtNewPwd.ClientID %>').value = "";
            document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
            

            $(".progress-bar").css("width", "0%");

            $('.low-upper-case').removeClass('text-success');
            $('.low-upper-case i').addClass('fa-file-text').removeClass('fa-check');

            $('.one-number').removeClass('text-success');
            $('.one-number i').addClass('fa-file-text').removeClass('fa-check');

            $('.one-special-char').removeClass('text-success');
            $('.one-special-char i').addClass('fa-file-text').removeClass('fa-check');

            $('.eight-character').removeClass('text-success');
            $('.eight-character i').addClass('fa-file-text').removeClass('fa-check');


            $('#popover-password-top').removeClass('hide');
            $("#result").hide();

            return false

            return false
            
        }

        //function AvoidSpace(event) {
        //    var k = event ? event.which : window.event.keyCode;
        //    if (k == 32) return false;
        //}

        function BlockCharSpace(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (code == 91 || code == 92 || code == 124 || code == 60 ||  code == 126 || code == 30 || code == 35
                || code == 36  || code == 62 || code == 34 || code == 39 || code == 32 || code == 95) {
                e.preventDefault();
            }
        }

    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                    <%--class="fa fa-info-circle"--%>
                    <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i style="font-size: 36px"></i></a>
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

                                        <div class="span4">
                                            <div class="control-group">

                                                <label class="control-label">Old Password <span class="Mandotary">*</span></label>

                                                <div class="controls">

                                                    <div style="display:flex!important" class="input-group">

                                                        <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password" CssClass="input-text"
                                                            MaxLength="12" TabIndex="1" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>

                                                        <div class="input-group-append">
                                                            <span class="input-group-text">
                                                                <a href="#" class="toggle_hide_password">
                                                                    <i style="margin-left:-26px;" class="fas fa-eye-slash" aria-hidden="true"></i>
                                                                </a>
                                                            </span>
                                                        </div>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtOldPwd" ErrorMessage="Enter old password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">

                                                <label class="control-label">New Password <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div style="display:flex!important" class="input-group">


                                                        <%--onkeypress="return AvoidSpace(event)"--%>
                                                        <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"
                                                            CssClass="input-text" MaxLength="12" TabIndex="2" onkeypress="return BlockCharSpace(event)" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>

                                                        <div class="input-group-append">
                                                            <span class="input-group-text">
                                                                <a href="#" class="toggle_hide_password">
                                                                    <i style="margin-left:-26px;" class="fas fa-eye-slash" aria-hidden="true"></i>
                                                                </a>
                                                            </span>
                                                        </div>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="txtNewPwd" ErrorMessage="Enter new password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">

                                                <label class="control-label ">Confirm Password <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div style="display:flex!important" class="input-group">
                                                        <%--onkeypress="return AvoidSpace(event)"--%>
                                                        <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password"
                                                            CssClass="input-text" MaxLength="12" TabIndex="3" onkeypress="return BlockCharSpace(event)" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>

                                                        <div class="input-group-append">
                                                            <span class="input-group-text">
                                                                <a href="#" class="toggle_hide_password">
                                                                    <i style="margin-left:-26px;" class="fas fa-eye-slash" aria-hidden="true"></i>
                                                                </a>
                                                            </span>
                                                        </div>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="txtConfirmPwd" ErrorMessage="Enter confirm password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1"></div>
                                        <div class="span7">
                                            <div class="control-group">
                                                <label style="font-size: small">
                                                    <span class="Mandotary">*Password should be greater than or equal to 8 character (Must Contain at least 1 Capital Letter,1 Digit,1 Small Letter, 1 Special Character)</span></label>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                                <div class="space20"></div>

                                <%--start password strength--%>
                                <div class="control-group">

                                    <div class="controls">
                                        <p>New Password Strength: <span id="result"></span></p>

                                        <div class="progress">
                                            <div id="password-strength" class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                                            </div>
                                        </div>

                                        <ul class="list-unstyled">
                                            <li class=""><span class="low-upper-case"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Lower Case &amp; 1 Upper Case</li>
                                            <li class=""><span class="one-number"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Number (0-9)</li>
                                            <li class=""><span class="one-special-char"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Special Character</li>
                                            <li class=""><span class="eight-character"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp; Atleast 8 Character</li>
                                        </ul>

                                    </div>

                                </div>

                                <%--end--%>
                                <div class="text-center">

                                    <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="btn btn-primary"
                                        ValidationGroup="reg" Height="30px" Width="80px" OnClientClick="javascript:return ValidateMyForm();"
                                        OnClick="btnsubmit_Click" TabIndex="4" />

                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" OnClientClick="javascript:return ResetForm();"
                                        CssClass="btn btn-danger" TabIndex="5" /><br />


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



</asp:Content>
