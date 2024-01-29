<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TcMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
              alert('Enter DTr Code')
              document.getElementById('<%= txtTcCode.ClientID %>').focus()
              return false
          }


          if (document.getElementById('<%= cmbMake.ClientID %>').value.trim() == "-Select-") {
              alert('Select DTr Make Name')
              document.getElementById('<%= cmbMake.ClientID %>').focus()
              return false
          }

          if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "-Select-") {
              alert('Select valid Capacity')
              document.getElementById('<%= cmbCapacity.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtSerialNo.ClientID %>').value == "") {
              alert('enter the dtr slno')
              document.getElementById('<%= txtSerialNo.ClientID %>').focus()
              return false
          }

          if (document.getElementById('<%= cmbRating.ClientID %>').value.trim() == "--Select--") {
              alert('Select the rating')
              document.getElementById('<%= cmbRating.ClientID %>').focus()
              return false
          }

          if (document.getElementById('<%= cmbTcLocation.ClientID %>').value == "-Select-") {
              alert('Select DTr Current Location')
              document.getElementById('<%= cmbTcLocation.ClientID %>').focus()
                  return false
              }

              if (document.getElementById('<%= txtOilCapacity.ClientID %>').value.trim() == "") {
              alert('Enter Valid Oil Capacity')
              document.getElementById('<%= txtOilCapacity.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtWeight.ClientID %>').value.trim() == "") {
              alert('Enter Valid Weight of DTr')
              document.getElementById('<%= txtWeight.ClientID %>').focus()
                  return false
              }

              if (document.getElementById('<%= cmbRating.ClientID %>').value == "-Select-") {
              alert('Select Rating')
              document.getElementById('<%= cmbRating.ClientID %>').focus()
                  return false
              }


          }

          function DisplayFullImage(ctrlimg) {
              txtCode = "<HTML><HEAD>"
           + "</HEAD><BODY TOPMARGIN=0 LEFTMARGIN=0 MARGINHEIGHT=0 MARGINWIDTH=0><CENTER>"
           + "<IMG src='" + ctrlimg.src + "' BORDER=0 NAME=FullImage "
           + "onload='window.resizeTo(document.FullImage.width,document.FullImage.height)'>"
           + "</CENTER>"
           + "</BODY></HTML>";
              mywindow = window.open('', 'image', '');
              mywindow.document.open();
              mywindow.document.write(txtCode);
              mywindow.document.close();

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
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <h3 class="page-title">DTR Master
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
                    <asp:Button ID="Button2" runat="server" Text="Inward View"
                        OnClientClick="javascript:window.location.href='TcInwardView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>

                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="Button1" runat="server" Text="DTR View"
                        OnClientClick="javascript:window.location.href='TCMasterView.aspx'; return false;"
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
                            <h4><i class="icon-reorder"></i>DTR Master</h4>
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
                                                <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfDTRImagePath" runat="server" />
                                                        <asp:HiddenField ID="hdfDTRNameplateImagePath" runat="server" />
                                                        <asp:TextBox ID="txtTcCode" runat="server" AutoPostBack="true"
                                                            onkeypress="javascript:return OnlyNumber(event);" MaxLength="8"
                                                            OnTextChanged="txtTcCode_TextChanged"></asp:TextBox>
                                                        <asp:TextBox ID="txtTcID" runat="server" Visible="false" Width="20px"></asp:TextBox><br />
                                                        <asp:LinkButton ID="lnkDTRHistory" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkDTRHistory_Click">View DTR History</asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">DTr Serial No<span class="Mandotary"> *</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%-- onkeypress="javascript:return OnlyNumber(event);"  --%>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="20" CssClass="auto-style1"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Make<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" TabIndex="10"
                                                            AutoPostBack="true" OnSelectedIndexChanged="cmbMake_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:HiddenField ID="hdfCapacity" runat="server" />
                                                <label class="control-label">Capacity(in KVA)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--<asp:TextBox ID="txtTcCapacity" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="10" ></asp:TextBox>--%>
                                                        <asp:DropDownList ID="cmbCapacity" Enabled="false" runat="server">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <%--<div class="control-group">--%>
                                            <div class="control-group" runat="server" id="Div1" style="display: none">
                                                <label class="control-label">Manufacturing Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="ManufactureCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtManufactureDate">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Purchasing Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPurchaseDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="PurchaseCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtPurchaseDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Supplier Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%-- <asp:TextBox ID="txtSupplyId" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"  ></asp:TextBox>
                                      <asp:Button ID="btnSupplyId" Text="S" class="btn btn-primary" runat="server" />--%>
                                                        <asp:DropDownList ID="cmbSupplier" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoNo" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Condition of TC</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbConditionOfTC" runat="server">
                                                            <%--<asp:ListItem Value="0">--select--</asp:ListItem>
                                    <asp:ListItem Value="1">New</asp:ListItem>
                                    <asp:ListItem Value="2">Repair</asp:ListItem>
                                    <asp:ListItem Value="3">RP Good</asp:ListItem>
                                    <asp:ListItem Value="4">RE Good</asp:ListItem>
                                    <asp:ListItem Value="5">Faulty</asp:ListItem>
                                    <asp:ListItem Value="6">Mobile Transformer</asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Cooling</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCooling" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Oil</asp:ListItem>
                                                            <asp:ListItem Value="2">Dry</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">TC Type</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTCtype" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Open Bushing</asp:ListItem>
                                                            <asp:ListItem Value="2">Cable Entry</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Last Failure Date</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastFailureDate" runat="server" MaxLength="10"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Last Failure Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastrFailureType" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">No of Failure</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFailCount" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Original Cost</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOriginalCost" runat="server" MaxLength="20" onkeypress="javascript:return OnlyIntegers();"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Insurance (in Rs)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInsurance" runat="server" MaxLength="20" CssClass="auto-style1" onkeypress="javascript:return OnlyIntegers();"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">DTr Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPrice" runat="server" MaxLength="9"  onpaste="return false" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                        <asp:HiddenField ID="hdfTcCode" runat="server" />
                                                        <asp:HiddenField ID="hdfTcLocation" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Life Span</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcLifeSpan" runat="server"  onpaste="return false" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Oil Capacity(in Litre)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilCapacity" runat="server"  onpaste="return false" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Oil Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOilType" runat="server" TabIndex="15">
                                                            <asp:ListItem Selected="True" Value="1"> Mineral oil </asp:ListItem>
                                                            <asp:ListItem Value="2"> Ester Oil </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Weight of DTr(in KG)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWeight" runat="server"  onpaste="return false" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">warranty Period(in Months)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWarrentyPeriod" runat="server" MaxLength="2"   onpaste="return false" autocomplete="off" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                        <%--  <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                     TargetControlID="txtWarrentyPeriod"></ajax:CalendarExtender> --%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Last Service Date<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastServiceDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtLastServiceDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Rating<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRating" runat="server" TabIndex="15"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <!--<div class="control-group" runat="server" id="dvStar" style="display:none">
                        <label class="control-label">Star Rated</label>
                        <div class="controls">   
                            <div class="input-append">
                                <asp:DropDownList ID="cmbStarRated" runat="server"  TabIndex="16">                                   
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>   -->
                                            <div class="control-group">
                                                <label class="control-label">Current Location</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbTcLocation" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbTcLocation_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Core Type</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCoreType" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Stacked Core</asp:ListItem>
                                                            <asp:ListItem Value="2">Amorphous</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Tap Charger</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTapCharger" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="2">No</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Last Repair Count</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastRepairCount" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Last Repair Cost</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastRepairCost" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Infosys Asset ID(RAPDRP)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRAPDRP" runat="server" MaxLength="20" onkeypress="javascript:return OnlyIntegers();"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Component ID(RAPDRP)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtComponentID" runat="server" MaxLength="20" onkeypress="javascript:return OnlyIntegers();"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">DTr Depreciation</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtdepreciation" runat="server" MaxLength="20" onkeypress="javascript:return OnlyIntegers();"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>

                                <div class="form-horizontal" align="center">

                                    <div class="span3"></div>
                                    <div class="span1">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                            OnClientClick="javascript:return ValidateMyForm();" CssClass="btn btn-primary"
                                            OnClick="cmdSave_Click" />
                                    </div>
                                    <%-- <div class="span1"></div>  OnClientClick="javascript:return ResetForm();"--%>
                                    <div class="span1">
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-primary" OnClick="cmdReset_Click" /><br />
                                    </div>

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                                <br />
                                <br />

                                <br />
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group" runat="server" id="dvDTrCode" style="display: none">
                                                <div align="center">
                                                    <label>Name Plate Photo </label>
                                                    <div align="center">
                                                        <asp:Image ID="imgDTrCode" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                            runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group" runat="server" id="dvNamePlate" style="display: none">
                                                <div align="center">
                                                    <label>DTr Code Photo </label>
                                                    <div align="center">
                                                        <asp:Image ID="imgNamePlate" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                            runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
        <div class="row-fluid" runat="server" id="divField" style="display: none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Basic Details of DTC</h4>
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
                                            <label class="control-label">DTC Code<span class="Mandotary"> *</span> </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcCode" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnDtcSearch" runat="server" Text="S" Visible="false"
                                                        CssClass="btn btn-primary" OnClick="btnDtcSearch_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Transformer Centre Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span1"></div>
                            </div>
                            <div class="space20"></div>

                            <div class="form-horizontal" align="center">

                                <div class="span3"></div>
                                <div class="span1">
                                    <asp:Button ID="btnReset" runat="server" Text="Reset" Visible="false"
                                        CssClass="btn btn-primary" OnClick="btnReset_Click" /><br />
                                </div>
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
    <%--   style="display:none"--%>
    <div class="row-fluid" runat="server" id="divRepairer" style="display: none">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Basic Details of Repairer</h4>
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
                                        <label class="control-label">Repairer Name</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtRepairerName" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Repairer Address </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtReAddress" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span5">
                                    <div class="control-group">
                                        <label class="control-label">Repairer Mobile No.</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtReMobileNo" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Repairer EmailId</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtReEmailId" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <div class="space20"></div>


                    </div>
                </div>

                <div class="space20"></div>
                <!-- END FORM-->

            </div>
        </div>
        <!-- END SAMPLE FORM PORTLET-->
    </div>



    <div class="row-fluid" runat="server" id="divStore" style="display: none">

        <div class="span12">

            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Basic Details of Store/Transformer Bank</h4>
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
                                        <label class="control-label">Store Name</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtStoreName" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="control-group">
                                        <label class="control-label">Store Address</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtStoreAddress" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="span5">

                                    <div class="control-group">
                                        <label class="control-label">Store Incharge</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtStoreincharge" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Store Incharge MobileNo.</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtStoreMobile" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="space20"></div>


                            </div>
                        </div>

                        <div class="space20"></div>
                        <!-- END FORM-->

                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

    </div>
    </div>
    
</asp:Content>
