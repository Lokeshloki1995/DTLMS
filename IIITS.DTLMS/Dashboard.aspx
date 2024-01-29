<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IIITS.DTLMS.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 


 <script type="text/javascript">
     // $(window).on('load', function () {
     //    $('#myModal1').modal('show');
     // });
     $(function () {
         cuenta = 0;
         txtArray = ["** Click here to View the Updated Features", "** Click here to View the Updated Features"];
         setInterval(function () {
             cuenta++;
             $("#titulos").fadeOut(100, function () {
                 $(this)
                   .text(txtArray[cuenta % txtArray.length])
                   .css('color', cuenta % 2 == 0 ? 'red' : 'blue')
                   .fadeIn(100);
             });
         }, 3000);
     });
 </script>
    <style type="text/css">
        span#titulos {
            margin-left: 0%;
            /*float: left;*/
            padding: 0px 0px 0px 0px;
            font-size: 14px;
            font-weight: bolder;
            cursor: pointer;
        }

        img#ContentPlaceHolder1_Chart1 {
            display:block;
            margin-left: auto;
            margin-right: auto;
        }
        img#ContentPlaceHolder1_FailureReasonChart {
            display:block;
            margin-left: auto;
            margin-right: auto;
        }
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(/img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .auto-style2 {
            left: 0px;
            top: 3px;
        }

        input#ContentPlaceHolder1_cmdexport {
            float: right !important;
        }

        .auto-style3 {
            left: 0px;
            top: 23px;
        }

        .modal-header {
            padding: 9px 15px;
            border-bottom: 1px solid #eee;
            text-align: center !important;
            background: radial-gradient(ellipse at center, #0264d6 1%,#1c2b5a 100%);
        }

        blink {
            -webkit-animation: 2s linear infinite condemned_blink_effect;
            animation: 2s linear infinite condemned_blink_effect;
        }

        @-webkit-keyframes condemned_blink_effect {
            // for Safari 4.0 - 8.0 0% {
                visibility: hidden;
            }

            50% {
                visibility: hidden;
            }

            100% {
                visibility: visible;
            }
        }

        @keyframes condemned_blink_effect {
            0% {
                visibility: hidden;
            }

            50% {
                visibility: hidden;
            }

            100% {
                visibility: visible;
            }
        }

        a#ContentPlaceHolder1_grdDTCFailureAbstractoffice_lblofficeName_4 {
            TEXT-DECORATION: NONE;
            POINTER-EVENTS: NONE;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">

    </asp:ScriptManager>
    <div class="container-fluid">
        <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
       <%-- <button type="button" class="close" data-dismiss="modal">&times;</button>--%>
        <h4 style="font-size: 22.5px;
    font-weight: bolder!important;
    color: #fff;
    text-transform: uppercase;" class="modal-title">Updated Features</h4>
      </div>
      <div class="modal-body">
        <ul><li>Transformer repair process has been changed as per directions from Corporate Office dated 23rd March 2021.</li>
            <li>In Replacement flow, after Work Order, users can generate Indent or RI based on their requirement.</li>
<li>In Replacement flow, during estimation, users need to select the "Core Type" and "Insulation Type" to generate correct SR based estimation.</li>
<li>In Replacement flow, the Store Keeper can invoice the same failed DTr, after "Repairer estimate" creation by SO and subsequent Work Award initiation from Circle office and received back in "Repaired Good" condition in store.</li>
        </ul>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>


         <div id="myModal1" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
       <%-- <button type="button" class="close" data-dismiss="modal">&times;</button>--%>
        <h4 style="font-size: 22.5px;
    font-weight: bolder!important;
    color: #fff;
    text-transform: uppercase;" class="modal-title">Important Note!!!!!!</h4>
      </div>
      <div class="modal-body">
        <ul><li>Due to problem with PGRS Server ,We request you to Manually take the PGRS docket number by calling 1912 until further notice </li>
        </ul>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

        <div class="row-fluid">
            <div class="span10">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Dashboard
                </h3>


                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
            
           <div class="span2" style="/*float: right;*/ margin-top: 20px; /*margin-right: 12px*/"> 
                <a target="_blank" href="/DashboardForm/DownLoad.aspx"   style="font-size:12px;color:Blue;font-weight:bold">Download </a><span style="font-size:12px">(User Manual)</span>
            
                
            </div> 
            <br />
             <marquee id="PlayStoreLink" direction="left" onMouseOver="document.all.PlayStoreLink.stop()" onMouseOut="document.all.PlayStoreLink.start()"><blink>
