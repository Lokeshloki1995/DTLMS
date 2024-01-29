<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCMaintance.aspx.cs" Inherits="IIITS.DTLMS.Maintainance.DTCMaintance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
     <script  type="text/javascript">
      function ValidateSave() {
          if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
              alert('Enter valid Transformer Centre Code')
              document.getElementById('<%= txtDTCCode.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbType.ClientID %>').value.trim() == "--Select--") {
              alert('Select Maintenance Type')
              document.getElementById('<%= cmbType.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtMaintainanceDate.ClientID %>').value.trim() == "") {
              alert('Enter Maintenance Date')
              document.getElementById('<%= txtMaintainanceDate.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbMaintainedBy.ClientID %>').value.trim() == "--Select--") {
              alert('Select Maintained By')
              document.getElementById('<%= cmbMaintainedBy.ClientID %>').focus()
              return false
          }

      }


      function checkDate(sender, args) {
          if (sender._selectedDate < new Date()) {
              alert("You Cannot select Previous date than today!");
              sender._selectedDate = new Date();
              // set the date back to the current date
              sender._textbox.set_Value(sender._selectedDate.format(sender._format))
          }
      }
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                      Transformer Centre Maintenance                         
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
                      <asp:Button ID="cmdclose" runat="server" Text="Close" 
                          CssClass="btn btn-danger" onclick="cmdclose_Click" /></div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Transformer Centre Maintenance </h4>
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
                         <label class="control-label">Transformer Centre Code <span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                                               
                                <asp:TextBox ID="txtDTCCode"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="50"></asp:TextBox>           
                                <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" onclick="cmdSearch_Click" 
                                    />
                                </div>
                        </div>
                    </div>
                     <div class="control-group">
                         <label class="control-label">Transformer Centre Name <span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                                               
                                <asp:TextBox ID="txtDTCName"  runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>           
                              
                                  
                                </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                            <div class="controls">
                                <div class="input-append">
                                <asp:TextBox ID="txtTCCode" runat="server"  ReadOnly="true"></asp:TextBox>                                    
                                <asp:TextBox ID="txtTmId"  runat="server"  MaxLength="10" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtMaintainanceId"  runat="server" Visible="false"></asp:TextBox>  
                                 <asp:TextBox ID="txtMaintainDeId"  runat="server" Visible="false"></asp:TextBox>                                                
                                </div>
                            </div>
                        </div>
                   
                           
            </div>
                 <div class="span5">

                 <div class="control-group">
                    <label class="control-label">Type Of Maintenance<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:HiddenField ID="hdfTCCode" runat="server" />
                                <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" onselectedindexchanged="cmbType_SelectedIndexChanged">
                                
                                </asp:DropDownList>
                            </div>
                       </div>
                 </div>   
                     <div class="control-group">
                        <label class="control-label">Maintenance Date<span class="Mandotary"> *</span></label>                      
                        <div class="controls">
                            <div class="input-append">                                                      
                                <asp:TextBox ID="txtMaintainanceDate" runat="server" MaxLength="10"></asp:TextBox>
                                 <asp:CalendarExtender ID="txtMaintainanceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1" OnClientDateSelectionChanged="checkDate"
                                   TargetControlID="txtMaintainanceDate" Format="dd/MM/yyyy"></asp:CalendarExtender> 
                            </div>
                        </div>
                    </div>   
                    
                    
                     <div class="control-group">
                        <label class="control-label">Maintained By<span class="Mandotary"> *</span></label>                      
                        <div class="controls">
                            <div class="input-append">  
                               <asp:DropDownList ID="cmbMaintainedBy" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>                
                 </div>               
               
                         <div class="span1"></div>
                         <div class="space20"></div>
                                        </div>
  
                                      <div  class="text-center" ">
                                              
                                      
                                 
                                           <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary" onclick="cmdSave_Click" OnClientClick="javascript:return ValidateSave();"  />
                                   
                                 
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" onclick="cmdReset_Click" />
                                    
                                     
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                       </div>

                                     <div class="space20"></div>
                                    </div>
                                </div> 
                      
                            
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->

                    
                </div>

                    
            </div>
          

            <!-- END PAGE CONTENT-->

        

              <div class="row-fluid" runat="server" style="display:none" id="dvType">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Quaterly/Half Yearly </h4>
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
                        <label class="control-label">Connections<span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtConnections" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">Fuses<span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtFuses" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"  ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                                        
                      <div class="control-group">
                        <label class="control-label">Oil leakage<span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                                <asp:TextBox ID="txtOil" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                                            
                      <div class="control-group">
                        <label class="control-label">Bushing<span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                                <asp:TextBox ID="txtBushing" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    
                      <div class="control-group">
                        <label class="control-label"> Breather <span class="Mandotary"> *</span> </label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtBreather" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label"> Danger Plates<span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtDangerPlates" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                     <div class="control-group">
                        <label class="control-label"> Anti-Climbing Devices <span class="Mandotary"> *</span>  </label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtAntiClimbing" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"  ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    

                    
                                            
            </div>
                   <div class="span5">

                    
                      <div class="control-group">
                        <label class="control-label"> Lightning Arrestor<span class="Mandotary"> *</span> </label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtLightningArrestor" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">G.O Switches<span class="Mandotary"> *</span>   </label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtGoSwitches" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"></asp:TextBox>
                            </div>
                        </div>
                    </div> 

                      <div class="control-group">
                        <label class="control-label">Load Balancing <span class="Mandotary"> *</span></label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtLoadBalancing" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>


                      <div class="control-group" runat="server" id="dvSupport" style="display:none">  
                      <label class="control-label">Supports<span class="Mandotary"> *</span></label>                   
                      
                            <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txtSupports" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"  ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group" runat="server" id="dvExplosion" style="display:none">
                       <label class="control-label">Explosion Vent Diaphragm<span class="Mandotary"> *</span></label>
                                         
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtExplosion" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"  ></asp:TextBox>
                            </div>
                        </div>
                    </div> 

                      <div class="control-group" runat="server" id="dvArcing" style="display:none">
                       <label class="control-label">Arcing horns<span class="Mandotary"> *</span></label>
                                        
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtArcingHorns" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group" runat="server" id="dvEarthing" style="display:none">
                        <label class="control-label">Earthing<span class="Mandotary"> *</span></label>
                        
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtEarthing" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>    
                    
                      <div class="control-group">
                        <label class="control-label"> General Condition Nuts <span class="Mandotary"> *</span>  </label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtGeneralCondition" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"  ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label"> LT switch/LT Protection Kit <span class="Mandotary"> *</span>  </label>
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtLTSwitch" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                             
                      <div class="control-group" runat="server" id="dvVoltage" style="display:none">
                       <label class="control-label"> Voltage<span class="Mandotary"> *</span>  </label>
                         
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtVoltage" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group" runat="server" id="dvEarthTesting" style="display:none">
                       <label class="control-label"> Earth Testing <span class="Mandotary"> *</span>  </label>
                           
                           <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtEarthTesting" runat="server" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,500);" style="resize:none;"  ></asp:TextBox>
                            </div>
                        </div>
                    </div>           
            </div>
               
                         <div class="span1"></div>
                </div>

                           <div class="space20"></div>                                       
                          <div class="space20"></div>

                           
                               <div class="space20"></div>   
                                    </div>
                                </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>


              <div class="row-fluid" style="display:none" runat="server" id="dvDTCMaintain">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Transformer Centre Maintenance View </h4>
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
                                     
                          <asp:GridView ID="grdDtcMaintainance" 
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" Visible="true" ShowFooter="true"                                
                                 CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onrowcommand="grdDtcMaintainance_RowCommand" 
                                        onpageindexchanging="grdDtcMaintainance_PageIndexChanging">
                                <Columns>
                                    
                                    <asp:TemplateField AccessibleHeaderText="TM_DT_CODE" HeaderText="Transformer Centre Code"  >                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("TM_DT_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                              <asp:TextBox ID="txtDTCCode" runat="server" placeholder="Enter Dtc Code"></asp:TextBox>
                                   </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="TM_TC_CODE" HeaderText="DTr Code"  Visible="true">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblDTrCode" runat="server" Text='<%# Bind("TM_TC_CODE") %>' style="word-break:break-all" width="100px" ></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                             <asp:TextBox ID="txtDTrCode" runat="server" placeholder="Enter DTr Code"></asp:TextBox>
                                           </FooterTemplate>
                                  </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="TM_DATE" HeaderText="Maintenance Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMainDate" runat="server" Text='<%# Bind("TM_DATE") %>' style="word-break:break-all" width="80px" ></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                               <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="30px"
                                                   ToolTip="Search" CommandName="search" />
                                       </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="US_FULL_NAME" HeaderText="Maintained By" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMainBy" runat="server" Text='<%# Bind("US_FULL_NAME") %>' style="word-break:break-all" width="150px" ></asp:Label>
                                        </ItemTemplate>
                                         
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="MD_NAME" HeaderText="Maintenance Type" >                                
                                    <ItemTemplate>                                       
                                        <asp:Label ID="lblMaintype" runat="server" Text='<%# Bind("MD_NAME") %>' style="word-break:break-all" width="100px" ></asp:Label>
                                    </ItemTemplate>
                                  </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

         
           
               
                         <div class="span1"></div>
                </div>

                           <div class="space20"></div>                                       
                          <div class="space20"></div>

                           
                               <div class="space20"></div>   
                                    </div>
                                </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->

         </div>
</asp:Content>
