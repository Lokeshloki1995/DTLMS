<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeliverPendingSearch.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.DeliverPendingSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
   <script type="text/javascript" >
//       function ValidateMyForm() {

//           if (document.getElementById('<%= txtWoNo.ClientID %>').value.trim() == "") {
//               alert('Enter Valid Purchase Order No')
//               document.getElementById('<%= txtWoNo.ClientID %>').focus()
//               return false
//           }
//       }
            </script>
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

 <div>
      <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Transformer Pending to Recieve                   
                                      
                   </h3>
                 <%--       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                            <h4><i class="icon-reorder"></i> Transformer Pending to Recieve   </h4>
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
                                            <label class="control-label">Purchase Order No</label>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:TextBox ID="txtWoNo" runat="server" MaxLength="50" ></asp:TextBox>
                                                     <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                      TabIndex="2"  />      
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"> </asp:DropDownList>

                                                        <asp:TextBox ID="txtsstoreid" runat="server" Width="20px" Visible="false"></asp:TextBox> 
                                                    </div>
                                                </div>
                                        </div>                                    

                                     

                                    </div>
                                       
                    <div class="span5">                      

                                  <div class="control-group">
                                        <label class="control-label">Supplier</label>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:DropDownList ID="cmbSupplier" runat="server"> </asp:DropDownList>
                                                </div>
                                            </div>
                                     </div>
                    </div>                                   
                </div>
                <div class="space20"></div>
                <div class="space20"></div>
                <div  class="text-center">
                   
                        <asp:Button ID="cmdLoad" runat="server" Text="Load Pending Transformer"  OnClientClick="javascript:return ValidateMyForm()"
                            CssClass="btn btn-primary" onclick="cmdLoad_Click" />
                 
               
                        <asp:Button ID="cmdReset" runat="server" Text="Reset"  CssClass="btn btn-danger"  onclick="cmdReset_Click" />
                  
                
                
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

              <div class="row-fluid" runat="server"  >
                    <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Testing Pass Details</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="">
                         <asp:Button ID="cmbExport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickDeliverPendingSearch" /><br />
                         </div>
                            <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                           <asp:GridView ID="grdTestingPass" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server"  
                                TabIndex="5" onrowcommand="grdTestingPass_RowCommand" 
                                        onpageindexchanging="grdTestingPass_PageIndexChanging" OnSorting="grdTestingPass_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                    <asp:TemplateField AccessibleHeaderText="RSM_ID" HeaderText="Master Id" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblRepairMasterId" runat="server" Text='<%# Bind("RSM_ID") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   

                                    <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' style="word-break: break-all;" width="120px"></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("PODATE") %>' style="word-break: break-all;" width="80px"></asp:Label>                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="ISSUEDATE" HeaderText="Issue Date" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIssueDate" runat="server" Text='<%# Bind("ISSUEDATE") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" SortExpression="SUP_REPNAME">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupRepName" runat="server" Text='<%# Bind("SUP_REPNAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PO_QUANTITY" HeaderText="Total Quantity">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalQty" runat="server" Text='<%# Bind("PO_QUANTITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PENDING_QNTY" HeaderText="Pending Qty for Recieve">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblPendingQty" runat="server" Text='<%# Bind("PENDING_QNTY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DELIVERED_QNTY" HeaderText="Recieved Quantity">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveredQty" runat="server" Text='<%# Bind("DELIVERED_QNTY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Recieve">
                                    <ItemTemplate>
                                        <center>
                                           <asp:ImageButton  ID="imgbtnRecieve" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"  
                                              CommandName="Recieve"   Width="12px" />
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


               <div  class="row-fluid" >
                    <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Pending for Testing</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div class="">
                         <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickPendingTesting" /><br />
                         </div>
                            <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                           <asp:GridView ID="grdTestPending" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server"  
                                TabIndex="5" onpageindexchanging="grdTestPending_PageIndexChanging" OnSorting="grdTestPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                
                                    <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("pono") %>' style="word-break: break-all;" width="120px"></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("podate") %>' style="word-break: break-all;" width="80px"></asp:Label>                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="ISSUEDATE" HeaderText="Issue Date" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIssueDate" runat="server" Text='<%# Bind("ISSUEDATE") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" SortExpression="SUP_REPNAME">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupRepName" runat="server" Text='<%# Bind("SUP_REPNAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PO_QUANTITY" HeaderText="Total Quantity">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalQty" runat="server" Text='<%# Bind("PO_QUANTITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PENDING_QNTY" HeaderText="Pending Qty for Testing">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblPendingQty" runat="server" Text='<%# Bind("PENDING_QNTY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DELIVERED_QNTY" HeaderText="Delivered Quantity" Visible="false">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveredQty" runat="server" Text='<%# Bind("DELIVERED_QNTY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
 
                                </Columns>

                            </asp:GridView>
                                     </div>
                                    </div>

          

                    </div>
                </div>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Recieve The Transformer for Which Testing is Completed it will also Display the Testing Pass Details and Pending for Testing Details
                        </p>
                      <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Select PO by clicking On Search Button and Click On Load Pending Transformer
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Receive The Transformer User Need To Click On <u>Receive</u> LinkButton
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Receive Button is Clicked User Need To Enter Receive Details and  after that <u>Recieve</u> Button To Receive The TC in Store
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