<img style="width: 3%!important;float: left!important;margin-left: -4%;margin-top: -1%;" src="img/new.png" />
                   </blink><asp:HyperLink ID="lnkPlayStoreLin" Target="_blank" style="font-size:15px;color:Blue;font-weight:bold" NavigateUrl="https://play.google.com/store/apps/details?id=com.iiits.DtlmsHescom" runat="server">** Please UnInstall Old APK And  Install New APK from Play Store **</asp:HyperLink></marquee>


            <%--<br /><br />
             <div class="input-append" style="margin-top: 22px!important;">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <asp:LinkButton ID="lnkAndroidManual" runat="server" 
                                   style="font-size:12px;color:Blue;font-weight:bold" onclick="lnkAndroidManual_Click" 
                                      >DTLMS Android Application</asp:LinkButton>                     
                            </div>--%>
<%--            <div class="span12">
                 
               <a target="_blank" data-toggle="modal" data-target="#myModal"><blink>
<img style="width: 3%!important;float: left!important;margin-left: -4%;margin-top: -1%;" src="img/new.png" />
                   </blink>
                  <span style="color:red"id="titulos">** Click here to View the Updated Features</span>  </a>            
  
            </div>--%>
           <%-- <br />--%>
   <%--             <marquee id="PlayStoreLink" direction="left" onMouseOver="document.all.PlayStoreLink.stop()" onMouseOut="document.all.PlayStoreLink.start()"><asp:HyperLink ID="lnkPlayStoreLin" Target="_blank" style="font-size:15px;color:Blue;font-weight:bold" NavigateUrl="https://play.google.com/store/apps/details?id=com.iiits.dtlms" runat="server"> Click Here to Download via Google PlayStore </asp:HyperLink></marquee>

                <br />
                <marquee behavior="pgrs" direction="left" onmouseover="this.stop();" onmouseout="this.start();"><span style="color:red"> "Please Obtain PGRS Docket Number Manually by calling 1912"</span></marquee>--%>

        </div>
    </div>
    <!-- END PAGE HEADER-->
    <!-- BEGIN PAGE CONTENT-->
    <div class="row-fluid">

        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Dashboard</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>
                    </span>
                  <%--   <span class="tools">
                        <a href="/DashboardForm/TutorialVideos.aspx"   title="Download Tutorial videos" class="icon-facetime-video"></a>

                    </span>--%>
                    <span class="tools">
                        <a href="/DashboardForm/UploadCirculars.aspx"   title="Upload Circulars" class="icon-cloud-upload"></a>
                    </span>
                  <span class="tools">
                        <a href="/DashboardForm/CircularDetails.aspx"   title="Download Circulars" class="icon-download-alt"></a>

    
                    </span> 
                </div>



                <div class="widget-body">
                    
                    <div class="form-horizontal">
                        <div class="row-fluid">

                            <div class="span8">
                                <asp:Label ID="lblStatus" runat="server" Text="Location :" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                                <asp:Label ID="lblLocation" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                    Font-Size="Medium"></asp:Label>
                                 <asp:HiddenField ID="hdfLocationCode" runat="server" />
                                <asp:LinkButton style="margin-left:320px!important" ID="lnkChange" runat="server" OnClick="lnkChange_Click">Select Location</asp:LinkButton>
                                <br />
                                <br />
<%--                                <asp:Label ID="Label3" runat="server" Text="Total No. Of DTC :" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>                                
                                <%-- <asp:Label ID="Label1" runat="server" Text="Total No. Of DTC :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>--%>
<%--                                <a href="/MasterForms/DTCView.aspx?URLRedirect=true" target="_blank" style="font-weight: bold; color: Gray; font-size: medium">
                                <asp:Label ID="lblTotalDTC" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                    Font-Size="Medium"></asp:Label></a>--%>
                             <asp:Label ID="Label3" runat="server" Text="Total No. Of DTC :" Font-Bold="true"
                                    Font-Size="Medium" style="float:left"></asp:Label>
                              <asp:linkbutton ID="lblDTC" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                    Font-Size="Medium" OnClick="lnkChange_dtc"></asp:linkbutton>

                                <br />

