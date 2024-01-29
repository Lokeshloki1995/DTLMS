<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/functions.js"></script>

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

            if (document.getElementById('<%= cmbDesignation.ClientID %>').value.trim() == "--Select--") {
                alert('Select Designation')
                document.getElementById('<%= cmbDesignation.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtRole.ClientID %>').value.trim() == "") {
                alert('Enter the Role')
                document.getElementById('<%= txtRole.ClientID %>').focus()
                return false

            }

            <%--var Role = document.getElementById('<%= txtRole.ClientID %>').value;
            var Rolecon = /^([a-zA-Z]+\s)*[a-zA-Z]+$/;
            if (!Role.match(Rolecon)) {
                alert('Enter valid  Role Name')
                document.getElementById('<%= txtRole.ClientID %>').focus()
                return false
            }--%>


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
                <h3 class="page-title" runat="server" id="Create">Create Role 
                </h3>
                <h3 class="page-title" runat="server" id="Update">Update Role 
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
                <asp:Button ID="cmdClose" runat="server" Text="Role View"
                    OnClientClick="javascript:window.location.href='RoleView.aspx'; return false;"
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
                        <h4 id="CreateRole" runat="server"><i class="icon-reorder"></i>Create Role</h4>
                        <h4 id="UpdateRole" runat="server"><i class="icon-reorder"></i>Update Role</h4>
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
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtRoleId" runat="server" MaxLength="500" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Designation  <span class="Mandotary">*</span> </label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDesignation" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Role Name <span class="Mandotary">*</span></label>

                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtRole" runat="server" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Role Type<span class="Mandotary"> *</span> </label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdRolelevel" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                    </div>

                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>

                                <div class="text-center">

                                    <asp:Button ID="cmdSave" runat="server" Text="Save"
                                        CssClass="btn btn-primary"
                                        OnClientClick="javascript:return ValidateMyForm()" OnClick="cmdSave_Click" />


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
