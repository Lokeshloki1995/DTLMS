<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreDashboard.aspx.cs" Inherits="IIITS.DTLMS.StoreDashboard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <style type="text/css">
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

        .auto-style1 {
            left: 0px;
            top: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    StoreDashboard<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Dashboard
                </h3>

                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
<%--            <div style="float: right; margin-top: 20px; margin-right: 12px">
                <a target="_blank" href="/DashboardForm/DownLoad.aspx" style="font-size: medium">Download </a><span>(User Manual & Android App)</span>
            </div>--%>
        </div>
    </div>
    <!-- END PAGE HEADER-->
    <!-- BEGIN PAGE CONTENT-->
    <div class="row-fluid">

        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
           
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Dashboard</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>
                    </span>
                </div>



                <div class="widget-body">
                    <div class="form-horizontal">
                        <div class="row-fluid">

                            <div class="span4">
                                <asp:Label ID="lblStatus" runat="server" Text="Location :" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                                <asp:Label ID="lblLocation" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                    Font-Size="Medium"></asp:Label>
                                <br />
                                <br />
                                
                            </div>
                            <%--<div class="span2">
                                <asp:HiddenField ID="hdfLocationCode" runat="server" />
                                <div class="span2" style="/*float: right;*/ margin-top: 20px; /*margin-right: 12px*/"> 
                <a target="_blank" href="/DashboardForm/DownLoad.aspx"   style="font-size:12px;color:Blue;font-weight:bold">Download </a><span style="font-size:12px">(User Manual)</span>
            
                
            </div>
                            </div>--%>
                              <asp:HiddenField ID="hdfLocationCode" runat="server" />
                                <div class="span2" style="float: right; margin-top: -5px; margin-right: 12px"> 
                <a target="_blank" href="/DashboardForm/DownLoad.aspx"   style="font-size:12px;color:Blue;font-weight:bold">Download </a><span style="font-size:12px">(User Manual)</span>
            
                
            </div>
                            <div class="span4">
                                <%--<marquee  onmouseover="this.stop()" onmouseout="this.start()">
                              <a target="_blank" href="/DtcMissMatch/UnAllocateDetails.aspx" style="font-weight:bold;color:Red;cursor:pointer;text-decoration:none" >* Click Here to know UnAllocated DTC Details *</a></marquee>--%>
                                <%--<input type="button" id="btn" value="* Click Here to know UnAllocated DTC Details *" runat="server" onclick="LoadUnMapDetails_Click" />--%>
                            </div>

                            <div style="float: right;">
                            </div>
<%--							     <div class="">                                
                                <asp:LinkButton ID="lnkMD_Dashboard" runat="server" OnClick="lnkMD_Dashboard_Click"><span style= "color:CadetBlue;font-size:medium;font-weight:bold;margin-left:500px"> MD Dashboard </span></asp:LinkButton>
                            </div>--%>

                        </div>
                    </div>
                </div>

                <div class="widget-body">
                    <div class="">
                        <ul class="breadcrumb" style="font-weight: bold">
                            <li>
                   
                        <%--<div style="margin-bottom: -19px; padding: 0px 0px 0px 20px;"
                            class="col-md-6">--%>

                            <asp:LinkButton ID="lnkFailurePend" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkConditionPending"> <b>  </b></asp:LinkButton>

                            <%--<a href="#" >--%> <b> Transformer In Store</b>
                               <asp:Label ID="lblDate" runat="server"></asp:Label><%--</a>--%>
                       </li>
                            </ul>
                        
                    </div>
                    </div>
                    
                    <!--BEGIN METRO STATES-->
                    <div class="container-fluid">
                    <div class="row">
                    <div class="metro-nav">

                        <div class="metro-nav-block nav-block-orange col-md-3" style="background-color: #d9534f">
                            <asp:LinkButton ID="newTC" runat="server" OnClick="NewTC_Click">
                                <i class="icon-bar-chart"></i>
                                <div class="info">
                                    <asp:Label ID="lblNewTC" runat="server"></asp:Label>
                                </div>
                                <div class="status">New</div>
                            </asp:LinkButton>
                        </div>

                        <div class="metro-nav-block nav-block-grey col-md-3"style="background-color:#008B8B">
                            <asp:LinkButton ID="RepairGood" runat="server" OnClick="RepairGood_Click">
                                <i class="icon-anchor"></i>
                                <div class="info">
                                    <asp:Label ID="lblRepairGood" runat="server"></asp:Label>
                                </div>
                                <div class="status">Repair Good</div>
                            </asp:LinkButton>
                        </div>
                 
                        <div  class="metro-nav-block nav-block-blue col-md-3" style="background-color: #5cb85c;">
                            <asp:LinkButton ID="LnkReleaseGood" runat="server" OnClick="ReleaseGood_Click">
                                <i class="icon-suitcase"></i>
                                <div class="info">
                                    <asp:Label ID="lblReleaseGood" runat="server"></asp:Label>
                                </div>
                                <div class="status">Release Good</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-orange col-md-2" >
                            <asp:LinkButton ID="LnkFaulty" runat="server" OnClick="Faulty_Click">
                                <i class="icon-ambulance"></i>
                                <div class="info">
                                    <asp:Label ID="lblFaulty" runat="server"></asp:Label>
                                </div>
                                <div class="status">Faulty</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-yellow col-md-2"  style="background-color: #006400">
                            <asp:LinkButton ID="LnkMobileTC" runat="server" OnClick="ScrapTC_Click">
                                <i class="icon-cloud"></i>
                                <div class="info">
                                    <asp:Label ID="lblScrapTC" runat="server"></asp:Label>
                                </div>
                                <div class="status">Scrap</div>
                            </asp:LinkButton>
                        </div>                        
                    </div>
                    </div>
                    </div>
                    <div class="space10"></div>
                    <div class="space10"></div>


                    <ul class="breadcrumb" style="font-weight: bold">
                        <li>
                            <asp:LinkButton ID="LinkButton6" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkCapacityWiseTransformer_Click"> </asp:LinkButton>

                            <%--<a href="#" >--%>  Details  of Capacity wise Transformer
                               <asp:Label ID="Label1" runat="server"></asp:Label><%--</a>--%>                        

                        </li>

                    </ul>

                    <div class="metro-nav">
                           <div class="metro-nav-block nav-block-blue " style="background-color: #1EDBDE;">
                            <asp:LinkButton ID="Capacityless25" runat="server" OnClick="Capacityless25_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblCapacityless25" runat="server"></asp:Label>
                                </div>
                                <div class="status"><25 KVA</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-blue " style=" background-color: #5cb85c;">
                            <asp:LinkButton ID="Capacity25_100" runat="server" OnClick="Capacity25_100_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblCapacity25_100" runat="server"></asp:Label>
                                </div>
                                <div class="status">25-100 KVA</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-blue" >
                            <asp:LinkButton ID="Capacity125_250" runat="server" OnClick="Capacity125_250_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblCapacity125_250" runat="server"></asp:Label>
                                </div>
                                <div class="status">125-250 KVA</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-yellow" style="background-color: #818EBB">
                            <asp:LinkButton ID="Capacitygreater250" runat="server" OnClick="Capacitygreater250_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblCapacitygreater250" runat="server"></asp:Label>
                                </div>
                                <div class="status">>250 KVA</div>
                            </asp:LinkButton>
                        </div>

                    </div>



                    <div class="space10"></div>
                    <div class="space10"></div>


              <ul class="breadcrumb" style="font-weight: bold">
                        <li>
                            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkbank_Click"> </asp:LinkButton>

                            <%--<a href="#" >--%>   Transformer In Bank
                               <asp:Label ID="Label2" runat="server"></asp:Label><%--</a>--%>                        

                        </li>

                    </ul>

                    <div class="metro-nav">
                           <div class="metro-nav-block nav-block-blue " style="background-color: #1EDBDE;">
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_Good_click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblGood" runat="server"></asp:Label>
                                </div>
                                <div class="status"> GOOD</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-blue " style=" background-color: #5cb85c;">
                            <asp:LinkButton ID="LinkButton3" runat="server"  OnClick="lnk_Release_Good_click" >
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblReleaseGood1" runat="server"></asp:Label>
                                </div>
                                <div class="status"> RELEASE GOOD</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-blue" >
                            <asp:LinkButton ID="LinkButton4" runat="server"  OnClick="lnk_Repair_Good_click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblRepairgood1" runat="server"></asp:Label>
                                </div>
                                <div class="status"> REPAIR GOOD</div>
                            </asp:LinkButton>
                        </div>

                    </div>



                    <div class="space10"></div>
                    <div class="space10"></div>



                    <ul class="breadcrumb" style="font-weight: bold">

                        <li>
                            <asp:LinkButton ID="lnkFaultyView" runat="server" ForeColor="#4a8bc2"
                                OnClick="lnkTransformer_Click"></asp:LinkButton>
                             Details of  Transformer
                           
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
                            <asp:LinkButton ID="TotalPendingfor_Issue" runat="server" OnClick="TotalPendingfor_Issue_Click">
                                <i class="icon-inbox"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalPendingfor_Issue" runat="server"></asp:Label>
                                </div>
                                <div class="status"> Pending for Issue</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-olive" style="background-color: #008B8B">
                            <asp:LinkButton ID="TotalPendingfor_Repair" runat="server" OnClick="TotalPendingfor_Repair_Click">
                                <i class="icon-suitcase"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalPendingfor_Repair" runat="server"></asp:Label>
                                </div>
                                <div class="status">Pending for Repair</div>
                            </asp:LinkButton>
                        </div>
                        <div class="metro-nav-block nav-block-yellow" style="background-color: #5cb85c;">
                            <asp:LinkButton ID="TotalPendingto_Recive" runat="server" OnClick="TotalPendingto_Recive_Click">
                                <i class="icon-home"></i>
                                <div class="info">
                                    <asp:Label ID="lblTotalPendingto_Recive" runat="server"></asp:Label>
                                </div>
                                <div class="status"> Pending to Recive</div>
                            </asp:LinkButton>
                        </div>

                  

                    </div>

                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

               
           
       

        <!--END METRO STATES-->
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
                   
                </div>
                <div class="space20"></div>
                <div class="space20"></div>

            </div>
        </asp:Panel>
    </div>
    </div>
</asp:Content>