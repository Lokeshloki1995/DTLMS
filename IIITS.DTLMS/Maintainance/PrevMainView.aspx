<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PrevMainView.aspx.cs" Inherits="IIITS.DTLMS.Maintainance.PrevMainView" %>
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
           <%--  BEGIN PAGE HEADER--%>
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                      Preventive Maintenance View
                   </h3>
<%--                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                                          <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> ddd </button>
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
                            <h4><i class="icon-reorder"></i> Preventive Maintenance View </h4>
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
                            <asp:RadioButton ID="rdbQuarterly" runat="server" Text="Quarterly" CssClass="radio" 
                                  GroupName="a" Checked="true" AutoPostBack="true" oncheckedchanged="rdbQuarterly_CheckedChanged" 
                                  />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbHalfYearly" runat="server"  Text="Half Yearly" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true" oncheckedchanged="rdbHalfYearly_CheckedChanged" 
                                   />
                            </div>

                             <div style="float:right;">
                                <div style="float:right" >
                                   
                                <div class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" /><br /></div>
                                     <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickMaintainance" /><br />
                                          </div>
      
                             </div>
   
                      </div>

                    <div class="space20"> </div>
                                 
                                <!-- END FORM-->
                           
                          <asp:GridView ID="grdPendingTcMaintainance"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"  ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdTcMaintainance_PageIndexChanging" 
                                    onrowcommand="grdTcMaintainance_RowCommand"  OnSorting="grdPendingTcMaintainance_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                  <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="Transformer Centre Name" SortExpression="DT_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblrepairtype" runat="server" Text='<%# Bind("DT_NAME") %>' style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                          <asp:Panel ID="panel1" runat="server" DefaultButton="cmdSearch" >
                                             <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name" Width="200px"></asp:TextBox>
                                       </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                   
                                   <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Transformer Centre code" SortExpression="DT_CODE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTmDtCode" runat="server" Text='<%# Bind("DT_CODE") %>' style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                         <asp:Panel ID="panel2" runat="server" DefaultButton="cmdSearch" >
                                              <asp:TextBox ID="txtDTCCode" runat="server" placeholder="Enter DTC Code" Width="150px"></asp:TextBox>
                                   </asp:Panel>
                                   </FooterTemplate>
                                    </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="DT_TC_ID" HeaderText="DTR Code" SortExpression="DT_TC_ID">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTmTcCode" runat="server" Text='<%# Bind("DT_TC_ID") %>' style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                               <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="30px"
                                                ToolTip="Search" CommandName="search" />
                                   </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TM_MAINTAIN_TYPE" HeaderText="Maintenance Type" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMaintainType" runat="server" Text='<%# Bind("TM_MAINTAIN_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="LAST_SERVICE_DATE" HeaderText="Last Service Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lbldate" runat="server" Text='<%# Bind("DT_LAST_SERVICE_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" OnClick="imgBtnEdit_Click"
                                                Width="12px" />
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Preventive Maintenance Details and New Transformer Centre Maintenance Plan Can Be Created.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Preventive Maintenance View Can Be Filtered by Selecting <u>Quarterly</u> and <u>Half Yearly Radio</u> Button
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New Preventive Maintenance Plan Can Be Created By Clicking <u>New</u> Button
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
