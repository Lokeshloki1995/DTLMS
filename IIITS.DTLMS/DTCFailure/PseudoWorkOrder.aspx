<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PseudoWorkOrder.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.PseudoWorkOrder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

   <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8" >
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                       Work Order</h3>
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
                      <asp:Button ID="cmdClose" runat="server" Text="Close"  
                                       CssClass="btn btn-primary" onclick="cmdClose_Click" />
                </div>
                    

             </div>
           
             
            <!-- END PAGE HEADER-->

              <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid" runat="server" id="dvBasic"  >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Basic Details</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                           
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
                                                <label class="control-label"> 
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure Id"></asp:Label>
                                                 <span class="Mandotary"> *</span></label>
                       
                                             <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                       
                                                        <asp:TextBox ID="txtFailureId"  runat="server" ReadOnly="true"
                                                            onkeypress="javascript:return OnlyNumber(event);"  MaxLength="10" TabIndex="1"></asp:TextBox>
                                                      <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" 
                                                             TabIndex="2" Visible="false" /><br />
                                                      <asp:LinkButton ID="lnkViewFailure" runat="server" style="font-size:12px;color:Blue"
                                                            onclick="lnkViewFailure_Click" >View Failure Information</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                              <div class="control-group">
                                                <label class="control-label">Transformer Centre Code</label>
                       
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtDTCCode"  runat="server"   MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                       <asp:TextBox ID="txtType" runat="server"  Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Transformer Centre Name</label>                      
                                                <div class="controls">
                                                    <div class="input-append">                                                  
                                                        <asp:TextBox ID="txtDTCName"  runat="server" ReadOnly="true"></asp:TextBox>
                                                 <asp:TextBox ID="txtDTCId"  runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                           <div class="control-group">
                                                <label class="control-label"></label>                      
                                                <div class="controls">
                                                    <div class="input-append">                                                  
                                                        <asp:LinkButton ID="lnkEstReport" runat="server" onclick="lnkEstReport_Click" style="font-size:12px;color:Blue"
                                                        >Print Estimation Report</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                               
                                    </div>
                                       
                                    <div class="span5">
                                               <div class="control-group">
                                                    <label class="control-label">
                                                        <asp:Label ID="lblDateText" runat="server" Text="Failure Date"></asp:Label>
                                                     </label>
                        
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtFailureDate" runat="server"  MaxLength="10" ReadOnly="true" ></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server"  MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                   

                                              <div class="control-group">
                                                <label class="control-label">DTr Code</label>
                       
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtTCCode"  runat="server"  ReadOnly="true"></asp:TextBox>                                                
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Declared By</label>
                       
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtDeclaredBy"  runat="server" ReadOnly="true"></asp:TextBox>                                                
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
            <!-- END PAGE CONTENT-->




            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid" runat="server" id="dvWorkOrder">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                  

          <div class="row-fluid" runat="server" id="dvComments" style="display:none">
                    <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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
                                                   <label class="control-label">Comments<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4"  TextMode="MultiLine" 
                                                              Width="550px" Height="125px" style="resize:none"  onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
                                                       <asp:TextBox ID="txtWOId" runat="server" visible="false" Width="20px"
                                        MaxLength="100" ></asp:TextBox>      
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

                           <div  class="form-horizontal" align="center">

                                   <div class="span3"></div>
                                        <div class="span5">
                                           <asp:Button ID="cmdSave" runat="server" Text="Save" 
                                           CssClass="btn btn-primary" 
                                                onclick="cmdSave_Click" TabIndex="13"   />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span5">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" Visible="false"
                                             CssClass="btn btn-primary"   TabIndex="14"  /><br />
                                    </div>
                                    <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>                                            
                               </div>

                                                          
    </div>
  </div>
</div>

   <!-- END PAGE CONTENT-->
         </div>


</asp:Content>
