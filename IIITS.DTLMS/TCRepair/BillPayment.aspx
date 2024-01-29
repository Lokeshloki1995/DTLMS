<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="BillPayment.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.BillPayment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="../Scripts/functions.js" type="text/javascript"></script>
     <script type="text/javascript" >
         function ValidateMyForm() {

             if (document.getElementById('<%= txtWONo.ClientID %>').value == "") {
                 alert('Enter Valid Work Order No')
                 document.getElementById('<%= txtWONo.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtBRNo.ClientID %>').value == "") {
                 alert('Enter Valid BR Number')
                 document.getElementById('<%= txtBRNo.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtAmount.ClientID %>').value == "") {
                 alert('Enter Valid Amount')
                 document.getElementById('<%= txtAmount.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtPaymentDate.ClientID %>').value == "") {
                 alert('Enter Valid Payment Date')
                 document.getElementById('<%= txtPaymentDate.ClientID %>').focus()
                 return false
             }
         }
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
                    TC Bill Payment
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
                      <asp:Button ID="cmdClose" runat="server" Text="TC Bill View"                                     
                            CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='TCBillView.aspx'; return false;" /></div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>TC Bill Payment</h4>
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
                        <label class="control-label">WO No<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                                                                 
                                <asp:TextBox ID="txtWONo" runat="server" TabIndex="1"></asp:TextBox>  
                                <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                    onclick="cmdSearch_Click" TabIndex="2"  />                           
                            </div>
                        </div>
                    </div>
   
             
                           <div class="control-group">
                        <label class="control-label">PO No</label>
                        <div class="controls">
                            <div class="input-append">
                                  <asp:TextBox ID="txtPONo" runat="server" TabIndex="3" ReadOnly="true"></asp:TextBox> 
                            </div>
                        </div>
                    </div>

                        <div class="control-group">
                                    <label class="control-label">PO Date</label>
                                    <div class="controls">
                                        <div class="input-append">                                                                                 
                                            <asp:TextBox ID="txtPOdate" runat="server" TabIndex="4" ></asp:TextBox>  
                                             <asp:CalendarExtender ID="CalendarExtender1_txtPOdate" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtPOdate" Format="dd/MM/yyyy" ></asp:CalendarExtender>        
                                            <asp:TextBox ID="txtBillId" runat="server" TabIndex="1" Width="20px" Visible="false"></asp:TextBox>                             
                                        </div>
                                    </div>
                                </div>

                    </div>
                    <div class="span5">
                                            
                                <div class="control-group">
                        <label class="control-label">BR No<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                                                                 
                                <asp:TextBox ID="txtBRNo" runat="server" TabIndex="1"></asp:TextBox>  
                                                  
                            </div>
                        </div>
                    </div>
   
             
                           <div class="control-group">
                        <label class="control-label">Amount<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                  <asp:TextBox ID="txtAmount" runat="server" TabIndex="3"></asp:TextBox> 
                            </div>
                        </div>
                    </div>

                        <div class="control-group">
                                    <label class="control-label">Payment Date<span class="Mandotary"> *</span></label>
                                    <div class="controls">
                                        <div class="input-append">                                                                                 
                                            <asp:TextBox ID="txtPaymentDate" runat="server" TabIndex="4"></asp:TextBox>  
                                             <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtPaymentDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>        
                                            
                                        </div>
                                    </div>
                                </div>

                                </div>

                                    <div class="span1"></div>
                                        </div>
                                       
                                         <div class="space20"></div>
                                    <div  class="form-horizontal" align="center">

                                    <div class="span3"></div>
                                     <div class="span2">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save"  OnClientClick="javascript:return ValidateMyForm()"
                                        CssClass="btn btn-primary"  TabIndex="6" onclick="cmdSave_Click"  />
                                      </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary" onclick="cmdReset_Click" TabIndex="7" /><br />
                                    </div>
                                      <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                        </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           <asp:GridView ID="grdWo" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" 
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" 
                                TabIndex="8" onpageindexchanging="grdWo_PageIndexChanging"  >
                                <Columns>

                                     <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="DELIVERED" HeaderText="Deliver Status">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDeliver" runat="server" Text='<%# Bind("DELIVERED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SM_NAME" HeaderText="Store">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblStore" runat="server" Text='<%# Bind("SM_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
     
                                    <asp:TemplateField AccessibleHeaderText="DELIVERY_DATE" HeaderText="Delivery Date">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Bind("DELIVERY_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI Number">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblRINo" runat="server" Text='<%# Bind("TR_RI_NO") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                               
                                </Columns>

                            </asp:GridView>
                        
                             <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    
                            <div class="space20"></div>
                                <div class="space20"></div>
                      
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
</asp:Content>
