<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PrevMaintanance.aspx.cs" Inherits="IIITS.DTLMS.Maintainance.PrevMaintanance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.lnkSubmit
{    
    margin:0px 0px 0px 0px;
    background:url(~/Styles/images/edit64x64.png) left center no-repeat;
    padding: 0em 1.2em;
    font: 8pt "tahoma";
    color: #336699;
    text-decoration: none;
    font-weight: normal;
    letter-spacing: 0px;
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
                       Preventive Maintenance Details
                   </h3>
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
                            <h4><i class="icon-reorder"></i> Preventive Maintenance View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                                <div class="space20">  </div>
                                 <div style="float:left" >
                              <%--  <div class="span8">--%>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblFeeder" runat="server" Text="Feeder" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:DropDownList ID="cmbIndexSelection" runat="server" AutoPostBack="true" onselectedindexchanged="cmbIndexSelection_SelectedIndexChanged">                                
                                   
                                   </asp:DropDownList><br /></div>

                        <div class="widget-body">
                       
                                  
                                    <div class="space20"> </div>
                                 
                                <!-- END FORM-->
                           
                        
                            <asp:GridView ID="grdPrevMaintainance" 
                                AutoGenerateColumns="false"  PageSize="10"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" 
                                runat="server"
                                    onrowcommand="grdPrevMaintainance_RowCommand"  >
                                <Columns>
                                    
                                
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Transformer Centre code" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="Transformer Centre Name" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="LAST_SERVICE_DATE" HeaderText="Last Service Date" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblLastServiceDate" runat="server" Text='<%# Bind("LAST_SERVICE_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="EXPECTED_SERVICEDATE" HeaderText="Expected Service Date" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblExptdServiceDate" runat="server" Text='<%# Bind("EXPECTED_SERVICEDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <center>
                                          <%--  <asp:ImageButton  ID="imgBtnEdit" runat="server" AlternateText = "ppp" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" 
                                                Width="12px" CommandName="Maintainance" />--%>
                                              <asp:LinkButton ID="imgBtnEdit"  CommandName="Maintainance"  runat="server">
                                              <img src="../Styles/images/edit64x64.png" alt="" style="height:20px"/>Maintenance</asp:LinkButton>
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
                        
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                  <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
      </div>
      </div>

</asp:Content>
