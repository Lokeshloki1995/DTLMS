<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FieldEnumeration.aspx.cs" Inherits="IIITS.DTLMS.Internal.FieldEnumeration" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="https://cdnjs.cloudflare.com/ajax/libs/fancybox/1.3.4/jquery.fancybox-1.3.4.css" />
    <style type="text/css">
        a.fancybox img {
            border: none;
            box-shadow: 0 1px 7px rgba(0,0,0,0.6);
            -o-transform: scale(1,1);
            -ms-transform: scale(1,1);
            -moz-transform: scale(1,1);
            -webkit-transform: scale(1,1);
            dib transform: scale(1,1);
            -o-transition: all 0.2s ease-in-out;
            -ms-transition: all 0.2s ease-in-out;
            -moz-transition: all 0.2s ease-in-out;
            -webkit-transition: all 0.2s ease-in-out;
            transition: all 0.2s ease-in-out;
        }

        a.fancybox:hover img {
            position: relative;
            z-index: 999;
            -o-transform: scale(1.03,1.03);
            -ms-transform: scale(1.03,1.03);
            -moz-transform: scale(1.03,1.03);
            -webkit-transform: scale(1.03,1.03);
            transform: scale(1.03,1.03);
        }
    </style>

<%--    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.0.min.js"></script>--%>
    <script type="text/javascript" src="../js/Jquery.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fancybox/1.3.4/jquery.fancybox-1.3.4.pack.min.js"></script>
    // To display in the same page 
    <script type="text/javascript">
        $(function ($) {
            var addToAll = false;
            var gallery = true;
            var titlePosition = 'inside';
            $(addToAll ? 'img' : 'img.fancybox').each(function () {
                var $this = $(this);
                var title = $this.attr('title');
                var src = $this.attr('data-big') || $this.attr('src');
                var a = $('<a href="#" class="fancybox"></a>').attr('href', src).attr('title', title);
                $this.wrap(a);
            });
            if (gallery)
                $('a.fancybox').attr('rel', 'fancyboxgallery');
            $('a.fancybox').fancybox({
                titlePosition: titlePosition
            });
        });
        $.noConflict();

    </script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">



        function ValidateMyForm() {

            if (document.getElementById('<%= cmbCircle.ClientID %>').value == "--Select--") {
                alert('Select Circle')
                document.getElementById('<%= cmbCircle.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbDivision.ClientID %>').value.trim() == "--Select--") {
                alert('Select Division')
                document.getElementById('<%= cmbDivision.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbsubdivision.ClientID %>').value.trim() == "--Select--") {
                alert('Select Sub Division')
                document.getElementById('<%= cmbsubdivision.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbSection.ClientID %>').value.trim() == "--Select--") {
                alert('Select Section')
                document.getElementById('<%= cmbSection.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbFeeder.ClientID %>').value.trim() == "--Select--") {
                alert('Select Feeder')
                document.getElementById('<%= cmbFeeder.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtwelddate.ClientID %>').value.trim() == "") {
                alert('Enter Enumeration Date')
                document.getElementById('<%= txtwelddate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbRework.ClientID %>').value.trim() == "--Select--") {
                alert('Select the Rework Type')
                document.getElementById('<%= cmbRework.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                alert('Enter DTr CODE')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                alert('Enter 9 digit DTC Code')
                document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDTCName.ClientID %>').value.trim() == "") {
                alert('Enter DTC Name')
                document.getElementById('<%= txtDTCName.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtConnectedHP.ClientID %>').value.trim() == "") {
                alert('Enter Total Connected HP')
                document.getElementById('<%= txtConnectedHP.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtConnectedKW.ClientID %>').value.trim() == "") {
                alert('Enter Total Connected Kw')
                document.getElementById('<%= txtConnectedKW.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbPlatformType.ClientID %>').value.trim() == "--Select--") {
                alert('Select Platform Type')
                document.getElementById('<%= cmbPlatformType.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbBreakerType.ClientID %>').value.trim() == "--Select--") {
                alert('Select Breaker Type')
                document.getElementById('<%= cmbBreakerType.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbDTCMetered.ClientID %>').value.trim() == "--Select--") {
                alert('Select DTC Meter')
                document.getElementById('<%= cmbDTCMetered.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbHTProtection.ClientID %>').value.trim() == "--Select--") {
                alert('Select HT Protection')
                document.getElementById('<%= cmbHTProtection.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbLTProtection.ClientID %>').value.trim() == "--Select--") {
                alert('Select LT Protection')
                document.getElementById('<%= cmbLTProtection.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbGrounding.ClientID %>').value.trim() == "--Select--") {
                alert('Select Grounding')
                document.getElementById('<%= cmbGrounding.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbLightArrester.ClientID %>').value.trim() == "--Select--") {
                alert('Select Lighting Arrestors')
                document.getElementById('<%= cmbLightArrester.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbLoadtype.ClientID %>').value.trim() == "--Select--") {
                alert('Select Load Type')
                document.getElementById('<%= cmbLoadtype.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbProjecttype.ClientID %>').value.trim() == "--Select--") {
                alert('Select Project Type')
                document.getElementById('<%= cmbProjecttype.ClientID %>').focus()
                return false
            }

            var FromdateInput = document.getElementById('<%= txtwelddate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;

            if (FromdateInput != "") {
                if (!FromdateInput.match(goodDate)) {
                    alert("Please Enter Valid Enumeration Date ");
                    document.getElementById('<%= txtwelddate.ClientID %>').focus()
                    return false;
                }
            }

            var LTInput = document.getElementById('<%= txtltLine.ClientID %>').value;
            var decimalvalue = /^[0-9]*\.[0-9]{2}$ or ^[0-9]*\.[0-9][0-9]$/;
            if (LTInput != "") {
                if (!LTInput.match(decimalvalue)) {
                    alert("Please Enter Valid LT line Length (eg: 123.123) ");
                    document.getElementById('<%= txtltLine.ClientID %>').focus()
                    return false;
                }
            }

             var HTInput = document.getElementById('<%= txtHtLine.ClientID %>').value;
            var decimalvalue = /^[0-9]{0,3}.?[0-9]{0,3}$/;
            if (HTInput != "") {
                if (!HTInput.match(decimalvalue)) {
                    alert("Please Enter Valid HT line Length (eg: 123.123) ");
                    document.getElementById('<%= txtHtLine.ClientID %>').focus()
                    return false;
                }

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span12">
                    <h3 class="page-title">Field Enumeration
                        <asp:Button ID="Button1" Style="float: right;" runat="server" Text="Close"
                            CssClass="btn btn-primary" OnClick="cmdClose_Click" />
                        <asp:Button ID="Button2" Style="float: right; margin-right: 54px; display:none"
                            runat="server" Text="New DTC Without TC" OnClientClick="javascript:window.location.href='NewDtcCommission.aspx'; return false;"
                            CssClass="btn btn-primary" />
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
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Field Enumeration</h4>
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
                                                <label class="control-label">Circle Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtStatus" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtfeederbifurcation" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true"
                                                            TabIndex="2" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Sub Division Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbsubdivision" runat="server" AutoPostBack="true"
                                                            TabIndex="3" OnSelectedIndexChanged="cmbsubdivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtEnumDetailsId" runat="server" MaxLength="50" Visible="false" Width="50px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Section Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection" runat="server" TabIndex="4" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--<asp:DropDownList ID="cmbFeeder" runat="server" TabIndex="5"
                                                            AutoPostBack="true" OnSelectedIndexChanged="cmbFeeder_SelectedIndexChanged">
                                                        </asp:DropDownList>--%>

                                                        <asp:DropDownList ID="cmbFeeder" runat="server" TabIndex="5"
                                                            AutoPostBack="true" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Enumeration Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtwelddate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                       
                                                        <asp:HiddenField ID="hdfDivision" runat="server" />
                                                        <asp:HiddenField ID="hdfSubdivision" runat="server" />
                                                        <asp:HiddenField ID="hdfSection" runat="server" />
                                                        <asp:HiddenField ID="hdfFeeder" runat="server" />
                                                        <asp:HiddenField ID="hdfDTCWithoutDTrFlag" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="space20"></div>
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group" visible="false">
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRework" runat="server" TabIndex="10" Visible="false">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" visible="false">
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTaggedDTR" runat="server" Visible="false">
                                                            <asp:ListItem Text="No" Value="0" Selected="True"> </asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="2"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Transformer / DTC Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="bs-docs-example">
                                <ul class="nav nav-tabs" id="myTab">
                                    <li class="active" runat="server" id="liTCDetails"><a data-toggle="tab" href="#ContentPlaceHolder1_TCDetails">Transformer Details</a></li>
                                    <li runat="server" id="liDTCDetails"><a data-toggle="tab" href="#ContentPlaceHolder1_DTCDetails">DTC Details</a></li>
                                    <li runat="server" id="liOtherDetails"><a data-toggle="tab" href="#ContentPlaceHolder1_otherDetails">DTC Other Details</a></li>
                                </ul>
                                <asp:Label ID="lblNote" runat="server" ForeColor="blue" Visible="false"></asp:Label>
                                <div class="tab-content" id="myTabContent">

                                    <!--STARTS FIRST TAB -->
                                    <div id="TCDetails" class="tab-pane fade in active" runat="server">
                                        <div class="row-fluid">

                                            <div class="widget-body">
                                                <div class="widget-body form">
                                                    <!-- BEGIN FORM-->
                                                    <div class="form-horizontal">
                                                        <div class="row-fluid">
                                                            <div class="span5">
                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Code<span id="Mplatenum" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtTcCode" runat="server" MaxLength="8" TabIndex="9" ReadOnly="true" 
                                                                                onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Make<span id="Mdtrmake" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbMake" runat="server" TabIndex="10"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="cmbMake_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Capacity<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbCapacity" runat="server" AutoPostBack="true" TabIndex="11" OnSelectedIndexChanged="cmbCapacity_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTr SLno<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtTcslno" runat="server" MaxLength="15" autocomplete="off" TabIndex="12"></asp:TextBox><br />

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="Div1" style="display: none">
                                                                    <label class="control-label">Manufacture Date<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="10"
                                                                                TabIndex="13"></asp:TextBox>
                                                                            <ajax:CalendarExtender ID="txtManufactureDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtManufactureDate" Format="dd/MM/yyyy">
                                                                            </ajax:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="space10"></div>
                                                                <div class="space10"></div>
                                                                <div class="control-group" runat="server" id="dvNamePlate" style="display: none">
                                                                    <div align="center">
                                                                        <label>Name Plate Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgNamePlate" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="span5">
                                                                <div class="control-group">
                                                                    <label class="control-label">Tank Capacity(in Litre)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtTankCapacity" runat="server" MaxLength="10" TabIndex="14" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox><br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">Location Type<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmdloctype" runat="server" TabIndex="2">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">Rating<span id="Mrating" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbRating" runat="server" TabIndex="16"
                                                                                AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group" runat="server" id="dvStar" style="display: none">
                                                                    <label class="control-label">Star Rated</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbStarRated" runat="server" TabIndex="17">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">Name Plate Photo<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupNamePlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                TabIndex="18" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Code Photo<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupSSPlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                TabIndex="19" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group" runat="server" id="dvSSPlate" style="display: none">
                                                                    <div align="center">
                                                                        <label>DTr Code Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgSSPlate" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="space10"></div>
                                        </div>
                                    </div>
                                    <!--END FIRST TAB-->

                                    <!--STARTS SECOND TAB -->
                                    <div id="DTCDetails" class="tab-pane fade" runat="server">
                                        <div class="row-fluid">
                                            <div class="widget-body">
                                                <div class="widget-body form">
                                                    <!-- BEGIN FORM-->
                                                    <div class="form-horizontal">
                                                        <div class="row-fluid">
                                                            <div class="span5">
                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Code (DTLMS)<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="8" TabIndex="17" ReadOnly="true"></asp:TextBox>
                                                                            <asp:TextBox ID="txtNamePlatePhotoPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="txtSSPlatePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="txtDTLMSDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="txtOLDDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="txtIPDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="txtInfosysPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="txtDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Code (Ip Enum)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtIPDTCCode" runat="server" MaxLength="6" autocomplete="off" TabIndex="20"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">TIMS Code(HESCOM)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <%--<asp:TextBox ID="txtoldDTCCode" runat="server" MaxLength="12" TabIndex="21" AutoPostBack="true"
                                                                                OnTextChanged="txtoldDTCCode_TextChanged"></asp:TextBox>--%>

                                                                            <asp:TextBox ID="txtoldDTCCode" runat="server" MaxLength="12" TabIndex="21" AutoPostBack="true"
                                                                                ></asp:TextBox>
                                                                            <asp:TextBox ID="txtInfosysAsset" runat="server" MaxLength="12" TabIndex="22" onkeypress="javascript:return OnlyNumber(event);" Visible="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Name<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50"  autocomplete="off" TabIndex="23"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="dvDTLMSPhoto" style="display: none">
                                                                    <div align="center">
                                                                        <label>DTC Code (DTLMS) Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgDTLMS" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group" runat="server" id="dvIPEnumPhoto" style="display: none">
                                                                    <div align="center">
                                                                        <label>DTC Code (Ip Enum) Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgIPEnum" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="span5">

                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Code (DTLMS) Photo<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupDTLMSCodePhoto" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                TabIndex="26" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Code (Ip Enum) Photo</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupIPEnum" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                TabIndex="27" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">TIMS Code(HESCOM) Photo </label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupOldCodePhoto" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                TabIndex="28" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Photo<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupDTCPhoto" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                TabIndex="30" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="space10"></div>
                                                                <div class="space10"></div>
                                                                <div class="space10"></div>

                                                                <div class="control-group" runat="server" id="dvInfosys" style="display: none">
                                                                    <div align="center">
                                                                        <label>Infosys Asset ID Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgInfosys" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="dvOldDTCBESCOM" style="display: none">
                                                                    <div align="center">
                                                                        <label>TIMS Code(HESCOM) Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgOldCode" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="dvDTCPhoto" style="display: none">
                                                                    <div align="center">
                                                                        <label>DTC Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgDTCPhoto" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="space20"></div>
                                    </div>
                                    <!--END SECOND TAB-->

                                    <!--STARTS THIRD TAB -->
                                    <div id="otherDetails" class="tab-pane fade" runat="server">
                                        <div class="row-fluid">

                                            <div class="widget-body">
                                                <div class="form-horizontal">
                                                    <div class="row-fluid">
                                                        <div class="span2">
                                                        </div>

                                                        <div class="span2">
                                                            <asp:RadioButton ID="rdbDTLMS" runat="server" Text="New Details" CssClass="radio"
                                                                GroupName="a" Checked="true" AutoPostBack="true"
                                                                OnCheckedChanged="rdbDTLMS_CheckedChanged" TabIndex="31" />
                                                        </div>

                                                        <div class="span2">
                                                            <%--<asp:RadioButton ID="rdbOldDtc" runat="server" Text="Old DTC Details" CssClass="radio"
                                                                GroupName="a" AutoPostBack="true" OnCheckedChanged="rdbOldDtc_CheckedChanged" Visible="false"
                                                                TabIndex="32" />--%>
                                                        </div>

                                                        <div class="span4">
                                                            <%--<asp:RadioButton ID="rdbIPEnum" runat="server" Text="IP Enumeration DTC Details"
                                                                CssClass="radio" GroupName="a" AutoPostBack="true"
                                                                OnCheckedChanged="rdbIPEnum_CheckedChanged" TabIndex="33" Visible="false" />--%>
                                                        </div>



                                                    </div>
                                                </div>
                                            </div>

                                            <div class="widget-body">
                                                <div class="widget-body form">



                                                    <!-- BEGIN FORM-->
                                                    <div class="form-horizontal">
                                                        <div class="row-fluid">
                                                            <div class="span1"></div>
                                                            <div class="span5">


                                                                <div class="control-group">
                                                                    <label class="control-label">Internal Code</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" autocomplete="off" TabIndex="34"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Connected KW<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtConnectedKW" runat="server" MaxLength="6" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false" TabIndex="35"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>  

                                                                <div class="control-group">
                                                                    <label class="control-label">Connected HP<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false" TabIndex="36"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Commission Date<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtCommisionDate" runat="server" TabIndex="38" MaxLength="10"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtCommisionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtCommisionDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Commission Date<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDtrCommissionDate" runat="server" TabIndex="38" MaxLength="10"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtDtrCommissionDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Last Service Date</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtServiceDate" runat="server" TabIndex="39" MaxLength="10"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtServiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtServiceDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Platform Type<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbPlatformType" runat="server"
                                                                                TabIndex="40">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtDTCId" runat="server" TabIndex="15" MaxLength="10" Visible="false"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Breaker Type<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbBreakerType" runat="server"
                                                                                TabIndex="41">
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>



                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Meters Available<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbDTCMetered" runat="server"
                                                                                TabIndex="42" AutoPostBack="true" OnSelectedIndexChanged="cmbDTCMeters_SelectedIndexChanged">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group"  runat="server" id="idtxtKWHReading">
                                                                    <label class="control-label">KWH Reading <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append"> 
                                                                            <asp:TextBox ID="txtKWHReading" runat="server" MaxLength="11" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false" TabIndex="37"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">HT Protection<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbHTProtection" runat="server"
                                                                                TabIndex="43">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="span5">

                                                                <div class="control-group">
                                                                    <label class="control-label">LT Protection<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbLTProtection" runat="server"
                                                                                TabIndex="44">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Grounding<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbGrounding" runat="server"
                                                                                TabIndex="45">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">Lightning Arresters<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbLightArrester" runat="server" TabIndex="46">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Load Type<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbLoadtype" runat="server" TabIndex="47">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Project/Scheme Type<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbProjecttype" runat="server" TabIndex="48">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">LT Line Length (in Km)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtltLine" runat="server" TabIndex="49" MaxLength="7"  placeholder="Eg: 111.111" autocomplete="off"
                                                                                onpaste="return false"  onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">HT Line Length (in Km)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtHtLine" runat="server" TabIndex="49" MaxLength="7" placeholder="Eg: 111.111" autocomplete="off"
                                                                                onpaste="return false" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Depreciation</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDepreciation" runat="server" TabIndex="50" MaxLength="10" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Latitude</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtLatitude" runat="server" TabIndex="51" MaxLength="10" ReadOnly="true" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Longitude</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtLongitude" runat="server" TabIndex="52" MaxLength="10" ReadOnly="true" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>





                                                                <div class="control-group" runat="server" id="dvDTCPhoto1" style="display: none">
                                                                    <div align="center">
                                                                        <label>DTC Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgDTCPhoto1" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                            </div>

                                                        </div>
                                                        <div class="span1"></div>
                                                    </div>
                                                    <div class="space20"></div>


                                                </div>
                                            </div>

                                            <div class="space10"></div>


                                        </div>
                                    </div>
                                    <!--END THIRD TAB-->


                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>



            <div class="widget-body">

                <div class="text-center">


                    <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary"
                        TabIndex="53" Width="145px" OnClick="cmdSave_Click" OnClientClick="javascript:return ValidateMyForm()" />

                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                        CssClass="btn btn-danger" TabIndex="54" Width="105px"
                        OnClick="cmdReset_Click" />
                    <%-- <div class="span1">  
                    <asp:Button ID="cmdTempButton" runat="server" Text="Temp Button" 
                    CssClass="btn btn-primary"  TabIndex="54" Width="105px" OnClick="cmdTempButton_Click" 
                           /><br />
                </div>--%>


                    <asp:Button ID="cmdLoadImage" runat="server" Text="Load Image" CssClass="btn btn-primary"
                        TabIndex="53" Width="105px" OnClick="cmdLoadImage_Click" Visible="false" />

                    <%--<asp:Button ID="cmdDelete" runat="server" Text="Delete" CssClass="btn btn-primary"
                        TabIndex="53" Width="105px" OnClick="cmdDelete_Click" Visible="false" />--%>



                    <asp:Button ID="cmdReject" runat="server" Text="Reject" Visible="false"
                        CssClass="btn btn-primary" TabIndex="54" Width="105px" OnClick="cmdReject_Click" />



                    <%--<asp:Button ID="cmdNext" runat="server" Text="Next  >>" Visible="false"
                        CssClass="btn btn-primary" TabIndex="55" Width="105px" OnClick="cmdNext_Click" />--%>


                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <div class="widget-body">
                <div class="widget-body form">
                    <asp:GridView ID="grdFieldEnumDetails" CssClass="table table-striped table-bordered table-advance table-hover"
                        AllowPaging="true" PageSize="10" runat="server" AutoGenerateColumns="false"
                        OnPageIndexChanging="grdFieldEnumDetails_PageIndexChanging"
                        OnRowCommand="grdFieldEnumDetails_RowCommand">
                        <Columns>

                            <asp:TemplateField AccessibleHeaderText="ED_ID" HeaderText="Enumeration Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEnumId" runat="server" Text='<%# Bind("ED_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Section Code" HeaderText="Section Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblOffcode" runat="server" Text='<%# Bind("ED_OFFICECODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Feeder Code" HeaderText="Feeder Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblfeedercode" runat="server" Text='<%# Bind("ED_FEEDERCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Welding Date" HeaderText="Date of Fixing">
                                <ItemTemplate>
                                    <asp:Label ID="lblWelddate" runat="server" Text='<%# Bind("ED_WELD_DATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Plate Number" HeaderText="Plate Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DTE_TC_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Make" HeaderText="Make Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Capacity" HeaderText="Capacity">
                                <ItemTemplate>
                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("DTE_CAPACITY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DTC Code" HeaderText="DTC Code(DTLMS)">
                                <ItemTemplate>
                                    <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("DTE_DTCCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DTC Code" HeaderText="DTC Code" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCescDTCCode" runat="server" Text='<%# Bind("DTE_CESCCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DTC Code" HeaderText="DTC Code" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIpDTCCode" runat="server" Text='<%# Bind("DTE_IPCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" Visible="false">
                                <ItemTemplate>
                                    <center>
                                        <%--  <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" 
                                                Width="12px" CommandName="Modify" OnClientClick="return confirm ('Are you sure, you want to Edit the Details');" />--%>
                                        <asp:ImageButton ID="img" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" CommandName="remove"
                                            Width="12px" OnClientClick="return confirm ('Are you sure, you want to Remove');" Visible="false" />
                                    </center>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="lblHead" runat="server" Text="Remove"></asp:Label>
                                    </center>
                                </HeaderTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    <div class="space20"></div>
                    <div class="space20"></div>


                    <div class="space5"></div>
                </div>
            </div>
            <%--  <div class="row-fluid" id="">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Details</h4>
                        <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                
                                           

                </div>
            </div>         
        </div>--%>
        </div>
    </div>
</asp:Content>
