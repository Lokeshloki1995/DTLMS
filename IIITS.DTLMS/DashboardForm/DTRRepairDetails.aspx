<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="DTRRepairDetails.aspx.cs"  
     Inherits="IIITS.DTLMS.DashboardForm.DTRRepairDetails" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
<style type="text/css">
   
    .Warning
    { 
            font-weight: bold;
            text-align:center;
         
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
    <title></title>

    
</head>
<body>
     <form id="form1" runat="server">
<%--</asp:Content>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>

      <div >
         <div class="container-fluid" >
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid"  >
               <div class="span12"  >
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
             <%-- Faulty DTR Details--%>
                  
                   </h3>
                   <ul class="breadcrumb" style="display:none;">
                       
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
     
     <div class="row-fluid" style =" width:1350px; " >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue"  >
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i> Faulty DTR Details</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body" >
                         <div style="float:right" >
                                   <asp:Button ID="cmdExport" runat="server" Text="Export To Excel"  
                                        CssClass="btn btn-primary" onclick="cmdExport_Click" 
                                                />
                                                 <asp:HiddenField ID="hdfOffCode" runat="server" />

                                            </div> 
                                <div class="space20"></div>
                                <!-- END FORM-->
    
    
    <div style="width:1285px; overflow:auto;">
    <asp:GridView ID="grdComplete" 
     AutoGenerateColumns="false"  PageSize="10" 
    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
   
    CssClass="table table-striped table-bordered table-advance table-hover" 
    AllowPaging="true"  runat="server" onrowcreated="grdComplete_RowCreated"
         onpageindexchanging="grdComplete_PageIndexChanging" AllowSorting="true" OnSorting="grdComplete_Sorting">
         <HeaderStyle HorizontalAlign="center" CssClass="both" />            
  <Columns>
          <asp:TemplateField AccessibleHeaderText="tc_code" HeaderText="DTR Code" Visible="true" SortExpression="tc_code">
            <EditItemTemplate>
                <asp:TextBox ID="txtDtrCode" runat="server" Text='<%# Bind("tc_code") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>                                                 
                <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("tc_code") %>'  width="100px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" Visible="true" SortExpression="TM_NAME">
            <EditItemTemplate>
                <asp:TextBox ID="txtDtrSlNo" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblDtrSlNo" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break: break-all;" width="180px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
          <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SlNo"
            Visible="true" SortExpression="TC_SLNO">
            <EditItemTemplate>
                <asp:TextBox ID="txtAddress" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;"  width="100px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
          <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" >
            <EditItemTemplate>
                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("TC_CAPACITY") %>' style="word-break: break-all;"  width="100px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="dt_code" HeaderText="Transformer Centre Code" SortExpression="dt_code">
            <EditItemTemplate>
                <asp:TextBox ID="txtDtcCode" runat="server" Text='<%# Bind("dt_code") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("dt_code") %>' style="word-break: break-all;"  width="100px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
          <asp:TemplateField AccessibleHeaderText="dt_name" HeaderText="Transformer Centre Name" SortExpression="dt_name">
            <EditItemTemplate>
                <asp:TextBox ID="txtDtcName" runat="server" Text='<%# Bind("dt_name") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("dt_name") %>' style="word-break: break-all;"  width="180px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
          <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI No" SortExpression="TR_RI_NO">
            <EditItemTemplate>
                <asp:TextBox ID="txtRiNo" runat="server" Text='<%# Bind("TR_RI_NO") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblRiNo" runat="server" Text='<%# Bind("TR_RI_NO") %>' style="word-break: break-all;" width="150px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI Date" >
           
            <ItemTemplate>
                <asp:Label ID="lblRIDate" runat="server" Text='<%# Bind("TR_RI_DATE") %>' style="word-break: break-all;" width="100px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="sm_name" HeaderText="Store Name" SortExpression="sm_name">
            <EditItemTemplate>
                <asp:TextBox ID="txtStoreName" runat="server" Text='<%# Bind("sm_name") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("sm_name") %>' style="word-break: break-all;" width="120px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer Name" SortExpression="SUP_REPNAME">
            <EditItemTemplate>
                <asp:TextBox ID="txtSupRepName" runat="server" Text='<%# Bind("SUP_REPNAME") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblSupRepName" runat="server" Text='<%# Bind("SUP_REPNAME") %>' style="word-break: break-all;" width="150px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
          <asp:TemplateField AccessibleHeaderText="RSM_ISSUE_DATE" HeaderText="Issue Date" SortExpression="RSM_ISSUE_DATE">
            <EditItemTemplate>
                <asp:TextBox ID="txtIssueDate" runat="server" Text='<%# Bind("RSM_ISSUE_DATE") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblissueDate" runat="server" Text='<%# Bind("RSM_ISSUE_DATE") %>' style="word-break: break-all;"  width="100px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
          <asp:TemplateField AccessibleHeaderText="SUP_INSP_DATE" HeaderText="Inspected Date" SortExpression="SUP_INSP_DATE">
            <EditItemTemplate>
                <asp:TextBox ID="txtInspDate" runat="server" Text='<%# Bind("SUP_INSP_DATE") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblInspdate" runat="server" Text='<%# Bind("SUP_INSP_DATE") %>' style="word-break: break-all;"  width="100px" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="INSP_BY" HeaderText="Inspected By" SortExpression="INSP_BY">
            <EditItemTemplate>
                <asp:TextBox ID="txtInspBy" runat="server" Text='<%# Bind("INSP_BY") %>'></asp:TextBox> 
                     </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblInspectedBy" runat="server" Text='<%# Bind("INSP_BY") %>' style="word-break: break-all;"  width="120px"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


    </Columns>

 </asp:GridView>
 
 </div>
                           
            </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
            </div>
            </div>
         </form>
</body>
</html>

<%--</asp:Content>--%>
