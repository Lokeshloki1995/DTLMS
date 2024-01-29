<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DTrRepairCenterDetails.aspx.cs" MasterPageFile="~/DTLMS.Master" Inherits="IIITS.DTLMS.Reports.DTrRepairCenterDetails" %>
 
   <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
   <script  type="text/javascript"></script>

     <%-- %><script type="text/javascript">
         function ValidateMyForm() {

             if (document.getElementById('<%= cmbLocation.ClientID %>').value.trim() == "--Select--") {
                 alert('Please Select the location Type')
                 document.getElementById('<%= cmbLocation.ClientID %>').focus()
                                 return false
                             }
                         }
          </script>   --%>
  
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
                <%--  <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px">--%></i></a>   DTR Repairer Center Details Report
               </h3>
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
            <div style="float:right;margin-top:20px;margin-right:12px" >
               <%-- <asp:Button ID="Button1" runat="server" Text="Store View" 
                  OnClientClick="javascript:window.location.href='StoreView.aspx'; return false;"
                  CssClass="btn btn-primary" />--%>
            </div>
         </div>
         <!-- END PAGE HEADER-->
         <!-- BEGIN PAGE CONTENT-->
         <div class="row-fluid">
            <div class="span12">


               <!-- BEGIN SAMPLE FORMPORTLET-->
               <div class="widget blue">
                  <div class="widget-title">
                     <h4><i class="icon-reorder"></i>  DTR Repair Center Details Report             
                     </h4>
                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
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
                              <div class="span1"></div>
                              <div class="span5">
                                  
                                  <div class="control-group">
                                    <label class="control-label">
                                    Zone </label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="2"
                                             OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                    <div class="control-group">
                                    <label class="control-label">
                                    Circle </label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="3"
                                             OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                  <div class="control-group">
                                    <label class="control-label" >
                                    Division</label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="4">
                                               
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                  
                                  
                            
                              </div>
                              <div class="span1"></div>
                           </div>
                           <div class="space20"></div>
                           <div class="space20"></div>
                           <div  class="text-center" align="center">
                             
                                 <asp:Button ID="cmdReport" runat="server" Text="Generate Report" 
                                    CssClass="btn btn-primary" onclick="cmdReport_Click" OnClientClick="javascript:return ValidateMyForm()"/>
                             
                                 <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                    CssClass="btn btn-danger" onclick="cmdReset_Click" />
                                 
                             
                                  
                                 <br />
                              </div>
                              <%-- <div class="span1"></div>--%>
                              <div class="span7"></div>
                              <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                           </div>
                        </div>
                     </div>
                     <div class="space20"></div>
                     <!-- END FORM-->
                  </div>
               </div>
               <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
               <!-- END SAMPLE FORM PORTLET-->
           <%-- <div class="span1"></div>--%>
                              <div class="span7"></div>
                              <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                           </div>
            
                        

                        </div>
         
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
                        <i class="fa fa-info-circle"></i>* The Report is Used to View the List of Repairers with the RCO No,Date and Other Details.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* you can Take Full Report by seclecting nothing in the From.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button crystal Report will be Generated and can be Downloaded in PDF Format.
</p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    </asp:Content>
