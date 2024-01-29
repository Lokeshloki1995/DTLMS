<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Dashboard_Old.aspx.cs" Inherits="IIITS.DTLMS.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
         .ascending th a {
        background:url(/img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(/img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
         background: url(/img/sort_both.png) no-repeat;
         display: block;
          padding: 0 4px 0 20px;
     }
     
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
 <div class="container-fluid">
 <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
               
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Dashboard
                   </h3>
                
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
             <div style="float:right;margin-top:20px;margin-right:12px" >
                     <a target="_blank" href="/DashboardForm/DownLoad.aspx" style="font-size:medium">Download </a><span> (User Manual & Android App)</span>
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
                                     <asp:Label ID="lblLocation" runat="server"  Font-Bold="true"  ForeColor="CadetBlue"
                                        Font-Size="Medium"></asp:Label>
                                        <br /><br />
                                         <a  href="/MasterForms/DTCView.aspx" target="_blank" style="font-weight:bold;color:Gray;font-size:medium">Total No. Of DTC :</a>
                                       <%-- <asp:Label ID="Label1" runat="server" Text="Total No. Of DTC :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>--%>
                                     <asp:Label ID="lblTotalDTC" runat="server"  Font-Bold="true"  ForeColor="CadetBlue" 
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span2">
                              <asp:HiddenField ID="hdfLocationCode" runat="server" />
                              <asp:LinkButton ID="lnkChange" runat="server" onclick="lnkChange_Click">Select Location</asp:LinkButton>
                          </div>
                          <div class="span4">
                              <%--<marquee  onmouseover="this.stop()" onmouseout="this.start()">
                              <a target="_blank" href="/DtcMissMatch/UnAllocateDetails.aspx" style="font-weight:bold;color:Red;cursor:pointer;text-decoration:none" >* Click Here to know UnAllocated DTC Details *</a></marquee>--%>
                               <%--<input type="button" id="btn" value="* Click Here to know UnAllocated DTC Details *" runat="server" onclick="LoadUnMapDetails_Click" />--%>
                            </div>

                             <div style="float:right;">
                             
                             </div>

                      </div>
                        </div>
                     </div>

                        <div class="widget-body">

                      <ul class="breadcrumb" style="font-weight:bold">
                       <li>
                           <asp:LinkButton ID="lnkFailurePend" runat="server" ForeColor="#4a8bc2"
                                           onclick="lnkFailurePend_Click"  >View Details </asp:LinkButton>

                           <%--<a href="#" >--%>  of  DTC Failure Pending Replacement
                               <asp:Label ID="lblDate" runat="server" ></asp:Label><%--</a>--%>
                           
                       </li>                      
                       <%--<li class="pull-right search-wrap">
                           <form action="search_result.html" class="hidden-phone">
                               <div class="input-append search-input-area">--%>
                                  <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                  
                               <%--</div>
                                 <asp:LinkButton ID="lnkFailurePend" runat="server" ForeColor="#4a8bc2"
                                           onclick="lnkFailurePend_Click"  >View Details  </asp:LinkButton>
                              
                           </form>
                       </li>--%>
                   </ul>

                <!--BEGIN METRO STATES-->
                <div class="metro-nav">

                 <div class="metro-nav-block nav-block-orange" style="background-color:#d9534f">
                 <asp:LinkButton ID="Failure" runat="server" onclick="Failure_Click"><i class="icon-bar-chart"></i><div class="info"><asp:Label ID="lblToatlPending" runat="server" ></asp:Label></div><div class="status">Pending DTC For Replacement</div></asp:LinkButton>
                    </div>

                  <div class="metro-nav-block nav-block-grey" style="width:166px;background-color:#008B8B" >
                  <asp:LinkButton ID="estimation" runat="server" onclick="estimation_Click" ><i class="icon-anchor"></i><div class="info"> <asp:Label ID="lblPendingEstimation" runat="server" ></asp:Label></div><div class="status" >Pending for Estimation</div></asp:LinkButton>
                    </div>

                     <div class="metro-nav-block nav-block-blue" style="width:166px;background-color:#5cb85c;">
                     <asp:LinkButton ID="workorder" runat="server" onclick="workorder_Click" ><i class="icon-suitcase"></i><div class="info"> <asp:Label ID="lblPendingWO" runat="server" ></asp:Label></div><div class="status" >Pending for WorkOrder</div></asp:LinkButton>
                    </div>
                    <div class="metro-nav-block nav-block-blue" style="width:148px;">
                     <asp:LinkButton ID="indent" runat="server" onclick="indent_Click" ><i class="icon-inbox"></i><div class="info"><asp:Label ID="lblPendingIndent" runat="server" ></asp:Label></div><div class="status">Pending for Indent</div></asp:LinkButton>
                    </div>
                    <div class="metro-nav-block nav-block-yellow" style="width:172px; background-color: #006400">
                    <asp:LinkButton ID="invoice" runat="server" onclick="invoice_Click" > <i class="icon-cloud"></i><div class="info" ><asp:Label ID="lblPendingCommission" runat="server" ></asp:Label></div><div class="status">Pending for Commission</div></asp:LinkButton>
                    </div>

                    <div class="metro-nav-block nav-block-grey" style="width:192px;">
                    <asp:LinkButton ID="Decommissioning" runat="server" OnClick="Decommissioning_Click"  ><i class="icon-bell"></i><div class="info" ><asp:Label ID="lblPendingDeCommission" runat="server" ></asp:Label></div><div class="status">Pending for DeCommission</div></asp:LinkButton>
                    </div>

                    <div class="metro-nav-block nav-light-brown" style="width:171px;background-color:#f0ad4e">
                    <asp:LinkButton ID="RI" runat="server" onclick="RI_Click" > <i class="icon-retweet"></i><div class="info"><asp:Label ID="lblPendingRI" runat="server" ></asp:Label></div><div class="status">Pending for RI / CR</div></asp:LinkButton>
                    </div>

                </div>

                
                  <div class="space10"></div>
                   <div class="space10"></div>

                    <ul class="breadcrumb" style="font-weight:bold">
                       <li>
                            <%--<asp:Label ID="lblMonthwiseTitle" runat="server" ></asp:Label>--%>
                           <%--<asp:LinkButton ID="lnkDTCFailure" runat="server"  
                               >DTC Failure Trend</asp:LinkButton>--%>
                               DTC Failure Trend
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
                              
                               <asp:Chart ID="Chart1" runat="server" >
                                   <Legends>
                                    <asp:Legend Name="Legend1">
                                    </asp:Legend>
                                  </Legends>
                                    <Titles>
                                        <asp:Title Name="NewTitle">
                                        </asp:Title>
                                     </Titles>
                                    <Series>
                                        <asp:Series ChartType="Column"  >
                                        </asp:Series>
                                        <asp:Series ChartType="Column">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
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




                 <ul class="breadcrumb" style="font-weight:bold">

                       <li>
                           <asp:LinkButton ID="lnkFaultyView" runat="server" ForeColor="#4a8bc2" 
                            onclick="lnkFaultyView_Click"  >View Details </asp:LinkButton>
                            of  Faulty DTR Details
                           
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
                    <div class="metro-nav-block nav-block-orange" style="background-color:#d9534f">
                       <asp:LinkButton ID="LinkButton4" runat="server" onclick="TotalFaulty_Click" > <i class="icon-ambulance"></i><div class="info"> <asp:Label ID="lblTotalFaulty" runat="server" ></asp:Label></div><div class="status" >Total Faulty DTr</div></asp:LinkButton>
                    </div>
                    <div class="metro-nav-block nav-olive" style="background-color:#008B8B">
                       <asp:LinkButton ID="LinkButton3" runat="server" onclick="Faulty_field_Click" > <i class="icon-ban-circle"></i><div class="info"><asp:Label ID="lblFaultyField" runat="server" ></asp:Label></div><div class="status">Faulty DTr at Field</div></asp:LinkButton>
                    </div>
                    <div class="metro-nav-block nav-block-yellow" style="background-color:#5cb85c;">
                      <asp:LinkButton ID="LinkButton2" runat="server" onclick="Faulty_Store_Click" > <i class="icon-home"></i><div class="info"><asp:Label ID="lblFaultyStore" runat="server" ></asp:Label></div><div class="status">Faulty DTr at Store</div></asp:LinkButton>
                    </div>

                     <div class="metro-nav-block nav-light-blue" runat="server" id="dvReject" >
                        <asp:LinkButton ID="LinkButton1" runat="server" onclick="Faulty_Repairer_Click" > <i class="icon-hospital"></i><div class="info"><asp:Label ID="lblFaultyRepairer" runat="server" ></asp:Label></div><div class="status">Faulty DTr at Repairer</div></asp:LinkButton>
                    </div>

                     <div class="metro-nav-block nav-block-blue" runat="server">
                     <asp:LinkButton ID="LinkButton" runat="server" onclick="Tcfailed_Click" > <i class="icon-inbox"></i><div class="info"><asp:Label ID="LabelTcfailed" runat="server" ></asp:Label></div><div class="status">Repaired and Good dtr</div></asp:LinkButton>
                    </div>

                 </div>
               

                  <div class="space10"></div>
                   <div class="space10"></div>
                 <ul class="breadcrumb" style="font-weight:bold">
              
                       <li>
                        My Inbox Status
                           
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
                    <div class="metro-nav-block nav-block-orange" style="background-color:#d9534f">
                        <a  href="/Approval/ApprovalInbox.aspx" target="_blank">
                            <i class="icon-bar-chart"></i>
                            <div class="info"> <asp:Label ID="lblTotalWorkflow" runat="server" ></asp:Label></div>
                            <div class="status" >Total Inbox Item</div>
                        </a>
                    </div>
                     <div class="metro-nav-block nav-olive" style="background-color:#008B8B">
                        <a  href="/Approval/ApprovalInbox.aspx?RefType=1" target="_blank">
                            <i class="icon-shield"></i>
                            <div class="info"><asp:Label ID="lblApprovedWorkflow" runat="server" ></asp:Label></div>
                            <div class="status">Approved</div>
                        </a>
                    </div>

                    <div class="metro-nav-block nav-block-yellow" style="background-color:#5cb85c;">
                        <a  href="/Approval/ApprovalInbox.aspx" target="_blank">
                            <i class="icon-check-minus"></i>
                            <div class="info"><asp:Label ID="lblPendingWorkflow" runat="server" ></asp:Label></div>
                            <div class="status">Pending </div>
                        </a>
                    </div>
                   

                     <div class="metro-nav-block nav-light-blue" runat="server" id="Div1" >
                        <a  href="/Approval/ApprovalInbox.aspx?RefType=3" target="_blank">
                            <i class="icon-remove-sign"></i>
                            <div class="info"><asp:Label ID="lblRejectedWorkflow" runat="server" ></asp:Label></div>
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

              <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i> DTC Failure Abstract</h4>
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
                                 <div  class="span5">                        
                                   <div class="control-group">
                                        <label class="control-label">Capacity(in KVA)</label>
                                        <div class="controls">
                                            <div class="input-append">
                                   
                                                 <asp:DropDownList ID="cmbCapacity" runat="server"  AutoPostBack="true"
                                                        onselectedindexchanged="cmbCapacity_SelectedIndexChanged" >
                                                 </asp:DropDownList>   
                                             </div>
                                         </div> 
                                  </div>
                         
                            </div>
                                 <div  class="span5">
                                    <div class="control-group">
                                        <label class="control-label">Section</label>
                                        <div class="controls">
                                           <div class="input-append">                                   
                                             <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true"
                                                    onselectedindexchanged="cmbSection_SelectedIndexChanged" >
                                             </asp:DropDownList>                             
                                            </div>
                                        </div> 
                                    </div>
                            </div>
                                     <div class="">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickDTCFailureAbstract" /><br />
                                          </div>
                              </div>   
                             </div>
                      </div>
                   </div>                                                                        
                        <asp:GridView ID="grdDTCFailureAbstract" 
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="false"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" 
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" 
                                    onpageindexchanging="grdDashboard_PageIndexChanging"  OnSorting="grdDTCFailureAbstract_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                          
                                     <asp:TemplateField AccessibleHeaderText="Capacity" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="Section Code" HeaderText="Section Code" SortExpression="DF_LOC_CODE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblSectionCode" runat="server" Text='<%# Bind("DF_LOC_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="Section Name" HeaderText="Section Name" SortExpression="SECTION">                                    
                                        <ItemTemplate>
                                            <asp:Label ID="lblSectionName" runat="server" Text='<%# Bind("SECTION") %>' Width="200px" style="word-break:break-all"></asp:Label>
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

                                     <asp:TemplateField AccessibleHeaderText="Current Financial Year" HeaderText="Current Financial Year">
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

               <ajax:modalpopupextender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="imgbtnClose"
                                  PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                        <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                                <div style="display:none">
                                    <asp:Button ID="btnshow" runat="server" Text="Button"  />
                                 </div>
                            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="362px"  Width="534px">
                                 <div class="widget blue">
                                     <div class="widget-title" >
                                          <h4>Select Office</h4>
                                           <div style="float:right">
                                             <asp:ImageButton ID="imgbtnClose" runat="server" ImageUrl="~/img/Manual/close1.png" Width="40px" />
                                           </div>
                                       <div class="space20"></div>
                                         <%--<div class="row-fluid">--%>
                                        <div class="span1"></div>
                                     <div class="space20" >
                                <div class="span1"></div>              
   
                          <div class="span6">
                                    
                              <asp:GridView ID="grdOffices" AutoGenerateColumns="false" 
                                   CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="true"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                    PageSize="6" AllowPaging="True" 
                                  onrowcommand="grdOffices_RowCommand" 
                                  onpageindexchanging="grdOffices_PageIndexChanging"  >
                                  <Columns>
                                    <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Office Code" >                                  
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>' style="word-break:break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="100px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name" >                                  
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffName" runat="server" Text='<%# Bind("OFF_NAME") %>' style="word-break:break-all" Width="150px"> </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Office Name" Width="200px" ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Select" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lnkSelect" runat="server" CommandName="submit">Select</asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                     </FooterTemplate>
                                  </asp:TemplateField>

                                   </Columns>
                             </asp:GridView>

                                  <div class="space20" align="center">

                                  <div  class="form-horizontal" align="center"> 
                                         <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red" ></asp:Label>  
                                       
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
