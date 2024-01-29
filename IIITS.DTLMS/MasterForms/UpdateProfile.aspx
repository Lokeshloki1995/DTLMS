<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="UpdateProfile.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.UpdateProfile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
        <script type="text/javascript">
        $(document).ready(function () {

            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })
        });
       
    </script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtFullName.ClientID %>').value.trim() == "") {
                alert('Enter Full Name')
                document.getElementById('<%= txtFullName.ClientID %>').focus()
                return false
            }

           
            if (document.getElementById('<%= txtEmailId.ClientID %>').value.trim() == "") {
                alert('Enter EmailId')
                document.getElementById('<%= txtEmailId.ClientID %>').focus()
                return false
            }

            var EmailId = document.getElementById('<%= txtEmailId.ClientID %>').value;
            var EmailIdcon = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            if (!EmailId.match(EmailIdcon)) {
                alert('Enter valid  EmailId')
                document.getElementById('<%= txtEmailId.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
                alert('Enter MobileNo.')
                document.getElementById('<%= txtMobile.ClientID %>').focus()
                return false
            }
            

     }
        function ResetForm() {
           
            document.getElementById('<%= txtFullName.ClientID %>').value = "";
            document.getElementById('<%= txtEmailId.ClientID %>').value = "";
            document.getElementById('<%= txtMobile.ClientID %>').value = "";

        

         return false
     }

     function AvoidSpace(event) {
         var k = event ? event.which : window.event.keyCode;
         if (k == 32) return false;
     }
         </script>

    <script language="Javascript" type="text/javascript">


         function onlyAlphabets(e, t) {
             var code = ('charCode' in e) ? e.charCode : e.keyCode;
             if ( // space
               !(code == 32) && // Special characters
                !(code == 46) &&
               !(code == 47) &&
               !(code > 64 && code < 91) && // upper alpha (A-Z)
               !(code > 96 && code < 123)) { // lower alpha (a-z)
                 e.preventDefault();
             }
         }

    </script> 
        <script language="Javascript" type="text/javascript">
        function OnlyNumberHyphen(e, t) {
            var text = document.getElementById("txtMobile").value;
            var regx = /^[6-9]\d{9}$/;
            if (regx.test(text))
                alert("valid");
            else
                alert("invalid");
        }

        function validateEmail(txtEmailId) {
            //var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/; 
            var reg = /^([a-zA-Z0-9._%+-])+\@([a-zA-Z0-9.-])+\.([A-Za-z]{2,4})$/;

            if (reg.test(txtEmailId.value) == false) {
                alert('Invalid Email Address');
                return false;
            }

            return true;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container-fluid">
           <div class="row-fluid">
                <div class="span12">
                    <h3 class="page-title">Update Profile
                          <div style="float:right;margin-top: 20px;">
                    <asp:Button ID="Close" runat="server" Text="Back"
                       OnClick="cmdClose_Click"
                        CssClass="btn btn-danger" />
                </div>
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
               </div><br />

          <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Update Profile</h4>
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
                                                <label class="control-label">Full Name<span class="Mandotary">* </span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtFullName" runat="server" MaxLength="100" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                        <asp:TextBox ID="txtSignImagePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtuserID" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            
<%--                                            <div class="control-group">
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSignImagePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtuserID" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>


                                              <div class="control-group">
                                                <label class="control-label">Email Id<span class="Mandotary">* </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" ></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Mobile No<span class="Mandotary">* </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtMobile" runat="server"  onkeypress="javascript:return OnlyNumberHyphen(this,event);" MaxLength="10"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            </div>

                                 <%--     <div class="span5" >--%>
                                              <%-- <div class="control-group">

                                                <label class="control-label">Old Password <span class="Mandotary"></span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password" CssClass="input-text"
                                                            MaxLength="12" TabIndex="1"></asp:TextBox>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtOldPwd" ErrorMessage="Enter old password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>



                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">New Password <span class="Mandotary"></span></label>

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
                                                <label class="control-label">Confirm Password <span class="Mandotary"></span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password"
                                                            CssClass="input-text" MaxLength="12" TabIndex="3" onkeypress="return AvoidSpace(event)"></asp:TextBox>

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="txtConfirmPwd" ErrorMessage="Enter confirm password"
                                                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>

                                            </div>--%>
                                    <%--    </div>--%>



                                    </div>
                                    <br />
                                     <div class="text-center" align="center">

                                   
                                   
                                        <asp:Button ID="btnsubmit" runat="server" Text="Update" CssClass="btn btn-primary"
                                             Height="30px" Width="80px" 
                                            OnClick="btnsubmit_Click" TabIndex="4" />

                                   
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" OnClientClick="javascript:return ResetForm();"
                                            CssClass="btn btn-danger" TabIndex="5" /><br />
                                  
                                    <div class="span7"></div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
         </div>

     </div>
</asp:Content>