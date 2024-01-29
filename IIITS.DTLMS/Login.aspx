<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IIITS.DTLMS.Login" %>

<html>


<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="Content-type" content="text/html; charset=utf-8">
    <meta content="" name="description" />
    <meta content="" name="author" />
    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" rel="stylesheet" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css" rel="stylesheet">
    <link href="js/bootsrap1.css" rel="stylesheet" />
    <link href="Styles/LoginPage/uniform.default.css" rel="stylesheet" type="text/css" />
    <!-- END GLOBAL MANDATORY STYLES -->
    <!-- BEGIN PAGE LEVEL STYLES -->
    <link href="Styles/LoginPage/login-soft.css" rel="stylesheet" type="text/css" />
    <!-- END PAGE LEVEL SCRIPTS -->
    <!-- BEGIN THEME STYLES -->
    <link href="Styles/LoginPage/components-rounded.css" id="style_components" rel="stylesheet" type="text/css" />
    <link href="Styles/LoginPage/plugins.css" rel="stylesheet" type="text/css" />
    <%-- <link href="Styles/LoginPage/layout.css" rel="stylesheet" type="text/css" />--%>
    <link href="Styles/LoginPage/default.css" rel="stylesheet" type="text/css" id="style_color" />
    <%--<link href="Styles/LoginPage/custom.css" rel="stylesheet" type="text/css"/>--%>
    <!-- END THEME STYLES -->
    <link rel="shortcut icon" href="img/Bescom.jpg" />
    <script src="js/jqueryOne.js" type="text/javascript"></script>
    <link href="js/bootsrapOne.css" rel="stylesheet" />

    <script src="js/bootstrapOne.js" type="text/javascript"></script>
    <script src="js/particle.js"></script>
    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <script src="Styles/LoginPage/jquery.validate.min.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->
    <script src="Styles/LoginPage/metronic.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/layout.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/demo.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/login-soft.js" type="text/javascript"></script>
    <script type="text/javascript">
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            // target the link
            $(".toggle_hide_password").on('click', function (e) {
               debugger

                // get input group of clicked link
                var input_group = $(this).closest('.input-icon')

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

        function preventMultipleSubmissions() {
            $('#<%=cmdFSave.ClientID %>').prop('disabled', true);
        }
        window.onbeforeunload = preventMultipleSubmissions;

    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <script type="text/javascript">
        jQuery(document).ready(function () {

            $('input').on('input', function () {
                $(this).val($(this).val().replace(/[^a-z0-9!@_+=*&%.()]/gi, ''));
            });

            const d = new Date();
            let year = d.getFullYear();
            document.getElementById("demo").innerHTML = year;

            Metronic.init(); // init metronic core components
            Layout.init(); // init current layout
            Login.init();
            Demo.init();
            // init background slide images
            //    $.backstretch([
            //    "Styles/LoginPage/DTC.jpg",
            //    "Styles/LoginPage/DT-4.jpg",
            //    "Styles/LoginPage/DT-6.jpg",
            //    "Styles/LoginPage/T1.jpg"
            //    ], {
            //        fade: 1000,
            //        duration: 8000
            //    }
            //);
        });

        //let passwordInput = document.getElementById('txtConfirmPassword'),
        jQuery(document).ready(function () {

            $('#txtNewpwd').keyup(function () {
                debugger;
                var password = $('#txtNewpwd').val();

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

    </script>
    <%-- <script type="text/javascript">
     $(function () {
         cuenta = 0;
         txtArray = ["For new Registrations click here to enroll your details", "For new Registrations click here to enroll your details"];
         setInterval(function () {
             cuenta++;
             $("#titulos").fadeOut(100, function () {
                 $(this)
                   .text(txtArray[cuenta % txtArray.length])
                   .css('color', cuenta % 2 == 0 ? 'red' : 'white')
                   .fadeIn(100);
             });
         }, 3000);
     });
 </script>--%>
    <style>
        i.fa.fa-eye, .fa-eye-slash:before {
            font-size: 16px;
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
    <script type="text/javascript"> var message = '';
        function clickIE() { if (event.button == 2) { return false; } }
        function clickNS(e) {
            if (document.layers || (document.getElementById && !document.all)) {
                if (e.which == 2 || e.which == 3) { return false; }
            }
        }
        if (document.layers) { document.captureEvents(Event.MOUSEDOWN); document.onmousedown = clickNS; }
        else if (document.all && !document.getElementById) { document.onmousedown = clickIE; }
        document.oncontextmenu = new Function('return false')

    </script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtUsername.ClientID %>').value.trim() == "") {
                alert('Enter Valid User Name and Password')
                document.getElementById('<%= txtUsername.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtPassword.ClientID %>').value.trim() == "") {
                alert('Enter Valid User Name and Password')
                document.getElementById('<%= txtPassword.ClientID %>').focus()
                return false
            }
        }

        function ValidateNumber() {
            if (document.getElementById('<%= txtEmail.ClientID %>').value.trim() == "") {
                alert('Please Enter Register Mail Id / Mobile number to get OTP')
                document.getElementById('<%= txtEmail.ClientID %>').focus()
                return false
            }
        }

        function ValidatePassword() {

            if (document.getElementById('<%= txtOTP.ClientID %>').value.trim() == "") {
                alert('Enter Valid OTP')
                document.getElementById('<%= txtOTP.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtNewpwd.ClientID %>').value.trim() == "") {
                alert('Enter Valid New Password')
                document.getElementById('<%= txtNewpwd.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtCnfrmPwd.ClientID %>').value.trim() == "") {
                alert('Enter Valid Confirmation Password')
                document.getElementById('<%= txtCnfrmPwd.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtNewpwd.ClientID %>').value.trim() != "") {
                var pass = document.getElementById('<%= txtNewpwd.ClientID %>').value
                //if (!pass.match(/^(?=.{8,})(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[#!*()$%^&+-={}@@]).*$/)) {
                if (!pass.match(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,12}$/)) {
                    alert("Password should be greater than or equal to 8 character (Must Contain at least 1 Capital Letter,1 Digit,1 Small Letter, 1 Special Character)")
                    document.getElementById('<%=txtNewpwd.ClientID %>').focus()
                    return false;
                }


            }

            if (document.getElementById('<%= txtCnfrmPwd.ClientID %>').value.trim() != "") {
                var pass = document.getElementById('<%= txtCnfrmPwd.ClientID %>').value
                if (!pass.match(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,12}$/)) {
                    alert("Password should be greater than or equal to 8 character (Must Contain at least 1 Capital Letter,1 Digit,1 Small Letter, 1 Special Character)")
                    document.getElementById('<%=txtCnfrmPwd.ClientID %>').focus()
                    return false;
                }

            }
            if(document.getElementById('<%= txtNewpwd.ClientID %>').value.trim() != document.getElementById('<%= txtCnfrmPwd.ClientID %>').value.trim()){
                alert("New Password and Confirmation Password should be same.")
                document.getElementById('<%=txtCnfrmPwd.ClientID %>').focus()
                return false;
            }
            
        }

        function ValidateForgotYourPassword() {

            if (document.getElementById('<%= txtUsername.ClientID %>').value.trim() == "") {
                debugger;
                alert('Please Enter Valid User Name');
                $("#form1").show();
                $("#ResetPwd").hide();
                document.getElementById('<%= txtUsername.ClientID %>').focus();               
                return false;
            }
            else {
                jQuery('.ResetPwd-form').show();
                jQuery('.login-form').hide();
               jQuery('.logooo').hide();
                
            }
        }
        //function stopPropagation(event) {
        //    debugger;            
        //    event.stopPropagation();
        //}
    </script>
    <style>
        body {
            background: #0264d6; /* Old browsers */
            background: -moz-radial-gradient(center, ellipse cover, #0264d6 1%, #1c2b5a 100%); /* FF3.6+ */
            background: -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(1%,#0264d6), color-stop(100%,#1c2b5a)); /* Chrome,Safari4+ */
            background: -webkit-radial-gradient(center, ellipse cover, #0264d6 1%,#1c2b5a 100%); /* Chrome10+,Safari5.1+ */
            background: -o-radial-gradient(center, ellipse cover, #0264d6 1%,#1c2b5a 100%); /* Opera 12+ */
            background: -ms-radial-gradient(center, ellipse cover, #0264d6 1%,#1c2b5a 100%); /* IE10+ */
            background: radial-gradient(ellipse at center, #0264d6 1%,#1c2b5a 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#0264d6', endColorstr='#1c2b5a',GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            height: calc(100vh);
            width: 100%;
            /*overflow: hidden !important;*/
        }

        .form-group {
            position: relative;
        }

            .form-group input[type="password"] {
                padding-right: 30px;
            }

            .form-group .glyphicon {
                right: 6px;
                position: absolute;
                top: 12px;
            }

            .form-group .glyphiconConPass {
                right: 6px;
                position: absolute;
                top: 12px;
            }
    </style>
    <style type="text/css">
        span#titulos {
            margin-right: 0%;
            float: right;
            padding: 0px 0px 0px 0px;
            font-size: 20px;
            font-weight: bolder;
            cursor: pointer;
        }
    </style>
    <style>
        .ResetPwd-form {
            margin-top: -59px !important;
        }

        input#btnResetPwd {
            margin-top: -58px;
        }

        canvas {
            z-index: -1;
            top: 0;
            left: 0;
        }

        #particles-js {
            width: 100%;
            height: 100%;
            position: absolute;
            opacity: 0.3;
        }

        div#Form2 {
            border-right: 1px solid #fff !important;
            padding: 9px 45px 9px 0px !important;
        }

        .login .content .Reset-password {
            margin-left: -37% !important;
        }

        blink {
            -webkit-animation: 2s linear infinite condemned_blink_effect;
            animation: 2s linear infinite condemned_blink_effect;
        }

        @-webkit-keyframes condemned_blink_effect {
            // for Safari 4.0 - 8.0 0% {
                visibility: hidden;
            }

            50% {
                visibility: hidden;
            }

            100% {
                visibility: visible;
            }
        }

        @keyframes condemned_blink_effect {
            0% {
                visibility: hidden;
            }

            50% {
                visibility: hidden;
            }

            100% {
                visibility: visible;
            }
        }
    </style>




</head>

<body class="login">
    <div>
    </div>
    <div id="particles-js">
        <div class="UserRegistration" runat="server" id="userrgt">
            <p>

                <%--<marquee id="PlayStoreLink" direction="left" onMouseOver="document.all.PlayStoreLink.stop()" onMouseOut="document.all.PlayStoreLink.start()">
                    <asp:HyperLink ID="lnkPlayStoreLin" Target="_blank" style="font-size:15px;color:white;font-weight:bold ; margin-top: 0%; margin-right: 2%; font-size: 19px; z-index: 9999999!important"" 
                         NavigateUrl="https://docs.google.com/spreadsheets/d/1KkNnwdEp3I2XlGTYaaq9cj2K98c1Kwhj1eTJOV-v0GU/edit?usp=sharing" runat="server"> For new User Creation click here to enroll your details</asp:HyperLink>

                </marquee>--%>
            </p>
        </div>
        <canvas height="651" width="1366" style="width: 100%; height: 100%;"></canvas>
    </div>
    <div class="logo" style="color: antiquewhite; font-weight: bold">
        <marquee behavior="pgrs" direction="left" onmouseover="this.stop();" onmouseout="this.start();">
                  <blink>
               <img style="width: 2%!important;float: left!important;margin-top: 0%;" src="img/new.png" />
                   </blink>  <a href="https://docs.google.com/spreadsheets/d/1KkNnwdEp3I2XlGTYaaq9cj2K98c1Kwhj1eTJOV-v0GU/edit?usp=sharing" 
                    target="_blank" style="color: white; s margin-top: 0%; margin-right: 2%; font-size: 19px; z-index: 9999999!important"
                    >For new User Creation click here to enroll your details </a>
                  </marquee>
        <h1 style="text-transform: capitalize">Distribution Transformer LifeCycle 
                <br />
            Management Software</h1>
        <%--<a href="index.html">
	<img src="../../assets/admin/layout4/img/logo-big.png" alt=""/>
	</a>--%>
    </div>
    <%-- <div class="span5">
                 
               <a href="https://docs.google.com/spreadsheets/d/1KkNnwdEp3I2XlGTYaaq9cj2K98c1Kwhj1eTJOV-v0GU/edit?usp=sharing" target="_blank" data-toggle="modal" data-target="#myModal" style="width: 7%!important;float: right!important;margin-right: 0%;margin-top: 1%;">
                  <span style="color:red"id="titulos">For new Registrations click here to enroll your details</span>  </a>            
  
            </div>--%>


    <%--<blink>
<img style="width: 7%!important;float: right!important;margin-left: -4%;margin-top: -1%;" src="img/new.png" />
                   </blink>--%>
    <form id="form1" runat="server">

        <div class="menu-toggler sidebar-toggler">
        </div>
        <!-- END SIDEBAR TOGGLER BUTTON -->
        <!-- BEGIN LOGIN -->
        <div>
            <div class="content" runat="server" id="UserNamePswPag">
                <div class="row" >
                    <!-- BEGIN LOGIN FORM -->
                    <div class="col-md-10">
                        <div id="Form2" class="login-form" runat="server">
                            <%-- <h3 class="form-title" style="font-weight: bold">Login to your account</h3>--%>
                            <div class="alert alert-danger display-hide">
                                <button class="close" data-close="alert"></button>
                                <span>Enter any username and password. </span>
                            </div>
                            <div style="margin-left: -40%;" class="form-group">
                                <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                                <label class="control-label visible-ie8 visible-ie9">Login Type</label>
                                <div class="input-icon">
                                    <i style="color: #000" class="fa fa-user"></i>

                                    <asp:TextBox ID="txtUsername" autocomplete="off" class="form-control placeholder-no-fix" MaxLength="20" placeholder="User Name"
                                       oncopy="return false" onpaste="return false" oncut="return false"  runat="server"></asp:TextBox>
                                 

                                   <%-- <asp:TextBox ID="txtUserId" autocomplete="off" visible="false"    runat="server" ></asp:TextBox>--%> <%--this is to store UserId--%>

                                </div>
                            </div>
                            <div style="margin-left: -40%;" class="form-group">
                                <label class="control-label visible-ie8 visible-ie9">Section Office</label>
                                <div class="input-icon">

                                    <i style="color: #000" class="fa fa-lock"></i>
                                    <asp:TextBox ID="txtPassword" class="form-control placeholder-no-fix" placeholder="Password" autocomplete="off" MaxLength="12" runat="server"
                                        TextMode="Password" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>
                                    <span class="glyphicon glyphicon-eye-open"></span>
                                </div>
                            </div>

                            <div class="form-actions form-group">

                                <asp:Button ID="cmdLogin" runat="server" Text="Login" OnClientClick="javascript:return ValidateMyForm()"
                                    class="btn blue pull-right " Style="z-index: 9999999!important" OnClick="cmdLogin_Click" />



                                <%--<div class="forget-password" runat="server" id="dvForgtPwd">		
			<p>
				 <a href="javascript:;" id="forget-password" style="color:White" >
				Forgot your password ? </a>
				
			</p>
		  </div>--%>

                                <div class="Reset-password" runat="server">
                                    <p>
                                        <%--onclick="event.stopPropagation();"--%>
                                        <a id="Reset-password" style="color: White; font-size: 11px; z-index: 9999999!important"
                                            onclick="ValidateForgotYourPassword()">Forgot your password ?  </a>

                                        <%-- <a href="javascript:;" id="ForgotYourPsw" class="ForgotYourPsw" style="color: White; font-size: 11px; z-index: 9999999!important"
                                            onclick="javascript:return ValidateForgotYourPassword()">Forgot your password ?  </a>--%>
                                    </p>

                                </div>

                                <%--  <div> <p>
                           <%-- <a href="javascript:;" id="UserRegistration" style="color: White;font-size: 11px;z-index: 9999999!important">Forgot your password ?  </a>--%>
                                <%-- <a href="https://docs.google.com/spreadsheets/d/1KkNnwdEp3I2XlGTYaaq9cj2K98c1Kwhj1eTJOV-v0GU/edit?usp=sharing" id="UserRegistration" target="_blank" style="color:red; font-size: 17px;z-index: 9999999!important">User Registration. </a>

                    </p></div> --%>
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="logooo" runat="server" id="logoblock" >
                            <img style="width: 114px; border-bottom-right-radius: 29px; border-top-left-radius: 29px; margin-top: 18px; box-shadow: 0px 3px 8px 6px #888888" src="img/bescomlogo.jpg" />
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <!-- END LOGIN FORM -->

                <!-- BEGIN FORGOT PASSWORD FORM -->

                <!-- END FORGOT PASSWORD FORM -->
            </div>

            <div class="content form-group" runat="server" id="ResetPswPag">
                <!-- BEGIN FORGOT PASSWORD BY OTP FORM -->
                <div id="ResetPwd" runat="server" class="ResetPwd-form" action="index.html" method="post">
                    <h3>Forget Password ?</h3>
                    <p>
                        Enter your e-mail ID / Mobile No. below to reset your password.
                    </p>
                    <div class="form-group">
                        <div class="input-icon">
                            <i class="fa fa-envelope"></i>
                            <asp:TextBox ID="txtEmail" class="form-control placeholder-no-fix" placeholder="Email / Mobile No" autocomplete="off" oncopy="return false" onpaste="return false" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label ID="lblFMsg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="form-actions">
                        <%--<button type="button" id="back-btn" class="btn">
			<i class="m-icon-swapleft"></i> Back </button>--%>
                        <div class="Reset-password" runat="server" id="Div2">
                            <asp:Button ID="cmdFSave" runat="server" Text="Get OTP"
                                class="btn blue pull-right " OnClick="cmdFSave_Click" OnClientClick="javascript:return ValidateNumber()" onchange="javascript:preventMultipleSubmissions();" />
                        </div>
                    </div>
                    <p>
                        Enter OTP
                    </p>
                    <div class="form-group">
                        <div class="input-icon">
                            <i class="fa fa-user"></i>
                            <asp:TextBox ID="txtOTP" class="form-control placeholder-no-fix" autocomplete="off" oncopy="return false" onpaste="return false" placeholder="Enter OTP" MaxLength="9" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <p>
                        Enter New Password
                    </p>
                    <div class="form-group">
                        <div class="input-icon">
                            <i style="color: #000" class="fa fa-lock"></i>
                            <asp:TextBox ID="txtNewpwd" class="form-control placeholder-no-fix input-text"
                                autocomplete="off" TextMode="Password" placeholder="New Password"
                                MaxLength="12" runat="server" oncopy="return false"
                                onpaste="return false" oncut="return false"></asp:TextBox>
                               <div class="input-group-append">
                                <span style="float: right; margin-top: -24px; margin-right: 13px; ">
                                    <a href="#" class="toggle_hide_password">
                                        <i class="fas fa-eye-slash" style="color: #000"></i></a>
                                </span>
                            </div>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="txtNewpwd" ErrorMessage="Enter new password"
                                ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <p>
                        Enter Confirm New Password
                    </p>
                    <div class="form-group">
                        <div class="input-icon">
                            <i style="color: #000" class="fa fa-lock"></i>
                            <asp:TextBox ID="txtCnfrmPwd" class="form-control placeholder-no-fix input-text" autocomplete="off" TextMode="Password"
                                MaxLength="12" placeholder="Confirm New Password" runat="server" oncopy="return false"
                                onpaste="return false" oncut="return false"></asp:TextBox>
                            <%--<span class="glyphicon glyphicon-eye-open"></span>--%>

                            <div class="input-group-append">
                                <span style="float: right; margin-top: -24px; margin-right: 13px;">
                                    <a href="#" class="toggle_hide_password">
                                        <i class="fas fa-eye-slash" style="color: #000"></i></a>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="form-actions">
                        <button type="button" id="back-btn" class="btn" style="background-color: #fffafa">
                            <i class="m-icon-swapleft"></i>Back
                        </button>
                        <div class="Reset-password" runat="server" id="Div1">
                            <asp:Button ID="btnResetPwd" runat="server" Text="Reset Password" OnClientClick="javascript:return ValidatePassword()"
                                class="btn blue pull-right " OnClick="btnResetPwd_Click" />
                        </div>
                    </div>

                    <%--start password strength--%>
                    <div class="control-group">

                        <div class="controls">
                            <p>New Password Strength: <span id="result"></span></p>
                            <div class="progress">
                                <div id="password-strength" class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                                </div>
                            </div>
                            <ul class="list-unstyled" style="color: white">
                                <li class=""><span class="low-upper-case"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Lower Case &amp; 1 Upper Case</li>
                                <li class=""><span class="one-number"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Number (0-9)</li>
                                <li class=""><span class="one-special-char"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Special Character</li>
                                <li class=""><span class="eight-character"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp; Atleast 8 Character</li>
                            </ul>
                        </div>
                    </div>
                    <%--end--%>

                </div>
                <!-- END FORGOT PASSWORD BY OTP FORM -->
            </div>

        </div>

        <!-- END LOGIN -->
        <!-- BEGIN COPYRIGHT -->
        <div style="color: #fff; font-size: 16px" class="copyright">
            <label id="demo"></label>
            &copy; Idea Infinity IT Solutions (P) Ltd.
        </div>

    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            function launchParticlesJS(a, e) { var i = document.querySelector("#" + a + " > canvas"); pJS = { canvas: { el: i, w: i.offsetWidth, h: i.offsetHeight }, particles: { color: "#fff", shape: "circle", opacity: 1, size: 2.5, size_random: true, nb: 200, line_linked: { enable_auto: true, distance: 100, color: "#fff", opacity: 1, width: 1, condensed_mode: { enable: true, rotateX: 65000, rotateY: 65000 } }, anim: { enable: true, speed: 1 }, array: [] }, interactivity: { enable: true, mouse: { distance: 100 }, detect_on: "canvas", mode: "grab", line_linked: { opacity: 1 }, events: { onclick: { enable: true, mode: "push", nb: 4 } } }, retina_detect: false, fn: { vendors: { interactivity: {} } } }; if (e) { if (e.particles) { var b = e.particles; if (b.color) { pJS.particles.color = b.color } if (b.shape) { pJS.particles.shape = b.shape } if (b.opacity) { pJS.particles.opacity = b.opacity } if (b.size) { pJS.particles.size = b.size } if (b.size_random == false) { pJS.particles.size_random = b.size_random } if (b.nb) { pJS.particles.nb = b.nb } if (b.line_linked) { var j = b.line_linked; if (j.enable_auto == false) { pJS.particles.line_linked.enable_auto = j.enable_auto } if (j.distance) { pJS.particles.line_linked.distance = j.distance } if (j.color) { pJS.particles.line_linked.color = j.color } if (j.opacity) { pJS.particles.line_linked.opacity = j.opacity } if (j.width) { pJS.particles.line_linked.width = j.width } if (j.condensed_mode) { var g = j.condensed_mode; if (g.enable == false) { pJS.particles.line_linked.condensed_mode.enable = g.enable } if (g.rotateX) { pJS.particles.line_linked.condensed_mode.rotateX = g.rotateX } if (g.rotateY) { pJS.particles.line_linked.condensed_mode.rotateY = g.rotateY } } } if (b.anim) { var k = b.anim; if (k.enable == false) { pJS.particles.anim.enable = k.enable } if (k.speed) { pJS.particles.anim.speed = k.speed } } } if (e.interactivity) { var c = e.interactivity; if (c.enable == false) { pJS.interactivity.enable = c.enable } if (c.mouse) { if (c.mouse.distance) { pJS.interactivity.mouse.distance = c.mouse.distance } } if (c.detect_on) { pJS.interactivity.detect_on = c.detect_on } if (c.mode) { pJS.interactivity.mode = c.mode } if (c.line_linked) { if (c.line_linked.opacity) { pJS.interactivity.line_linked.opacity = c.line_linked.opacity } } if (c.events) { var d = c.events; if (d.onclick) { var h = d.onclick; if (h.enable == false) { pJS.interactivity.events.onclick.enable = false } if (h.mode != "push") { pJS.interactivity.events.onclick.mode = h.mode } if (h.nb) { pJS.interactivity.events.onclick.nb = h.nb } } } } pJS.retina_detect = e.retina_detect } pJS.particles.color_rgb = hexToRgb(pJS.particles.color); pJS.particles.line_linked.color_rgb_line = hexToRgb(pJS.particles.line_linked.color); if (pJS.retina_detect && window.devicePixelRatio > 1) { pJS.retina = true; pJS.canvas.pxratio = window.devicePixelRatio; pJS.canvas.w = pJS.canvas.el.offsetWidth * pJS.canvas.pxratio; pJS.canvas.h = pJS.canvas.el.offsetHeight * pJS.canvas.pxratio; pJS.particles.anim.speed = pJS.particles.anim.speed * pJS.canvas.pxratio; pJS.particles.line_linked.distance = pJS.particles.line_linked.distance * pJS.canvas.pxratio; pJS.particles.line_linked.width = pJS.particles.line_linked.width * pJS.canvas.pxratio; pJS.interactivity.mouse.distance = pJS.interactivity.mouse.distance * pJS.canvas.pxratio } pJS.fn.canvasInit = function () { pJS.canvas.ctx = pJS.canvas.el.getContext("2d") }; pJS.fn.canvasSize = function () { pJS.canvas.el.width = pJS.canvas.w; pJS.canvas.el.height = pJS.canvas.h; window.onresize = function () { if (pJS) { pJS.canvas.w = pJS.canvas.el.offsetWidth; pJS.canvas.h = pJS.canvas.el.offsetHeight; if (pJS.retina) { pJS.canvas.w *= pJS.canvas.pxratio; pJS.canvas.h *= pJS.canvas.pxratio } pJS.canvas.el.width = pJS.canvas.w; pJS.canvas.el.height = pJS.canvas.h; pJS.fn.canvasPaint(); if (!pJS.particles.anim.enable) { pJS.fn.particlesRemove(); pJS.fn.canvasRemove(); f() } } } }; pJS.fn.canvasPaint = function () { pJS.canvas.ctx.fillRect(0, 0, pJS.canvas.w, pJS.canvas.h) }; pJS.fn.canvasRemove = function () { pJS.canvas.ctx.clearRect(0, 0, pJS.canvas.w, pJS.canvas.h) }; pJS.fn.particle = function (n, o, m) { this.x = m ? m.x : Math.random() * pJS.canvas.w; this.y = m ? m.y : Math.random() * pJS.canvas.h; this.radius = (pJS.particles.size_random ? Math.random() : 1) * pJS.particles.size; if (pJS.retina) { this.radius *= pJS.canvas.pxratio } this.color = n; this.opacity = o; this.vx = -0.5 + Math.random(); this.vy = -0.5 + Math.random(); this.draw = function () { pJS.canvas.ctx.fillStyle = "rgba(" + this.color.r + "," + this.color.g + "," + this.color.b + "," + this.opacity + ")"; pJS.canvas.ctx.beginPath(); switch (pJS.particles.shape) { case "circle": pJS.canvas.ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2, false); break; case "edge": pJS.canvas.ctx.rect(this.x, this.y, this.radius * 2, this.radius * 2); break; case "triangle": pJS.canvas.ctx.moveTo(this.x, this.y - this.radius); pJS.canvas.ctx.lineTo(this.x + this.radius, this.y + this.radius); pJS.canvas.ctx.lineTo(this.x - this.radius, this.y + this.radius); pJS.canvas.ctx.closePath(); break } pJS.canvas.ctx.fill() } }; pJS.fn.particlesCreate = function () { for (var m = 0; m < pJS.particles.nb; m++) { pJS.particles.array.push(new pJS.fn.particle(pJS.particles.color_rgb, pJS.particles.opacity)) } }; pJS.fn.particlesAnimate = function () { for (var n = 0; n < pJS.particles.array.length; n++) { var q = pJS.particles.array[n]; q.x += q.vx * (pJS.particles.anim.speed / 2); q.y += q.vy * (pJS.particles.anim.speed / 2); if (q.x - q.radius > pJS.canvas.w) { q.x = q.radius } else { if (q.x + q.radius < 0) { q.x = pJS.canvas.w + q.radius } } if (q.y - q.radius > pJS.canvas.h) { q.y = q.radius } else { if (q.y + q.radius < 0) { q.y = pJS.canvas.h + q.radius } } for (var m = n + 1; m < pJS.particles.array.length; m++) { var o = pJS.particles.array[m]; if (pJS.particles.line_linked.enable_auto) { pJS.fn.vendors.distanceParticles(q, o) } if (pJS.interactivity.enable) { switch (pJS.interactivity.mode) { case "grab": pJS.fn.vendors.interactivity.grabParticles(q, o); break } } } } }; pJS.fn.particlesDraw = function () { pJS.canvas.ctx.clearRect(0, 0, pJS.canvas.w, pJS.canvas.h); pJS.fn.particlesAnimate(); for (var m = 0; m < pJS.particles.array.length; m++) { var n = pJS.particles.array[m]; n.draw("rgba(" + n.color.r + "," + n.color.g + "," + n.color.b + "," + n.opacity + ")") } }; pJS.fn.particlesRemove = function () { pJS.particles.array = [] }; pJS.fn.vendors.distanceParticles = function (t, r) { var o = t.x - r.x, n = t.y - r.y, s = Math.sqrt(o * o + n * n); if (s <= pJS.particles.line_linked.distance) { var m = pJS.particles.line_linked.color_rgb_line; pJS.canvas.ctx.beginPath(); pJS.canvas.ctx.strokeStyle = "rgba(" + m.r + "," + m.g + "," + m.b + "," + (pJS.particles.line_linked.opacity - s / pJS.particles.line_linked.distance) + ")"; pJS.canvas.ctx.moveTo(t.x, t.y); pJS.canvas.ctx.lineTo(r.x, r.y); pJS.canvas.ctx.lineWidth = pJS.particles.line_linked.width; pJS.canvas.ctx.stroke(); pJS.canvas.ctx.closePath(); if (pJS.particles.line_linked.condensed_mode.enable) { var o = t.x - r.x, n = t.y - r.y, q = o / (pJS.particles.line_linked.condensed_mode.rotateX * 1000), p = n / (pJS.particles.line_linked.condensed_mode.rotateY * 1000); r.vx += q; r.vy += p } } }; pJS.fn.vendors.interactivity.listeners = function () { if (pJS.interactivity.detect_on == "window") { var m = window } else { var m = pJS.canvas.el } m.onmousemove = function (p) { if (m == window) { var o = p.clientX, n = p.clientY } else { var o = p.offsetX || p.clientX, n = p.offsetY || p.clientY } if (pJS) { pJS.interactivity.mouse.pos_x = o; pJS.interactivity.mouse.pos_y = n; if (pJS.retina) { pJS.interactivity.mouse.pos_x *= pJS.canvas.pxratio; pJS.interactivity.mouse.pos_y *= pJS.canvas.pxratio } pJS.interactivity.status = "mousemove" } }; m.onmouseleave = function (n) { if (pJS) { pJS.interactivity.mouse.pos_x = 0; pJS.interactivity.mouse.pos_y = 0; pJS.interactivity.status = "mouseleave" } }; if (pJS.interactivity.events.onclick.enable) { switch (pJS.interactivity.events.onclick.mode) { case "push": m.onclick = function (o) { if (pJS) { for (var n = 0; n < pJS.interactivity.events.onclick.nb; n++) { pJS.particles.array.push(new pJS.fn.particle(pJS.particles.color_rgb, pJS.particles.opacity, { x: pJS.interactivity.mouse.pos_x, y: pJS.interactivity.mouse.pos_y })) } } }; break; case "remove": m.onclick = function (n) { pJS.particles.array.splice(0, pJS.interactivity.events.onclick.nb) }; break } } }; pJS.fn.vendors.interactivity.grabParticles = function (r, q) { var u = r.x - q.x, s = r.y - q.y, p = Math.sqrt(u * u + s * s); var t = r.x - pJS.interactivity.mouse.pos_x, n = r.y - pJS.interactivity.mouse.pos_y, o = Math.sqrt(t * t + n * n); if (p <= pJS.particles.line_linked.distance && o <= pJS.interactivity.mouse.distance && pJS.interactivity.status == "mousemove") { var m = pJS.particles.line_linked.color_rgb_line; pJS.canvas.ctx.beginPath(); pJS.canvas.ctx.strokeStyle = "rgba(" + m.r + "," + m.g + "," + m.b + "," + (pJS.interactivity.line_linked.opacity - o / pJS.interactivity.mouse.distance) + ")"; pJS.canvas.ctx.moveTo(r.x, r.y); pJS.canvas.ctx.lineTo(pJS.interactivity.mouse.pos_x, pJS.interactivity.mouse.pos_y); pJS.canvas.ctx.lineWidth = pJS.particles.line_linked.width; pJS.canvas.ctx.stroke(); pJS.canvas.ctx.closePath() } }; pJS.fn.vendors.destroy = function () { cancelAnimationFrame(pJS.fn.requestAnimFrame); i.remove(); delete pJS }; function f() { pJS.fn.canvasInit(); pJS.fn.canvasSize(); pJS.fn.canvasPaint(); pJS.fn.particlesCreate(); pJS.fn.particlesDraw() } function l() { pJS.fn.particlesDraw(); pJS.fn.requestAnimFrame = requestAnimFrame(l) } f(); if (pJS.particles.anim.enable) { l() } if (pJS.interactivity.enable) { pJS.fn.vendors.interactivity.listeners() } } window.requestAnimFrame = (function () { return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || window.oRequestAnimationFrame || window.msRequestAnimationFrame || function (a) { window.setTimeout(a, 1000 / 60) } })(); window.cancelRequestAnimFrame = (function () { return window.cancelAnimationFrame || window.webkitCancelRequestAnimationFrame || window.mozCancelRequestAnimationFrame || window.oCancelRequestAnimationFrame || window.msCancelRequestAnimationFrame || clearTimeout })(); function hexToRgb(c) { var b = /^#?([a-f\d])([a-f\d])([a-f\d])$/i; c = c.replace(b, function (e, h, f, d) { return h + h + f + f + d + d }); var a = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(c); return a ? { r: parseInt(a[1], 16), g: parseInt(a[2], 16), b: parseInt(a[3], 16) } : null } window.particlesJS = function (d, c) { if (typeof (d) != "string") { c = d; d = "particles-js" } if (!d) { d = "particles-js" } var b = document.createElement("canvas"); b.style.width = "100%"; b.style.height = "100%"; var a = document.getElementById(d).appendChild(b); if (a != null) { launchParticlesJS(d, c) } };

            /* particlesJS('dom-id', params);
            /* @dom-id : set the html tag id [string, optional, default value : particles-js]
            /* @params: set the params [object, optional, default values : check particles.js] */

            /* config dom id (optional) + config particles params */
            particlesJS('particles-js', {
                particles: {
                    color: '#fff',
                    shape: 'circle', // "circle", "edge" or "triangle"
                    opacity: 3,
                    size: 4,
                    size_random: true,
                    nb: 560,
                    line_linked: {
                        enable_auto: true,
                        distance: 100,
                        color: '#fff',
                        opacity: 1,
                        width: 1,
                        condensed_mode: {
                            enable: false,
                            rotateX: 600,
                            rotateY: 600
                        }
                    },
                    anim: {
                        enable: true,
                        speed: 1
                    }
                },
                interactivity: {
                    enable: true,
                    mouse: {
                        distance: 250
                    },
                    detect_on: 'canvas', // "canvas" or "window"
                    mode: 'grab',
                    line_linked: {
                        opacity: .5
                    },
                    events: {
                        onclick: {
                            enable: true,
                            mode: 'push', // "push" or "remove" (particles)
                            nb: 4
                        },
                        "onhover": {
                            "enable": true,
                            "mode": "grab",
                            "color": "white"
                        },
                    }
                },
                /* Retina Display Support */
                retina_detect: true
            });
        });
    </script>
    <script type="text/javascript">
        $(".glyphicon-eye-open").on("click", function () {
            $(this).toggleClass("glyphicon-eye-close");
            var type = $("#txtPassword").attr("type");
            if (type == "text")
            { $("#txtPassword").prop('type', 'password'); }
            else
            { $("#txtPassword").prop('type', 'text'); }
        });
    </script>
    <script type="text/javascript">
        $("#toggleNewPassword").on("click", function () {
            $(this).toggleClass("#toggleNewPassword");
            var type = $("#txtNewpwd").attr("type");
            if (type == "text")
            { $("#txtNewpwd").prop('type', 'password'); }
            else
            { $("#txtNewpwd").prop('type', 'text'); }
        });
    </script>
    <script type="text/javascript">
            $("#togglePassword").on("click", function () {
                $(this).toggleClass("#togglePassword");
                var type = $("#txtCnfrmPwd").attr("type");
                if (type == "text")
                { $("#txtCnfrmPwd").prop('type', 'password'); }
                else
                { $("#txtCnfrmPwd").prop('type', 'text'); }
            });
    </script>
</body>
</html>
