<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransSupplierView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TransSupplierView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      
     <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

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
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    DTR Supplier View
                   </h3>
<%--                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                            <h4><i class="icon-reorder"></i> Supplier Details View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                       

                                <div style="float:right" >
                                <div class="span6">
                                   <asp:Button ID="cmdNew" runat="server" Text="New Supplier" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" /><br /></div>
                                    <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickSupplier" /><br />
                                          </div>

                                            </div>

                      
                                <div class="space20"></div>
                                <!-- END FORM-->

     <asp:GridView ID="grdSupplierDetails" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"  
     ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
     CssClass="table table-striped table-bordered table-advance table-hover"  
      runat="server"  onpageindexchanging="grdSupplierDetails_PageIndexChanging" 
                                    onrowcommand="grdSupplierDetails_RowCommand" 
                                    onrowdatabound="grdSupplierDetails_RowDataBound" OnSorting="grdSupplierDetails_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                              
     <Columns>
         <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
        <asp:TemplateField AccessibleHeaderText="TS_ID" HeaderText="Id" Visible="false">
          
            <ItemTemplate>                                                 
                <asp:Label ID="lblSuppId" runat="server" Text='<%# Bind("TS_ID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier Name" Visible="true" SortExpression="TS_NAME">
          
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("TS_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
             <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                    <asp:TextBox ID="txtSupplierName" runat="server" placeholder="Enter Supplier Name" ></asp:TextBox>
                    </asp:Panel>
             </FooterTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_ADDRESS" HeaderText="Addresss" Visible="false" >
           
            <ItemTemplate>
                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("TS_ADDRESS") %>' style="word-break: break-all;" width="220px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_PHONE" HeaderText="Phone no">
           
            <ItemTemplate>
                <asp:Label ID="lblphone" runat="server" Text='<%# Bind("TS_PHONE") %>'></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
                 <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
             </FooterTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_EMAIL" HeaderText="Email Id">
           
            <ItemTemplate>
                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("TS_EMAIL") %>' style="word-break: break-all;" width="150px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField AccessibleHeaderText="TS_BLACK_LISTED" HeaderText="Black List">
           
            <ItemTemplate>
                <asp:Label ID="lblblacklist" runat="server" Text='<%# Bind("TS_BLACK_LISTED") %>' style="word-break: break-all;" width="80px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_BLACKED_UPTO" HeaderText="Black Listed Upto">
          
            <ItemTemplate>
                <asp:Label ID="lbldate" runat="server" Text='<%# Bind("TS_BLACKED_UPTO") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Edit">
            <ItemTemplate>
                <center>
                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" title="Click To Edit"
                          Width="12px" CommandName="create" />
                </center>
            </ItemTemplate>
        </asp:TemplateField>


         <asp:TemplateField HeaderText="Status" Visible="false"> 
             <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("TS_STATUS") %>' ></asp:Label>
                    <center>
                        <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                          tooltip="Click to Activate Supplier" OnClientClick="return confirm ('Are you sure, you want to Activate Supplier');" width="10px" />        
                        <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                           tooltip="Click to DeActivate Supplier"  OnClientClick="return confirm ('Are you sure, you want to DeActivate Supplier');" />        
                   </center>
             </ItemTemplate>
       </asp:TemplateField>   
    </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
</asp:GridView>

<div class="span7"></div>
 <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Existing Supplier and New Supplier Can Be Added
                        </p>
                         
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Existing Supplier Details Can Be Edited By Clicking Edit Button
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New Supplier Can Be Added By Clicking New Supplier Button
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
