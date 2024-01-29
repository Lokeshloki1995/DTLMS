<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ApprovalHistory.aspx.cs" Inherits="IIITS.DTLMS.Approval.ApprovalHistory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/functions.js" type="text/javascript" ></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
    <div class="container-fluid" >
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid" >
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                 
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                   Approval History
                </h3>
                <ul class="breadcrumb" style="display:none">
                       
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text"/>
                                <button class="btn" type="button"><i class="icon-search"></i> </button>
                            </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
             <div style="float:right;margin-top:20px;margin-right:12px" >
                    <asp:Button ID="cmdClose" runat="server" Text="Close"  OnClientClick="javascript:window.location.href='ApprovalInbox.aspx'; return false;"
                                    CssClass="btn btn-danger" />
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        
     
         <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue" >
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Approval History</h4>
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
                               <%-- <div class="span1"></div>--%>
                               <div class="span12" runat="server" id="dvDTCPanel">

                                   <asp:Label ID="lblDTCLoad" runat="server" Text="Transformer Centre Code"></asp:Label> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtDTCCode" runat="server" ReadOnly="true"></asp:TextBox> &nbsp&nbsp&nbsp&nbsp 
                                   
                                   <asp:Label ID="lbl3" runat="server" Text="Transformer Centre Name"></asp:Label> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox> &nbsp&nbsp&nbsp&nbsp 
                                 
                                   <asp:Label ID="lbl2" runat="server" Text="DTR Code"></asp:Label> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtDTrCode" runat="server" ReadOnly="true"></asp:TextBox> &nbsp&nbsp&nbsp&nbsp 
                             </div>
                             <div class="space20"></div>
                             <div class="span12">
                                  <asp:Label ID="Label1" runat="server" Text="Work Name :" ForeColor="slategray" Font-Size="15px"></asp:Label>
                                  <asp:Label ID="lblWorkName" runat="server" ForeColor="#2FADE7" Font-Size="15px"></asp:Label>
                             </div>

                              <div class="span12">
                                  <asp:Label ID="Label2" runat="server" Text="Current Status :" ForeColor="slategray" Font-Size="15px"></asp:Label>
                                  <asp:Label ID="lblCurrentStatus" runat="server" ForeColor="#2FADE7" Font-Size="15px"></asp:Label>
                                  <asp:TextBox ID="txtRecordId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                  <asp:TextBox ID="txtBOId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                  <asp:HiddenField ID="hdfOfcCode" runat="server" />
                              </div>
                             
                       <div class="space20"></div>
                          <asp:GridView ID="grdApprovalHistory" 
                                AutoGenerateColumns="false" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"  
                                runat="server" onpageindexchanging="grdApprovalHistory_PageIndexChanging">
                                <Columns>      
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>                         
                                     <asp:TemplateField AccessibleHeaderText="INITIATOR" HeaderText="Approver" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("INITIATOR") %>' style="word-break:break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                         
                                     <asp:TemplateField AccessibleHeaderText="WO_CR_ON" HeaderText="Created On" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("WO_CR_ON") %>' style="word-break:break-all" Width="130px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="WO_USER_COMMENT" HeaderText="Comments" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDTrCode" runat="server" Text='<%# Bind("WO_USER_COMMENT") %>' style="word-break:break-all" Width="250px"></asp:Label>
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
              </div>                                   
            </div>
            </div>
           
           
             <asp:Label ID="lblMessage" runat="server" ForeColor="Red" ></asp:Label>
           <div class="span3"></div>
          <asp:Label ID="lblTcDetails" runat="server" ForeColor="Blue"></asp:Label> 



</asp:Content>
