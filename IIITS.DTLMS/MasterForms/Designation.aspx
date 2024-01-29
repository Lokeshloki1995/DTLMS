<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Designation.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.Designation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%--  <script type="text/javascript" src="../Scripts/functions.js" ></script>--%>
  
                            <script type="text/javascript">
        $(document).ready(function () {

            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })

        });
       
    </script>

    <script type="text/javascript" >
      function ValidateMyForm() {

          if (document.getElementById('<%= txtDesignation.ClientID %>').value.trim() == "") {
              alert('Enter the Designation')
              document.getElementById('<%= txtDesignation.ClientID %>').focus()
              return false
         
          }

          <%--var Designation = document.getElementById('<%= txtDesignation.ClientID %>').value;
          var Designationcon = /^([a-zA-Z]+\s)*[a-zA-Z]+$/;
          if (!Designation.match(Designationcon)) {
              alert('Enter valid  Designation')
              document.getElementById('<%= txtDesignation.ClientID %>').focus()
                return false
            }--%>

          if (document.getElementById('<%= txtDescription.ClientID %>').value.trim() == "") {
                alert('Enter the Description')
                document.getElementById('<%= txtDescription.ClientID %>').focus()
                  return false
              }
          }

 </script>
        <script language="Javascript" type="text/javascript">


            function onlyAlphabets(e, t) {
                var code = ('charCode' in e) ? e.charCode : e.keyCode;
                if ( // space
                  !(code == 32) && // Special characters
                    !(code > 39 && code < 42) &&
                    !(code > 45 && code < 48) &&
                  !(code > 64 && code < 91) && // upper alpha (A-Z)
                  !(code > 96 && code < 123)) { // lower alpha (a-z)
                    e.preventDefault();
                }
            }
            </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title" runat="server" id="Create">
                   Create Designation
                   </h3>
                   <h3 class="page-title" runat="server" id="Update">
                     Update Designation                                    
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
                      <asp:Button ID="cmdClose" runat="server" Text="Designation View" 
                                      OnClientClick="javascript:window.location.href='DesignationView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
                          
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4 id="CreateDesignation" runat="server"><i class="icon-reorder"></i>Create Designation</h4>
                            <h4 id="UpdateDesignation" runat="server"><i class="icon-reorder"></i>Update Designation</h4>
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
                                             <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtDesignationId" runat="server" MaxLength="500" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
   
                 <div class="control-group">
                        <label class="control-label">Designation Name <span class="Mandotary"> *</span></label>
                 
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtDesignation" runat="server" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                            </div>
                        </div>
                    </div>
     
                    <div class="control-group">
                        <label class="control-label">Description  <span class="Mandotary"> *</span> </label>
                        
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtDescription" runat="server" 
                                    style = "resize:none" Height="50px" TextMode="MultiLine" 
                                    onkeypress="return AllowSpecialchar(event);"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                        </div> 
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" >
                                    
                                        
                                     
                                    <asp:Button ID="cmdSave" runat="server" Text="Save"  
                                       CssClass="btn btn-primary" 
                                              OnClientClick="javascript:return ValidateMyForm()"   onclick="cmdSave_Click" />
                      
                         
                          
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" onclick="cmdReset_Click" /><br />
                         
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                                </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           
                        
                            
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>
         
    
</asp:Content>
