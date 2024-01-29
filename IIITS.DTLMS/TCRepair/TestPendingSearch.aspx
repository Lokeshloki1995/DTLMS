<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TestPendingSearch.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TestPendingSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
      <script type="text/javascript" >
          function ValidateMyForm() {

              if (document.getElementById('<%= txtPONo.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Purchase Order No')
                  document.getElementById('<%= txtPONo.ClientID %>').focus()
                  return false
              }
          }
      </script>
 <%--       <script type="text/javascript">

        function preventMultipleSubmissions() {
   $('#<%=cmdSave.ClientID %>').prop('disabled', true);

}

window.onbeforeunload = preventMultipleSubmissions;
 </script>   onchange="javascript:preventMultipleSubmissions();"--%>
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
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                   Transformer Inspection at Repair Center
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
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Transformer Inspection at Repair Center</h4>
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
                                            <label class="control-label">Purchase Order No <span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    
                                                     <asp:TextBox ID="txtPONo" runat="server" MaxLength="50" ></asp:TextBox>
                                                     <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                      TabIndex="2"   />  
                                                    <asp:TextBox ID="txtsstoreid" runat="server" Width="20px" Visible="false"></asp:TextBox>    
                                                </div>
                                            </div>
                                        </div>

                                         <div class="control-group">
                                    <label class="control-label">Capacity(in KVA)</label>
                                        <div class="controls">
                                            <div class="input-append">
                                               <asp:DropDownList ID="cmbCapacity" runat="server"> </asp:DropDownList>
                                        
                                            </div>
                                        </div>
                                </div> 
                                        <%-- <div class="control-group">
                                            <label class="control-label">Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"> </asp:DropDownList>
                                                    </div>
                                                </div>
                                        </div>

                                        
                                      <div class="control-group">
                                        <label class="control-label">Supplier</label>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:DropDownList ID="cmbSupplier" runat="server"> </asp:DropDownList>
                                                </div>
                                            </div>
                                     </div>
                                       --%>

                                    </div>
                                       
                             <div class="span5">


                                <div class="control-group">
                                    <label class="control-label">Make</label>
                                        <div class="controls">
                                            <div class="input-append">
                                            <asp:DropDownList ID="cmbMake" runat="server"> </asp:DropDownList>                                                       
                                            </div>
                                        </div>
                                </div>
                                   
                                <%--<div class="control-group">
                                    <label class="control-label">Capacity(in KVA)</label>
                                        <div class="controls">
                                            <div class="input-append">
                                               <asp:DropDownList ID="cmbCapacity" runat="server"> </asp:DropDownList>
                                        
                                            </div>
                                        </div>
                                </div>--%> 
                            </div>                                   
                </div>
                <div class="space20"></div>
                <div class="space20"></div>
                <div  class="text-center">
                    
                        <asp:Button ID="cmdLoad" runat="server" Text="Load Pending Transformer"  OnClientClick="javascript:return ValidateMyForm()"
                            CssClass="btn btn-primary" onclick="cmdLoad_Click" />
                    
                  
                        <asp:Button ID="cmdReset" runat="server" Text="Reset"  CssClass="btn btn-danger"  onclick="cmdReset_Click" />
                    
                    
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickTestPendingSearch" OnClientClick="javascript:return ValidateMyForm()"/><br />
                     
             
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>                                            
                </div>
            </div>
                        </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           <asp:GridView ID="grdPendingTc" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" DataKeyNames="RSD_ID"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdPendingTc_PageIndexChanging" 
                                TabIndex="5" OnSorting="grdPendingTc_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
                                <Columns>

                                  

                                     <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="TC ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>'></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select" >                                
                                        <ItemTemplate>                                       
                                            <asp:CheckBox ID="chkSelect" runat="server"  />
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">                                
                                    <ItemTemplate>                                       
                                    <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                    </ItemTemplate>
                                  </asp:TemplateField>
                                    
                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("CAPACITY") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
     
                                    <asp:TemplateField AccessibleHeaderText="RSM_ISSUE_DATE" HeaderText="Issue Date" Visible="false">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("RSM_ISSUE_DATE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" Visible="false">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("SUP_REPNAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField AccessibleHeaderText="RSM_PO_DATE" HeaderText="PO Date" >
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("RSM_PO_DATE") %>' style="word-break: break-all;" width="150px"></asp:Label>
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
                            <asp:Button ID="cmdDeliver" runat="server" Text="Click to Inspect"  
                            CssClass="btn btn-primary"  Visible="false" onclick="cmdDeliver_Click" 
                                    TabIndex="6"  />
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

                                
                                    <asp:TemplateField AccessibleHeaderText="PONO" HeaderText="PO No" SortExpression="PONO">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("PONO") %>' style="word-break: break-all;" width="120px"></asp:Label>                                  
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Inspect The Transformer at Repair Center
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Need To Enter Or Search PO No in Purchase Order No Textbox
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Purchase Order No is Entered Click On <u>Load Pending Transformer</u> Button,It will List Transformer Which Are Pending For Inspection for That Particular PO No
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Can Select Checkboxes For The Transformer which They Want To Inspect after That They Need To Click <u>Click To Inspect</u> Button To Inspect The TC
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
