<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="QCEnumApprove.aspx.cs" Inherits="IIITS.DTLMS.Internal.QCEnumApprove" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="https://cdnjs.cloudflare.com/ajax/libs/fancybox/1.3.4/jquery.fancybox-1.3.4.css" />

    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=btnApproval.ClientID %>').prop('disabled', true);
         }

         window.onbeforeunload = preventMultipleSubmissions;

    </script>

    <style type="text/css">
        a.fancybox img {
            border: none;
            box-shadow: 0 1px 7px rgba(0,0,0,0.6);
            -o-transform: scale(1,1);
            -ms-transform: scale(1,1);
            -moz-transform: scale(1,1);
            -webkit-transform: scale(1,1);
            transform: scale(1,1);
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
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fancybox/1.3.4/jquery.fancybox-1.3.4.pack.min.js"></script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span8">
                    <h3 class="page-title">Enumeration Details
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='QCApproval.aspx'; return false;" />
                    <asp:TextBox ID="txtApproveId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Enumeration Details</h4>
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


                                            <div class="control-group" runat="server" id="dvCircle">
                                                <label class="control-label">Circle Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvDiv">
                                                <label class="control-label">Division Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvSub">
                                                <label class="control-label">Sub Division Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbsubdivision" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbsubdivision_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvSection">
                                                <label class="control-label">Section Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection" runat="server" TabIndex="2" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbsection_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvLocType" style="display: none">
                                                <label class="control-label">Location Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbLocationType" runat="server" AutoPostBack="true"
                                                            TabIndex="2" OnSelectedIndexChanged="cmbLocationType_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvLocName" style="display: none">
                                                <label class="control-label">
                                                    <asp:Label ID="lblRepairerName" runat="server" Text="Location Name"></asp:Label>
                                                    <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbLocationName" runat="server" TabIndex="2" OnSelectedIndexChanged="cmbLocationName_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" id="dvdivision" runat="server" visible="false">
                                                <label class="control-label">Division<span class="Mandotary">*</span>  </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmdDiv" runat="server" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group" runat="server" id="dvLocAddress" style="display: none">
                                                <label class="control-label">
                                                    <asp:Label ID="lblAddress" runat="server" Text="Location Address"></asp:Label>
                                                    <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLocAddress" runat="server" TextMode="MultiLine" MaxLength="100" TabIndex="3"
                                                            Style="resize: none" ReadOnly="true" Height="80px" onkeyup="return ValidateTextlimit(this,100);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:HiddenField ID="hdfDivision" runat="server" />
                                            <asp:HiddenField ID="hdfSubdivision" runat="server" />
                                            <asp:HiddenField ID="hdfSection" runat="server" />
                                            <asp:HiddenField ID="hdfFeeder" runat="server" />
                                            <asp:HiddenField ID="hdfoperator" runat="server" />
                                            <asp:HiddenField ID="hdfLocName" runat="server" />
                                            <asp:HiddenField ID="hdfDTCWithoutDTrFlag" runat="server" />
                                        </div>
                                        <div class="span5">

                                            <div class="control-group" runat="server" id="dvFeeder">
                                                <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeeder" runat="server" TabIndex="3">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Enumeration Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtwelddate" runat="server" MaxLength="50" TabIndex="5"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtwelddate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtwelddate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--                                            <div class="control-group">
                                                <label class="control-label">Operator 1<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmboperator1" runat="server" AutoPostBack="true"
                                                            TabIndex="6" OnSelectedIndexChanged="cmboperator1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Operator 2<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmboperator2" runat="server" TabIndex="7">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <%--<label class="control-label">Tagged DTR  <span class="Mandotary">*</span>  </label>--%>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTaggedDTR" runat="server" Visible="false">
                                                            <asp:ListItem Text="No" Value="0" Selected="True"> </asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>

                                            </div>


                                            <div class="control-group" id="divTaggedId" runat="server" visible="false">
                                                <label class="control-label">DTr Tagged in <span class="Mandotary">*</span>  </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTaggedLocation" runat="server">
                                                            <asp:ListItem Text="--Select--" Value="0" Selected="True"> </asp:ListItem>
                                                            <asp:ListItem Text="Store" Value="1"> </asp:ListItem>
                                                            <asp:ListItem Text="Field" Value="2"> </asp:ListItem>
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
                                                                    <div align="center">
                                                                        <label>Name Plate Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgNamePlate" BorderColor="lightgray" BorderWidth="3px" Height="300px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Code<span id="Mplateno" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10" TabIndex="8" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                                            <asp:TextBox ID="txtEnumDetailsId" runat="server" MaxLength="50" Visible="false" Width="50px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtEnumType" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Make<span id="Mmake" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbMake" runat="server" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="cmbMake_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Capacity <span id="Mcap" runat="server" class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="10">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="dvTransType" style="display: none">
                                                                    <label class="control-label">Transformer Type<span id="Mtc_type" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbTranstype" runat="server" TabIndex="13">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">DTr SLno <span id="Tcslno" runat="server" class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtTcslno" autocomplete="off" runat="server" MaxLength="15" TabIndex="11"></asp:TextBox><br />
                                                                            <asp:CheckBox ID="chkSlnoNotExist" runat="server" Text="Not Exists" CssClass="checkbox" Visible="false" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="span5">

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>DTr Code Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgSSPlate" BorderColor="lightgray" BorderWidth="3px" Height="300px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="Div1" style="display: none">
                                                                    <%--<div class="control-group">--%>
                                                                    <label class="control-label">Manufacture Date<span id="Mmfdate" runat="server" class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="50" TabIndex="12"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtManufactureDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtManufactureDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Tank Capacity(in Litre)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtTankCapacity" runat="server" MaxLength="10" TabIndex="12" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox><br />

                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="control-group" id="dvfieldLocation" runat="server" style="display: none">
                                                                    <label class="control-label">Location Type <span id="Mltyp" runat="server" class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmdloctype" runat="server" TabIndex="2">
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <%--  <div class="control-group">
                                                                    <label class="control-label">Weight Of Transformer(in KG)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtWeight" runat="server" MaxLength="10" TabIndex="12"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox><br />

                                                                        </div>
                                                                    </div>
                                                                </div>--%>

                                                                <div class="control-group">
                                                                    <label class="control-label">Rating <span id="Mrtng" runat="server" class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbRating" runat="server" TabIndex="11"
                                                                                AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" runat="server" id="dvStar" style="display: none">
                                                                    <label class="control-label">Star Rated</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbStarRated" runat="server" TabIndex="11">
                                                                            </asp:DropDownList>
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
                                                                            <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="9" autocomplete="off" TabIndex="14"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>DTC Code (DTLMS) Photo</label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgDTLMS" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>



                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Code (Ip Enum)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtIPDTCCode" runat="server" MaxLength="10" autocomplete="off" TabIndex="16"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>DTC Code (Ip Enum) Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgIPEnum" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <%--                                                                 <div class="control-group">
                                                                    <label class="control-label">Enumeration Date</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtEnumerationdate" runat="server" MaxLength="7" TabIndex="7"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtEnumeration_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtEnumerationdate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>--%>

                                                                <%--                                                                <div class="control-group">
                                                                    <label class="control-label"></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:CheckBox ID="chkIsIPEnum" runat="server" Text="Is IP Enumeration Done" CssClass="checkbox" />
                                                                        </div>
                                                                    </div>
                                                                </div>--%>


                                                                <%-- <div class="control-group">
                                                                    <label class="control-label">DTC Name<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDTCName" runat="server" MaxLength="100" TabIndex="4"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>DTC Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgDTCPhoto" BorderColor="lightgray" BorderWidth="3px" Height="300px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                            </div>
                                                            <div class="span5">
                                                                <div class="control-group">
                                                                    <label class="control-label">TIMS Code(HESC)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtoldDTCCode" runat="server" MaxLength="10" autocomplete="off" TabIndex="15"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>TIMS Code(HESC) Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgOldCode" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <%-- <div class="control-group">
                                                                    <label class="control-label">Infosys Asset ID</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtInfosysAsset" runat="server" MaxLength="6" TabIndex="19"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>Infosys Asset ID Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgInfosys" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Name<span class="Mandotary"> *</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDTCName" runat="server" MaxLength="100" autocomplete="off" TabIndex="4"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <div align="center">
                                                                        <label>DTC Photo </label>
                                                                        <div align="center">
                                                                            <asp:Image ID="imgDTCPhoto" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <%-- <div class="control-group">
                                                                    <label class="control-label">Enumeration Date</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtEnumerationdate" runat="server" MaxLength="7" TabIndex="7"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtEnumeration_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtEnumerationdate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label"></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:CheckBox ID="chkIsIPEnum" runat="server" Text="Is IP Enumeration Done" CssClass="checkbox" />
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
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
                                                            <%--  <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>--%>
                                                        </div>

                                                        <div class="span2">
                                                            <asp:RadioButton ID="rdbDTLMS" runat="server" Text="New Details" CssClass="radio"
                                                                GroupName="a" Checked="true" AutoPostBack="true" />
                                                        </div>

                                                        <%--                                                        <div class="span2" visible="false">
                                                            <asp:RadioButton ID="rdbOldDtc" runat="server" Text="Old DTC Details" CssClass="radio"
                                                                GroupName="a" AutoPostBack="true" />
                                                        </div>

                                                        <div class="span4" visible="false">
                                                            <asp:RadioButton ID="rdbIPEnum" runat="server" Text="IP Enumeration DTC Details"
                                                                CssClass="radio" GroupName="a" AutoPostBack="true" />
                                                        </div>--%>
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
                                                                            <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" autocomplete="off" TabIndex="4"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Connected KW <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtConnectedKW" runat="server" MaxLength="6" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false" TabIndex="5"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Connected HP <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6" autocomplete="off"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"  onpaste="return false" TabIndex="6"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            

                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Commission Date <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtCommisionDate" runat="server" TabIndex="24" autocomplete="off" MaxLength="10"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtCommisionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtCommisionDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">DTr Commission Date <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDtrcommissiondate" runat="server" autocomplete="off" TabIndex="24" MaxLength="10"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtDtrcommissiondate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Last Service Date</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtServiceDate" runat="server" TabIndex="23" MaxLength="10"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtServiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtServiceDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Platform Type <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbPlatformType" runat="server"
                                                                                TabIndex="20">
                                                                                <%--                                                                                <asp:ListItem Text="--select--" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="Single Pole" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Double Pole" Value="2"></asp:ListItem>--%>
                                                                            </asp:DropDownList>


                                                                            <asp:TextBox ID="txtDTCId" runat="server" TabIndex="15" MaxLength="10" Visible="false"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Breaker Type <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbBreakerType" runat="server"
                                                                                TabIndex="9">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="GOS" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="DOLO" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="NONE" Value="3"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>



                                                                <div class="control-group">
                                                                    <label class="control-label">DTC Meters Available <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbDTCMetered" runat="server"
                                                                                TabIndex="10" AutoPostBack="true" OnSelectedIndexChanged="cmbDTCMeters_SelectedIndexChanged">
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
                                                                                onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false" TabIndex="7"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">HT Protection <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbHTProtection" runat="server"
                                                                                TabIndex="11">
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
                                                                    <label class="control-label">LT Protection <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbLTProtection" runat="server"
                                                                                TabIndex="12">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Grounding <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbGrounding" runat="server"
                                                                                TabIndex="13">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">Lightning Arresters <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbLightArrester" runat="server" TabIndex="14">
                                                                                <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Load Type <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbLoadtype" runat="server" TabIndex="14">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Project/Scheme Type <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:DropDownList ID="cmbProjecttype" runat="server" TabIndex="14">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">LT Line Length (in KM)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtltLine" runat="server" TabIndex="16" MaxLength="7" autocomplete="off" 
                                                                                 onpaste="return false"  onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                  <div class="control-group">
                                                                    <label class="control-label">HT Line Length (in KM)</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtHtLine" runat="server" TabIndex="49" MaxLength="7" autocomplete="off"
                                                                                 onpaste="return false" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group">
                                                                    <label class="control-label">Depreciation</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtDepreciation" runat="server" TabIndex="16" MaxLength="10" autocomplete="off"
                                                                                 onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Latitude</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtLatitude" runat="server" TabIndex="16" MaxLength="10" autocomplete="off" 
                                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group">
                                                                    <label class="control-label">Longitude</label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtLongitude" runat="server" TabIndex="16" MaxLength="10" autocomplete="off"
                                                                                 onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
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

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Comments for Approve/Pending for Clarification/Reject</h4>
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
                                                <label class="control-label">Comments<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemark" runat="server" MaxLength="500" TabIndex="4" TextMode="MultiLine"
                                                            Width="550px" Height="125px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,500)"></asp:TextBox>
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


            <div class="widget-body">

                <div class="form-horizontal" align="center">
                    <div class="span3"></div>
                    <%--<div class="span1">
                        <asp:Button ID="btnApproval" runat="server" Text="Approval" CssClass="btn btn-primary"
                            TabIndex="20" OnClick="btnApproval_Click" />
                    </div>--%>

                    <%--<div class="span1">
                        <asp:Button ID="tempButton" runat="server" Text="Temp Button"  CssClass="btn btn-primary" 
                            TabIndex="20" OnClick="tempButton_Click"       />
                    </div>   --%>

                    <div class="span2">
                        <%--<asp:Button ID="btnPending" runat="server" Text="Pending for Clarification" CssClass="btn btn-primary"
                            TabIndex="20" OnClick="btnPending_Click" Visible="false" />--%>
                    </div>
                    <div class="span1">
                        <asp:Button ID="btnApproval" runat="server" Text="Approval" CssClass="btn btn-primary" onchange="javascript:preventMultipleSubmissions();"
                            TabIndex="20" OnClick="btnApproval_Click" />
                    </div>
                    <div class="span1">
                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-primary"
                            TabIndex="20"
                            OnClick="btnReject_Click" />
                    </div>

                    <div class="span1">
                        <asp:Button ID="cmdNextDetails" runat="server" Text="Next" CssClass="btn btn-primary"
                            TabIndex="20" OnClick="cmdNextDetails_Click" Visible="false" />
                    </div>

                    <%-- <div class="span2">  
                    <asp:Button ID="btnReset" runat="server" Text="Close" 
                    CssClass="btn btn-primary"  TabIndex="21" Width="105px" onclick="btnReset_Click" 
                         /><br />
                </div>--%>
                    <div class="span7"></div>
                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                </div>


            </div>


        </div>
    </div>
</asp:Content>
