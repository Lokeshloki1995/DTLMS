<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreIndentView.aspx.cs" Inherits="IIITS.DTLMS.StoreTransfer.StoreIndentView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                  Indent View
                  
                   </h3>
                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
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
                            <h4><i class="icon-reorder"></i> Store Indent View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                                </span>
                        </div>
                        <div class="widget-body">
                       
                        <div class="row-fluid">
                             <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span3">
                            <asp:RadioButton ID="rdbPendingIndent" runat="server" Text="Pending for Invoice" CssClass="radio" 
                                  GroupName="a" Checked="true" AutoPostBack="true" oncheckedchanged="rdbPendingIndent_CheckedChanged"
                                   />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbAlreadyCompleted" runat="server"  Text="Completed Invoice for Indent" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true" oncheckedchanged="rdbAlreadyCompleted_CheckedChanged"
                                   />
                            </div>

                             <div style="float:right;">
                               <div style="float:right" >
                                   <div class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" /><br /></div>
                                   <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickStoreindent" /><br />
                                          </div>
                                 </div> 
                             </div>
                        </div>
                      
                                <div class="space20"></div>
                                <!-- END FORM-->
                                                  
                            <asp:GridView ID="grdIndentView" 
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdIndentView_PageIndexChanging" 
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" 
                                onrowcommand="grdIndentView_RowCommand" OnSorting="grdIndentView_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="SI_ID" HeaderText="Indent Id" Visible=false>                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIndentId" runat="server" Text='<%# Bind("SI_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
  
                                    <asp:TemplateField AccessibleHeaderText="SI_NO" HeaderText="Indent Number" SortExpression="SI_NO">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblSiNo" runat="server" Text='<%# Bind("SI_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                         <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                            <asp:TextBox ID="txtIndentNo" runat="server" placeholder="Enter Indent No " Width="150px" MaxLength="50" ></asp:TextBox>
                                       </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SI_DATE" HeaderText="Indent Date">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIndentDate" runat="server" Text='<%# Bind("SI_DATE") %>'></asp:Label>                                  
                                        </ItemTemplate>
                                        <FooterTemplate>
                                          <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Requested No. of Transformers">                                    
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("SO_QNTY") %>'></asp:Label>
                                        </ItemTemplate>          
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SI_TO_STORE" HeaderText="To Store" SortExpression="SI_TO_STORE">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblToStore" runat="server" Text='<%# Bind("SI_TO_STORE") %>' style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server"  ImageUrl="../img/Manual/view.png"  
                                              CommandName="Submit"   />
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Store Indent Details and To Create New Store Indent.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Store Indent Can Be Filtered By Selecting <u>Pending for Invoice</u> and <u>Completed Invoice for Indent</u> Radio Button
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New Store Indent Can Be Created By Clicking <u>New</u> Button
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
