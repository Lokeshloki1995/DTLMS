<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="IndentView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.IndentView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function ConfirmIndent() {
        var sTypeValue = document.getElementById('<%= cmbType.ClientID %>');
        var selectedText = sTypeValue.options[sTypeValue.selectedIndex].innerHTML;
        var result = confirm('Are you sure,Do you want to Create Indent for ' + selectedText + ' ?');
        if (result) {
            return true;
        }
        else {
            return false;
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
          <%--             <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                            <h4><i class="icon-reorder"></i> Indent View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                         <div class="form-horizontal">
                                    <div class="row-fluid">
                       <div style="float:right" >
                            <div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickIndent" /><br />
                                       </div>
                           </div>
                              <%--  <div class="span8">--%>

                               <div class="span4">
                                <asp:Label ID="lblType" runat="server" Text="Type" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" 
                                       onselectedindexchanged="cmbType_SelectedIndexChanged" >  
                                               
                                         <asp:ListItem Text="Failure" Value="1" Selected="True"></asp:ListItem>
                                         <asp:ListItem Text="Failure With Enhancement" Value="4"></asp:ListItem>
                                          <asp:ListItem Text="Enhancement" Value="2"></asp:ListItem>
                                          <asp:ListItem Text="New Transformer Centre" Value="3"></asp:ListItem>
                                    </asp:DropDownList>   
                               </div>

                              <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" Visible="false"
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span1">
                            <asp:RadioButton ID="rdbViewAll" runat="server" Text="View All" CssClass="radio" 
                                  GroupName="a"   AutoPostBack="true" style="display:none"
                                  oncheckedchanged="rdbViewAll_CheckedChanged" />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbAlready" runat="server"  Text="Already Created" Checked="true"
                                   CssClass="radio" GroupName="a"  AutoPostBack="true" style="display:none"
                                   oncheckedchanged="rdbAlready_CheckedChanged" />
                            </div>

                             <div style="float:right;">
                                 <asp:Button ID="cmdNew" runat="server" Text="New" OnClientClick="return ConfirmIndent();"
                                       CssClass="btn btn-primary" onclick="cmdNew_Click" Visible="false" />
                             </div>

                                
                      </div>
                        </div>
                        </div>
                          
                        <div class="widget-body">
                          <div class="form-horizontal">
                              <div class="row-fluid">
                                     <asp:Label ID="lblGridType" runat="server"  Font-Bold="true" ForeColor="#4A8BC2"
                                        Font-Size="Medium"></asp:Label>
                               </div>
                           </div>
                       </div>
         <asp:GridView ID="grdIndent" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"  
         CssClass="table table-striped table-bordered table-advance table-hover" ShowFooter="true"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
          runat="server"  onrowcommand="grdIndent_RowCommand" onrowdatabound="grdIndent_RowDataBound" 
                            onpageindexchanging="grdIndent_PageIndexChanging" OnSorting="grdIndent_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
            <Columns>
    
             <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure Id" Visible="false">
                     <ItemTemplate>
                        <asp:Label ID="lblFailureId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                      <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                     <asp:TextBox ID="txtFailureId" runat="server" placeholder="Enter Failure Id " Width="150px" MaxLength="10" ></asp:TextBox>
                   </asp:Panel>
                   </FooterTemplate>
             </asp:TemplateField>

               <%-- Both Columns are same but adding for User Interface Purpose--%>
            <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Enhancement ID" Visible="false">                           
                <ItemTemplate> 
                    <asp:Label ID="lblEnhanceId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                </ItemTemplate>
                 <FooterTemplate>
                   <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                     <asp:TextBox ID="txtEnhanceId" runat="server" placeholder="Enter Enhancement Id " Width="150px" MaxLength="10" ></asp:TextBox>
                  </asp:Panel>
                  </FooterTemplate>
            </asp:TemplateField>
     
            <asp:TemplateField AccessibleHeaderText="TI_ID" HeaderText="Indent Id" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblIndentId" runat="server" Text='<%# Bind("TI_ID") %>'></asp:Label>
                    </ItemTemplate>
             </asp:TemplateField>

              <asp:TemplateField AccessibleHeaderText="WO_SLNO" HeaderText="WO Slno" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWOSlno" runat="server" Text='<%# Bind("WO_SLNO") %>'></asp:Label>
                    </ItemTemplate>
             </asp:TemplateField>
 

             <asp:TemplateField AccessibleHeaderText="DF_DTC_CODE" HeaderText="DTC Code" SortExpression="DF_DTC_CODE">
                     <ItemTemplate>
                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>'></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                       <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                       <asp:TextBox ID="txtDtcCode" runat="server" placeholder="Enter DTC Code " Width="150px" MaxLength="9" ></asp:TextBox>
                    </asp:Panel>
                    </FooterTemplate>
                </asp:TemplateField>
    
    
                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">
                    <ItemTemplate>
                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                       <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                       <asp:TextBox ID="txtDtrCode" runat="server" placeholder="Enter DTr Code" Width="150px" MaxLength="6"></asp:TextBox>
                    </asp:Panel>
                    </FooterTemplate>
                </asp:TemplateField>

                 <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order No." SortExpression="WO_NO">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkOrderNo" runat="server" Text='<%# Bind("WO_NO") %>'></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                       <asp:Panel ID="panel5" runat="server" DefaultButton="imgBtnSearch" >
                       <asp:TextBox ID="txtWoNo" runat="server" placeholder="Enter WO No " Width="150px" MaxLength="17" ></asp:TextBox>
                  </asp:Panel>
                    </FooterTemplate>
                </asp:TemplateField>

                 <asp:TemplateField AccessibleHeaderText="" HeaderText="Indent Created" Visible="false">           
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="" HeaderText="Indent NO" SortExpression="TI_INDENT_NO">           
                    <ItemTemplate>
                        <asp:Label ID="lblIndentNo" runat="server" Text='<%# Bind("TI_INDENT_NO") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                       <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                     </FooterTemplate>
                </asp:TemplateField>
        
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <center>
                        <asp:LinkButton runat="server"  CommandName="CreateNew" ID="lnkCreate" >
                                <img src="../Styles/images/Create.png" style="width:20px" />Create Indent</asp:LinkButton>
                              <asp:LinkButton runat="server"  CommandName="Create" ID="lnkUpdate"  visible="false" >
                                             <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                            
                        </center>
                    </ItemTemplate>
                     <HeaderTemplate>
                        <center>
                            <asp:Label ID="lblHeader" runat="server" Text="Action" ></asp:Label>
                        </center>
                     </HeaderTemplate>
                </asp:TemplateField>


        
            </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
       </asp:GridView>

           <asp:GridView ID="grdNewDTCIndent" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"                                  
                            CssClass="table table-striped table-bordered table-advance table-hover" Visible="false"
                                runat="server" ShowHeaderWhenEmpty="True" 
                            EmptyDataText="No Records Found" ShowFooter="true"
                            onpageindexchanging="grdNewDTCIndent_PageIndexChanging" 
                            onrowcommand="grdNewDTCIndent_RowCommand" 
                            onrowdatabound="grdNewDTCIndent_RowDataBound" >
         
                                <Columns>

                                 <asp:TemplateField AccessibleHeaderText="TI_ID" HeaderText="Indent Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndentId1" runat="server" Text='<%# Bind("TI_ID") %>'></asp:Label>
                                    </ItemTemplate>
                               </asp:TemplateField>

                                 <asp:TemplateField AccessibleHeaderText="WO_SLNO" HeaderText="WO Slno" Visible="false">                           
                                        <ItemTemplate> 
                                           <asp:Label ID="lblWOSlno1" runat="server" Text='<%# Bind("WO_SLNO") %>'></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WO No" >          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWoNo" runat="server" Text='<%# Bind("WO_NO") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:Panel ID="panel6" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtWoNo" runat="server" placeholder="Enter Wo No" Width="150px" MaxLength="17" ></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date" >          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWoDate" runat="server" Text='<%# Bind("WO_DATE") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                             <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="" HeaderText="Indent Created">           
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus1" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Indent No" >          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIndentNo1" runat="server" Text='<%# Bind("TI_INDENT_NO") %>' style="word-break: break-all;" width="120px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Indent Date" >          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIndentDate" runat="server" Text='<%# Bind("TI_INDENT_DATE") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <center>
                                        
                                          <asp:LinkButton runat="server"  CommandName="CreateNew" ID="lnkCreate1" >
                                             <img src="../Styles/images/Create.png" style="width:20px" />Create Indent</asp:LinkButton>
                                         <asp:LinkButton runat="server"  CommandName="Create" ID="lnkUpdate1"  visible="false" >
                                             <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                            
                                        </center>
                                    </ItemTemplate>
                                     <HeaderTemplate>
                                        <center>
                                            <asp:Label ID="lblHeader" runat="server" Text="Action" ></asp:Label>
                                        </center>
                                     </HeaderTemplate>
                                </asp:TemplateField>
                        
                                </Columns>

                            </asp:GridView>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Types Of Indent(Failure,Failure with Enhancement,Enhancement,New Transformer Centre)
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To View Indent Details For The Particular Type Select From Type DropDownlist
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To View More Details about Indent, Click On View LinkButton
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
