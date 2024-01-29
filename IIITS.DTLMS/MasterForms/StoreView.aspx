<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.StoreView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function ConfirmStatus(status) {

        var result = confirm('Are you sure,Do you want to ' + status + ' User?');
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
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Store View
                   </h3>
             <%--          <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                            <h4><i class="icon-reorder"></i> Store View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                              
                      
                             
                                <div style="float:right" >
                                <div class="span6">
                                   <asp:Button ID="cmdNew" runat="server" Text="New Store" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" /><br /></div>
                                    <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickStore" /><br />
                                          </div>

                                            </div>
                                  
                                    <div class="space20"> </div>
                                 
                                <!-- END FORM-->
                           
                        
                            <asp:GridView ID="grdStore" 
                                AutoGenerateColumns="false"  PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AllowPaging="true"  CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" onpageindexchanging="grdStore_PageIndexChanging" 
                                    ShowFooter="true" onrowcommand="grdStore_RowCommand" onrowdatabound="grdStore_RowDataBound" 
                                 OnSorting="grdStore_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                    
                                <Columns>
                                     <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStoreId" runat="server" Text='<%# Bind("SM_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="SM_NAME" HeaderText="Store Name" SortExpression="SM_NAME">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("SM_NAME") %>' style="word-break: break-all;" width="120"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtStoreName" runat="server" placeholder="Enter Store Name" ></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SM_Location" HeaderText="Location" SortExpression="SM_OFF_CODE">                           
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("SM_OFF_CODE") %>' style="word-break: break-all;" width="200px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                         <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtLocation" runat="server" placeholder="Enter Location" ></asp:TextBox>
                                        </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                   
                                  <asp:TemplateField AccessibleHeaderText="SM_Mobile" HeaderText="Mobile No">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobile" runat="server" Text='<%# Bind("SM_MOBILENO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SM_InchargeName" HeaderText="Incharge Name" SortExpression="SM_STORE_INCHARGE">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblInchargeName" runat="server" Text='<%# Bind("SM_STORE_INCHARGE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SM_Email" HeaderText="Email Id">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblMailId" runat="server" Text='<%# Bind("SM_EMAILID") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
 
                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                              

                                 <asp:TemplateField HeaderText="Status" Visible="false"> 
                                     <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("SM_STATUS1") %>' ></asp:Label>
                                         <center>
                                            <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                                           tooltip="Click to Activate Store" OnClientClick="return confirm ('Are you sure, you want to Activate Store');" width="10px" />        
                                            <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                                           tooltip="Click to DeActivate Store"  OnClientClick="return confirm ('Are you sure, you want to DeActivate Store');" />        
                                        </center>
                                    </ItemTemplate>
                               </asp:TemplateField>

                              </Columns>

                            </asp:GridView>
                        
                        </div>

                         <ajax:modalpopupextender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                                  PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                             <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                                <div style="display:none">
                                    <asp:Button ID="btnshow" runat="server" Text="Button"  />
                                 </div>
                                    <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="362px"  Width="434px">
                                        <div class="widget blue" >
                                             <div class="widget-title" >
                                                   <h4>Give Reason </h4>
                                            <div class="space20"></div>
                                         <%--<div class="row-fluid">--%>
                                          <div class="span1"></div>
                                            <div class="space20" >
                                             <div class="span1"></div>              
   
                                          <div class="span5">
                                    
                                            <div class="control-group" style="font-weight: bold">
                                              <label class="control-label">Reason<span class="Mandotary"> *</span></label>
   
                                             <div class="controls">
                                                <div class="input-append" align="center">
                                                    <div class="span3"></div>                                           
                                                   <asp:TextBox ID="txtReason" runat="server" MaxLength="500" TabIndex="4"  TextMode="MultiLine" style="resize:none" 
                                                                                            onkeyup="javascript:ValidateTextlimit(this,100)"></asp:TextBox>
                                                    
                                                </div>
                                            </div>
                                            </div>
      
                                        <div align="center">
                                             <div class="control-group" style="font-weight: bold">
                                             <label class="control-label">Effect From<span class="Mandotary"> *</span></label>
                                             <div class="controls" >
                                                <div class="input-append" align="center">
                                                  <div class="span3"></div>         
                                                     <asp:TextBox ID="txtEffectFrom" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                          <ajax:calendarextender ID="CalendarExtender1" runat="server" 
                                                             CssClass="cal_Theme1" TargetControlID="txtEffectFrom" Format="dd/MM/yyyy"></ajax:calendarextender>                                                 
             
                                                 </div>        
                                             </div>
                                         </div>
                                     </div>  
       
                                   <div>                                   
                                    <div class="control-group" style="font-weight: bold">
   
                                    <div class="controls">
                                        <div class="input-append">
                                            <div class="span3"></div>       
                                             <div  class="span7">      
                                                <asp:Button ID="cmdSubmit" runat="server" CssClass="btn btn-primary" 
                                                       onclick="cmdSubmit_Click" TabIndex="10" Text="Submit" /> 
                                              </div> 
                                               <div  class="span1">                                        
                                                 <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary" 
                                                                           TabIndex="10" Text="Close" /> 
                                               </div>
                                             </div>
                                        </div>
                                      </div>
                                    </div>        
       

                                  <div class="space20" align="center">

                                  <div  class="form-horizontal" align="center"> 
                                         <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red" ></asp:Label>  
                                   </div>

                                  
                                    </div>
                                    </div>
                                </div>  
                                </div>
                                <div class="space20"></div>
                                <div class="space20"></div>

                            </div>
                                    </asp:Panel>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Existing
                        Store and New Store Can Be Added
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Existing Store Details Can Be Edited By Clicking
                        Edit Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New Store Can Be Added By Clicking New Store
                        Button
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



