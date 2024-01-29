<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreEnumeration.aspx.cs" Inherits="IIITS.DTLMS.Internal.StoreEnumeration" %>

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
    </style>
<%--    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.0.min.js"></script>--%>
      <script type="text/javascript" src="../js/Jquery.min.js"></script>
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
    <script type="text/javascript">

        function ValidateMyForm() {

            debugger;
            if (document.getElementById('<%=cmbTaggedDTR.ClientID%>').value == "1") {
                if (document.getElementById('<%=cmbTaggedLocation.ClientID%>').value == "0") {

                    alert('Please Select  DTR Tagged In')
                    document.getElementById('<%=cmbTaggedLocation.ClientID%>').focus()
                    return false
                }
            }


            if (document.getElementById('<%= cmbLocationType.ClientID %>').value == "--Select--") {
                alert('Select Location Type')
                document.getElementById('<%= cmbLocationType.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= cmbLocationName.ClientID %>').value.trim() == "--Select--") {
                if (document.getElementById('<%= cmbLocationType.ClientID %>').value == "3") {
                     alert('Select Repairer Name')

                 }
                 else {
                    alert('Select Division Name')
                 }
                 //alert('Select Location Name')
                 document.getElementById('<%= cmbLocationName.ClientID %>').focus()
                 return false
             }


            //             if (document.getElementById('<%= txtAddress.ClientID %>').value.trim() == "") {
            //                 alert('Enter Valid Address')
            //                 document.getElementById('<%= txtAddress.ClientID %>').focus()
            //                 return false
            //             }

<%--            if (document.getElementById('<%= cmboperator1.ClientID %>').value.trim() == "--Select--") {
                alert('Select Operator1')
                document.getElementById('<%= cmboperator1.ClientID %>').focus()
                 return false
             }--%>
<%--             if (document.getElementById('<%= cmboperator2.ClientID %>').value.trim() == "--Select--") {
                alert('Select Operator2')
                document.getElementById('<%= cmboperator2.ClientID %>').focus()
                 return false
             }--%>

             if (document.getElementById('<%= txtwelddate.ClientID %>').value.trim() == "") {
                alert('Enter Date of Fixing')
                document.getElementById('<%= txtwelddate.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                alert('Enter SS plate Number')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= cmbMake.ClientID %>').value.trim() == "--Select--") {
                alert('Select DTr Make name')
                document.getElementById('<%= cmbMake.ClientID %>').focus()
                 return false
             }
            //             if (document.getElementById('<%= cmbCapacity.ClientID %>').value.trim() == "--Select--") {
            //                 alert('Select Capacity')
            //                 document.getElementById('<%= cmbCapacity.ClientID %>').focus()
            //                 return false
            //             }


            if (document.getElementById('<%= cmbTranstype.ClientID %>').value.trim() == "--Select--") {
                alert('Select Transformer Type')
                document.getElementById('<%= cmbTranstype.ClientID %>').focus()
                 return false
             }

             var FromdateInput = document.getElementById('<%= txtwelddate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
            if (!FromdateInput.match(goodDate)) {
                alert("Please enter valid Date of Fixing date");
                document.getElementById('<%= txtwelddate.ClientID %>').focus()
            return false;
        }
        var FromdateInput = document.getElementById('<%= txtManufactureDate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
            if (!FromdateInput.match(goodDate)) {
                alert("Please enter valid Manufacture date");
                document.getElementById('<%= txtManufactureDate.ClientID %>').focus()
            return false;
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
                <div class="span8">
                    <h3 class="page-title">Store Enumeration
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
                        CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='EnumerationView.aspx'; return false;" />
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Store Enumeration</h4>
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

                                            <%--     <div class="control-group">
                                                   <label class="control-label">Circle Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbCircle" runat="server"  AutoPostBack="true" 
                                                                TabIndex="1" onselectedindexchanged="cmbCircle_SelectedIndexChanged" >                                   
                                                            </asp:DropDownList>
                                                           
                                                       </div>
                                                    </div>
                                               </div>
                  
                                               <div class="control-group">
                                                   <label class="control-label">Division Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbdivision" runat="server"  
                                                                TabIndex="1" >                                                                                              
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtRecID" runat="server" MaxLength="50" Enabled ="false" visible="false" ></asp:TextBox> 
                                                       </div>
                                                    </div>
                                               </div>--%>
                                            <div class="control-group">
                                                <label class="control-label">Location Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbLocationType" runat="server" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbLocationType_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtStatus" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtNamePlatePhotoPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtSSPlatePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                    </div>

                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="lblRepairerName" runat="server" Text="Location Name"></asp:Label>
                                                    <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbLocationName" runat="server" TabIndex="2" OnSelectedIndexChanged="cmbLocationName_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" id="dvdivision" runat="server" visible="false">
                                                <label class="control-label">Division<span class="Mandotary">*</span>  </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" TabIndex="9" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="lblAddress" runat="server" Text="Location Address"></asp:Label>
                                                    <span class="Mandotary">*</span>
                                                </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAddress1" runat="server" MaxLength="100" TabIndex="2" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" MaxLength="100" onkeyup="javascript:ValidateTextlimit(this,100)"
                                                            TabIndex="3" Style="resize: none" Height="80px" ReadOnly="true"></asp:TextBox>
                                                         <asp:HiddenField ID="hdfLocationName" runat="server" />
                                                        <%----%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">



                                            <%--   <div class="control-group">
                                                   <label class="control-label">Location Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:TextBox ID="txtLocation" runat="server" MaxLength="50" TabIndex="2" ></asp:TextBox>
                                                       </div>
                                                    </div>
                                               </div>     --%>

<%--                                            <div class="control-group">
                                                <label class="control-label">Operator 1<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmboperator1" runat="server" AutoPostBack="true"
                                                            TabIndex="4" OnSelectedIndexChanged="cmboperator1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdfOperaator2" runat="server" />
                                                        <asp:HiddenField ID="hdfLocationName" runat="server" />

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Operator 2<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmboperator2" runat="server" TabIndex="5"
                                                            OnSelectedIndexChanged="cmboperator2_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">Enumeration Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtwelddate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                      <%--  <asp:CalendarExtender ID="txtwelddate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtwelddate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group" runat="server" visible="false">
                                                <label class="control-label">Tagged DTR  <span class="Mandotary">*</span>  </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTaggedDTR" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbTaggedDTRIndecChanged_Click">
                                                            <asp:ListItem Text="No" Value="0" Selected="True"> </asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group" id="divTaggedId" runat="server" visible="false">
                                                <label class="control-label">DTR Tagged in <span class="Mandotary">*</span>  </label>
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
                    <!-- BEGIN BASIC PORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Transformer Details</h4>
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
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcCode" runat="server" MaxLength="6" TabIndex="7"
                                                            onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Make<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" TabIndex="8" AutoPostBack="true" OnSelectedIndexChanged="cmbMake_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTranstype" runat="server" TabIndex="10">
                                                            
                                                            <%--<asp:ListItem Text="--Select--" Value="0"> </asp:ListItem>--%>
                                                         <%--   <asp:ListItem Text="GOOD" Value="1"> </asp:ListItem>
                                                            <asp:ListItem Text="FAULTY" Value="3"> </asp:ListItem>
                                                            <asp:ListItem Text="SCRAP" Value="4"> </asp:ListItem>
                                                             <asp:ListItem Text="REPAIR GOOD" Value="2"> </asp:ListItem>
                                                            <asp:ListItem Text="RELEASE GOOD" Value="11"> </asp:ListItem>--%>



                                                        </asp:DropDownList>
                                                         <asp:HiddenField ID="hdnTRansType" runat="server" />
                                                        <asp:TextBox ID="txtEnumDetailsId" runat="server" MaxLength="50" Visible="false" Width="50px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr SLno<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcslno" runat="server" MaxLength="15" TabIndex="11"></asp:TextBox><br />

                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="control-group" runat="server" id="Div1" style="display: none">
                                          <%--  <div class="control-group">--%>
                                                <label class="control-label">Manufacture Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="50"
                                                            TabIndex="12"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtManufactureDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtManufactureDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
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
                                                        <asp:TextBox ID="txtTankCapacity" runat="server" MaxLength="10" TabIndex="13"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox><br />

                                                    </div>
                                                </div>
                                            </div>

<%--                                            <div class="control-group ">
                                                <label class="control-label">Weight Of DTr(in KG)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWeight" runat="server" MaxLength="10" TabIndex="14"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox><br />

                                                    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">Rating <span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRating" runat="server" TabIndex="15"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvStar" style="display: none">
                                                <label class="control-label">Star Rated</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStarRated" runat="server" TabIndex="16">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Name Plate Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupNamePlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif" 
                                                            TabIndex="17" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Code Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupSSPlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif" 
                                                            TabIndex="18" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="space20"></div>
                                            <div class="space20"></div>
                                            <div class="space20"></div>
                                            <div class="space10"></div>
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
                    </div>
                    <!-- END BASIC PORTLET-->
                </div>

            </div>

            <div class="widget-body">

                <div class="text-center" align="center">


                    <asp:Button ID="cmdSave" runat="server" Text="Save"
                        OnClientClick="javascript:return ValidateMyForm();" CssClass="btn btn-primary"
                        TabIndex="19" OnClick="cmdSave_Click" />


                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                        CssClass="btn btn-danger" TabIndex="20"
                        OnClick="cmdReset_Click" /><br />

                    <div class="span1">
                        <asp:Button ID="cmdReject" runat="server" Text="Reject" Visible="false"
                            CssClass="btn btn-primary" TabIndex="54" Width="105px" OnClick="cmdReject_Click" /><br />
                    </div>
                    <div class="span1">
                        <asp:Button ID="cmdNext" runat="server" Visible="false" Text="Next >>"
                            CssClass="btn btn-primary" TabIndex="20" OnClick="cmdNext_Click" /><br />
                    </div>
                    <%-- <div class="span1">  
                    <asp:Button ID="tempButton" runat="server" Text="Temp Button" 
                        CssClass="btn btn-primary"  TabIndex="20" OnClick="tempButton_Click" 
                           /><br />
                </div>--%>
                    <div class="span7"></div>
                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <%--  <div class="widget blue">--%>
                    <div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <asp:GridView ID="GridStoreEnumDetails" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" PageSize="10" runat="server" AutoGenerateColumns="false"
                                    OnPageIndexChanging="GridStoreEnumDetails_PageIndexChanging" OnRowCommand="GridStoreEnumDetails_RowCommand">
                                    <Columns>

                                        <asp:TemplateField AccessibleHeaderText="ED_ID" HeaderText="Ed Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEdID" runat="server" Text='<%# Bind("ED_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="Location Type" HeaderText="Location Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationType" runat="server" Text='<%# Bind("ED_LOCTYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- <asp:TemplateField AccessibleHeaderText="Location Name" HeaderText="Location Name"  >                                
                                    <ItemTemplate>                                       
                                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("LOC_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                        <asp:TemplateField AccessibleHeaderText="Date of Fixing" HeaderText="Date of Fixing">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWelddate" runat="server" Text='<%# Bind("ED_WELD_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="Make" HeaderText="Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="Capacity" HeaderText="Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("DTE_CAPACITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" Visible="false">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" Visible="false"
                                                        Width="12px" CommandName="Modify" OnClientClick="return confirm ('Are you sure, you want to Edit the Details');" />
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

                                <div class="form-horizontal" align="center">
                                    <div class="span4"></div>
                                    <div class="span2">
                                    </div>
                                    <div class="span1">
                                    </div>
                                    <div class="span7"></div>
                                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                <div class="space5"></div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
