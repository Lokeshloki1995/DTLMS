<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/DTLMS.Master" CodeBehind="BudgetMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.BudgetMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
     window.onload = function () {
         var seconds = 5;
         setTimeout(function () {
             document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
     };

     function Validate() {

         if (document.getElementById('<%= txtBudgetNo.ClientID %>').value.trim() == "") {
             alert('Enter the OM Number')
             document.getElementById('<%= txtBudgetNo.ClientID %>').focus()
                return false

         }
         if (document.getElementById('<%= txtBudgetDate.ClientID %>').value.trim() == "") {
             alert('Enter the Budget Date')
             document.getElementById('<%= txtBudgetDate.ClientID %>').focus()
             return false

         }
         var FromdateInput = document.getElementById('<%= txtBudgetDate.ClientID %>').value;
         var validdate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
         if (!FromdateInput.match(validdate)) {
             alert("Please enter valid Budget  date");
             document.getElementById('<%= txtBudgetDate.ClientID %>').focus()
            return false;
        }
         if (document.getElementById('<%= txtBudgetAmount.ClientID %>').value.trim() == "" && document.getElementById('<%= txtOb.ClientID %>').value.trim() == "") {
             alert('Enter the Budget Amount or opening Balance')
             document.getElementById('<%= txtBudgetAmount.ClientID %>').focus()
             document.getElementById('<%= txtOb.ClientID %>').focus()
             return false

         }

         if (document.getElementById('<%= cmbAccCode.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbAccCode.ClientID %>').value.trim() == "") {
             alert('Select the Account code')
             document.getElementById('<%= cmbAccCode.ClientID %>').focus()
                return false
         }

         if (document.getElementById('<%= cmbFinYear.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbFinYear.ClientID %>').value.trim() == "") {
             alert('Select the Financial Year')
             document.getElementById('<%= cmbFinYear.ClientID %>').focus()
             return false
         }


        
    }
     
   </script>

    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

      <div class="container-fluid">

          <div class="row-fluid">
               <div class="span8" >
                   <h3 class="page-title">
                       Budget Master</h3>
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
         <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="btnBudgetview" class="btn btn-primary" Text="Budget View"
                        OnClientClick="javascript:window.location.href='BudgetView.aspx'; return false;" runat="server" />
                </div>
                    

             </div>

          <div class="row-fluid" runat="server">
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Budget Master</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                           
                            </span>
                        </div>
                         <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                 
                                    <div class="row-fluid">
                                     <div class="span5">
                                          <div class="control-group">
                                                <label class="control-label">OM No. <span class="Mandotary"> *</span></label>
                                              <asp:TextBox ID="txtBudgetId" runat="server" MaxLength="100" Visible="false">0</asp:TextBox>
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtBudgetNo"  runat="server" MaxLength="50" onkeypress="javascript:return AllowSpecialchar(event);"></asp:TextBox>                                                                              
                                                    </div>
                                                </div>
                                            </div>

                                          <div class="control-group">
                                                <label class="control-label">Date <span class="Mandotary"> *</span> </label>                        
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtBudgetDate" runat="server" MaxLength="10" CssClass="auto-style1" onkeypress="javascript:return AllowSpecialchar(event);"></asp:TextBox>
                                                       <ajax:CalendarExtender ID="CalendarExtender_txtBudgetDate" runat="server" CssClass="cal_Theme1"  
                                                             Format="dd/MM/yyyy"  TargetControlID="txtBudgetDate"></ajax:CalendarExtender>                            
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">Amount</label>
                       
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtBudgetAmount"  runat="server" MaxLength="20" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>                                                                              
                                                    </div>
                                                </div>
                                            </div>

                                          <div class="control-group">
                                                <label class="control-label">Opening Balance <span class="Mandotary"> *</span></label>
                       
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtOb"  runat="server" MaxLength="20" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>                                                                              
                                                    </div>
                                                </div>
                                            </div>

                                         </div>
                                         <div class="span5">
                                             <div class="control-group">
                                                <label class="control-label">Account code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbAccCode" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                               <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" Enabled="false">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                              <div class="control-group">
                                                <label class="control-label">Financial Year<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFinYear" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">OB Date  As on.</label>                        
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                        <asp:TextBox ID="txtobdate" runat="server" MaxLength="10" CssClass="auto-style1" onkeypress="javascript:return AllowSpecialchar(event);"></asp:TextBox>
                                                       <ajax:CalendarExtender ID="CalendarExtender_txtobdate" runat="server" CssClass="cal_Theme1"  
                                                             Format="dd/MM/yyyy"  TargetControlID="txtobdate"></ajax:CalendarExtender>                            
                                                    </div>
                                                </div>
                                            </div>
                                             <br/>
                                              <br />


                                         </div>

                                        </div>
                                                                            <div class="text-center">

                                    
                                            <asp:Button ID="cmdSave" runat="server" Text="Save"
                                                 CssClass="btn btn-primary"   OnClientClick="javascript:return Validate()" OnClick="cmdSave_Click" />
                                       
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="cmdReset_Click" />
       
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                    </div>
                                </div>
                             </div>
                        </div>
                   </div>
          </div>
    </asp:Content>