<%--                                <asp:Label ID="lblDTRCount" runat="server" Text="Total No. Of Transformers :" Font-Bold="true"
                                    Font-Size="Medium" style="float:left"></asp:Label>
                                <a href="/MasterForms/TcMasterView.aspx?URLRedirect=true&Locationcode="'+hdfLocationCode.value+'"" target="_blank" style="font-weight: bold; color: Gray; font-size: medium">
                                <asp:Label ID="lblTotalDTR" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                    Font-Size="Medium"></asp:Label></a>--%>


                                    <asp:Label ID="lblDTRCount" runat="server" Text="Total No. Of Transformers :" Font-Bold="true"
                                    Font-Size="Medium" style="float:left"></asp:Label>
                              <asp:linkbutton ID="lblDTR" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                    Font-Size="Medium" OnClick="lnkChange_Click1"></asp:linkbutton>
                                <br />
                                <br />
                            </div>
                         
                           <%-- <div class="span2">
                               
                            </div>--%>
<%--                            <div class="">                                
                                <asp:LinkButton ID="lnkSLA_Dashboard" runat="server" OnClick="lnkSLA_Dashboard_Click"><span style= "color:CadetBlue;font-size:medium;font-weight:bold;/*margin-left:544px;*/float: right;"> SLA Dashboard </span></asp:LinkButton>
                            <br />                              
                                <asp:LinkButton ID="lnkMD_Dashboard" runat="server" OnClick="lnkMD_Dashboard_Click"><span style= "color:CadetBlue;font-size:medium;font-weight:bold;float:right;"> MD Dashboard </span></asp:LinkButton>
                            </div>--%>
                            <div class="span4">
                              <marquee id="mrqLeft" direction ="left" onmouseover="this.stop()" onmouseout="this.start()">
                              <a target="_blank" href="/DashboardForm/LatestUpdates.aspx" style="font-weight:bold;color:Red;cursor:pointer;text-decoration:none" >* Click here for Recent Changes Done in Appication *</a></marquee>
                               <%--<input type="button" id="btn" value="* Click Here to know UnAllocated DTC Details *" runat="server" onclick="LoadUnMapDetails_Click" />--%>
                            </div>
                            <div class="span4">
                                <%--<marquee  onmouseover="this.stop()" onmouseout="this.start()">
                              <a target="_blank" href="/DtcMissMatch/UnAllocateDetails.aspx" style="font-weight:bold;color:Red;cursor:pointer;text-decoration:none" >* Click Here to know UnAllocated DTC Details *</a></marquee>--%>
                                <%--<input type="button" id="btn" value="* Click Here to know UnAllocated DTC Details *" runat="server" onclick="LoadUnMapDetails_Click" />--%>
                            </div>

                            <div style="float: right;">
                            </div>

                        </div>
                    </div>
                </div>

                <br />

<%--starts hear--%>

                <div class="widget-body">
                    <div class="container-fluid">
                    <div class="row">
                       
                        <div style="margin-bottom:-26px;width:95%!important ; margin-left:35px;"
                            class="col-md-6 breadcrumb">

                            <asp:LinkButton ID="lnkFailurePend" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkFailurePend_Click"> <b>  </b></asp:LinkButton>

                            <%--<a href="#" >--%> <b>  Details of  DTr Failure Pending Replacement</b>
                               <asp:Label ID="lblDate" runat="server"></asp:Label><%--</a>--%>
                        </div>
                        
                       <%-- <div style="float: right!important"
                            class="col-md-6">

                            <asp:LinkButton ID="LinkButton5" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkFailurePend_Click"><b>  </b> </asp:LinkButton>--%>

                            <%--<a href="#" >--%> <%--<b> Details of Minor DTR Failure Pending Replacement</b>
                               <asp:Label ID="Label2" runat="server"></asp:Label>--%><%--</a>--%>
                        <%--</div>--%>
                      
                    </div>
                    </div>
                    <br />
                    <br />
                    

