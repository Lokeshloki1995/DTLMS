<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PoMasterView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.PoMasterView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
    .ascending th a {
        background:url(img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
         background: url(img/sort_both.png) no-repeat;
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
                 Purchase Order View
                  
                   </h3>
                 <%--      <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Purchase Order View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

                        <div class="widget-body">
                         <div style="float:right" >
                             <div class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New PO" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click"  /><br /></div>

                              <div   style="display:flex!important;padding-left: 0.5cm"  class="span5">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickPOMaster" /><br />
                                          </div>

                                            </div> 
                                <div class="space20"></div>
                                <!-- END FORM-->
                                                  
                            <asp:GridView ID="grdPoMasterView" 
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdPoMasterView_PageIndexChanging" 
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" 
                                onrowcommand="grdPoMasterView_RowCommand"   ShowFooter="True" OnSorting="grdPOmaster_Sorting" AllowSorting="true">
                             <HeaderStyle CssClass="both"/>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="PO_ID" HeaderText="PO Id" Visible=false>                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPoId" runat="server" Text='<%# Bind("PO_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                         
                                         
                                     <asp:TemplateField AccessibleHeaderText="Po No" HeaderText="PO No" SortExpression="PO_NO">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("PO_NO") %>' style="word-break:break-all" Width="120px"></asp:Label>                                 
                                        </ItemTemplate>

                                         <FooterTemplate>
                                         <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtsPoNumber" runat="server"  Width="150px"  placeholder="Enter PO Number" ToolTip="Enter PO Number to Search"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="PO_DATE" HeaderText="Po Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("PO_DATE") %>' style="word-break:break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                             <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9"/>
                                         </FooterTemplate>  
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PO_SUPPLIER_ID" HeaderText="Supplier Name" SortExpression="PO_SUPPLIER_ID">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("PO_SUPPLIER_ID") %>' style="word-break:break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PB_QUANTITY" HeaderText="Quantity">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoQuantity" runat="server" Text='<%# Bind("PB_QUANTITY") %>' style="word-break:break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                  <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"  
                                              CommandName="Submit"   Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All PO Details,Existing PO Details Can be Edited and New PO can Be Create.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit PO Details Click On Edit Button Enter Details And Click On Update Button To Update the Details.
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Create New PO Click On New Button.
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
