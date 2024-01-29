<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTCMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

     <script type="text/javascript" >
         function ValidateMyForm() {
             if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                 alert('Enter Valid  Transformer Centre Code')
                 document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtDTCName.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Transformer Centre Name')
                 document.getElementById('<%= txtDTCName.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtOMSection.ClientID %>').value.trim() == "") {
                 alert('Enter OM Section')
                 document.getElementById('<%= txtOMSection.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtInternalCode.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Internal Code')
                 document.getElementById('<%= txtInternalCode.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtConnectedKW.ClientID %>').value.trim() == "") {
                 alert('Enter Connected KW')
                 document.getElementById('<%= txtConnectedKW.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtConnectedHP.ClientID %>').value.trim() == "") {
                 alert('Enter Connected HP')
                 document.getElementById('<%= txtConnectedHP.ClientID %>').focus()
                 return false
             }

//             if (document.getElementById('<%= txtKWHReading.ClientID %>').value == "") {
//                 alert('Enter KWHReading')
//                 document.getElementById('<%= txtKWHReading.ClientID %>').focus()
//                 return false
//             }
             if (document.getElementById('<%= cmbPlatformType.ClientID %>').value == "-Select-") {
                 alert('Select PlatformType')
                 document.getElementById('<%= cmbPlatformType.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtTcSlNo.ClientID %>').value.trim() == "") {
                 alert('Enter valid Trandformer Sl No.')
                 document.getElementById('<%= txtTcSlNo.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtConnectionDate.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Connection Date')
                 document.getElementById('<%= txtConnectionDate.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtInspectionDate.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Inspection Date')
                 document.getElementById('<%= txtInspectionDate.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtServiceDate.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Service Date')
                 document.getElementById('<%=txtServiceDate.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtCommisionDate.ClientID %>').value.trim() == "") {
                 alert('Enter Commission Date')
                 document.getElementById('<%=txtCommisionDate.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtFeederChngDate.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Feeder Change Date')
                 document.getElementById('<%=txtFeederChngDate.ClientID %>').focus()
                 return false
             }
         }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server"> 
 </asp:ScriptManager>
     
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
               Commisioning of DTC
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
                      <asp:Button ID="cmdClose" runat="server" Text="DTC View"                                     
                            CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='DTCView.aspx'; return false;" /></div>
                                      
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Commissioning of DTC</h4>
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
                        <label class="control-label">Transformer Centre Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                     <asp:TextBox ID="txtDTCId"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="4" Visible="false"></asp:TextBox>                    
                                <asp:TextBox ID="txtDTCCode"  runat="server" 
                                         onkeypress="javascript:return OnlyNumber(event);" MaxLength="6" TabIndex="1"></asp:TextBox>            
                            </div>
                        </div>
                    </div>
   
              
                 <div class="control-group">
                        <label class="control-label">Transformer Centre Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" 
                                    onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="2" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    
                                                        <div class="control-group">
                        <label class="control-label">O & M Section<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtOMSection" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                        <asp:Button ID="btnOmSearch" runat="server" Text="S" 
                                             CssClass="btn btn-primary" />                
                            </div>
                        </div>
                    </div>
                      
                                  <div class="control-group">
                        <label class="control-label">Internal Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" TabIndex="4"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                                                      

                        <div class="control-group">
                        <label class="control-label">Connected KW<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectedKW" runat="server" MaxLength="6"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="5"   ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                    
                         <div class="control-group">
                        <label class="control-label">Connected HP<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="6"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                   
                                   
                                   
                                  <div class="control-group">
                        <label class="control-label">KWH Reading</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtKWHReading" runat="server" MaxLength="10"  
                                        onkeypress="javascript:return OnlyNumber(event);" TabIndex="7"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                     <div class="control-group">
                        <label class="control-label">Platform Type<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbPlatformType" runat="server" AutoPostBack="true" 
                                     TabIndex="8">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Single Pole" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="Double Pole" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Breaker Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlBreakertype"   runat="server"
                                     TabIndex="9">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="GOS" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="DOLO" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    

                    <div class="control-group">
                        <label class="control-label">Transformer Centre Meters Available</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddldtcmeters"  runat="server"
                                     TabIndex="10">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">HT Protection</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlhtprotection"   runat="server"
                                     TabIndex="11">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">LT Protection</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlLTProtection"  runat="server"
                                     TabIndex="12">
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
                        <label class="control-label">Grounding</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlgrounding"   runat="server"
                                     TabIndex="13">
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
                                 <asp:DropDownList ID="ddlArresters"    runat="server"
                                     TabIndex="14">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>

                    
                    <div class="control-group">
                        <label class="control-label">HT Line Length</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txthtLine"  runat="server" TabIndex="15" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);" ></asp:TextBox>               
                            </div>                            
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">LT Line Length</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txtltLine" runat="server" TabIndex="16" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);" ></asp:TextBox>               
                            </div>                            
                        </div>
                    </div>

                                  <div class="control-group">
                        <label class="control-label">Transformer SlNo<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTcSlNo" runat="server" MaxLength="10"  
                                        onkeypress="javascript:return OnlyNumber(event);" TabIndex="17"></asp:TextBox>
                                       <asp:Button ID="cmdSearch" runat="server" Text="S"  
                                       CssClass="btn btn-primary" onclick="cmdSearch_Click" TabIndex="18" />    
                                <asp:TextBox ID="txtOldTCCode" runat="server" Visible="false" Width="20px"></asp:TextBox>     
                                                    
                            </div>
                        </div>
                    </div>      
                                  
                                     <div class="control-group">
                        <label class="control-label">Transformer Make<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTCMake" runat="server" Enabled="false" TabIndex="19" ></asp:TextBox>
                                         <asp:TextBox ID="txtTCCode" runat="server" Visible="false" Width="20px"></asp:TextBox>  
                                <asp:HiddenField ID="hdfTcCode" runat="server" />                           
                            </div>
                        </div>
                    </div>      
                                  <div class="control-group">
                        <label class="control-label">Transformer Capacity<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtCapacity" runat="server"  Enabled="false" TabIndex="20" ></asp:TextBox>
                                                                      
                            </div>
                        </div>
                    </div>      
                                  
                  
                           <div class="control-group">
                        <label class="control-label">Connection Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectionDate" runat="server" TabIndex="21" MaxLength="10" ></asp:TextBox>
                                     <asp:CalendarExtender ID="txtConnectionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtConnectionDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                   
                            </div>
                        </div>
                    </div>
                    
                                  <div class="control-group">
                        <label class="control-label">Prev. Insp Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtInspectionDate" runat="server" TabIndex="22" MaxLength="10" ></asp:TextBox>
                                       <asp:CalendarExtender ID="txtInspectionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtInspectionDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                        
                            </div>
                        </div>
                    </div>
                    
                    <div class="control-group">
                        <label class="control-label">Prev. Service Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtServiceDate" runat="server" TabIndex="23" MaxLength="10" ></asp:TextBox>
                                      <asp:CalendarExtender ID="txtServiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtServiceDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                         
                            </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Commision Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtCommisionDate" runat="server" TabIndex="24" MaxLength="10" ></asp:TextBox>
                                      <asp:CalendarExtender ID="txtCommisionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtCommisionDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                       
                            </div>
                        </div>
                    </div>
                   
                    <div class="control-group">
                        <label class="control-label">Feeder Chng Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtFeederChngDate" runat="server" TabIndex="25"  MaxLength="10" ></asp:TextBox>
                                       <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtFeederChngDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                          
                            </div>
                        </div>
                    </div>
                    
      
                     </div>  
  
                                      </div>                
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" 
                                                TabIndex="26" />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary" onclick="cmdReset_Click" TabIndex="27" /><br />
                                    </div>
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
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
          

            <!-- END PAGE CONTENT-->
         </div>
         
      </div>




</asp:Content>
