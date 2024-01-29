<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/DTLMS.Master"  CodeBehind="TcInwardView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

            <style>
        input#ContentPlaceHolder1_cmdNew{
            margin-right:10px
        }
    </style>
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
        background:url(img/sort_both.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
    </style>  

    <script type="text/javascript" src="../js/jquery-OneEightThree.min.js"></script>
    

    <script type="text/javascript" src="../assets/bootstrap-toggle-buttons/static/js/jquery.min.js"></script>

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
                     Inward DTR View
                   </h3>
<%--                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                          <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Inward DTR View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                                <div style="float:right" >
                                <div style="display:flex!important" class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New DTR Inward " 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" />
                                  
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickTCMaster" /><br />
                                          </div>

                                            </div>
                                  
                                
                                <!-- END FORM-->
                          <div class="widget-body form">
                            <!-- BEGIN FORM-->
                              <div class="form-horizontal">


                                  <div id="Details" runat="server" class="row-fluid" style="display:none">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">


                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged"
                                                           >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged"
                                                            >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                           
                                        </div>
                                        <div class="span5">
                                           
                                                <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged"
                                                            ></asp:DropDownList>                                                        
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">
                                                    OMU </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOMSection" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--<div class="control-group">
                                                <label class="control-label">
                                                    Station </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStation" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="Button1" runat="server" Text="Load" CssClass="btn btn-primary" Visible="True"
                                         OnClick="cmdLoad_Click" />
                                                    </div>
                                                </div>
                                            </div>--%>

                                        </div>
                                    
                                        <div class="span1">
                                        </div>
                                    </div>
                                 <div class="space20">
                                        </div>

                                <div class="row-fluid">
                               <%-- <div class="span1"></div>--%>
                                                                            <div class="span1">
                                        </div>
                               <div class="span5">
                                  <div class="control-group">
                                <label class="control-label">Make Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbMake" runat="server"  TabIndex="9">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
                                   <div class="control-group">
                                <label class="control-label">Store</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbDivision" runat="server"  TabIndex="9">                                   
                                                </asp:DropDownList>       
                                             <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" Width="116px" OnClick="cmdLoad_Click" />
                                        </div>
                                   </div>
                              </div>
                                   <%--<div class="control-group">
                                <label class="control-label">Current Location</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbCurrentLoc" runat="server" TabIndex="9" OnSelectedIndexChanged="cmbCurrentLoc_SelectedIndexChanged" >                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>--%>

                             </div>

                             <div class="span5">
  
                             <div class="control-group">
                                <label class="control-label">Capacity(in KVA)</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbCapacity" runat="server"  TabIndex="9">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>

                                 <div class="control-group" runat="server" visible="false" id="divStore">
                                <label class="control-label">Store</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbstore" runat="server" TabIndex="9">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
               
                               <div class="span5">
                                  
                                   
                                </div> 
                               </div>   
                                                              
                                    </div>
                                  <br />
                                   <asp:Label ID="lblTotalDTr" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>         
                               </div>
                            </div>
                        
                            <asp:GridView ID="grdTcMaster" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdTcMaster_PageIndexChanging" 
                                    onrowcommand="grdTcMaster_RowCommand"    OnSorting="grdTcMaster_Sorting" AllowSorting="true" >
                             <HeaderStyle CssClass="both"  />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText=" Transformer ID"  Visible="false">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblTcId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" SortExpression="TC_CODE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="90px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtTCCode" runat="server" placeholder="Enter DTr Code" width="100px" MaxLength="8"></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SlNo" SortExpression="TC_SLNO">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                           <FooterTemplate>
                                           <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtTCSlno" runat="server" placeholder="Enter DTr Slno"  width="100px" ></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="TC_MAKE_ID" HeaderText="Make" SortExpression="TC_MAKE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcMake" runat="server" Text='<%# Bind("TC_MAKE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                           <FooterTemplate>
                                           <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtMake" runat="server" placeholder="Enter Make" ></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TC_MAKE_ID" HeaderText="Make" SortExpression="TC_MAKE_ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcMakeiD" runat="server" Text='<%# Bind("TC_MAKE_ID") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                           
                                    </asp:TemplateField>


                                     <asp:TemplateField AccessibleHeaderText="TC_PO_NO" HeaderText="PO No" SortExpression="TC_PO_NO" HeaderStyle-Width="50px" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblpono" runat="server" Text='<%# Bind("TC_PO_NO") %>' style="word-break: break-all;" width="50px"></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                           <asp:Panel ID="panel5" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtpono" runat="server" placeholder="Enter PO No" width="100px"></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>  
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_DI_NO" HeaderText="DI No" SortExpression="TC_DI_NO" HeaderStyle-Width="5%">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lbldino" runat="server" Text='<%# Bind("TC_DI_NO") %>' style="word-break: break-all;" width="50px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:Panel ID="panel6" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtdino" runat="server" placeholder="Enter DI No" width="100px"></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>      
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="Division" SortExpression="DIV_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDiv" runat="server" Text='<%# Bind("DIV_NAME") %>' style="word-break: break-all;" width="110px"></asp:Label>
                                        </ItemTemplate> 
                                          <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>            
                                    </asp:TemplateField>--%>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                       <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="TC_LIFE_SPAN" HeaderText="Life Span" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcLifeSpan" runat="server" Text='<%# Bind("TC_LIFE_SPAN") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                   
                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create" 
                                                Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                CausesValidation="false" />
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All DTR Details Which are come Trough purchase order.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Existing DTR Details Can Be Edited By Clicking Edit Button
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New DTR  Can Be Added By Clicking New DTR Button
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
 
