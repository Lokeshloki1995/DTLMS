<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StockAlert.aspx.cs" Inherits="IIITS.DTLMS.Transaction.StockAlert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div>
     
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">

               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                   Stock Alert
                   </h3>
                   
                      <div style="float:right" >
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

                <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Stock Alert
                                    <a style="float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px;padding: 0px 0px 0px 3px !important"></i></a>

                            </h4>
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
                        <label class="control-label">Indent No<span class="Mandotary"> *</span></label>
                         
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtIndentNoSearch" runat="server" 
                                        MaxLength="10" ></asp:TextBox>
                                         <asp:TextBox ID="txtindentid" runat="server" Visible="false"
                                        MaxLength="10" ></asp:TextBox>
                                           <asp:Button ID="btnSearchId" Text="S" class="btn btn-primary" 
                                        runat="server" onclick="btnSearchId_Click"   
                                      />
                                 <asp:TextBox ID="txtSearchId"  runat="server" Width="20px" Visible="false" ></asp:TextBox>    
                                                       
                            </div>
                        </div>
                    </div>
                   <div class="control-group">
                        <label class="control-label">Failure Id</label>
                           
                           <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtFailureId" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                    </div>       
                    <div class="control-group">
                        <label class="control-label">DTr Capacity(in KVA)</label>
                           
                           <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtTcCapacity" runat="server" Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                    </div>  
                    </div>   
                    <div class="space20"></div>
                                        
                                    <div  class="text-center">

                                       
                                    <asp:Button ID="cmdSave" runat="server" Text="Alert Me When Stock Arrives" 
                                       OnClientClick="javascript:return Validate()" CssClass="btn btn-primary" 
                                                onclick="cmdSave_Click" />
                                    
                                          
                                           
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                       OnClientClick="javascript:return Validate()" CssClass="btn btn-danger" onclick="cmdReset_Click" />
                                      
                                       
   
         <div class="span1">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Set Stock Alert Notification
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Need To Enter Indent No and Then Click On <u>Alert
                            Me When Stock Arrives</u> Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Alert is Set User Will Get Notification Once Stock is Arrived
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
