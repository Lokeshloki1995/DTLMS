<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="LECCreate.aspx.cs" Inherits="IIITS.DTLMS.POFlow.LECCreate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .input-append input{
  position: initial!important;
        }
      .form-horizontal .control-label{
          padding-top:0px!important;
      }
    </style>
    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;

    </script>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function onlyAlphabets(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (
              !(code == 32) &&
                 !(code == 47) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }


        function Alphanumeric(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (
              !(code == 32) &&
              !(code == 45) &&
              !(code > 46 && code < 58) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }
        function LoginName(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (
              !(code == 95) &&
                !(code > 47 && code < 58) &&
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
        $(document).ready(function () {
            $('textarea').attr('maxlength', '150');
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
        function ValidateMyForm() {
            debugger;
            if (document.getElementById('<%= txtContractorname.ClientID %>').value.trim() == "") {
                alert('Please Enter Name of the Contractor/Firm.')
                document.getElementById('<%=txtContractorname.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtLicenceNumber.ClientID %>').value.trim() == "") {
                alert('Please Enter Licence Number.')
                document.getElementById('<%=txtLicenceNumber.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtLecRegisterdate.ClientID %>').value.trim() == "") {
                alert('Please Select Licence Registration Date.')
                document.getElementById('<%=txtLecRegisterdate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtLecValidupto.ClientID %>').value.trim() == "") {
                alert('Please Select Licence Valid Upto.')
                document.getElementById('<%=txtLecValidupto.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtGstnumber.ClientID %>').value.trim() == "") {
                alert('Please Enter GST Number.')
                document.getElementById('<%=txtGstnumber.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtContactnumber.ClientID %>').value.trim() == "") {
                alert('Please Enter Contact Number.')
                document.getElementById('<%=txtContactnumber.ClientID %>').focus()
                return false
            }
            var EmailId = document.getElementById('<%= txtEmailId.ClientID %>').value;
            var EmailIdcon = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
            if (!EmailId.match(EmailIdcon)) {
                alert('Please Enter Valid E-Mail ID')
                document.getElementById('<%= txtEmailId.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtAddress.ClientID %>').value.trim() == "") {
                alert('Please Enter Office Address.')
                document.getElementById('<%=txtAddress.ClientID %>').focus()
                return false
            }

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title" runat="server" id="Create">Licenced Electrical Contractor Creation                                    
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
                    <asp:Button ID="Button1" runat="server" Text="LEC View"
                        CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='LECView.aspx'; return false;" />
                    <%----%>
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateLEC" runat="server"><i class="icon-reorder"></i>Licenced Electrical Contractor Creation </h4>
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
                                                <label class="control-label">Name of the Contractor/Firm<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdnlecid" runat="server" />

                                                        <asp:TextBox ID="txtContractorname" runat="server" AutoComplete="off" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Licence Number<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLicenceNumber" runat="server" AutoComplete="off"
                                                            MaxLength="20" onkeypress="javascript:return AlphanumericLEC(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Licence Registration Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLecRegisterdate" runat="server" MaxLength="11"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="LecRegisterCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtLecRegisterdate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Licence Valid Upto<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtLecValidupto" runat="server" MaxLength="11"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="ValiduptoCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtLecValidupto" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div id="divcircle" runat="server">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        GST Number <span class="Mandotary">*</span>
                                                    </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtGstnumber" runat="server" AutoComplete="off" oninput="this.value = this.value.toUpperCase()"
                                                                MaxLength="15"
                                                                onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Contact Number <span class="Mandotary">*</span>
                                                    </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtContactnumber" runat="server" AutoComplete="off"
                                                                MaxLength="10"  onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                        E-Mail ID <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtEmailId" AutoComplete="off" runat="server" MaxLength="40" onkeypress="javascript:return validateEmail(txtEmailId);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Office Address <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="150" onkeypress="return Address(event,this);" Style="resize: none" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>
                                <div class="text-center">
                                    <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click"
                                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        OnClick="cmdReset_Click" CssClass="btn btn-danger" /><br />
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
    </div>
</asp:Content>
