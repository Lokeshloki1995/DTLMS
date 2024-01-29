<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RepairerTransformer.aspx.cs" Inherits="IIITS.DTLMS.Reports.RepairerTransformer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../Scripts/functions.js" type="text/javascript"></script>
    <script  type="text/javascript">
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Repairer Performance Report
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
                <div style="float:right;margin-top:20px;margin-right:12px" >           
              </div>
                </div>
                 <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Select Location
                                 <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px;padding:0px 0px 0px 0px!important"></i></a> 
                            </h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
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
                                                    Zone </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                          
                                           <!--   <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>-->

                                              

                                              <div class="control-group">
                                                <label class="control-label">
                                                    From Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Capacity </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                          
                                        </div>
                                        <div class="span5">
                                             <div class="control-group">
                                                <label class="control-label">
                                                    Circle </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        
                                           
                                        
                                         

                                               <div class="control-group">
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                  
                                        </div>
                                       
                                    </div>
                                </div>
                            </div>
                         
                            <!-- END FORM-->
                        </div>
                    </div>
                    </div>
                    </div>



                        <div class="row-fluid">
                        <div class="span12">

                        <div class="widget blue">
                        <div class="widget-title">
                         <h4> <i class="icon-reorder"></i>Report Type </h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
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
                        <label class="control-label">Report Type<span class="Mandotary"> *</span></label>
                        
                        <div class="controls">
                        <asp:DropDownList ID="cmbReportType" runat="server" CssClass="input_select" >
                        <asp:ListItem Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Pending Analysis Report" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Delivered Analysis Report" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        </div>
                        </div>

                          <div class="span5"> 
                         <div class="control-group">
                        <label class="control-label">Reprier Name</label>

                        <div class="controls">
                        <asp:DropDownList ID="cmbRepairerName" runat="server" CssClass="input_select" 
                        AutoPostBack ="true">
                        </asp:DropDownList>
                        </div>
                        </div>
                        </div>


                        </div>
                        </div>
                        <!-- END SAMPLE FORM PORTLET-->
                        </div>
                        </div>
          
                        <!-- END PAGE CONTENT-->
                        </div>

                       

                        </div>
                        </div>

                           <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" >
                                      
                                            <asp:Button ID="cmdGenerate" runat="server" Text="Generate" 
                                                CssClass="btn btn-primary" TabIndex="10" onclick="cmdGenerate_Click" />
                                    
                                        
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                TabIndex="11" onclick="cmdReset_Click"/>
                             
                              
                                                <asp:Button ID="Button1" runat="server" Text="Export to Excel"  CssClass="btn btn-info" 
                                                    TabIndex="12" onclick="Export_click" /><br />
                                    
                                    
                                      
                         
                            </div>
                              </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>




                    <!-- END SAMPLE FORM PORTLET-->
                </div>
          <!-- BEGIN PAGE CONTENT-->
             <div class="">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Repairer Estimation/Workorder Details </h4>
                            <%--<span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>--%>
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
                                                    Zone
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone4" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbZone4_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle1" runat="server" AutoPostBack="true" TabIndex="2"
                                                            OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv1" runat="server" AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="cmbDiv1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate1" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">
                                                    Transformer Make</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake1" runat="server" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:Label ID="lblCoilType" class="control-label" runat="server" Text="Select Coil Type"></asp:Label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCoilType" runat="server" TabIndex="11">
                                                            <asp:ListItem Value="0">--select--</asp:ListItem>
                                                               <asp:ListItem Value="1">Single Coil</asp:ListItem>
                                                                <asp:ListItem Value="2">Multi Coil</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <asp:Label ID="lblStar" class="control-label" runat="server">Select Rating Type</asp:Label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStarType" runat="server" TabIndex="15">
                                                             <asp:ListItem  Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem  Value="1">Star Rate</asp:ListItem>
                                                            <asp:ListItem  Value="2">Conventional</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span5">


                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv1" runat="server" AutoPostBack="true" TabIndex="4"
                                                            OnSelectedIndexChanged="cmbSubDiv1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    O & M Section</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection1" runat="server" TabIndex="5">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                           

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Seclect Capacity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity2" runat="server" TabIndex="6">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate1" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender6" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Gurantee Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbguranty1" runat="server" TabIndex="13">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <asp:Label ID="lblwoundType" class="control-label" runat="server">Select Winding Type</asp:Label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbwound" runat="server" TabIndex="17">
                                                           <asp:ListItem Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Aluminium Winding</asp:ListItem>
                                                            <asp:ListItem Value="2">Copper Winding</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <asp:Label ID="lblstages" class="control-label" runat="server">Select Stage<span class="Mandotary">*</span></asp:Label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStage1" runat="server" TabIndex="12">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Repairer Estimation</asp:ListItem>
                                                            <asp:ListItem Value="2">Repairer WorkOrder</asp:ListItem>                                                            
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="space20">
                                        </div>
                                        <div style="padding-left: 160px;">
                                            <asp:RadioButtonList ID="rdbReportType" runat="server" CssClass="radio" RepeatDirection="Horizontal" Width="900px">
                                                <asp:ListItem Value="1" Selected="True">TC Failed</asp:ListItem>
                                                <asp:ListItem Value="2">WorkOrdered</asp:ListItem>
                                                <asp:ListItem Value="3">Indent</asp:ListItem>
                                                <asp:ListItem Value="4">Invoice</asp:ListItem>
                                                <asp:ListItem Value="5">Decommission</asp:ListItem>
                                                <asp:ListItem Value="6">Return Invoice</asp:ListItem>
                                                <asp:ListItem Value="7">CR Pending</asp:ListItem>
                                                <asp:ListItem Value="8">CR Completed</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="span5">--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">

                            <div class="text-center" align="center">
                                                                                   
                                <asp:Button ID="Button9" runat="server" Text="Export Excel" CssClass="btn btn-primary" OnClientClick="javascript:return ValidateMyForm()"
                                    TabIndex="12" OnClick="Export_click4" />
                                
                                 <asp:Button ID="Button8" runat="server" Text="Reset" CssClass="btn btn-danger"
                                    TabIndex="11" OnClick="cmdReset1_Click" />
                                     

                                <asp:Label ID="lblmsg1" runat="server" ForeColor="Red"></asp:Label>
                            </div><br />

                        </div>
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>

    </div>
   
        <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* This Report Will Display Repairer Performance</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Select Circle, division, FromDate, ToDate,capacity </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Select Report Type , there is 2 options in it </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* If you select Pending Analysis report. Pending TC's at Repairer records will display </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* If you select Delivered Analysis report. Delevered TC's Repairer records will display </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>

