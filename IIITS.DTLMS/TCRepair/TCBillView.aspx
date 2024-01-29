<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TCBillView.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TCBillView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                     TC Bill View
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i> TC Bill View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                        
                                <div style="float:right" >
                                <div class="span2">
                                   <asp:Button ID="cmdNew" runat="server" Text="New Bill" 
                                              CssClass="btn btn-primary" onclick="cmdNew_Click" /><br /></div>

                                            </div>
                                  
                                                      
                                <div class="space20"></div>
                                <!-- END FORM-->
                                                  
                            <asp:GridView ID="grdTCBill" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdTCBill_PageIndexChanging" 
                                    onrowcommand="grdTCBill_RowCommand" >
                                <Columns>
                                         <asp:TemplateField AccessibleHeaderText="BT_ID" HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblBillId" runat="server" Text='<%# Bind("BT_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="BT_WO_NO" HeaderText="Work Order No.">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWONo" runat="server" Text='<%# Bind("BT_WO_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField AccessibleHeaderText="BT_PO_NO" HeaderText="PO No">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("BT_PO_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="BT_PO_DATE" HeaderText="PO Date">                                     
                                        <ItemTemplate>                                        
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("BT_PO_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   

                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" 
                                            CommandName="Editt" Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
      </div>
      </div>
</asp:Content>