<%--i need to make changes hear--%>

                    <!--BEGIN METRO STATES-->
                    <div class="container-fluid">
                    <div class="row" style="margin-left:2px;">
                    <div class="metro-nav">
                        

                        <div class="metro-nav-block nav-block-orange col-md-1" style="background-color: #d9534f;width:15%">
                            <asp:LinkButton ID="Failure" runat="server" OnClick="Failure_Click" CssClass="auto-style2">
                                <i class="icon-bar-chart"></i>
                                <div class="info">
                                    <asp:Label ID="lblToatlPending" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending DTr Replacement</div>
                            </asp:LinkButton>
                        </div>

                       <%--<div class="metro-nav-block nav-block-orange col-md-1" style="background-color: #b065c2;width:15%">
                            <asp:LinkButton ID="lnkFailureApprove" runat="server" OnClick="lnkFailureApprove_Click">
                                <i class="icon-bell-alt"></i>
                                <div class="info">
                                    <asp:Label ID="lblFailureApprove" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Failure Approve</div>
                            </asp:LinkButton>
                        </div>--%>

                        <div class="metro-nav-block nav-block-grey col-md-1" style="background-color: #008B8B;width:15%">
                            <asp:LinkButton ID="estimation" runat="server" OnClick="estimation_Click">
                                <i class="icon-anchor"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingEstimation" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Estimation</div>
                            </asp:LinkButton>
                        </div>
                   
<%--added hear--%>

                       

                        <%--<div  style="background-color: #5cb85c;width:15%" class="metro-nav-block nav-block-blue  col-md-1";>
                            <asp:LinkButton ID="LnkSingleWO" runat="server" OnClick="Singleworkorder_Click" CssClass="auto-style2">
                                <i class="icon-suitcase"></i>
                                <div class="info">
                                    <asp:Label ID="lblSingleWO" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for WorkOrder</div>
                            </asp:LinkButton>
                        </div>
                        <div style="width:15%"class="metro-nav-block nav-block-orange col-md-1">
                            <asp:LinkButton ID="LnkReceiveTC" runat="server" OnClick="ReceiveTC_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblReceiveTC" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending Receive DTR</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-yellow col-md-1" style="background-color: #006400;width:15%">
                            <asp:LinkButton ID="LnkComission" runat="server" OnClick="Comission_Click">
                                <i class="icon-cloud"></i>
                                <div class="info">
                                    <asp:Label ID="lblComission" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Commission</div>
                            </asp:LinkButton>
                        </div> --%> 
                            
                        <div class="metro-nav">
                        <div class="metro-nav-block nav-block-blue col-md-1" style="width:15%!important; background-color: #5cb85c;">
                            <asp:LinkButton ID="LinkButton11" runat="server" OnClick="workorder_Click">
                                <i class="icon-suitcase"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingWO" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for WorkOrder</div>
                            </asp:LinkButton>
                        </div>

                        <div class="metro-nav-block nav-block-yellow col-md-1" style="width: 15%!important; background-color: #006400">
                            <asp:LinkButton ID="LinkButton10" runat="server" OnClick="invoice_Click">
                                <i class="icon-cloud"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingCommission" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Commission</div>
                            </asp:LinkButton>
                        </div>
                            


                         <div class="metro-nav-block nav-light-brown col-md-1" style="width: 15%!important; background-color: #f0ad4e">
                            <asp:LinkButton ID="LinkButton8" runat="server" OnClick="RI_Click">
                                <i class="icon-retweet"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingRI" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for RI </div>
                            </asp:LinkButton>
                        </div>


                        <div class="metro-nav-block nav-light-brown col-md-1" style="width: 15%!important; background-color: #402875">
                            <asp:LinkButton ID="LinkButton9" runat="server" OnClick="CR_Click">
                                <i class="icon-retweet"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingCR" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for  CR</div>
                            </asp:LinkButton>
                        </div>
                                                  
                    </div>
                    </div>
                    </div>
                    <div class="space10"></div>
                    <div class="space10"></div>

