<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ApprovalPriority.aspx.cs" Inherits="IIITS.DTLMS.Approval.ApprovalPriority" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="../Scripts/functions.js" type="text/javascript" ></script>
  <script type="text/javascript">
      function ConfirmDelete() {
          var result = confirm('Are you sure you want to Delete?');
          if (result) {
              return true;
          }
          else {
              return false;
          }
      }
      function ValidateMyForm() {

          if (document.getElementById('<%= cmbModule.ClientID %>').value == "--Select--") {
              alert('Select Module Name')
              document.getElementById('<%= cmbModule.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbForm.ClientID %>').value == "--Select--") {
              alert('Select Form Name')
              document.getElementById('<%= cmbForm.ClientID %>').focus()
              return false
          }
      }
      function ValidateForm() {
          if (document.getElementById('<%= cmbRoles.ClientID %>').value == "--Select--") {
              alert('Select Role Name')
              document.getElementById('<%= cmbRoles.ClientID %>').focus()
              return false
          }
      }
    </script>

        <script type="text/javascript">

    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip(); 
    });
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                 
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
             Priority Mapping
                </h3>
                <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
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
                <div class="widget blue" >
                    <div class="widget-title" >
                        <h4><i class="icon-reorder"></i>Basic Details</h4>
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

            <label class="control-label" style="font-size: medium; font-weight: bold;" >Module Name<span class="Mandotary"> *</span></label>
                <div class="controls">
                    <div class="input-append">
                        
                                <asp:DropDownList ID="cmbModule" runat="server" AutoPostBack="true" onselectedindexchanged="cmbModule_SelectedIndexChanged" 
                                   >
                                </asp:DropDownList>
                                 <asp:TextBox ID="txtWorkOrderId" runat="server" Visible="false"></asp:TextBox>

                    </div>
               </div>
               </div>

</div>                    <div class="span5">
                    <label class="control-label" style="font-size: medium; font-weight: bold;" >Form Name<span class="Mandotary"> *</span></label>
                <div class="controls">
                    <div class="input-append">
                       
                        <asp:TextBox ID="txtModules" runat="server" Visible="false"></asp:TextBox>
                                <asp:DropDownList ID="cmbForm" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="cmbForm_SelectedIndexChanged">
                                </asp:DropDownList>
                       <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                    </div>
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
          <asp:Label ID="lblRoles" runat="server" ForeColor="Blue"></asp:Label> 
       
            <div class="row-fluid">
                <div class="span12">
           
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue" >
                    <div class="widget-title" >
                        <h4><i class="icon-reorder"></i>Approval Priority</h4>
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
                                     <div class="span5">
                
                    <div class="control-group">

                     <label class="control-label" style="font-size: medium; font-weight: bold;" >Role Name<span class="Mandotary"> *</span></label>
                    <div class="controls">
                     <div class="input-append">
                        <asp:DropDownList ID="cmbRoles" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                        </div>
               </div>
                </div> 
              </div>
               <div class="span4">
                   <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" onclick="cmdAdd_Click" />    
                         <div class="span5"> 
                        <div class="space20"></div>
                        <div class="space20"></div>
                         
                </div>
               
              </div>

               <div class="span10">
                 <asp:Label ID="lblPriorityMsg" runat="server" ForeColor="Blue">Note : Priority 1 is Creator of Selected Form</asp:Label> 
                </div>
            
             

                              <asp:GridView ID="grdRoles" 
                                AutoGenerateColumns="false"  PageSize="5" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" 
                                runat="server" TabIndex="16" onrowcommand="grdRoles_RowCommand" onpageindexchanging="grdRoles_PageIndexChanging">
                                <Columns>
                                 <asp:TemplateField AccessibleHeaderText="BO_NAME" HeaderText="Modulename" >                                
                                        <ItemTemplate>                                       
                                             <asp:Label ID="lblBoId" runat="server" Text='<%# Bind("BO_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblModuleName" runat="server" Text='<%# Bind("BO_NAME") %>'></asp:Label>
                                               
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="RO_NAME" HeaderText="RoleName">                                
                                        <ItemTemplate>                                       
                                             <asp:Label ID="lblRoId" runat="server" Text='<%# Bind("RO_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RO_NAME") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="LEVEL" HeaderText="Priority" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("WM_LEVEL") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" 
                                              CommandName="Remove"  Width="12px" OnClientClick="return ConfirmDelete();" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                             <div  class="space15"></div>
                                    <div  class="form-horizontal" align="center">
                                        <div class="span4" align="center">
                                         <asp:Button ID="btnSave" runat="server" Text="Save"  value="Remove Single Quotes" 
                                       OnClientClick="return ValidateMyForm();" CssClass="btn btn-primary" onclick="btnSave_Click" 
                                                />
                                       
                                      
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" 
                                       CssClass="btn btn-danger" onclick="btnReset_Click"
                                                />
                                         </div>
                                         </div>
                                    </div>
                                      
                                   <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                         </div>                 
                   </div>
                 
                   
                   </div>                                                                                                    
                    
                        <div class="span3"> </div>
                        
             </div>
                                             
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used For Priority Mapping
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>In Module Name DropDownList Module Can Be Selected and In Form Name DropDownList Form Can Be Selected
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Module Name and Form is Selected, Approval Priority Can Be Created(Priority 1 will Be the Creator)
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>After Creating Approval Priority Click <u>Update</u> Button to Save The Changes
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
