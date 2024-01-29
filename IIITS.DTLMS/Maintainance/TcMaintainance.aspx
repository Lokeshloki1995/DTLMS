<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMaintainance.aspx.cs" Inherits="IIITS.DTLMS.Maintainance.TcMaintainance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

   <script  type="text/javascript">

       function ValidateMyForm() {
           if (document.getElementById('<%= txtDtcCode.ClientID %>').value == "") {
               alert('Enter Valid Transformer Centre code')
               document.getElementById('<%= txtDtcCode.ClientID %>').focus()
               return false
           }
           if (document.getElementById('<%= txtTCCode.ClientID %>').value == "") {
               alert('Enter Transformer code')
               document.getElementById('<%= txtTCCode.ClientID %>').focus()
               return false
           }
           if (document.getElementById('<%= cmboilLevel.ClientID %>').value == "--Select--") {
               alert('Select Oil Level')
               document.getElementById('<%= cmboilLevel.ClientID %>').focus()
               return false
           }

           if (document.getElementById('<%= cmbArrester.ClientID %>').value == "--Select--") {
               alert('Select Lightning Arrestor')
               document.getElementById('<%= cmbArrester.ClientID %>').focus()
               return false
           }
           if (document.getElementById('<%= txtRadiator.ClientID %>').value == "") {
               alert('Enter Radiator')
               document.getElementById('<%= txtRadiator.ClientID %>').focus()
               return false
           }  
  
           if (document.getElementById('<%= txtInspectedBy.ClientID %>').value == "") {
               alert('Enter Inspected By')
               document.getElementById('<%= txtInspectedBy.ClientID %>').focus()
               return false
           }
           if (document.getElementById('<%= txtInspDate.ClientID %>').value == "") {
               alert('Enter Inspected Date')
               document.getElementById('<%= txtInspDate.ClientID %>').focus()
               return false
           }
           if (document.getElementById('<%= txtTmDescription.ClientID %>').value == "") {
               alert('Enter Description')
               document.getElementById('<%= txtTmDescription.ClientID %>').focus()
               return false
           }

       }

       function ResetForm() {
           document.getElementById('<%= txtDtcCode.ClientID %>').value = "";
           document.getElementById('<%= txtTCCode.ClientID %>').value = "";
           document.getElementById('<%= txtInspDate.ClientID %>').value = "";
           document.getElementById('<%= cmboilLevel.ClientID %>').value = "--Select--";         
           document.getElementById('<%= txtTmDescription.ClientID %>').value = "";
           document.getElementById('<%= txtRadiator.ClientID %>').value = "";
           document.getElementById('<%= txtInspectedBy.ClientID %>').value = "";
           document.getElementById('<%= cmbArrester.ClientID %>').value = "--Select--";
        

           return false
       }
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    Maintenance Entry</h3>
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
                          CssClass="btn btn-primary" onclick="cmdclose_Click" /></div>

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Maintenance Entry</h4>
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
                                <asp:TextBox ID="txtDtcCode" runat="server" onkeypress="return OnlyNumber(this,event);"  MaxLength="10"></asp:TextBox>
                                <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" onclick="cmdSearch_Click" Visible="true" />
                                <asp:TextBox ID="txtform" runat="server" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                                                    
                        <div class="control-group">
                        <label class="control-label">DTr Code</label>
                            <div class="controls">
                                <div class="input-append">
                                <asp:TextBox ID="txtTCCode" runat="server"  ReadOnly="true"></asp:TextBox>      
                              
                                <asp:TextBox ID="txtTmId"  runat="server"  MaxLength="10" Visible="false"></asp:TextBox>                                                 
                                </div>
                            </div>
                        </div>
                   

                        <div class="control-group">
                        <label class="control-label">Oil Level<span class="Mandotary"> *</span></label>
                        <div class="controls">
                        <div class="input-append">
                         <asp:TextBox ID="txtLastServiceDate" runat="server"  ReadOnly="true" Width="20px" Visible="false"></asp:TextBox>   
                           <asp:DropDownList ID="cmboilLevel" runat="server">
                            </asp:DropDownList>            
                                                       
                        </div>
                        </div>
                        </div>
                                                    
                                                         
                                           

                    <div class="control-group">
                        <label class="control-label">Lightning Arrestor <span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:DropDownList ID="cmbArrester" runat="server">
                            </asp:DropDownList>                                                 
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label"> Radiator <span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtRadiator" runat="server" MaxLength="50"  ></asp:TextBox>                                                       
                            </div>
                        </div>
                    </div>

                    </div>
                    <div class="span5">

                    <div class="control-group">
                        <label class="control-label">Inspected By<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                            
                                <asp:TextBox ID="txtInspectedBy" runat="server" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Inspected Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                            
                                <asp:TextBox ID="txtInspDate" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                TargetControlID="txtInspDate"></asp:CalendarExtender>                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Remarks<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:TextBox ID="txtTmDescription" runat="server" onkeyup="return ValidateTextlimit(this,500);" style = "resize:none" Height="120px" TextMode="MultiLine"
                                    ></asp:TextBox>                 
                            </div>
                        </div>
                    </div>
                               
                      
                                  
                                   <div class="control-group">
                                    <label class="control-label"><span >
                                        <asp:Label ID="lblMessage" runat="server" ></asp:Label>
                                    
                                    </span></label>
                                               <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMaintanceID" runat="server" MaxLength="10" Visible ="false" ></asp:TextBox>
                                                       
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
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" 
                                      OnClientClick="javascript:return ValidateMyForm();" CssClass="btn btn-primary" onclick="cmdSave_Click" 
                                                 />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary"  
                                             OnClientClick="javascript:return ResetForm();" onclick="cmdReset_Click"  /><br />
                                    </div>
                                                <div class="span7"></div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                            
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
