<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Zone.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.Zone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/functions.js" type="text/javascript"></script>


                                <script type="text/javascript">
        $(document).ready(function () {
            $('textarea').attr('maxlength', '300');
            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })
            $('textarea').keyup(function () {
                $('textarea').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })

        });
       
    </script>
    <script type="text/javascript">
        
       
        function ValidateMyForm()
        {
            Debugger;
            if (document.getElementById('<%= txtZoneId.ClientID %>').value.trim() == "0") {
                 alert('Zone ID Cannot be 0')
                 document.getElementById('<%= txtZoneId.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtZoneId.ClientID %>').value.trim() == "") {
                 alert('Please Enter Zone ID')
                 document.getElementById('<%= txtZoneId.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtZoneName.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Zone Name')
                 document.getElementById('<%= txtZoneName.ClientID %>').focus()
                 return false
             }
            
          

           
             // var ZoneName = document.getElementById('<%= txtZoneName.ClientID %>').value;   
             // var ZoneNamecon = /^([a-zA-Z]+)(\s-\s)*[a-zA-Z]+$/;
             // if (!ZoneName.match(ZoneNamecon)) {
             // alert('Enter valid Zone Name')
             // document.getElementById('<%= txtZoneName.ClientID %>').focus()
             // return false
             // }

             if (document.getElementById('<%= txtFullName.ClientID %>').value.trim() == "") {
                 alert('Please Enter Name Of Head')
                 document.getElementById('<%= txtFullName.ClientID %>').focus()
                 return false
             }
            

             if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
                 alert('Enter Mobile No')
                 document.getElementById('<%= txtMobile.ClientID %>').focus()
                 return false
             }
            if (document.getElementById('<%= txtPhone.ClientID %>').value.trim() == "") {
                 alert('Enter Phone No')
                 document.getElementById('<%= txtPhone.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtZoneAddress.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Register Address')
                 document.getElementById('<%= txtZoneAddress.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtEmailId.ClientID %>').value.trim() == "") {
                 alert('Enter  EmaiId')
                 document.getElementById('<%= txtEmailId.ClientID %>').focus()
                 return false
             }
         }
    </script>

            <script language="Javascript" type="text/javascript">


                function onlyAlphabets(e, t) {
                    var code = ('charCode' in e) ? e.charCode : e.keyCode;
                    if ( // space
                      !(code == 32) && // Special characters
                        !(code > 39 && code < 42) &&
                        !(code > 45 && code < 48) &&
                      !(code > 64 && code < 91) && // upper alpha (A-Z)
                      !(code > 96 && code < 123)) { // lower alpha (a-z)
                        e.preventDefault();
                    }
                }

                function Address(e, t) {
                    var code = ('charCode' in e) ? e.charCode : e.keyCode;
                    if ( // space
                       !(code == 32) &&// Special characters
                        !(code == 35) &&
                        !(code == 64) &&
                        !(code > 43 && code < 60) &&
                        !(code > 37 && code < 42) &&
                         !(code == 47) &&
                      !(code > 64 && code < 94) && // upper alpha (A-Z)
                      !(code > 96 && code < 126)) { // lower alpha (a-z)
                        e.preventDefault();
                    }
                }

            </script>
    <style type="text/css">
        .auto-style1 {
            left: -4px;
            top: -5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create">Create Zone
                </h3>
                <h3 class="page-title" runat="server" id="Update">Update Zone
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
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
            <div style="float: right; margin-top: 20px; margin-right: 12px">
                <%--   <asp:Button ID="cmdClose" runat="server" Text="Close" 
                                    CssClass="btn btn-primary" />--%>
            </div>
            <div style="float: right; margin-top: 20px; margin-right: 12px">
                <asp:Button ID="cmdClose" runat="server" Text="Zone View"
                    OnClientClick="javascript:window.location.href='ZoneView.aspx'; return false;"
                    CssClass="btn btn-primary" />
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4 id="CreateZone" runat="server"><i class="icon-reorder"></i>Create Zone</h4>
                        <h4 id="UpdateZone" runat="server"><i class="icon-reorder"></i>Update Zone</h4>
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
                                            <label class="control-label">Zone ID <span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtZoneId" runat="server" onkeypress="javascript:return OnlyNumber(event);" MaxLength="1"></asp:TextBox>
                                                    <asp:TextBox ID="txtZnId" runat="server" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Zone Name <span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtZoneName" runat="server" MaxLength="30" CssClass="auto-style1" onkeypress="return Address(event,this);"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Zone Head<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtFullName" runat="server" MaxLength="40" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Email ID<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="100" onkeypress="javascript:return validateEmail(txtEmailId);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>


                                    <div class="span5">



                                        <div class="control-group">
                                            <label class="control-label">Mobile No<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMobile" runat="server" onkeypress="javascript:return OnlyNumber(event);" MaxLength="10"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Phone<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="12" onkeypress="javascript:return OnlyNumberHyphen(this,event);"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Address<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtZoneAddress" runat="server" MaxLength="300" Height="60px"
                                                        TextMode="MultiLine" Style="resize: none"
                                                        onkeypress="return Address(event,300);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>



                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>

                                <div class="text-center" >
                                

                                
                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                            OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                               
                               
                              
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                
                                 
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                            </div>
                        </div>

                        <div class="space20"></div>



                        <!-- END FORM-->

                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

        <!-- END PAGE CONTENT-->
    </div>
</asp:Content>