<%--I nead to edeit hear--%>

                   <%-- <div class="container-fluid">
                     <div class="row">

                    <ul class="breadcrumb" style="font-weight: bold">
                        <li>
                            <asp:LinkButton ID="LinkButton6" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkFailurePend_Click"> </asp:LinkButton>--%>

                          <%--<a href="#" >--%> <%-- Details  of Major DTR Failure Pending Replacement--%>
                               <%--<asp:Label ID="Label1" runat="server"></asp:Label>--%><%--</a>--%>                       

                      <%--  </li>

                    </ul>

                  
                    <div class="metro-nav">
                        <div class="metro-nav-block nav-block-blue col-md-1" style="width:15%!important; background-color: #5cb85c;">
                            <asp:LinkButton ID="workorder" runat="server" OnClick="workorder_Click">
                                <i class="icon-suitcase"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingWO" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for WorkOrder</div>
                            </asp:LinkButton>
                        </div>--%>


                        <%--<div class="metro-nav-block nav-block-blue col-md-1" style="width: 15%!important;">
                            <asp:LinkButton ID="indent" runat="server" OnClick="indent_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingIndent" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Indent</div>
                            </asp:LinkButton>
                        </div>--%>

                      <%--  <div class="metro-nav-block nav-block-yellow col-md-1" style="width: 15%!important; background-color: #006400">
                            <asp:LinkButton ID="invoice" runat="server" OnClick="invoice_Click">
                                <i class="icon-cloud"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingCommission" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Commission</div>
                            </asp:LinkButton>
                        </div>--%>

                       <%-- <div class="metro-nav-block nav-block-grey col-md-1" style="width: 15%!important;">
                            <asp:LinkButton ID="Decommissioning" runat="server" OnClick="Decommissioning_Click">
                                <i class="icon-bell"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingDeCommission" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Decommission</div>
                            </asp:LinkButton>
                        </div>--%>

                        <%--<div class="metro-nav-block nav-light-brown col-md-1" style="width: 15%!important; background-color: #f0ad4e">
                            <asp:LinkButton ID="RI" runat="server" OnClick="RI_Click">
                                <i class="icon-retweet"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingRI" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for RI </div>
                            </asp:LinkButton>
                        </div>--%>


                        <%--<div class="metro-nav-block nav-light-brown col-md-1" style="width: 15%!important; background-color: #f0ad4e">
                            <asp:LinkButton ID="LinkButton7" runat="server" OnClick="CR_Click">
                                <i class="icon-retweet"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingCR" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for  CR</div>
                            </asp:LinkButton>
                        </div>
                    </div>

                    </div>
                    </div>--%>

<%--ends hear--%>


                   <%-- <div class="space10"></div>
                    <div class="space10"></div>--%>

<%--I need to refer from jear--%>

                    <ul class="breadcrumb" style="font-weight: bold">
                        <li>
                            <%--<asp:Label ID="lblMonthwiseTitle" runat="server" ></asp:Label>--%>
                            <%--<asp:LinkButton ID="lnkDTCFailure" runat="server"  
                               >DTC Failure Trend</asp:LinkButton>--%>
                               
                            




                            Transformer Failure Trend
                        </li>
                        <li class="pull-right search-wrap">
                            <form action="search_result.html" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                </div>
                            </form>
                        </li>
                    </ul>

                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <div>

                                <asp:Chart ID="Chart1" runat="server">
                                    <Legends>
                                        <asp:Legend Name="Legend1"> <%-------> --%>
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Name="NewTitle">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series ChartType="Column">
                                        </asp:Series>
                                        <asp:Series ChartType="Column">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1"> <%------->--%>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>

                            </div>

                            <div class="span3">
                            </div>
                        </div>

                    </div>


                    <div class="space10"></div>
                    <div class="space10"></div>

