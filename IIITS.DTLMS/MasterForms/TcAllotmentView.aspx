<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/DTLMS.Master" CodeBehind="TcAllotmentView.aspx.cs" 
    Inherits="IIITS.DTLMS.MasterForms.TcAllotmentView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <script type="text/javascript">

                function AllowOnlyAlphanumericNotAllowSpecial(evt) {
                    evt = (evt) ? evt : event;
                    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
                   ((evt.which) ? evt.which : 0));

                    if ((charCode > 47 && charCode < 58) ||
                        (charCode > 64 && charCode < 91) ||
                        (charCode > 96 && charCode < 123)) {
                        return true;
                    }

                    else { return false; }
                }
                function ValidateAlphaNoSplChr(evt) {
                    var keyCode = (evt.which) ? evt.which : evt.keyCode
                    if (event.keyCode == 8 || event.keyCode == 46
             ||  event.keyCode == 39) {
                        return true;
                    }
                    else if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32) {
                        return false;
                    }
                    else
                        return true;
                }
                function DisableSplChars(evt) {
                    var charCode = (evt.which) ? evt.which : event.keyCode
                    if (charCode == 92) {
                        return false;
                    } else {

                        return true;
                    }
                }

                function OnlyAlphaNumericSpecialChar(evt) {
                    var charCode = (evt.which) ? evt.which : event.keyCode
                    if (charCode > 31 && (charCode < 48 || charCode > 57)
                        && (charCode < 65 || charCode > 90)
                        && (charCode < 97 || charCode > 122) && (charCode == 95 &&
                        charCode == 45 && charCode == 47)) {
                        return false;
                    } else {

                        return true;
                    }
                }
                function DisableSplChars(evt) {
                    var charCode = (evt.which) ? evt.which : event.keyCode
                    if (charCode == 92 || charCode == 37 || charCode == 42) {
                        return false;
                    } else {

                        return true;
                    }
                }
                </script>

        <style>
        input#ContentPlaceHolder1_cmdNew{
            margin-right:10px
        }
    </style>
      <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 <br />
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                 Allotment View
                   </h3>
                                          <a style="margin-left:-372px!important;float:right!important"href="#" 
                                              data-toggle="modal" data-target="#myModal" title="Click For Help" >Help 
                                              <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
                            <h4><i class="icon-reorder"></i>Allotment View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                         <div style="float:right" >
                             <div style="display:flex!important" class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New Allotment" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click"  /><br />
                                    </div>

                                            </div> 
                                <div class="space20"></div>
                                <!-- END FORM-->

                                 <div class="space20"></div>
                                    <div class="row-fluid">
                                       
                              <%--          <div class="span6">--%>
                                            <div style="width: 100%!important; height: 360px!important; overflow: scroll!important">
                                                <asp:GridView ID="grdDIPendingTC" AutoGenerateColumns="false" PageSize="5"
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                    onrowcommand="grdDIPendingTC_RowCommand" ShowFooter="True"
                                    runat="server" TabIndex="16" style="width:100%!important;"   OnPageIndexChanging="grdDIPendingTC_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                   <%--         <%#Container.DataItemIndex+1 %>--%>
                                            <asp:Label ID="lblslnoId" runat="server" width="50" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIM_DI_NO" HeaderText="Dispatch No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDINO" width="100" runat="server" Text='<%# Bind("DIM_DI_NO") %>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                         <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtdiNumber" runat="server"  Width="110px"  placeholder="Enter DI Number" 
                                                 ToolTip="Enter DI Number to Search"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="PB_MAKE" HeaderText="Make" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("DI_MAKE_ID") %>'></asp:Label>
                                            </ItemTemplate>
 
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Width="200" Text='<%# Bind("MAKE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                             <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" 
                                                 Height="25px" ToolTip="Search" CommandName="Search" TabIndex="9"/>
                                         </FooterTemplate>  
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Store" HeaderText="Store Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstore" Width="200" runat="server" Text='<%# Bind("STORE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("DI_CAPACITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PO_RATING" HeaderText="Rating"  Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRatingId" runat="server" Text='<%# Bind("STAR_RATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="RATING" HeaderText="Rating">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRating" runat="server" width="200" Text='<%# Bind("STAR_RATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      
                                        <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="Total Quantity" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PENDING" HeaderText="Pending Quantity" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPenQuantity" runat="server" Text='<%# Bind("PENDING") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField AccessibleHeaderText="START_RANGE" HeaderText="Start Range">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstartrange" Width="100" runat="server" Text='<%# Bind("START_RANGE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField AccessibleHeaderText="END_RANGE" HeaderText="End Range">
                                            <ItemTemplate>
                                                <asp:Label ID="lblendrange" Width="100" runat="server" Text='<%# Bind("END_RANGE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIM_ID" HeaderText="DIM_ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldimid" runat="server" Text='<%# Bind("DIM_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIM_PO_ID" HeaderText="DIM_PO_ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpoid" runat="server" Text='<%# Bind("DIM_PO_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" width="100"  Text='<%# Bind("STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Action">
                                               <ItemTemplate>
                                                   <asp:LinkButton ID="lnkload" runat="server" ForeColor="Blue" Width="100"  Text="View"  
                                                        OnClick="cmdLoad_click"   CommandArgument='<%# Eval("DIM_DI_NO") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                                    </asp:GridView>
                                                    </div>
                                             <%-- </div>--%>
                            
                                </div>
                            </div>
                            </div>
                            </div>
                            </div>
               <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title">
                            <h4 id="CreateDI"  runat="server"><i class="icon-reorder"></i>Allotment View Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                            </span>
                        </div>
                             <div style="float:right;margin-right: 20px;margin-top: 10px;" >
                             <div style="display:flex!important" class="span5">
                          
                               <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                 style="margin-left:12px"  OnClick="Export_click" /><br />
                                </div>

                                            </div> 
                         <div class="space20"></div> 
                        <div style="width: 100%; height: 400px; overflow: scroll">      
                            <asp:GridView ID="grdAllotmentView" AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                  OnSorting="grdAllotmentView_Sorting"  onrowcommand="grdAllotmentView_RowCommand"  
                                runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" 
                                onpageindexchanging="grdAllotmentView_PageIndexChanging"                                    
                                 ShowFooter="True" AllowSorting="true" overflow=" scroll">
                             <HeaderStyle CssClass="both"/>
                                <Columns>

                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                          <%--  <%#Container.DataItemIndex+1 %>--%>
                                            <asp:Label ID="lblslnoId" runat="server" width="50" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="TCP_ID" HeaderText="ALT Id" Visible=false>                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblAltid" runat="server" Text='<%# Bind("TCP_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PO_NO" HeaderText="PO NO" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDIPoNo" runat="server" Text='<%# Bind("PO_NO") %>'></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                         <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtPoNo" runat="server" Width="110px" 
                                                 onkeypress="javascript:return DisableSplChars(event);" placeholder="Enter PO No" 
                                                 ToolTip="Enter PO No To Search" MaxLength="25"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                          
                                      <asp:TemplateField AccessibleHeaderText="DIM_DI_NO" HeaderText="DI NO" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDiNo" runat="server" Text='<%# Bind("DIM_DI_NO") %>'></asp:Label>
                                        </ItemTemplate>

                                          <FooterTemplate>
                                         <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtDiNo" runat="server"  Width="110px" 
                                                 onkeypress="javascript:return DisableSplChars(event);" placeholder="Enter DI No" 
                                                 ToolTip="Enter DI No To Search" MaxLength="25"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                         
                                     <asp:TemplateField AccessibleHeaderText="TCP_TC_CODE" HeaderText="DTr Code">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDTr" runat="server" Text='<%# Bind("TCP_TC_CODE") %>' 
                                                style="word-break:break-all" Width="60px"></asp:Label>                                 
                                        </ItemTemplate>

                                         <FooterTemplate>
                                         <asp:Panel ID="panel3" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtaltNumber" runat="server"  Width="110px" 
                                                 onkeypress="javascript:return AllowOnlyAlphanumericNotAllowSpecial(event);" 
                                                 placeholder="Enter DTr Code" ToolTip="Enter DTr Code To Search" MaxLength="8"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SM_NAME" HeaderText="Store Name">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStore" runat="server" Text='<%# Bind("SM_NAME") %>' 
                                                style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                         <asp:Panel ID="panel4" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtStoreName" runat="server"  Width="110px" 
                                                 onkeypress="javascript:return ValidateAlphaNoSplChr(event);" placeholder="Enter Store Name" 
                                                 ToolTip="Enter Store Name To Search" MaxLength="25"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField AccessibleHeaderText="MD_NAME" HeaderText="Rating">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblRating" runat="server" Text='<%# Bind("MD_NAME") %>' 
                                                style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                             <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" 
                                                 ToolTip="Search" CommandName="search" TabIndex="9"/>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DI_CAPACITY" HeaderText="Capacity">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("DI_CAPACITY") %>' 
                                                style="word-break:break-all" Width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' 
                                                style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_TC_SL_NO" HeaderText="TC Sl No">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblSlNo" runat="server" Text='<%# Bind("TCP_TC_SL_NO") %>' 
                                                style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_MANUFACTURE_DATE" HeaderText="Manufacture Date">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TCP_MANUFACTURE_DATE") %>' 
                                                style="word-break:break-all" Width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_TC_LIFE_SPAN" HeaderText="Life Span">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblLifeSpan" runat="server" Text='<%# Bind("TCP_TC_LIFE_SPAN") %>' 
                                                style="word-break:break-all" Width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_TC_WARRENTY_PERIOD" HeaderText="Warrenty Period">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrentyPeriod" runat="server" Text='<%# Bind("TCP_TC_WARRENTY_PERIOD") %>' 
                                                style="word-break:break-all" Width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_OIL_TYPE" HeaderText="Oil Type">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblOilType" runat="server" Text='<%# Bind("TCP_OIL_TYPE") %>' 
                                                style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_OIL_CAPACITY" HeaderText="TC Oil Capacity">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblOilCapacity" runat="server" Text='<%# Bind("TCP_OIL_CAPACITY") %>' 
                                                style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TCP_OIL_WEIGHT" HeaderText="TC Weight">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcWeight" runat="server" Text='<%# Bind("TCP_OIL_WEIGHT") %>' 
                                                style="word-break:break-all" Width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                             </div>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Tc Allocation Details 
                        from store,Existing  Allocation Details Can be Edited and New Tc Allocation can Be Added.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To View The Allotment Details  Click On View Button To Get The Details
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New  Allotment Click On  New Button
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
