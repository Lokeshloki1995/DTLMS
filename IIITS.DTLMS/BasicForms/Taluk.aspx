<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="Taluk.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.Taluk" %>

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


        function nospaces(val) {
            var value = val;

            if ((/^[\s+]/).test(value)) {

                alert("sorry, you are not allowed to enter any spaces in District Code");
                document.getElementById('<%= txtTalukName.ClientID %>').focus()
            }
        }

        function ValidateMyForm() {

            if (document.getElementById('<%= cmbDistName.ClientID %>').value == "---Select---") {
                alert('Select District Name')
                document.getElementById('<%= cmbDistName.ClientID %>').focus()
                return false

            }

            if (document.getElementById('<%= txtTalukCode.ClientID %>').value == "") {
                alert('Enter the Taluk Code')
                document.getElementById('<%= txtTalukCode.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtTalukName.ClientID %>').value == "") {
                alert('Enter the Taluk Name')
                document.getElementById('<%= txtTalukName.ClientID %>').focus()
                return false
            }

            //var TalukName = document.getElementById('<%= txtTalukName.ClientID %>').value;
           // var TalukNamecon = /^([a-zA-Z0-9])+(\s-\s)*([a-zA-Z0-9])+$/;
           // if (!TalukName.match(TalukNamecon)) {
                //alert('Enter valid  Taluk Name')
                //document.getElementById('<%= txtTalukName.ClientID %>').focus()
               // return false
            //}
        }
      
    </script>

    
            <script language="Javascript" type="text/javascript">


                function onlyAlphabets(e, t) {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create">
                    Create Taluk 
                </h3>
                <h3 class="page-title" runat="server" id="Update">
                    Update Taluk 
                </h3>
                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                        <div class="input-append search-input-area">
                            <input class="" id="appendedInputButton" type="text" />
                            <button class="btn" type="button">
                                <i class="icon-search"></i>
                            </button>
                        </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        <%--    <div style="float: right; margin-top: 20px; margin-right: 12px">
                <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary" /></div>--%>
            <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="cmdClose" runat="server" Text="Taluk View" 
                                      OnClientClick="javascript:window.location.href='TalukView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue" >
                    <div class="widget-title">
                        <h4 id="CreateTaluk" runat="server">
                            Create Taluk </h4>
                         <h4 id="UpdateTaluk" runat="server">
                            Update Taluk </h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>

                                    <div class="span5">
                                        <div class="control-group">
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtTlkId" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                District Name <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDistName" runat="server" AutoPostBack="true"
                                                        onselectedindexchanged="cmbDistName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Taluk Code <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtTalukCode" runat="server" MaxLength="2" onkeypress="javascript:return OnlyNumber(event);" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Taluk Name <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtTalukName" runat="server" MaxLength="40" onkeypress="return onlyAlphabets(event,this);" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="space20">
                                    </div>
                                    <div class="text-center">
                                       
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                                                CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                                   
                                        
                                            <asp:Button ID="cmdClear" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                OnClick="cmdClear_Click" /><br />
                                     
                                       
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                
                                <div class="space20">
                                </div>
                          
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
</asp:Content>