<%--I need to modify from hear--%>

                    <ul class="breadcrumb" style="font-weight: bold">
                        <li>
                            <%--<asp:Label ID="lblMonthwiseTitle" runat="server" ></asp:Label>--%>
                            <%--<asp:LinkButton ID="lnkDTCFailure" runat="server"  
                               >DTC Failure Trend</asp:LinkButton>--%>
             
                            Transformer Failure Reason
                        </li>
                        <li class="pull-right search-wrap">
                            <form action="search_result.html" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                </div>
                            </form>
                        </li>
                    </ul>

                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <div>

                                <asp:Chart ID="FailureReasonChart" runat="server">
                                    <Legends>
                                        <asp:Legend Name="Legend2">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Name="NewTitle">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series ChartType="Column">
                                        </asp:Series>
                                       <%-- <asp:Series ChartType="Column">
                                        </asp:Series>--%>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea2">
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>

                            </div>

                            <div class="span3">
                            </div>
                        </div>

                    </div>


                    <div class="space10"></div>
                    <div class="space10"></div>


                    <ul class="breadcrumb" style="font-weight: bold">

                        <li>
                            <asp:LinkButton ID="lnkFaultyView" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkFaultyView_Click"></asp:LinkButton>
                            Faulty DTr Details
                           
                        </li>
                        <%--<li class="pull-right search-wrap">
                           <form action="search_result.html" class="hidden-phone">
                               <div class="input-append search-input-area">--%>
                        <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>

                        <%--</div>--%>

                        <%--<asp:LinkButton ID="LinkButton5" runat="server" ForeColor="#4a8bc2"
                                           onclick="lnkFailurePend_Click"  >View Details  </asp:LinkButton>--%>

                        <%--</form>
                       </li>--%>
                    </ul>

                    <div class="metro-nav">
                        <div class="metro-nav-block nav-block-orange" style="background-color: #d9534f">
                            <asp:LinkButton ID="LinkButton4" runat="server" OnClick="TotalFaulty_Click">
                                <i class="icon-ambulance"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalFaulty" runat="server"></asp:Label>
                                </div>
                                <div class="status">Total Faulty DTr</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-olive" style="background-color: #008B8B">
                            <asp:LinkButton ID="LinkButton3" runat="server" OnClick="Faulty_field_Click">
                                <i class="icon-ban-circle"></i>
                                <div class="info">
                                    <asp:Label ID="lblFaultyField" runat="server"></asp:Label>
                                </div>
                                <div class="status">Faulty DTr at Field</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-yellow" style="background-color: #5cb85c;">
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="Faulty_Store_Click" >
                                <i class="icon-home"></i>
                                <div class="info">
                                    <asp:Label ID="lblFaultyStore" runat="server"></asp:Label>
                                </div>
                                <div class="status">Faulty DTr at Store</div>
                            </asp:LinkButton>
                        </div>

                        <div class="metro-nav-block nav-light-blue" runat="server" id="dvReject">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="Faulty_Repairer_Click">
                                <i class="icon-hospital"></i>
                                <div class="info">
                                    <asp:Label ID="lblFaultyRepairer" runat="server"></asp:Label>
                                </div>
                                <div class="status">Faulty DTr at Repairer</div>
                            </asp:LinkButton>
                        </div>

                        <div class="metro-nav-block nav-block-blue" runat="server">
                            <asp:LinkButton ID="LinkButton" runat="server" OnClick="Tcfailed_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="LabelTcfailed" runat="server"></asp:Label>
                                </div>
                                <div class="status">Repaired Good DTr</div>
                            </asp:LinkButton>
                        </div>

                    </div>

                    <div class="space10"></div>
                    <div class="space10"></div>

                    <ul class="breadcrumb" style="font-weight: bold">

                        <li>
                            <asp:LinkButton ID="lnkTotalDtr" runat="server" ForeColor="#4a8bc2" ></asp:LinkButton>
                            DTR Details
                           
                        </li>
                        </ul>

                    <div class="metro-nav">
                        <div class="metro-nav-block nav-block-orange" style="background-color: #d9534f">
                            <asp:LinkButton ID="lnkTotalDtrDetails" runat="server" OnClick="lnkTotalDtrDetails_Click">
                                <i class="icon-ambulance"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalDtrDetails" runat="server"></asp:Label>
                                </div>
                                <div class="status">Total DTr</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-olive" style="background-color: #008B8B">
                            <asp:LinkButton ID="lnkTotalFieldDtr" runat="server" OnClick="lnkTotalFieldDtr_Click">
                                <i class="icon-ban-circle"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalFileldDtr" runat="server"></asp:Label>
                                </div>
                                <div class="status">DTr at Field</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-olive" style="background-color: #5cb85c;">
                            <asp:LinkButton ID="lnkTotalStoreDtr" runat="server" OnClick="lnkTotalStoreDtr_Click">
                                <i class="icon-home"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalStoreDtr" runat="server"></asp:Label>
                                </div>
                                <div class="status">DTr at Store</div>
                            </asp:LinkButton>
                        </div>

                        <div class="metro-nav-block nav-light-blue" runat="server" id="Div2">
                            <asp:LinkButton ID="lnkTotalRepairerDtr" runat="server" OnClick="lnkTotalRepairerDtr_Click">
                                <i class="icon-hospital"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalRepairerDtr" runat="server"></asp:Label>
                                </div>
                                <div class="status">DTr at Repairer</div>
                            </asp:LinkButton>
                        </div>

                        <div class="metro-nav-block nav-block-blue" runat="server">
                            <asp:LinkButton ID="lnkTotalBankDtr" runat="server" OnClick="lnkTotalBankDtr_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalBankDtr" runat="server"></asp:Label>
                                </div>
                                <div class="status">DTr at Bank</div>
                            </asp:LinkButton>
                        </div>

                    </div>



                    <div class="space10"></div>
                    <div class="space10"></div>
                    <ul class="breadcrumb" style="font-weight: bold">

                        <li>My Inbox Status
                           
                        </li>

                        <li class="pull-right search-wrap">
                            <form action="search_result.html" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                </div>

                            </form>
                        </li>
                    </ul>

                    <div class="metro-nav">
                        <div class="metro-nav-block nav-block-orange" style="background-color: #d9534f">
                            <a href="/Approval/ApprovalInbox.aspx" target="_blank">
                                <i class="icon-bar-chart"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalWorkflow" runat="server"></asp:Label>
                                </div>
                                <div class="status">Total Inbox Item</div>
                            </a>
                        </div>
                        <div class="metro-nav-block nav-olive" style="background-color: #008B8B">
                            <a href="/Approval/ApprovalInbox.aspx?RefType=1" target="_blank">
                                <i class="icon-shield"></i>
                                <div class="info">
                                    <asp:Label ID="lblApprovedWorkflow" runat="server"></asp:Label>
                                </div>
                                <div class="status">Approved</div>
                            </a>
                        </div>

                        <div class="metro-nav-block nav-block-yellow" style="background-color: #5cb85c;">
                            <a href="/Approval/ApprovalInbox.aspx" target="_blank">
                                <i class="icon-check-minus"></i>
                                <div class="info">
                                    <asp:Label ID="lblPendingWorkflow" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending </div>
                            </a>
                        </div>


                        <div class="metro-nav-block nav-light-blue" runat="server" id="Div1">
                            <a href="/Approval/ApprovalInbox.aspx?RefType=3" target="_blank">
                                <i class="icon-remove-sign"></i>
                                <div class="info">
                                    <asp:Label ID="lblRejectedWorkflow" runat="server"></asp:Label>
                                </div>
                                <div class="status">Rejected</div>
                            </a>
                        </div>
                    </div>
                    
                   
                    
                    <div class="space10"></div>
                    <div class="space10"></div>

                </div>
            </div>
        </div>

        <!--END METRO STATES-->
    </div>
    <div class="row-fluid">
        <!--BEGIN METRO STATES-->

    </div>

    <div class="row-fluid" style="visibility:collapse">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Transformer Failure Abstract Capacity Wise</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>
                    </span>
                </div>
                <div class="widget-body">
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span3">
                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:DropDownList ID="cmbCapacity" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbCapacity_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                            OnClick="Export_clickDTCFailureAbstract" /><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="grdDTCFailureAbstract"
                        AutoGenerateColumns="false" PageSize="10" ShowFooter="false"
                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                        runat="server"
                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnRowCommand="grdDTCFailureAbstract_RowCommand" 
                        OnPageIndexChanging="grdDashboard_PageIndexChanging" OnSorting="grdDTCFailureAbstract_Sorting" AllowSorting="true">
                        <HeaderStyle CssClass="both" />
                        <Columns>

                            <asp:TemplateField AccessibleHeaderText="Capacity" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                <ItemTemplate>
                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField AccessibleHeaderText="Section Code" HeaderText="Section Code" SortExpression="DF_LOC_CODE" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSectionCode" runat="server" Text='<%# Bind("DF_LOC_CODE") %>'></asp:Label>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Section Name" HeaderText="Office Name" SortExpression="SECTION">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblSectionName" runat="server" Text='<%# Bind("SECTION") %>' Width="200px" CommandName="View" Style="word-break: break-all"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField AccessibleHeaderText="Current Month" HeaderText="Current Month">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrectMonth" runat="server" Text='<%# Bind("CURRENTMONTH") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="PREVIOUS Month" HeaderText="PREVIOUS Month">
                                <ItemTemplate>
                                    <asp:Label ID="lblPreviousMonth" runat="server" Text='<%# Bind("PREVIOUSMONTH") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Current Quarter" HeaderText="Current Quarter">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrentQrter" runat="server" Text='<%# Bind("CURRENTQUARTER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Current Financial Year" HeaderText="Current Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrentFinancialYear" runat="server" Text='<%# Bind("FAILURECOUNTOFYEAR") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>


    <div class="row-fluid">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Transformer Failure Abstract</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>
                    </span>
                </div>
                <div class="widget-body">
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                 
                                  
                                    <div class="">
                                         <asp:Button ID="cmdBack" runat="server" Text="Back"
                                        CssClass="btn btn-danger" OnClick="cmdBack_Click" /><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <asp:GridView ID="grdDTCFailureAbstractoffice"
                        AutoGenerateColumns="false" PageSize="10" ShowFooter="false"
                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                        runat="server"
                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnRowCommand="grdDTCFailureAbstractoffice_RowCommand" 
                        OnPageIndexChanging="grdDTCFailureAbstractoffice_PageIndexChanging" OnSorting="grdDTCFailureAbstractoffice_Sorting" AllowSorting="true">
                        <HeaderStyle CssClass="both" />
                        <Columns>

                         
                            <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="OFF_CODE" SortExpression="OFF_CODE" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbloffcode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Office Name" HeaderText="OFFICE NAME" >
                                <ItemTemplate>
