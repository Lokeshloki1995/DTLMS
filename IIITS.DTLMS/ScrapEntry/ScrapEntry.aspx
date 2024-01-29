<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ScrapEntry.aspx.cs" Inherits="IIITS.DTLMS.ScrapEntry.ScrapEntry" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript" >
        function ValidateMyForm() {

            if (document.getElementById('<%= txtOMDate.ClientID %>').value == "") {
                alert('Enter OM Date')
                document.getElementById('<%= txtOMDate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRemarks.ClientID %>').value == "") {
                alert('Enter Remarks')
                document.getElementById('<%= txtRemarks.ClientID %>').focus()
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
                Scrap Entry
                   </h3>
                     
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text" />
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
                <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="Button1" runat="server" Text="Close"  OnClientClick="javascript:window.location.href='TCSearch.aspx'; return false;"
                                       CssClass="btn btn-primary" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
  <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Scrap Entry</h4>
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
                                        <label class="control-label">OM No<span class="Mandotary"> *</span></label>                       
                                        <div class="controls">
                                            <div class="input-append">     
                                                <asp:HiddenField ID="hdfScrapDetailsId" runat="server" />                                  
                                                  <asp:TextBox ID="txtOMNo"  runat="server" MaxLength="25" TabIndex="5" ></asp:TextBox>           
                                                                             
                                            </div>
                                        </div>
                                     </div>  
                                     
                                      </div>
                                            
                                     <div class="span5">
                                               <div class="control-group">
                                                <label class="control-label">OM Date<span class="Mandotary"> *</span></label>                       
                                                <div class="controls">
                                                    <div class="input-append">                                             
                                                        <asp:TextBox ID="txtOMDate" runat="server" MaxLength="10"></asp:TextBox> 
                                                         <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" 
                                    TargetControlID="txtOMDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>                             
                                                    </div>
                                                </div>
                                             </div>

                                    </div>  

                                
                                            </div>
                                    
                                        </div>
                            <div class="space20"></div>
                                  <div class="form-horizontal">
                                     <div class="row-fluid">
                                    <div class="span1"></div>
                                   <div class="span7">
                                      <div class="control-group">
                                        <label class="control-label">Remarks<span class="Mandotary"> *</span></label>
                                        <div class="controls">
                                            <div class="input-append">        
                                               <%-- <asp:HiddenField ID="hdfTcCode" runat="server" />          --%>                                   
                                              <asp:TextBox ID="txtScrapId"  runat="server" Visible="false"></asp:TextBox>
                                                <asp:TextBox ID="txtRemarks"  runat="server" TextMode="MultiLine" MaxLength="400" style="resize:none;height:75px" 
                                                onkeyup="return ValidateTextlimit(this,400);" Width="500px" ></asp:TextBox>                                                                
                                            </div>
                                        </div>
                                 </div>
                                 </div>
                                 </div>
                                  </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return Validate()" CssClass="btn btn-primary" />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                              CssClass="btn btn-primary" onclick="cmdReset_Click" /><br />
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
                                    <%--<div class="span1"></div>
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
                                                    <asp:TextBox ID="txtMake" runat="server" MaxLength="50" TabIndex="14" ReadOnly="true" ></asp:TextBox>--%>
                                                      <asp:TextBox ID="txtSelectedTcId" runat="server" Visible="false" Width="20px" ></asp:TextBox>
                                                       <%--<asp:Button ID="cmdLoad" Text="Load" class="btn btn-primary" runat="server" 
                                                        onclick="cmdLoad_Click" TabIndex="15" 
                                                              />--%>
                                                <%--</div>
                                            </div>
                                        </div>                                   
                                        
                                    </div>                                   
                                                                     
                                </div>
                              
                            </div>--%>
                                                     
                        </div>
                                
                        <div class="space20"></div>
                        <!-- END FORM-->    
                        
                              <asp:GridView ID="grdFaultTC" 
                                AutoGenerateColumns="false"  DataKeyNames="TC_ID" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false"
                                runat="server" TabIndex="16" onrowcommand="grdFaultTC_RowCommand" 
                                       >
                                <Columns>

                                 <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                      
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

                                    

                                    <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier">
                                       
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
    <!-- END SAMPLE FORM PORTLET-->
            </div>
     </div>
          

            <!-- END PAGE CONTENT-->
</div>
    
    </div>
    </div>
</asp:Content>
