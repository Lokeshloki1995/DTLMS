<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransRepairerView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TransRepairerView" %>
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
                    DTR Repairer View
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
                            <h4><i class="icon-reorder"></i>Repairer Details View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                       

                      
                             
                                <div style="float:right" >
                                <div class="span7">
                                   <asp:Button ID="cmdNew" runat="server" Text="New Repairer" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" /></div>
                                     <div class="span3">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickRepairer" /><br />
                                          </div>

                                            </div>

                      
                                <div class="space20"></div>
                                <!-- END FORM-->
                           



 <asp:GridView ID="grdRepairer" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"  
 ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
 CssClass="table table-striped table-bordered table-advance table-hover"
  runat="server" onpageindexchanging="grdRepairer_PageIndexChanging" onrowcommand="grdRepairer_RowCommand" 
                                    onrowdatabound="grdRepairer_RowDataBound" OnSorting="grdRepairer_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
    <Columns>
        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
    <asp:TemplateField AccessibleHeaderText="TR_ID" HeaderText="Id" Visible="false">        
            <ItemTemplate>                                                 
                <asp:Label ID="lblRepairId" runat="server" Text='<%# Bind("TR_ID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Repairer Name" Visible="true" SortExpression="TR_NAME">           
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("TR_NAME") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
             <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                    <asp:TextBox ID="txtRepairerName" runat="server" placeholder="Enter Repairer Name" Width="100px" ></asp:TextBox>
             </asp:Panel>
             </FooterTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_ADDRESS" HeaderText="Addresss" Visible="false">          
            <ItemTemplate>
                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("TR_ADDRESS") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_PHONE" HeaderText="Phone no" Visible="true">          
            <ItemTemplate>
                <asp:Label ID="lblphone" runat="server" Text='<%# Bind("TR_PHONE") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
              <FooterTemplate>
                 <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
             </FooterTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_EMAIL" HeaderText="EmailId"
            Visible="true">
            
            <ItemTemplate>
                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("TR_EMAIL") %>' style="word-break: break-all;" width="120px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TR_OFFICECODE" HeaderText="Location"
            Visible="true" SortExpression="TR_OFFICECODE">
            
            <ItemTemplate>
                <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("TR_OFFICECODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField AccessibleHeaderText="TR_OFFICECODENAME" HeaderText="Location Name"
            Visible="true" SortExpression="TR_OFFICECODENAME">
            
            <ItemTemplate>
                <asp:Label ID="lblOfficeCodeName" runat="server" Text='<%# Bind("TR_OFF_CODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField AccessibleHeaderText="TS_BLACK_LISTED" HeaderText="Black List"
            Visible="true">
         
            <ItemTemplate>
                <asp:Label ID="lblblacklist" runat="server" Text='<%# Bind("TR_BLACK_LISTED") %>' style="word-break: break-all;" width="80px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField AccessibleHeaderText="TS_BLACKED_UPTO" HeaderText="Black Listed upto"
            Visible="false">
          
            <ItemTemplate>
                <asp:Label ID="lbldate" runat="server" Text='<%# Bind("TR_BLACKED_UPTO") %>' style="word-break: break-all;" width="100px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Edit">
            <ItemTemplate>
                <center>
                    <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                        Width="12px"  CommandName="create" />
                </center>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Status" Visible="false"> 
                <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("TR_STATUS") %>' ></asp:Label>
                    <center>
                        <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                        tooltip="Click to Activate Repairer" OnClientClick="return confirm ('Are you sure, you want to Activate Repairer');" width="10px" />        
                        <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                        tooltip="Click to DeActivate Repairer"  OnClientClick="return confirm ('Are you sure, you want to DeActivate Repairer');" />        
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Existing Repairer and New Repairer Can Be Added
                        </p>
                         
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Existing Repairer Details Can Be Edited By Clicking Edit Button
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New Repairer Can Be Added By Clicking New Repairer Button
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