<%--                                    <asp:Label ID="lblSectionCode" runat="server" Text='<%# Bind("OFF_NAME") %>'></asp:Label>--%>
                            <asp:LinkButton ID="lblofficeName" runat="server" Text='<%# Bind("OFF_NAME") %>' Width="200px" CommandName="View" Style="word-break: break-all"></asp:LinkButton>
                               

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Previous Count" HeaderText="PREVIOUS YEAR COUNT" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPreviousCount" runat="server" Text='<%# Bind("PREVIOUSCOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField AccessibleHeaderText="Present Count" HeaderText="PRESENT YEAR COUNT">
                                <ItemTemplate>
                                    <asp:Label ID="lblPresentCount" runat="server" Text='<%# Bind("PRESENTCOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="TOTA_DTC_FAILURE" HeaderText="TOTAL FAILURE">
                                <ItemTemplate>
                                    <asp:Label ID="lbltotaldtcfailure" runat="server" Text='<%# Bind("TOTAL_DTCCOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                         

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
            <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>

    <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="imgbtnClose"
        PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
    <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
        <div style="display: none">
            <asp:Button ID="btnshow" runat="server" Text="Button" />
        </div>
        <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="362px" Width="534px">
            <div class="widget blue">
                <div class="widget-title">
                    <h4>Select Office</h4>
                    <div style="float: right">
                        <asp:ImageButton ID="imgbtnClose" runat="server" ImageUrl="~/img/Manual/close1.png" Width="40px" />
                    </div>
                    <div class="space20"></div>
                    <%--<div class="row-fluid">--%>
                    <div class="span1"></div>
                    <div class="space20">
                        <div class="span1"></div>

                        <div class="span6">

                            <asp:GridView ID="grdOffices" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" ShowFooter="true"
                                ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                PageSize="6" AllowPaging="True"
                                OnRowCommand="grdOffices_RowCommand"
                                OnPageIndexChanging="grdOffices_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Office Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>' Style="word-break: break-all" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="70px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffName" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Office Name" Width="200px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="submit">Select</asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>

                            <div class="space20" align="center">

                                <div class="form-horizontal" align="center">
                                    <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>

                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
                <div class="space20"></div>
                <div class="space20"></div>

            </div>
        </asp:Panel>
    </div>

</asp:Content>
