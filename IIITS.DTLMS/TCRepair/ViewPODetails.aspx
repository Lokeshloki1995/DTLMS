<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ViewPODetails.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.ViewPODetails" %>
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
                 <%--  Transformer Inspection at Repair Center--%>
                       Repair Transformer  Details
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
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>  Repair Transformer  Details</h4>
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
                                         <%--    <label class="control-label">Rate Contract Order No<span class="Mandotary"> *</span></label>--%>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    
                                                     <asp:TextBox ID="txtPONo" runat="server" MaxLength="50" ></asp:TextBox>
                                                     <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                      TabIndex="2"   />      
                                                </div>
                                            </div>
                                        </div>
<%--
                                         <div class="control-group">
                                            <label class="control-label">Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"> </asp:DropDownList>
                                                    </div>
                                                </div>
                                        </div>--%>

                                        
<%--                                      <div class="control-group">
                                        <label class="control-label">Supplier</label>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:DropDownList ID="cmbSupplier" runat="server"> </asp:DropDownList>
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
                  
                  
                        <asp:Button ID="cmdReset" runat="server" Text="Reset"  
                            CssClass="btn btn-danger" onclick="cmdReset_Click"  />
           
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickPODetails"  OnClientClick="javascript:return ValidateMyForm()"/><br />
                                   
                   
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>                                            
                </div>
            </div>
                        </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           <asp:GridView ID="grdRepairerPoDetails" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" 
                                TabIndex="5" onpageindexchanging="grdRepairerPoDetails_PageIndexChanging" 
                               OnSorting="grdRepairerPoDetails_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SlNo" SortExpression="TC_SLNO">                                
                                    <ItemTemplate>                                       
                                    <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                    </ItemTemplate>
                                  </asp:TemplateField>
                                    
                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
     
                                    <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manufacture Date">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblManufactureDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="RSM_GUARANTY_TYPE" HeaderText="Guarantee Type">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("RSM_GUARANTY_TYPE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                               
                                </Columns>

                            </asp:GridView>
                        
                            
                        </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>

</asp:Content>
