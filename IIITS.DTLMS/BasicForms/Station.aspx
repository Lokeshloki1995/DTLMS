<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="Station.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.Station" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>
    <script type="text/javascript">

        function Validate() {

            <%--if (document.getElementById('<%= txtStatName.ClientID %>').value.trim() == "") {
                alert('Enter Station Name')
                document.getElementById('<%= txtStatName.ClientID %>').focus()
                return false
            }--%>

            if (document.getElementById('<%= cmbDistrict.ClientID %>').value == "--Select--") {
                alert('Select District')
                document.getElementById('<%= cmbDistrict.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbTalq.ClientID %>').value == "--Select--") {
                alert('Select Taluk')
                document.getElementById('<%= cmbTalq.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbZone.ClientID %>').value == "--Select--") {
                alert('Select Zone')
                document.getElementById('<%= cmbZone.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbCircle.ClientID %>').value == "--Select--") {
                alert('Select Circle')
                document.getElementById('<%= cmbCircle.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbDivision.ClientID %>').value == "--Select--") {
                alert('Select Division')
                document.getElementById('<%= cmbDivision.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbSubDiv.ClientID %>').value == "--Select--") {
                alert('Select SubDivision')
                document.getElementById('<%= cmbSubDiv.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtStationCode.ClientID %>').value.trim() == "") {
                alert('Enter Station Code')
                document.getElementById('<%= txtStationCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "--Select--") {
                alert('Select Voltage Class')
                document.getElementById('<%= cmbCapacity.ClientID %>').focus()
                return false
            }

           
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
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span9">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title" runat="server" id="Create">Create Station
                    </h3>
                    <h3 class="page-title" runat="server" id="Update">Update Station
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-right: 30px; margin-top: 20px" class="span2">
                    <asp:Button ID="cmdNewStation" class="btn btn-primary" Text="Station View" OnClientClick="javascript:window.location.href='StationView.aspx'; return false;"
                        runat="server" />
                </div>
            </div>

        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4 id="CreateStation" runat="server">
                            <i class="icon-reorder"></i>Create Station</h4>
                        <h4 id="UpdateStation" runat="server">
                            <i class="icon-reorder"></i>Update Station</h4>

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
                                            <label class="control-label">
                                                Station Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                   <%-- <asp:TextBox ID="txtStatName" runat="server" onkeypress="return onlyAlphabets(event,this);" MaxLength="100"></asp:TextBox>--%>
                                                   <asp:TextBox ID="txtStatName" runat="server" onkeypress= "javascript:return AllowOnlyAlphanumericNotAllowSpecial(event);" MaxLength="100"></asp:TextBox>
                                                    <asp:TextBox ID="txtStationId" runat="server" MaxLength="100" Visible="false" Width="20px">0</asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                District<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDistrict" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="cmbDistrict_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Taluk<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbTalq" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbTalq_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Zone<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbZone" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged"
                                                       >
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Circle<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle" AutoPostBack="true" runat="server"
                                                        OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDivision" AutoPostBack="true" runat="server"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">SubDiv Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDiv" AutoPostBack="true" runat="server"
                                                        OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                Station Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtStationCode" onkeypress="javascript:return OnlyNumber(event);" runat="server" MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5">

                                        <asp:Button ID="btnPopByID" runat="server" Text="" Style="display: none" />

                                        <div class="control-group">
                                            <label class="control-label">
                                                Voltage Class<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCapacity" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>



                                        <div class="control-group">
                                            <label class="control-label">
                                                Description</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDesc" runat="server" MaxLength="25" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,200);" Style="resize: none"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                MobileNo</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                EmailId</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" onkeypress="javascript:return validateEmail(txtEmailId);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="span1">
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="text-center">
                                       
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return Validate()"
                                                CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                               
                                  
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                OnClick="cmdReset_Click1" /><br />
                                      
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="space20"></div>
                            <!-- END FORM-->



                        </div>
                        <!-- END FORM-->




                        <!-- END SAMPLE FORM PORTLET-->
                    </div>
                </div>
                <!-- END PAGE CONTENT-->
            </div>

        </div>



    </div>
</asp:Content>
