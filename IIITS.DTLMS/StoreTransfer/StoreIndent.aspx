<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreIndent.aspx.cs" Inherits="IIITS.DTLMS.StoreTransfer.StoreIndent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript" ></script>

        <script type="text/javascript">

         function preventMultipleSubmissions() {
     $('#<%=cmdSave.ClientID %>').prop('disabled', true);
}
 
window.onbeforeunload = preventMultipleSubmissions;
 
    </script>


     <script type="text/javascript">

         function ConfirmDelete() {

             var result = confirm('Are you sure you want to Delete?');
             if (result) {
                 return true;
             }
             else {
                 return false;
             }
         }
             
             function ValidateMyForm() 
             {
                 if (document.getElementById('<%= txtIndentNumber.ClientID %>').value == "") {
                     alert('Enter Indent Number')
                     document.getElementById('<%= txtIndentNumber.ClientID %>').focus()
                     return false
                 }
                 
                 if (document.getElementById('<%= txtIndentDate.ClientID %>').value == "") {
                     alert('Enter Indent Date')
                     document.getElementById('<%= txtIndentDate.ClientID %>').focus()
                     return false
                 }
                 
                 if (document.getElementById('<%= ddlStore.ClientID %>').value == "--Select--") {
                     alert('Select Store Name')
                     document.getElementById('<%= ddlStore.ClientID %>').focus()
                     return false
                 }

                 if (document.getElementById('<%= txtDescription.ClientID %>').value == "") {
                     alert('Enter Description')
                     document.getElementById('<%= txtDescription.ClientID %>').focus()
                     return false
                 }
             }
             
             function ValidateForm() {
                 if (document.getElementById('<%= ddlCapacity.ClientID %>').value == "--Select--") {
                     alert('Select Capacity')
                     document.getElementById('<%= ddlCapacity.ClientID %>').focus()
                     return false
                 }
                 if (document.getElementById('<%= txtQuantity.ClientID %>').value == "") {
                     alert('Enter Quantity')
                     document.getElementById('<%= txtQuantity.ClientID %>').focus()
                     return false
                 }
                 if (document.getElementById('<%= txtQuantity.ClientID %>').value == "0") {
                     alert('Quantity should be Greater than 0')
                     document.getElementById('<%= txtQuantity.ClientID %>').focus()
                     return false
                 }
             }

        </script>

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                 Store Indent
              <div style="float:right" >
                             
                         <asp:Button ID="cmdClose" runat="server" Text="Close"
                            CssClass="btn btn-danger" onclick="cmdClose_Click" /><br />
                   </div>
           
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i>
                                   
                                  
                                    </button>
                               </div>
                                
                           </form>
                          
                       </li>
                       
                                    
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               <%-- <div style="float:right" >
                             
                                   <asp:Button ID="cmdClose" runat="server" Text="Indent View" 
                                              CssClass="btn btn-primary" onclick="cmdClose_Click" /><br /></div>
