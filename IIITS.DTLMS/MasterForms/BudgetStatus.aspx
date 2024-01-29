<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BudgetStatus.aspx.cs"  Inherits="IIITS.DTLMS.MasterForms.BudgetStatus" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
   <link href="/assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="/assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
    <link href="/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/style-responsive.css" rel="stylesheet" />
    <link href="/css/style-default.css" rel="stylesheet" id="style_color" />
    <link href="/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
    <link href="/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="Styles/calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/functions.js"></script>

 <script  type="text/javascript">
     window.onload = function () {
         var seconds = 5;
         setTimeout(function () {
             document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
         }, seconds * 1000);
     };
     function Validate() {
        

         if (document.getElementById('<%= cmbFinYear.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbFinYear.ClientID %>').value.trim() == "") {
             alert('Select the Financial Year')
             document.getElementById('<%= cmbFinYear.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= cmbAccCode.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbAccCode.ClientID %>').value.trim() == "") {
             alert('Select the Account code')
             document.getElementById('<%= cmbAccCode.ClientID %>').focus()
             return false
         }
     }

   </script>

   
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

      <div class="container-fluid">

          <div class="row-fluid">
               <div class="span8" >
                   <h3 class="page-title">
                       Budget Status</h3>
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
            <div  class="row-fluid" runat="server">
           <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Budget Status</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a> 
                            </span>
                              </div>
                            <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                 
                                    <div class="row-fluid">
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
                                                <label class="control-label">Account code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbAccCode" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                        <div class="control-group">
                                                <label class="control-label">Available Amnt</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtavlamnt"  runat="server" ReadOnly="true"></asp:TextBox> 

                                                    </div>
                                                </div>
                                            </div>

                                         <div class="form-horizontal" align="center">
                                             <div class="span5">
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load"
                                                 CssClass="btn btn-primary"  OnClientClick="javascript:return Validate()"  OnClick="cmdLoad_Click"  />
                                             </div>
                                              <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                             </div>
                                         <br />
                                        <br />
                                        <br />
                                        
                                           <%-- <div class="span5">
                                               <div class="control-group">
                                                <label class="control-label">Total Budget Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTotalBudget"  runat="server" Enabled="false"></asp:TextBox>                                                                              
                                                    </div>
                                                </div>
                                            </div>
                                             </div>
                                        <div class="span5">
                                         <div class="control-group">
                                                <label class="control-label">Available Budget Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtavlBudget"  runat="server" Enabled="false"></asp:TextBox>                                                                              
                                                    </div>
                                                </div>
                                            </div>
                                            </div>--%>
                                        <asp:GridView ID="grdBudgetstatus" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" 
                                    AllowPaging="true" OnPageIndexChanging="grdBudgetstatus_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="BT_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetId" runat="server" Text='<%# Bind("BT_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BT_ACC_CODE" HeaderText="Budget acc code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetacccode" runat="server" Text='<%# Bind("BT_ACC_CODE") %>' Style="word-break: break-all"
                                                    Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>'
                                                    Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="Work Order Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>'
                                                    Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BT_BM_AMNT" HeaderText="Budget Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbtbmamnt" runat="server" Text='<%# Bind("BT_BM_AMNT") %>'
                                                    Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BT_DEBIT_AMNT" HeaderText="Budget Debit Amnt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDEBITAMNT" runat="server" Text='<%# Bind("BT_DEBIT_AMNT") %>'
                                                    Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                         
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="BT_AVL_AMNT" HeaderText="Budget Avalable Amnt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAVLAMNT" runat="server" Text='<%# Bind("BT_AVL_AMNT") %>' Width="80px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>    
                                        
                                          <asp:TemplateField AccessibleHeaderText="BT_CREDIT_AMNT" HeaderText="Budget Credit Amnt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCREDITAMNT" runat="server" Text='<%# Bind("BT_CREDIT_AMNT") %>' Width="80px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>  
                                        
                                         <asp:TemplateField AccessibleHeaderText="BT_CRON" HeaderText="Budget Credit on" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCRON" runat="server" Text='<%# Bind("BT_CRON") %>' Width="80px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField> 
                                        
                                         <asp:TemplateField AccessibleHeaderText="BT_FIN_YEAR" HeaderText="Budget Fin Year" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFINYEAR" runat="server" Text='<%# Bind("BT_FIN_YEAR") %>' Width="80px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField> 
                                        
                                         <asp:TemplateField AccessibleHeaderText="BT_DIV_CODE" HeaderText="Budget Div code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDIVCODE" runat="server" Text='<%# Bind("BT_DIV_CODE") %>' Width="80px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>                        
                                    </Columns>
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>


                                        </div>
                                    </div>
                                </div>




                        </div>

             
          </div>

     </div>
    </div>
</form>
</body>
</html>