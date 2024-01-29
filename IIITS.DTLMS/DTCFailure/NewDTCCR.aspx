<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="NewDTCCR.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.NewDTCCR" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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

        .row-fluid .span5 {
            width: 43.17094% !important;
            *width: 40.11774868157847%;
            margin-right: 15px;
        }

        .control-group img {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.0.min.js"></script>
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

        // to display in next tab
        //function DisplayFullImage(ctrlimg) {
        //    txtCode = "<HTML><HEAD>"
        //    + "</HEAD><BODY TOPMARGIN=0 LEFTMARGIN=0 MARGINHEIGHT=0 MARGINWIDTH=0><CENTER>"
        //    + "<IMG src='" + ctrlimg.src + "' BORDER=0 NAME=FullImage "
        //    + "onload='window.resizeTo(document.FullImage.width,document.FullImage.height)'>"
        //    + "</CENTER>"
        //    + "</BODY></HTML>";
        //    mywindow = window.open('', 'image', '');
        //    mywindow.document.open();
        //    mywindow.document.write(txtCode);
        //    mywindow.document.close();

        //}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">New DTC CR Report
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
            <div style="float: right; margin-top: 20px; margin-right: 12px">
                <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary"
                    OnClick="cmdClose_Click" />
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>New DTC CR Report</h4>
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
                                                Transformer Centre Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:TextBox ID="txtDTCId" runat="server" ReadOnly="true" Width="20px" Visible="false"></asp:TextBox>
                                                    <%--<asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTCDetails_Click">View Transformer Centre Details</asp:LinkButton>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtNewDTr" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:TextBox ID="txtDtrCode" runat="server" ReadOnly="true" Width="20px" Visible="false"></asp:TextBox>
                                                    <asp:LinkButton ID="lnkNewDTr" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkNewDTr_Click">View DTr Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                Work Order Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtWrkOrderDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" runat="server" id="lblwoNo">
                                                Work Order NO</label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOAuto" runat="server" />
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                    <asp:HiddenField ID="hdfRefOffCode" runat="server" />
                                                    <asp:HiddenField ID="hdfWOSLNO" runat="server" />
                                                    <asp:HiddenField ID="hdfRecordId" runat="server" />
                                                    <asp:HiddenField ID="hdfSSPlatePath" runat="server" />
                                                    <asp:HiddenField ID="hdfDTLMSCODEPath" runat="server" />
                                                    <asp:HiddenField ID="hdfNamePlatePath" runat="server" />
                                                    <asp:HiddenField ID="hdfDTCPoto1Path" runat="server" />
                                                    <asp:HiddenField ID="hdfDTCPoto2Path" runat="server" />
                                                    <asp:HiddenField ID="hdfIpEnumarationPath" runat="server" />
                                                    <asp:HiddenField ID="hdfInfosysAssetIdPath" runat="server" />
                                                    <asp:HiddenField ID="hdfEnumarationId" runat="server" />
                                                    <asp:HiddenField ID="hdfTcslno" runat="server" />
                                                    <asp:HiddenField ID="hdfTcRating" runat="server" />
                                                    <asp:HiddenField ID="hdfcapacity" runat="server" />
                                                    <asp:HiddenField ID="hdfmakeid" runat="server" />
                                                    <asp:HiddenField ID="hdfLevel" runat="server" />
                                                    <asp:HiddenField ID="hdfTTK_flow" runat="server" />
                                                    <asp:HiddenField ID="hdftranscommission" runat="server" />
                                                    <asp:TextBox ID="txtActiontype" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtWONO" runat="server" ReadOnly="true" MaxLength="17" TabIndex="1" Style="resize: none" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" runat="server" id="dvfupDTLMS">
                                            <div class="control-group">
                                                <label class="control-label">DTLMS Code Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupDTLMSCODE" runat="server" AllowMultiple="False" Accept=".png,.jpg,.jpeg,.jpe,.jif,.jfif,.jfi"
                                                            TabIndex="27" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" runat="server" id="dvfupDTC1">
                                            <div class="control-group">
                                                <label class="control-label">DTC Photo 1  <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupDTCCODE1" runat="server" AllowMultiple="False" Accept=".png,.jpg,.jpeg,.jpe,.jif,.jfif,.jfi"
                                                            TabIndex="28" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group" runat="server" id="dvfupEnum">
                                            <div class="control-group">
                                                <label class="control-label">IP Enumaration Code </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupIpEnum" runat="server" AllowMultiple="False" Accept=".png,.jpg,.jpeg,.jpe,.jif,.jfif,.jfi"
                                                            TabIndex="30" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>




                                        <%--<div class="control-group" id="histroy" runat="server">
                                            <label class="control-label">View Histroy</label>
                                            <div class="controls">
                                                <asp:LinkButton runat="server" ID="lnkHistory" ToolTip="View History" OnClick="lnkHistory_Click">
                                         <img src="../img/Manual/View1.jpg" style="width:20px" alt="view" /></asp:LinkButton>
                                            </div>
                                        </div>--%>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Project Type<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdflowType" runat="server" TabIndex="3">
                                                        <asp:ListItem Value="1">PTK Flow</asp:ListItem>
                                                        <asp:ListItem Value="2">TTK Flow</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Name<span class="Mandotary"> </span>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlApproval" runat="server">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    CR Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCRDate" runat="server"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtCRDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row-fluid">
                                                <div class="control-group" runat="server" id="dvfupNamePlate">
                                                    <div class="control-group">
                                                        <label class="control-label">Name Plate Photo<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fupNamePlate" runat="server" AllowMultiple="False"
                                                                    TabIndex="18" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group" runat="server" id="dvfupSSplate">
                                                    <div class="control-group">
                                                        <label class="control-label">SS Plate Photo<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fupSSPlate" runat="server" AllowMultiple="False"
                                                                    TabIndex="19" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group" runat="server" id="dvfupDTC2">
                                                    <div class="control-group">
                                                        <label class="control-label">DTC Photo 2<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fupDTCCODE2" runat="server" AllowMultiple="False"
                                                                    TabIndex="29" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <%-- <div class="control-group" runat="server" id="dvfupInfosys">
                                           <div class="control-group">
                                                                    <label class="control-label">Infosys asset id </label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="fupInfosysId" runat="server" AllowMultiple="False"
                                                                                TabIndex="30" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                               </div>--%>








                                            <%--<div class="control-group" id="DivUpload" runat="server">
                                                        <label class="control-label">Upload File<span class="Mandotary">*</span> </label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fupFile" runat="server" />
                                                                <asp:HiddenField ID="HdfFilePath" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div> --%>

                                            <%--<div class="control-group">
                                                <label class="control-label">
                                                    Commissiong Inventory Qty<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInvQty" runat="server" MaxLength="5" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    DeCommissioning Inventory Qty<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                  <asp:TextBox ID="txtDecommInventry" runat="server" MaxLength="5" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </asp:Panel>
                                    </div>
                                    <div class="span1">
                                    </div>
                                </div>
                            </div>
                            <div class="container-fluid">
                            <div class="row">
                                <div runat="server" id="dvNamePlate" style="display: none; float: left!important" class="col-md-3">
                                    <div class="control-group">
                                        <div align="center">
                                            <label>Name Plate Photo </label>
                                            <div align="center">
                                                <asp:Image ID="imgNamePlate" Style="width: 318px!important; margin-right: 50px!important" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                    runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="dvSSplate" style="display: none; float: left!important" class="col-md-3">
                                    <div class="control-group">
                                        <div align="center">
                                            <label>SS Plate Photo </label>
                                            <div align="center">
                                                <asp:Image ID="imgSSplate" Style="width: 318px!important; margin-right: 50px!important" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                    runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="dvDTCpoto1" style="display: none; float: left!important" class="col-md-3">
                                    <div class="control-group">
                                        <div align="center">
                                            <label>DTC Photo 1 </label>
                                            <div align="center">
                                                <asp:Image ID="imgDTCpoto1" Style="width: 318px!important; margin-right: 50px!important" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                    runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div runat="server" id="dvDTCCode2" style="display: none; float: left!important" class="col-md-3">
                                    <div class="control-group">
                                        <div align="center">
                                            <label>DTC Photo 2 </label>
                                            <div align="center">
                                                <asp:Image ID="imgDTCCode2" Style="width: 318px!important; margin-right: 50px!important" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                    runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="dvDTLMSCode" style="display: none; float: left!important" class="col-md-3">
                                    <div class="control-group">
                                        <div align="center">
                                            <label>Transformer Centre Code Photo </label>
                                            <div align="center">
                                                <asp:Image ID="imgDTLMSCode" Style="width: 318px!important; margin-right: 50px!important" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                    runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div runat="server" id="dvIPEnum" style="display: none; float: left!important" class="col-md-3">
                                    <div class="control-group">
                                        <div align="center">
                                            <label>IP Enumaration Code </label>
                                            <div align="center">
                                                <asp:Image ID="imgIPEnum" Style="width: 318px!important; margin-right: 50px!important" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                    runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                </div>
                            <%--<asp:CheckBox ID="txtcertify" runat="server" Checked="true" Enabled="false" />
                                <asp:Label ID="Lbl" runat="server"
                                    Text="I certify that all the items that have been issued are used for this workorder."></asp:Label>--%>
                            <div class="space20">
                            </div>
                        </div>
                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


                    </div>
                </div>
            </div>

            <!-- END FORM-->
        </div>

        <%--<uc1:ApprovalHistoryView ID="ApprovalHistoryView" runat="server" />--%>

        <div class="row-fluid" runat="server" id="dvComments" style="display: none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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
                                                Comments<span class="Mandotary"> *</span></label>
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

        <div class="text-center">


            <asp:Button ID="cmdCR" runat="server" Text="CR Report" CssClass="btn btn-primary"
                OnClientClick="javascript:return ValidateMyForm()" OnClick="cmdCR_Click" />


        </div>
    </div>
</asp:Content>
