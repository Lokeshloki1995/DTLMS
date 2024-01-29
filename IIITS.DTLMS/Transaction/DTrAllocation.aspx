<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTrAllocation.aspx.cs" Inherits="IIITS.DTLMS.Transaction.DTrAllocation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript" ></script>
     <script  type="text/javascript">

         function ValidateMyForm() {
             if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                 alert('Select Valid DTr Code')
                 document.getElementById('<%= txtTcCode.ClientID %>').focus()
                 return false
             }
         }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                 
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                   DTR Allocation
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
            <%-- <div style="float:right;margin-top:20px;margin-right:12px" >
                    <asp:Button ID="cmdClose" runat="server" Text="Close"  OnClientClick="javascript:window.location.href='FaultTCSearch.aspx'; return false;"
                                    CssClass="btn btn-primary" />
            </div>--%>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>DTR Allocation</h4>
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
                               <div class="span5">
                                  <div class="control-group">

                                    <label class="control-label" >DTr Code<span class="Mandotary"> *</span></label>
                                        <div class="controls">
                                            <div class="input-append">
                                            <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10"></asp:TextBox>
                                             <asp:Button ID="btnSearchId" Text="S" class="btn btn-primary" runat="server" onclick="btnSearchId_Click"
                                                   /><br />
                                                 <asp:LinkButton ID="lnkDTrDetails" runat="server"  
                                                style="font-size:12px;color:Blue" onclick="lnkDTrDetails_Click" OnClientClick="javascript:return ValidateMyForm()"
                                                        >View DTr Details</asp:LinkButton>
                                            </div>
                                       </div>
                                     </div>
                                
                               </div>  
                               <div class="span5">
                                    <div class="control-group">

                                 <label class="control-label">Type</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbType" runat="server"  AutoPostBack="true"
                                                onselectedindexchanged="cmbType_SelectedIndexChanged" >
                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Store"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Field"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                   </div>
                                   </div>
                                 
                              <div class="control-group" style="display:none" id="dvStore" runat="server">
                                 <label class="control-label">Store</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbStore" runat="server" >                                         
                                            </asp:DropDownList>
                                        </div>
                                   </div>
                                   </div>

                                <div class="control-group" style="display:none" id="dvDTC" runat="server">
                                     <label class="control-label">DTC Code</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="6"></asp:TextBox>
                                             <asp:Button ID="cmdDTCSearch" Text="S" class="btn btn-primary" runat="server" />
                                        </div>
                                   </div>
                               </div>
               
                              
                               </div>            
                                    </div>
                               </div>
                               <div class="space20"></div>
                               <div class="form-horizontal">
                                <div class="span3"></div>
                                 <div class="span2">
                                    <asp:Button ID="cmdAllocate" runat="server" Text="Allocate" CssClass="btn btn-primary" 
                                        onclick="cmdAllocate_Click" Width="116px" />
                                </div> 
                                <div class="span2">
                                 <asp:Button ID="cmbReset" runat="server" Text="Reset" CssClass="btn btn-primary" 
                                         Width="116px" onclick="cmbReset_Click" />
                                </div>
                               
                               </div>

                               <div class="space20"></div>
                               </div>
                              </div>
                            </div>
              </div>                                   
            </div>

           <div class="row-fluid" runat="server" style="display:none" id="dvBasicDetails">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>DTR Basic Details</h4>
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
                               <div class="span12">                                 
                             <div class="space20"></div>
                               <asp:GridView ID="grdTcDetails"  AutoGenerateColumns="false" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"  
                                runat="server" onpageindexchanging="grdTcDetails_PageIndexChanging" 
                                        onrowcommand="grdTcDetails_RowCommand" >
                                <Columns>

                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDTrCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDTrSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break:break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="CURRENT_LOCATION" HeaderText="Current Location">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("CURRENT_LOCATION") %>' Style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("DTC_CODE") %>' Style="word-break:break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="DTC_NAME" HeaderText="DTC Name">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDTCName" runat="server" Text='<%# Bind("DTC_NAME") %>' Style="word-break:break-all" Width="200px"></asp:Label>
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