--%>
                                      
               </div>
          <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Capacity and Quantity</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form" >
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                 <asp:Panel ID="pnlApproval" runat="server" >
                                    <div class="row-fluid">
                                    <div class="span1"></div>
                  <div class="span5">
                    <div class="control-group">
                        <label class="control-label">Capacity <span class="Mandotary">*</span></label>                   
                        <div class="controls">
                            <div class="input-append">
                                <asp:DropDownList ID="ddlCapacity" runat="server"></asp:DropDownList>  
                                <asp:TextBox ID="txtActiontype" runat="server"  MaxLength="10" Visible="false" Width="20px"></asp:TextBox>      
                            </div>
                        </div>
                    </div>
                  </div>           
                                 
                    <div class="span5">
                        <div class="control-group">
                            <label class="control-label">Quantity<span class="Mandotary">*</span></label>
                            <div class="controls">
                                <div class="input-append">                                                       
                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8"  onkeypress="javascript:return OnlyNumber(event);" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                <div class="span1">
                    <asp:Button ID="cmdAdd" runat="server" Text="Add" onclick="cmdAdd_Click"  OnClientClick="return ValidateForm();" CssClass="btn btn-primary" />
                </div>                                    
                                                                                                                       
                    
                        <div class="span3"> </div>
                        <div class="span5"> 
                        <div class="space20"></div>
                        <div class="space20"></div>
                         </asp:Panel>  
                              <asp:GridView ID="grdTcTransfer" 
                                AutoGenerateColumns="false"  PageSize="5" DataKeyNames="SO_ID" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" 
                                runat="server" TabIndex="16" onrowcommand="grdTcTransfer_RowCommand" 
                                onpageindexchanging="grdTcTransfer_PageIndexChanging"    onrowdatabound="grdTcTransfer_RowDataBound" >
                                <Columns>
                                 <asp:TemplateField AccessibleHeaderText="SO_ID" HeaderText="SO ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblSoId" runat="server" Text='<%# Bind("SO_ID") %>'></asp:Label>
                                               <asp:TextBox ID="txtSoId" runat="server" ></asp:TextBox>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="SO_CAPACITY" HeaderText="Capacity">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("SO_CAPACITY") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Quantity" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("SO_QNTY") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Delete">
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
                    </div>
                 </div>
    <!-- END SAMPLE FORM PORTLET-->
            </div>
            </div>
              
           
              <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Store Indent</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                        <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                  <asp:Panel ID="pnlApprovalIndent" runat="server" >
                                    <div class="row-fluid">
                                <div class="span1"></div>
                                      <div class="span5">
                                         <div class="control-group">
                        <label class="control-label">Indent Number<span class="Mandotary">*</span></label>
                         
                        <div class="controls">
                            <div class="input-append">
                                <asp:TextBox ID="txtIndentId" runat="server" Visible="false"></asp:TextBox>
                                  <asp:TextBox ID="txtIndentNumber" runat="server" MaxLength="20"></asp:TextBox> 
                                 
                            </div>
                        </div>
                    </div>
                        <div class="control-group">
                         <label class="control-label">Indent date<span class="Mandotary">*</span></label>                       
                        <div class="controls">
                            <div class="input-append">
                                          <asp:TextBox ID="txtSiId" runat="server" Visible="false"></asp:TextBox>                 
                                <asp:TextBox ID="txtIndentDate" runat="server"></asp:TextBox>
                                   <asp:CalendarExtender ID="txtIndentDate_CalendarExtender1" runat="server" CssClass="cal_Theme1" 
                                       TargetControlID="txtIndentDate"  Format="dd/MM/yyyy"></asp:CalendarExtender>               
                                       
                            </div>
                        </div>
                      </div>
                    </div>                 
                        <div class="span5">
                             <div class="control-group">
                                   <label class="control-label">To Store<span class="Mandotary">*</span></label>                                         
                                          
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                       <asp:DropDownList ID="ddlStore" runat="server" AutoPostBack="true" 
                                                           onselectedindexchanged="ddlStore_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                   <div class="control-group">
                                           <label class="control-label">Description<span class="Mandotary">*</span></label>                                         
                                           
                                           <div class="controls">
                                                <div class="input-append">    
                                                 <asp:HiddenField ID="hdfCrBy" runat="server" />                                                      
                                                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="250" TextMode="MultiLine" Style="resize: none;" onkeyup="return ValidateTextlimit(this,250);"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div> 
                                        </div>

                                    
                                  <div class="space20"></div>
                                        

                              <div class="space20"></div>
                        
                              <asp:GridView ID="grdCapacityDetails" visible="false"
                                AutoGenerateColumns="false"  PageSize="5" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" 
                                runat="server" TabIndex="16" onpageindexchanging="grdCapacityDetails_PageIndexChanging">
                                <Columns>
                              

                                   <asp:TemplateField AccessibleHeaderText="SM_OFF_CODE" HeaderText="Location Name">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("SM_OFF_CODE") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                              
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="STOCKCOUNT" HeaderText="Quantity" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("STOCKCOUNT") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                   
                                </Columns>
                            </asp:GridView>

                             <div class="space20"></div>
                    <div class="space20"></div>
                        
                         <div class="space20"></div>

                     
         
      </div>
                                    </asp:Panel>


    </div>
    </div>
    </div>
    </div>

    </div>
    </div>


         <div class="row-fluid" runat="server" id="dvComments" style="display:none">
                    <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
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
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                         <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOAutoId" runat="server" />    
                                                          <asp:HiddenField ID="hdfLocCode" runat="server" />      
                                                           <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />  
                                                           <asp:HiddenField ID="hdfApproveStatus" runat="server" />  
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="8"  TextMode="MultiLine" 
                                                              Width="550px" Height="125px" style="resize:none"  onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
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
                        

                                    <div  class="text-center" align="center">

                                       
                                       
                                         <asp:Button ID="cmdSave" runat="server" Text="Save" 
                                              OnClientClick="return ValidateMyForm();" CssClass="btn btn-primary" onchange = "javascript:preventMultipleSubmissions();" 
                                                onclick="cmdSave_Click"  />
                                      
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" 
                                       CssClass="btn btn-danger" onclick="btnReset_Click" 
                                                />
                                         
                                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
         </div>
    </div>
    </div>
    
    
    
    </div>
</asp:Content>
