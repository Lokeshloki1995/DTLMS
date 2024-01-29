<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="QcDoneView.aspx.cs" Inherits="IIITS.DTLMS.Internal.QcDoneView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div >      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    QC Done View
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
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
                            <h4><i class="icon-reorder"></i> QC Done View </h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">                               
                            <div class="form-horizontal"> 
                                <div class="row-fluid">
                                    <div class="span1"></div>
                                        <div class="span5">
                                           <div class="control-group">
                                            <label class="control-label">Qc Done By</label>
                                        <div class="controls">
                                     <div class="input-append">   
                                        <asp:DropDownList ID="cmbQcDone" runat="server"  Width="200px" TabIndex="9">                                   
                                                </asp:DropDownList>
                                        </div>
                            </div>
                        </div>
                         <div class="control-group">
                                      <label class="control-label">Details Entered By</label>
                                         <div class="controls">
                                     <div class="input-append">  
                                        <asp:DropDownList ID="cmbDeEnter" runat="server"  Width="200px" TabIndex="9">                                   
                                                </asp:DropDownList>
                                                </div>
                                                 </div>
                        </div>
                        </div>
                                                 
                                                  <%--<div class="span5">
                                                  <div class="control-group">
                                                       <label class="control-label">DTC Code</label>
                                         <div class="controls">
                                     <div class="input-append">  
                                        <asp:TextBox ID="txtDtcCode"  runat="server" Width="200px"></asp:TextBox>
                                <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" 
                                    />
                                                </div>
                                                 </div>
                        </div>            
                                                  <div class="control-group">
                                                    <label class="control-label">SS Plate No/DTr Code</label>
                                        <div class="controls">
                                     <div class="input-append">  
                                        <asp:TextBox ID="txtDTrCode"  runat="server" Width="200px"></asp:TextBox>
            
                                <asp:Button ID="cmdSearch1" Text="S" class="btn btn-primary" runat="server" 
                                     />
                                                </div>
                               </div>
                               </div>
                                </div>--%>
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                   <asp:Button ID="cmdLoad" CssClass="btn btn-primary"  runat="server" Text="Load" 
                                       onclick="cmdLoad_Click" />
                               </div>
                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            </div>
                            </div>      
                              <div class="space20"></div>                          
                                <!-- END FORM-->                          
                                                    <asp:GridView ID="grdQcDone" 
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" Visible="true" ShowFooter="true"
                                
                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdQcDone_PageIndexChanging" 
                            onrowcommand="grdQcDone_RowCommand"
                           >
                                <Columns>
                                    
                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="ID"  Visible="false">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblEnumDetailsId" runat="server" Text='<%# Bind("ED_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="TYPE" HeaderText="Enum. Type"  Visible="true">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblEnuType" runat="server" Text='<%# Bind("TYPE") %>' style="word-break:break-all" width="100px" ></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="DTC CODE" HeaderText="DTC Code" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("DTE_DTCCODE") %>' style="word-break:break-all" width="80px" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                     <asp:TextBox ID="txtDTCCode" runat="server" placeholder="Enter DTC Code"></asp:TextBox>
                                   </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="DTC NAME" HeaderText="DTC Name" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDTCName" runat="server" Text='<%# Bind("DTE_NAME") %>' style="word-break:break-all" width="150px" ></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                              <asp:TextBox ID="txtDtcName" runat="server" placeholder="Enter DTC Name"></asp:TextBox>
                                   </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="TC Code" HeaderText="DTr Code" >                                
                                    <ItemTemplate>                                       
                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DTE_TC_CODE") %>' style="word-break:break-all" width="100px" ></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                   <asp:TextBox ID="txtTcCode" runat="server" placeholder="Enter DTr Code">
                                      </asp:TextBox>
                                   </FooterTemplate>
                                      
                                  </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="ED_CRBY" HeaderText="Details Entered By" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblOperator1" runat="server" Text='<%# Bind("ED_CRBY") %>' style="word-break:break-all" width="120px" ></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                             <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="30px"
                                                 ToolTip="Search" CommandName="search" />
                                   </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="ED_APPROVED_BY" HeaderText="QC Done By" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblQcDone" runat="server" Text='<%# Bind("ED_APPROVED_BY") %>' style="word-break:break-all" width="120px" ></asp:Label>
                                        </ItemTemplate>
                                          
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                
                </div>

                    
            <!-- END PAGE CONTENT-->
      </div>
 </div>
</asp:Content>
