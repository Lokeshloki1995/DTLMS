<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PMCInvoiceCreate.aspx.cs" Inherits="IIITS.DTLMS.POFlow.PMCInvoiceCreate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="screen" href="https://cdnjs.cloudflare.com/ajax/libs/fancybox/1.3.4/jquery.fancybox-1.3.4.css" />
    <style type="text/css">
        a.fancybox img {
            border: none;
            box-shadow: 0 1px 7px rgba(0,0,0,0.6);
            -o-transform: scale(1,1);
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

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../js/Jquery.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fancybox/1.3.4/jquery.fancybox-1.3.4.pack.min.js"></script>


    <script type="text/javascript">
        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }
        window.onbeforeunload = preventMultipleSubmissions;


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

        function AllowNumberAndSecialChe(field, evt) {
            debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            var keychar = String.fromCharCode(charCode);
            var input = field.value;

            //if (keychar[0] === "0" && field.value == '') {
            //    return false;
            //}

            if ((charCode > 47 && charCode < 58) || (keychar === "/" || keychar === "-" || keychar === "_")) {
                return true;
            }
            else {
                return false;
            }
        }

        function ValidateMyForm() {
            if (document.getElementById('<%= txtInvoiceno.ClientID %>').value.trim() == "") {
                alert('Please Enter Invoice No')
                document.getElementById('<%= txtInvoiceno.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtInvoiceDate.ClientID %>').value.trim() == "") {
                alert('Please Select Invoice Date')
                document.getElementById('<%= txtInvoiceDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtRemarkComments.ClientID %>').value.trim() == "") {
                alert('Please Enter Remarks')
                document.getElementById('<%= txtRemarkComments.ClientID %>').focus()
                return false
            }

        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create"></h3>


                <div style="float: right !important; margin-top: 20px !important;">
                    <div class="span2">
                        <asp:Button ID="cmdClose" type="button" runat="server" Text="Close" CssClass="btn btn-success" OnClick="cmdClose_Click" Style="float: left!important" />

                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                        <asp:HiddenField ID="hdfinvoiceId" runat="server" />
                        <asp:HiddenField ID="hdfTcOilTypeId" runat="server" />
                        <asp:HiddenField ID="HdnInvoiceid" runat="server" />


                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Width="20px" Visible="false"></asp:TextBox>
                    </div>
                </div>

                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="PmcInvoiceCreate.aspx" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text" />
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>
                                </button>
                            </div>
                        </form>
                    </li>
                </ul>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Location Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
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
                                                <label class="control-label">Circle Name<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true"
                                                            TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged" ReadOnly="true">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtStatus" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtfeederbifurcation" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division Name<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" ReadOnly="true"
                                                            TabIndex="2" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Sub Division Name<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbsubdivision" runat="server" AutoPostBack="true" ReadOnly="true"
                                                            TabIndex="3" OnSelectedIndexChanged="cmbsubdivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtEnumDetailsId" runat="server" MaxLength="50" Visible="false" Width="50px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Section Name<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection" runat="server" TabIndex="4" AutoPostBack="true" ReadOnly="true"
                                                            OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Feeder Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeeder" runat="server" TabIndex="5" ReadOnly="true"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Enumeration Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtwelddate" runat="server" MaxLength="10" TabIndex="6" ReadOnly="true"></asp:TextBox>

                                                        <asp:HiddenField ID="hdfDivision" runat="server" />
                                                        <asp:HiddenField ID="hdfSubdivision" runat="server" />
                                                        <asp:HiddenField ID="hdfSection" runat="server" />
                                                        <asp:HiddenField ID="hdfFeeder" runat="server" />
                                                        <asp:HiddenField ID="hdfDTCWithoutDTrFlag" runat="server" />
                                                        <asp:HiddenField ID="hdffdid" runat="server" />
                                                        <asp:HiddenField ID="hdfindentoffcode" runat="server" />
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
                                                                        <div class="control-group" runat="server" id="dvNamePlate">
                                                                            <div align="center">
                                                                                <label>Name Plate Photo</label>
                                                                                <div align="center">
                                                                                    <asp:Image ID="imgNamePlate" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                        runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <label class="control-label">DTr Code<span id="Mplatenum" runat="server"> </span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtTcCode" runat="server" MaxLength="7" TabIndex="9" ReadOnly="true"
                                                                                        onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="control-group">
                                                                            <label class="control-label">DTr Make<span id="Mdtrmake" runat="server"> </span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbMake" runat="server" TabIndex="10" ReadOnly="true"
                                                                                        AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>



                                                                        <div class="control-group">
                                                                            <label class="control-label">Capacity</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbCapacity" runat="server" AutoPostBack="true" TabIndex="11" ReadOnly="true">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">DTr SLno</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtTcslno" runat="server" ReadOnly="true" MaxLength="15" TabIndex="12"></asp:TextBox><br />

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Life Span</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtlifespan" runat="server" MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Oil Type</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtOiltype" runat="server" MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">DTr Weight</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txttcweight" runat="server" MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div class="span5">
                                                                        <div class="control-group" runat="server" id="dvSSPlate">

                                                                            <div align="center">
                                                                                <label>DTr Code Photo </label>
                                                                                <div align="center">
                                                                                    <asp:Image ID="imgSSPlate" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                                                        runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Manufacture Date</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="10"
                                                                                        TabIndex="13" ReadOnly="true"></asp:TextBox>
                                                                                    <ajax:CalendarExtender ID="txtManufactureDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        TargetControlID="txtManufactureDate" Format="dd/MM/yyyy">
                                                                                    </ajax:CalendarExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Tank Capacity(in Litre)</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtTankCapacity" runat="server" MaxLength="10" TabIndex="14" ReadOnly="true"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Location Type</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmdloctype" runat="server" TabIndex="2" ReadOnly="true">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <label class="control-label">Rating<span id="Mrating" runat="server"> </span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbRating" runat="server" TabIndex="16"
                                                                                        ReadOnly="true" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="control-group" runat="server" id="Div7">
                                                                            <label class="control-label">Warranty Period</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtWarrantyPeriod" runat="server" MaxLength="10" TabIndex="14" ReadOnly="true"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group" runat="server" id="Div8">
                                                                            <label class="control-label">DTr Oil Capacity</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtTCOilCapacity" runat="server" MaxLength="10" TabIndex="14" ReadOnly="true"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group" runat="server" id="Div9">
                                                                            <label class="control-label">DTR Amount</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="TxtDTrAmount" runat="server" MaxLength="10" TabIndex="14" ReadOnly="true"></asp:TextBox><br />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group" runat="server" id="dvStar" style="display: none">
                                                                            <label class="control-label">Star Rated</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbStarRated" runat="server" TabIndex="17" ReadOnly="true">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group" id="Div1" style="display: none;">
                                                                            <label class="control-label">Name Plate Photo</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:FileUpload ID="fupNamePlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                        TabIndex="18" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group" id="Div2" style="display: none;">
                                                                            <label class="control-label">DTr Code Photo</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:FileUpload ID="fupSSPlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                        TabIndex="19" />
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
                                                                            <label class="control-label">DTC Code (DTLMS)</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="7" TabIndex="17" ReadOnly="true"></asp:TextBox>
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
                                                                                    <asp:TextBox ID="txtIPDTCCode" runat="server" MaxLength="6" TabIndex="20" ReadOnly="true"></asp:TextBox>
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
                                                                            <label class="control-label">TIMS Code(HESCOM)</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtoldDTCCode" runat="server" MaxLength="12" TabIndex="21" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                                                                    <asp:TextBox ID="txtInfosysAsset" runat="server" MaxLength="12" TabIndex="22" onkeypress="javascript:return OnlyNumber(event);" Visible="false"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">DTC Name</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" TabIndex="23" ReadOnly="true"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group" id="Div3" style="display: none;">
                                                                            <label class="control-label">DTC Code (DTLMS) Photo</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:FileUpload ID="fupDTLMSCodePhoto" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                        TabIndex="26" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group" id="Div4" style="display: none;">
                                                                            <label class="control-label">DTC Code (Ip Enum) Photo</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:FileUpload ID="fupIPEnum" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                        TabIndex="27" />
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group" id="Div5" style="display: none;">
                                                                            <label class="control-label">TIMS Code(HESCOM) Photo </label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:FileUpload ID="fupOldCodePhoto" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif"
                                                                                        TabIndex="28" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group" id="Div6" style="display: none;">
                                                                            <label class="control-label">DTC Photo</label>
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
                                                                        TabIndex="31" />
                                                                </div>


                                                                <div class="span2">
                                                                    <asp:RadioButton ID="rdbOldDtc" runat="server" Text="Old DTC Details" CssClass="radio"
                                                                        GroupName="a" AutoPostBack="true" Visible="false"
                                                                        TabIndex="32" />
                                                                </div>

                                                                <div class="span4">
                                                                    <asp:RadioButton ID="rdbIPEnum" runat="server" Text="IP Enumeration DTC Details"
                                                                        CssClass="radio" GroupName="a" AutoPostBack="true"
                                                                        TabIndex="33" Visible="false" />
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
                                                                                    <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" TabIndex="34" ReadOnly="true"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Connected KW</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtConnectedKW" runat="server" MaxLength="6"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="35" ReadOnly="true"></asp:TextBox>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Connected HP</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="36" ReadOnly="true"></asp:TextBox>

                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="control-group">
                                                                            <label class="control-label">DTC Commission Date</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtCommisionDate" runat="server" TabIndex="38" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                                    <ajax:CalendarExtender ID="txtCommisionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        TargetControlID="txtCommisionDate" Format="dd/MM/yyyy">
                                                                                    </ajax:CalendarExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>







                                                                        <div class="control-group">
                                                                            <label class="control-label">DTr Commission Date</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDtrCommissionDate" runat="server" TabIndex="38" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                                    <ajax:CalendarExtender ID="txtDtrCommissionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        TargetControlID="txtDtrCommissionDate" Format="dd/MM/yyyy">
                                                                                    </ajax:CalendarExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Last Service Date</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtServiceDate" runat="server" TabIndex="39" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                                    <ajax:CalendarExtender ID="txtServiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        TargetControlID="txtServiceDate" Format="dd/MM/yyyy">
                                                                                    </ajax:CalendarExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Platform Type</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbPlatformType" runat="server" ReadOnly="true"
                                                                                        TabIndex="40">
                                                                                    </asp:DropDownList>
                                                                                    <asp:TextBox ID="txtDTCId" runat="server" TabIndex="15" MaxLength="10" Visible="false"></asp:TextBox>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Breaker Type</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbBreakerType" runat="server" ReadOnly="true"
                                                                                        TabIndex="41">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>



                                                                        <div class="control-group">
                                                                            <label class="control-label">DTC Meters Available</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbDTCMetered" runat="server" ReadOnly="true"
                                                                                        TabIndex="42" AutoPostBack="true">
                                                                                        <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">KWH Reading </label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtKWHReading" runat="server" MaxLength="11"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="37" ReadOnly="true"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">HT Protection</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbHTProtection" runat="server" ReadOnly="true"
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
                                                                            <label class="control-label">LT Protection</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbLTProtection" runat="server" ReadOnly="true"
                                                                                        TabIndex="44">
                                                                                        <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                                    </asp:DropDownList>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Grounding</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbGrounding" runat="server" ReadOnly="true"
                                                                                        TabIndex="45">
                                                                                        <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                                    </asp:DropDownList>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="control-group">
                                                                            <label class="control-label">Lightning Arresters</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbLightArrester" runat="server" TabIndex="46" ReadOnly="true">
                                                                                        <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Load Type</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">

                                                                                    <asp:DropDownList ID="cmbLoadtype" runat="server" TabIndex="47" ReadOnly="true">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>



                                                                        <div class="control-group">
                                                                            <label class="control-label">Project/Scheme Type</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:DropDownList ID="cmbProjecttype" runat="server" TabIndex="48" ReadOnly="true">
                                                                                    </asp:DropDownList>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">LT Line Length (in Km)</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtltLine" runat="server" TabIndex="49" MaxLength="7" placeholder="Eg: 111.111"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" ReadOnly="true"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">HT Line Length (in Km)</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtHtLine" runat="server" TabIndex="49" MaxLength="7" placeholder="Eg: 111.111"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" ReadOnly="true"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Depreciation</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDepreciation" runat="server" TabIndex="50" MaxLength="10"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" ReadOnly="true"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Latitude</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtLatitude" runat="server" TabIndex="51" MaxLength="10" ReadOnly="true"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Longitude</label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtLongitude" runat="server" TabIndex="52" MaxLength="10" ReadOnly="true"
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

                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Invoice Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
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
                                                <label class="control-label">Indent No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIndentno" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">PO No </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPOno" runat="server" MaxLength="20" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Invoice No<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append1">
                                                        <asp:TextBox ID="txtInvoiceno" runat="server" MaxLength="10" onkeypress="javascript:return AllowNumberAndSecialChe(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Remarks<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemarkComments" runat="server" MaxLength="100"
                                                            onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Indent Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIndentdate" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">PO Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtpodate" runat="server" MaxLength="15" onkeypress="javascript:return OnlyNumber(event);" ReadOnly="true"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="txtpodate_CalendarExtender3" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtpodate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Invoice Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="txtInvoiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtInvoiceDate">
                                                        </ajax:CalendarExtender>
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
        </div>
    </div>
    <div class="form-horizontal" style="text-align: center">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>


    <uc1:ApprovalHistoryView ID="ApprovalHistoryView" runat="server" />


    <div class="row-fluid" runat="server" id="dvComments" style="display: none">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Comments for Approve</h4>
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
                                                <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine"
                                                    Width="550px" Height="125px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
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

    <div class="space20"></div>
    <div class="text-center">
        <asp:Button ID="cmdSave" runat="server" Text="Approve" OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="cmdSave_Click" onchange="javascript:preventMultipleSubmissions();" />
        <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="btnReset_Click" />
    </div>
</asp:Content>
