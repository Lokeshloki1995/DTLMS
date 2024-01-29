<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ScrapEntryView.aspx.cs" Inherits="IIITS.DTLMS.ScrapEntry.ScrapEntryView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript" >
     function ValidateMyForm() {

         if (document.getElementById('<%= cmbStore.ClientID %>').value == "--Select--") {
             alert('Select Store to Search')
             document.getElementById('<%= cmbStore.ClientID %>').focus()
             return false
         }

//         if (document.getElementById('<%= txtWONo.ClientID %>').value.trim() == "") {
//             alert('Enter Valid OM No')
//             document.getElementById('<%= txtWONo.ClientID %>').focus()
//             return false
//         }
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
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    Scrap Entry View
                   </h3>
    <%--                   <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                                          <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Scrap Entry View</h4>
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
                                            <label class="control-label">OM No </label>
                                            <div class="controls">
                                                <div class="input-append">                                                                                                           
                                                     <asp:TextBox ID="txtWONo" runat="server" MaxLength="50" ></asp:TextBox>
                                                     <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                      TabIndex="2"   />      
                                                </div>
                                            </div>
                                        </div>
                                    <div class="control-group">
                                     <label class="control-label">Make</label>
                                     <div class="controls">
                                        <div class="input-append">
                                                       
                                            <asp:DropDownList ID="cmbMake" runat="server">
                                            </asp:DropDownList>  
            
                                
                                        </div>
                                    </div>
                                 </div>
   
             
                         

                    </div>
                    <div class="span5">

                      <div class="control-group">
                        <label class="control-label">Capacity(in KVA)</label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                 <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="1">
                                </asp:DropDownList>  
                            </div>
                        </div>
                    </div>
                
                     <div class="control-group" style="display:none">
                        <label class="control-label">Suplier/Repairer</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:DropDownList ID="cmbSupplier" runat="server" TabIndex="2">
                                </asp:DropDownList>  
                                                      
                            </div>
                        </div>
                    </div>

                       <div class="control-group">
                        <label class="control-label">Store<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:DropDownList ID="cmbStore" runat="server" TabIndex="3">
                                </asp:DropDownList>  
                                                       
                            </div>
                        </div>
                    </div>
                                                       
                      </div>

                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                         <div class="space20"></div>
                                    <div  class="text-center">

                                  
                                        <asp:Button ID="cmdLoad" runat="server" Text="Load Scrap Transformers"  OnClientClick="javascript:return ValidateMyForm()"
                                        CssClass="btn btn-primary" onclick="cmdLoad_Click" TabIndex="4" />
                            
                          
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" TabIndex="5" onclick="cmdReset_Click" />
                       
                                   
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-infos" 
                                          OnClick="Export_clickScrapEntry" OnClientClick="javascript:return ValidateMyForm()" />
                      
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                        </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           <asp:GridView ID="grdFaultTC" 
                                AutoGenerateColumns="false"  PageSize="10" DataKeyNames="TC_ID"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onrowcommand="grdFaultTC_RowCommand"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                onpageindexchanging="grdFaultTC_PageIndexChanging" TabIndex="6" OnSorting="grdFaultTC_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                 <asp:TemplateField AccessibleHeaderText="ST_ID" HeaderText="ST ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStId" runat="server" Text='<%# Bind("ST_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                 <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select" >                                
                                        <ItemTemplate>                                       
                                            <asp:CheckBox ID="chkSelect" runat="server"  />
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">                                
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
                                   
                                    <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" SortExpression="TS_NAME">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                   
                                    <asp:TemplateField HeaderText="Edit" Visible="false">
                                    <ItemTemplate>
                                        <center>                                     
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" 
                                              CommandName="Submit"   Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                               
                                </Columns>

                            </asp:GridView>
                        
                             <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    
                            <div class="space20"></div>
                                <div class="space20"></div>
                        <div  class="form-horizontal" align="center">

                        <div class="span3"></div>
                            <div class="span3">
                            <asp:Button ID="cmdSend" runat="server" Text="Send to Dispose"  
                            CssClass="btn btn-primary" onclick="cmdSend_Click" Visible="false" TabIndex="7"  />
                            </div>
                            <%-- <div class="span1"></div>--%>
                         <div class="space20"></div>
                        </div>
                          
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Search TC Which is Declared as Scrap and To Send TC For Disposal
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Enter or Search OM No from OM No TextBox
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Load Scrap TC Click On <u>Load Scrap Transformers</u> Button
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Select CheckBoxes For The TC which will be Sent For Disposal
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>After Seleting TC Click On <u>Send To Dispose</u> Button
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Fill All The Required Details and Click On <u>Save</u> Button to Save Dispose Details
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
