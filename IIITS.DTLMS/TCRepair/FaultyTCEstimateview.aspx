<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/DTLMS.Master" CodeBehind="FaultyTCEstimateview.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.FaultyTCEstimateview" %>

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
                    FaultyTC Details View
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
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>FaultyTC Details View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                         <div class="form-horizontal">
                                    <div class="row-fluid">
                      
                      </div>
                        </div>
                        </div>
                                <!-- END FORM-->
                           

                   <asp:GridView ID="grdFaultTC" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" DataKeyNames="TC_ID" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onrowcommand="grdFaultTC_RowCommand"  onrowdatabound="grdFailureDetails_RowDataBound"
                                onpageindexchanging="grdFaultTC_PageIndexChanging" TabIndex="6" OnSorting="grdFaultTC_Sorting">
                                <HeaderStyle CssClass="both" /> 
                                <Columns>
                                    <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                  
                                 <asp:TemplateField AccessibleHeaderText="RESTD ID" HeaderText="RESTD ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblrestdid" runat="server" Text='<%# Bind("RESTD_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" >                                
                                        <ItemTemplate>   
                                            <asp:LinkButton runat="server"  CommandName="CreateNew" Width="80px" ID="lnkCreateTC" >                                    
                                            <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break:break-all" Width="80px"></asp:Label>
                                            
                                </asp:LinkButton>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                             <asp:TextBox ID="txtDTRCode" runat="server" placeholder="Enter DTR CODE" Width="120px"></asp:TextBox>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break:break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                             <asp:TextBox ID="txtSlNo" runat="server" placeholder="Enter DTR SLNO" Width="120px"></asp:TextBox>
                                         </FooterTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField AccessibleHeaderText="TC_PREV_OFFCODE" HeaderText="Office Code" Visible="false">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("TC_PREV_OFFCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY" >
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' style="word-break:break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date" >
                                      
                                        <ItemTemplate>
                                          
                                            <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date" Visible="false">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee Period" Visible="false">
                                       

                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" Visible="false">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="RepSentcount" HeaderText="Sent To Repairer" Visible="false">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblRepSentcount" runat="server" Text='<%# Bind("RCOUNT") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type" Visible="false">                                   
                                        <ItemTemplate>
                                            <asp:Label ID="lblguarentee" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="Status" HeaderText="STATUS"  Visible="false">
                                      
                                        <ItemTemplate>
                                          
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' style="word-break:break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                                      <asp:TemplateField HeaderText="Action" >
                            <ItemTemplate>
                                <center>
    
                                      <asp:LinkButton runat="server"  CommandName="CreateNew" ID="lnkCreate" >
                                      <img src="../Styles/images/Create.png" style="width:20px" />repairer estimate</asp:LinkButton>
                                      <asp:LinkButton runat="server"  CommandName="Create" ID="lnkUpdate"  visible="false" >
                                      <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                      <asp:LinkButton runat="server"   ID="lnkWaiting"   CommandName="Preview"  >
                                      <img id="lnkbtnwait" src="../img/Manual/Wait.png" style="width:20px" />Waiting for Approval</asp:LinkButton>
                                      <asp:LinkButton runat="server" tooltip="Click To Download Estimation report" ID="lnkEstPrev"   visible="false" >
                                      <img id="Img1" src="../img/Manual/Pdficon.png" style="width:20px"/></asp:LinkButton>

                                </center>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <center>
                                    <asp:Label ID="lblHeader" runat="server" Text="Action" ></asp:Label>
                                </center>
                            </HeaderTemplate>
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
                        <div class="span7"></div>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Faulty Details and To Declare.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Declare Faulty Click On <u>PENDING </u> Button
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Faulty is Declared By Section Officer, 
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


