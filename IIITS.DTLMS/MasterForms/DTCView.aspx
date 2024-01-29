<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTCView" %>
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
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     DTC View
                   </h3>
        <%--               <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                                          <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Transformer Centre View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                              
                            <div style="float:right" >
                                <div class="span1">
                                   <asp:Button ID="cmdNew" runat="server" Text="New Transformer Centre Commission" Visible="false"
                                              CssClass="btn btn-primary" onclick="cmdNew_Click" /></div>
                                <div class="span6">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickDTCMaster" /><br />
                                          </div>


                               </div>
                               
                         <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">

                                <div id="Details" runat="server" class="row-fluid" style="display:none">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">


                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged"
                                                           >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged"
                                                            >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
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
                                                        <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged"
                                                            ></asp:DropDownList>                                                        
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">
                                                    OMU </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOMSection" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Station </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStation" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" Visible="True"
                                         OnClick="cmdLoad_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    
                                        <div class="span1">
                                        </div>
                                    </div>
                                 <div class="space20">
                                        </div>
                                <div class="row-fluid">
                               <%-- <div class="span1"></div>--%>
                                    <div class="span1">
                                        </div>
                               <div class="span5">
                                  <div class="control-group">
                                   <label class="control-label">Feeder Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbFeeder" runat="server"  TabIndex="9" 
                                                   AutoPostBack="true" onselectedindexchanged="cmbFeeder_SelectedIndexChanged">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
                             </div>

                             <div class="span5">
  
                             <div class="control-group">
                                <label class="control-label">Project/SchemeType</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbProjectType" runat="server"  TabIndex="9" AutoPostBack="true"
                                               onselectedindexchanged="cmbProjectType_SelectedIndexChanged">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
               
                               <div class="span5">
                                    
                                </div> 
                               </div>            
                                    </div>

                                     <asp:Label ID="lblTotalDTC" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>    
                               </div>
                            </div>
                                 
                                <!-- END FORM-->
                           
                        
                            <asp:GridView ID="grdDtc" 
                                AutoGenerateColumns="false"  PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" ShowFooter="true"
                                runat="server" onpageindexchanging="grdDtc_PageIndexChanging" onrowcommand="grdDtc_RowCommand" 
                                 OnSorting="grdDtc_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />   
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DT_ID" HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDtcId" runat="server" Text='<%# Bind("DT_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="FEEDER_NAME" HeaderText="Feeder Name" SortExpression="FEEDER_NAME" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblFeederName" runat="server" Text='<%# Bind("FEEDER_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                          <asp:Panel ID="panel5" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtFeederName" runat="server" placeholder="Enter Feeder Name" CssClass="span12"></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" SortExpression="DT_CODE">                                   
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                         <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtDTCCode" runat="server" placeholder="Enter DTC Code" CssClass="span12"></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DTC_NAME" HeaderText="DTC Name" SortExpression="DT_NAME">                          
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' style="word-break:break-all" Width="180px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                         <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name" Width="150px"></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code(SS Plate NO)">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                         <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtTCCode" runat="server" placeholder="Enter DTr Code" Width="150px"></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="DT_TOTAL_CON_KW" HeaderText="ConnectedKW" Visible="false">                                    
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtcKw" runat="server" Text='<%# Bind("DT_TOTAL_CON_KW") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                          
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField AccessibleHeaderText="DT_TOTAL_CON_HP" HeaderText="ConnectedHP" Visible="false">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtcHp" runat="server" Text='<%# Bind("DT_TOTAL_CON_HP") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR Capacity(in KVA)" SortExpression="TC_CAPACITY">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="DT_LAST_SERVICE_DATE" HeaderText="Last Service Date" Visible="false">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceDate" runat="server" Text='<%# Bind("DT_LAST_SERVICE_DATE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="DT_PROJECTTYPE" HeaderText="Project Type" Visible="false">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblProjectType" runat="server" Text='<%# Bind("DT_PROJECTTYPE") %>' style="word-break: break-all;" width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create" 
                                                Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                
                                </Columns>

                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

                            </asp:GridView>
                        
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                  <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Transformer Centre Details and DTC Details Can Be Edited.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit Transformer Centre Details Click On Edit Button Fill The Details and Click Save To Save The Details.
                        </p>
                       

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
