<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMakeMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TcMakeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

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
  <script  type="text/javascript">

      function ValidateMyForm() {
          if (document.getElementById('<%= txtMakeName.ClientID %>').value.trim() == "") {
              alert('Enter Valid Make Name')
              document.getElementById('<%= txtMakeName.ClientID %>').focus()
              return false
          }

         // var MakeName = document.getElementById('<%= txtMakeName.ClientID %>').value;
         // var MakeNamecon = /^([a-zA-Z]+\s)*[a-zA-Z]+$/;
         // if (!MakeName.match(MakeNamecon)) {
            //  alert('Enter valid   Make Name')
             // document.getElementById('<%= txtMakeName.ClientID %>').focus()
             // return false
         // }

          if (document.getElementById('<%= txtCode.ClientID %>').value.trim() == "") {
              alert('Enter Valid Description')
              document.getElementById('<%= txtCode.ClientID %>').focus()
              return false
          }
          
        
      }

      function ResetForm() {
          document.getElementById('<%= txtMakeName.ClientID %>').value = "";
          document.getElementById('<%= txtCode.ClientID %>').value = "";
          document.getElementById('<%= txtMakeId.ClientID %>').value = "";

          return false;
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

         function Description(e, t) {
             var code = ('charCode' in e) ? e.charCode : e.keyCode;
             if ( // space
               !(code == 32) && // Special characters
                  !(code > 39 && code < 42) &&
                 !(code > 43 && code < 48) &&
               !(code > 64 && code < 91) && // upper alpha (A-Z)
               !(code > 96 && code < 123)) { // lower alpha (a-z)
                 e.preventDefault();
             }
         }

        

         

    </script> 
   

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
                     Create Make
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
                      <asp:Button ID="Button1" runat="server" Text="Make View" 
                                      OnClientClick="javascript:window.location.href='TcMakeMasterView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
                         
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Create Make</h4>
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
                                                       
                                <asp:TextBox ID="txtMakeId"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="50" Visible ="false"></asp:TextBox>            
                                   
                            </div>
                        </div>
                    </div>

                                                        <div class="control-group">
                        <label class="control-label">Make Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtMakeName" runat="server" MaxLength="100" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                            &nbsp;
                            </div>
                        </div>
                    </div>
                                                        <div class="control-group">
                        <label class="control-label">Description<span class="Mandotary"> *</span></label><div class="controls">
                                                                <div class="input-append">
                                    <asp:TextBox ID="txtCode" runat="server" onkeypress="javascript:return AllowSpecialchar(event);"  MaxLength="100" style = "resize:none"
                                         ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label" ></label>
                         <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <div class="controls">
                            
                        </div>
                    </div>



                                  </div>
                                                         
                              <%--      <div class="span1"></div>--%>
                                        </div>
                     
                                        
                                    <div  class="text-center">

                                       
                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" 
                                                onclick="cmdSave_Click" />
                          
                                
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" onclick="cmdReset_Click" /><br />
                              
                                           
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                            
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
         
      </div>



</asp:Content>
