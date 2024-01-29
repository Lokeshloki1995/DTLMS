<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ScrapDisposal.aspx.cs" Inherits="IIITS.DTLMS.ScrapEntry.ScrapDisposal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript" >
        function ValidateMyForm() {
           
                
            if (document.getElementById('<%= txtAmount.ClientID %>').value == "") {
                alert('Enter Amount')
                document.getElementById('<%= txtAmount.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDescription.ClientID %>').value == "") {
                alert('Enter Description')
                document.getElementById('<%= txtDescription.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtQuantity.ClientID %>').value == "") {
                alert('Enter Valid Quantity')
                document.getElementById('<%= txtQuantity.ClientID %>').focus()
                return false
            }

        }

        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
            
    </script>
     <style type="text/css">
       

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
    <div >
     
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                Scrap Disposal
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
                       <asp:Button ID="cmdClose" runat="server" Text="Close" onclick="cmdClose_Click" 
                                       OnClientClick="javascript:return Validate()" CssClass="btn btn-primary" /></div>
                                      
              
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Scrap Disposal</h4>
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
                      <label class="control-label">Invoice No<span class="Mandotary"> *</span></label>                        
                        <div class="controls">
                            <div class="input-append">                                                  
                              <asp:TextBox ID="txtInvoiceNo" ReadOnly="true" runat="server"></asp:TextBox>                            
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                      <label class="control-label">Invoice Date<span class="Mandotary"> *</span></label>                        
                        <div class="controls">
                            <div class="input-append">                                                  
                              <asp:TextBox ID="txtInvoiceDate"  runat="server"></asp:TextBox>    
                               <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>                         
                            </div>
                        </div>
                    </div>

                   
                   <div class="control-group" >
                      <label class="control-label">Quantity<span class="Mandotary"> *</span></label>                        
                        <div class="controls">
                            <div class="input-append">   
                            <asp:HiddenField ID="hdfScrapDetailsId" runat="server" />                                                
                              <asp:TextBox ID="txtQuantity" runat="server" onkeypress="javascript:return AllowNumber(this,event);" 
                               MaxLength="6"></asp:TextBox>                            
                            </div>
                        </div>
                    </div>

                   <div class="control-group">
                      <label class="control-label">Amount<span class="Mandotary"> *</span></label>                        
                        <div class="controls">
                            <div class="input-append">                                                  
                              <asp:TextBox ID="txtAmount" MaxLength="12" runat="server"  onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>                            
                            </div>
                        </div>
                    </div>

                 </div>
                 <div class="span5">

                  <div class="control-group">
                      <label class="control-label">Send To<span class="Mandotary"> </span></label>                        
                        <div class="controls">
                            <div class="input-append">                                                  
                              <asp:TextBox ID="txtSendTo" MaxLength="200" runat="server"  ></asp:TextBox>                            
                            </div>
                        </div>
                  </div>

                   <div class="control-group">
                      <label class="control-label">Description<span class="Mandotary"> *</span></label>                         
                        <div class="controls">
                            <div class="input-append">                                                   
                                   <asp:TextBox ID="txtDescription"  runat="server" TextMode="Multiline"  MaxLength="500" 
                                   style="resize:none;height:70px" onkeyup="return ValidateTextlimit(this,500);"></asp:TextBox>                            
                            </div>
                        </div>
                    </div>
                    </div>
                 <div class="space20"></div>                                       
                    <div  class="form-horizontal" align="center">

                        <div class="span3"></div>
                        <div class="span1">
                              <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                              OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                        </div>
                        <div class="span1">  
                            <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                CssClass="btn btn-primary" onclick="cmdReset_Click"  /><br />
                        </div>
                 <div class="space20"></div>
                <%-- <div class="span1">--%>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>           
                 <%--</div>--%>
            </div>
         
        </div>

    </div>
    </div>
    </div>
    </div>

    </div>
 
        <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Selected Transformer</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form" >
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                    <div class="span1"></div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">DTr Code</label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                         <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10" TabIndex="12" ></asp:TextBox>
                                                          <asp:Button ID="cmdSearchTC" Text="S" class="btn btn-primary" runat="server" 
                                                             onclick="cmdSearchTC_Click" TabIndex="13" />
                                                    </div>
                                                </div>
                                        </div>

                                    </div>
                                       
                                    <div class="span4">
                                         <div class="control-group">
                                            <label class="control-label">Make</label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:TextBox ID="txtMake" runat="server" MaxLength="50" TabIndex="14" ></asp:TextBox>
                                                   
                                                      <asp:TextBox ID="txtSelectedTcId" runat="server" Visible="false" Width="20px" ></asp:TextBox>
                                                       <asp:Button ID="cmdLoad" Text="Load" class="btn btn-primary" runat="server" 
                                                        onclick="cmdLoad_Click" TabIndex="15" 
                                                              />
                                                </div>
                                            </div>
                                        </div>                                   
                                        
                                    </div>                                   
                                                                     
                                </div>
                              
                            </div>
                                                     
                        </div>
                                
                        <div class="space20"></div>
                        <!-- END FORM-->    
                        
                              <asp:GridView ID="grdFaultTC" 
                                AutoGenerateColumns="false"   DataKeyNames="TC_ID" 
                                ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false"
                                runat="server" TabIndex="16" onrowcommand="grdFaultTC_RowCommand" 
                                OnSorting="grdFaultTC_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                 <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                        
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                   
                                 <asp:TemplateField AccessibleHeaderText="SO_ID" HeaderText="SO_ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                           <asp:Label ID="lblScrapDetailsId" runat="server" Text='<%# Bind("SO_ID") %>'></asp:Label>
                                      
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="TC SlNo" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                      
                                        <ItemTemplate>
                                          
                                            <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    
                                    <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <%-- <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee ">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    

                                    <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" SortExpression="TS_NAME">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                   
                                   <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" 
                                              CommandName="Remove"  Width="12px" OnClientClick="return ConfirmDelete();" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                               
                                </Columns>

                            </asp:GridView>
                                             
                                    </div>                  
                    </div>
                 </div>

              <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>GatePass</h4>
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
                        <label class="control-label">Vehicle No</label>
                         <div class="span1">
                           <span class="Mandotary"> *</span>
                        </div>
                        <div class="controls">
                            <div class="input-append">
                                  <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>                   
                                <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                                <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
         
                         <label class="control-label">Receipient Name</label>
                         <div class="span1">
                           <span class="Mandotary"> *</span>
                        </div>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:HiddenField ID="hdfWFOId" runat="server" />
                            <asp:HiddenField ID="hdfWFDataId" runat="server" />
                            <asp:HiddenField ID="hdfWFOAutoId" runat="server" />                      
                                <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50"></asp:TextBox>
                                                                              
                            </div>
                        </div>
                    </div>

                        <label class="control-label">Challen Number</label>
                         <div class="span1">
                           <span class="Mandotary"> *</span>
                        </div>
                        <div class="controls">
                            <div class="input-append">
                                                   
                                <asp:TextBox ID="txtChallen" runat="server" MaxLength="50"></asp:TextBox>
                            
                            </div>
                        </div>
                    </div>
                  <div class="space20"></div>
                                        
                    <div  class="form-horizontal" align="center">

                        <div class="span3"></div>
                        <div class="span1">
                                      
                        </div>
                        <div class="span1">
                        <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass" 
                        CssClass="btn btn-primary" onclick="cmdGatePass_Click" Enabled="false"
                                />
                            </div>

                            <div class="space20"></div>
         
                     </div>

                  </div>
                </div>     
    </div>
    </div>
    </div>    
    </div>

    <!-- END SAMPLE FORM PORTLET-->
            </div>
    </div>
    </div>
</div>
</asp:Content>
