<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DtcMapping.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DtcMapping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript" src="../Scripts/functions.js" ></script>
    <script type="text/javascript" >
       
      
 </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
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
                   Transformer Centre Mapping
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
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> DTC Mapping </h4>
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
                        <label class="control-label">Transformer Centre Code</label>
                        <div class="controls">
                            <div class="input-append">
                                     <asp:TextBox ID="txtMappingId"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="4" Visible="false"></asp:TextBox>                    
                                <asp:TextBox ID="txtDTCCode"  runat="server" 
                                         onkeypress="javascript:return OnlyNumber(event);" MaxLength="6" 
                                         TabIndex="1"  ReadOnly="True"></asp:TextBox>      <br />  
                              <asp:LinkButton ID="lnkDTCDetails" runat="server"  style="font-size:12px;color:Blue" onclick="lnkDTCDetails_Click"
                                                        >View DTC Details</asp:LinkButton>    
                            </div>
                        </div>
                    </div>
   
              
                   <div class="control-group">
                        <label class="control-label">Transformer Centre Name</label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:HiddenField ID="hdfTCId" runat="server" />
                                <asp:HiddenField ID="hdfDTCId" runat="server" />
                                <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" 
                                    onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="2" 
                                    ReadOnly="True" ></asp:TextBox>
                            </div>
                        </div>
                    </div>


                   <div class="control-group">
                      <label class="control-label">Commission Date</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMappingDate" runat="server" TabIndex="21" MaxLength="10" 
                                        ReadOnly="True" ></asp:TextBox>
                                          
                            </div>
                        </div>
                    </div>

                                        </div>
                                                            
                                                           <%-- another span--%>
                    <div class="span5">
                   <div class="control-group">
                        <label class="control-label">Transformer Code</label>
                        <div class="controls">
                            <div class="input-append">                                                      
                                <asp:TextBox ID="txtTcCode"  runat="server" AutoPostBack="true"
                                    onkeypress="javascript:return OnlyNumber(event);"  MaxLength="10" ReadOnly="True"  ></asp:TextBox>
                               <br />    <asp:LinkButton ID="lnkDTrDetails" runat="server"  
                                        style="font-size:12px;color:Blue" onclick="lnkDTrDetails_Click"
                                                        >View DTr Details</asp:LinkButton>   
                               
                            </div>
                        </div>
                    </div>
   
   
                  <div class="control-group">
                        <label class="control-label">Transformer Serial No </label>
                        <div class="controls">
                            <div class="input-append">
                                       <%-- onkeypress="javascript:return OnlyNumber(event);"  --%>             
                                <asp:TextBox ID="txtSerialNo" runat="server"   MaxLength="20" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label">Transformer Make</label>
                        <div class="controls">
                            <div class="input-append">
                                          <asp:TextBox ID="txtMake" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="20" ReadOnly="True" ></asp:TextBox>                 
                             
                            </div>
                        </div>
                    </div>
                     <div class="control-group">
                        <label class="control-label">Capacity(in KVA)</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTcCapacity" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="10" ReadOnly="True" ></asp:TextBox>
                        
                                                       
                            </div>
                        </div>
                    </div>






                     </div>
                <div class="span1"></div>
                    </div>
                    <div class="space20"></div>
                                        
                <div  class="form-horizontal" align="center">

                    
                    
                </div>
                           
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
         
     <%-- </div>--%>

</asp:Content>
