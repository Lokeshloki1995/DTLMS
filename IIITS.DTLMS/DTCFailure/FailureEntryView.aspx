<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailureEntryView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.FailureEntryView" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link type="text/css"  href="../assets/jquery.dataTables.css" rel="stylesheet" />
 <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>--%>
    <script type="text/javascript">        //
        //$(document).ready(function () {
        //    $('#ContentPlaceHolder1_grdFailureDetails').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
            
        //        "sPaginationType": "full_numbers"
        //    });
        //});
    </script>
     <style type="text/css">
        table {
   
    overflow: scroll;
}
        /*td {
    border: 1px solid #ccc;
    text-align:center;
}*/
        /*.table-advance thead tr th {
    background-color: #438eb9!important;
    color: #fff;
}*/
        /*table.dataTable thead th, table.dataTable thead td {
    padding: 0px 0px;
    border-bottom: 1px solid #111;
    font-size: 12px!important;
}*/
       /*.table-advance tr td {
    border-left-width: 1px !important;
    border: 1px solid #d4d4d4;
    font-size:12px!important;
}
        th{
            white-space: nowrap;
        }*/
</style>

     
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
        /*table#ContentPlaceHolder1_grdFailureDetails {
            display: -webkit-box !important;
            overflow-x: scroll !important;
            width: 96%;
        }*/
        /*.table {
   
     table-layout: fixed;*/
   
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    Failure Details View
                   </h3>
<%--                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Failure Details View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                         <div class="form-horizontal">
                                    <div class="row-fluid">
                       <%-- <div style="float:left" >--%>
                              <%--  <div class="span8">--%>
                              <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span1">
                            <asp:RadioButton ID="rdbViewAll" runat="server" Text="View All" CssClass="radio" 
                                  GroupName="a" Checked="true" AutoPostBack="true"
                                  oncheckedchanged="rdbViewAll_CheckedChanged" />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbAlready" runat="server"  Text="Already Created" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true"
                                   oncheckedchanged="rdbAlready_CheckedChanged" />
                            </div>
                             <div style="float:right;">
                                 <div class="span4">
                                 <asp:Button ID="cmdNew" runat="server" Text="New" 
                                       style="visibility:hidden"  CssClass="btn btn-primary" onclick="cmdNew_Click" /></div>

                                  <div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickFailureEntry" /><br />
                         </div>
                                 

                             </div>

                                    <%--<asp:Label ID="lblStatus" runat="server" Text="Status" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:DropDownList ID="cmbIndexSelection" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="cmbIndexSelection_SelectedIndexChanged" >
                                 
                                     <asp:ListItem Text="To Be Created" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Already Created" Value="1"></asp:ListItem>
                                   </asp:DropDownList>--%>
                                   
                                   
                                <%--   </div>--%>
                      </div>
                        </div>
                        </div>
                                <!-- END FORM-->
                       <div class="container-fluid">    

                 <asp:GridView ID="grdFailureDetails" AutoGenerateColumns="false" 
                  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" 
                 CssClass="table table-striped table-bordered table-advance table-hover" ShowFooter="true"
                  runat="server"  onpageindexchanging="grdFailureDetails_PageIndexChanging"
                                 AllowPaging="true"  OnSorting="grdFailureDetails_Sorting" AllowSorting="true"  onrowcommand="grdFailureDetails_RowCommand" 
                            onrowdatabound="grdFailureDetails_RowDataBound" >
                     <%--                                <HeaderStyle CssClass="both" /> 
                     <PagerStyle CssClass="gvPagerCss " />
                                                    <HeaderStyle CssClass="both" />--%>
    
                    <Columns>
                     <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Id" Visible="false">
           
                            <ItemTemplate>                                       
                                <asp:Label ID="lblFailureId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                    <asp:TemplateField AccessibleHeaderText="DT_ID" HeaderText="Id" Visible="false">
                            <ItemTemplate>              
                                <asp:Label ID="lblId" runat="server" Width="50px" Text='<%# Bind("DT_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="SLNO" HeaderText="SlNo">
                            <ItemTemplate>              
                                <asp:Label ID="lblROWSLNO" runat="server" Text='<%# Bind("SLNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Transformer Centre Code"  SortExpression="DT_CODE">
                            <ItemTemplate>

                                <asp:LinkButton runat="server"  CommandName="CreateNew" Width="80px" ID="lnkCreateDTC" >
                                <asp:Label ID="lblDtcCode" runat="server" Width="80px" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                              <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="110px" MaxLength="9" ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField AccessibleHeaderText="DT_TIMS_CODE" HeaderText="TIMS Code"  SortExpression="DT_CODE">
                             <ItemTemplate>
                                <asp:Label ID="lblTimscode" runat="server" Text='<%# Bind("DT_TIMS_CODE") %>' style="word-break: break-all;" width="110px"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                              <asp:Panel ID="paneltims" runat="server" DefaultButton="imgBtnSearch" >
                                <asp:TextBox ID="txtTimsCode" runat="server" placeholder="Enter Tims Code " Width="110px" MaxLength="13" ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="Transformer Centre Name" SortExpression="DT_NAME">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%# Bind("DT_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                              <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                  <asp:TextBox ID="txtDtName" runat="server" placeholder="Enter DTC Name" Width="150px" MaxLength="50" ></asp:TextBox>
                            </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" >          
                            <ItemTemplate> 
                                <asp:LinkButton runat="server"  CommandName="CreateNew" ID="lnkCreateDTR" >                                                
                                <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                    </asp:LinkButton>
                            </ItemTemplate>
                              <FooterTemplate>
                                <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                 <asp:TextBox ID="txtDtrCode" runat="server"  placeholder="Enter DTr Code" Width="100px" MaxLength="10" ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr Slno">
                            <ItemTemplate>
                                <asp:Label ID="lblslno" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                            <ItemTemplate>
                                <asp:Label ID="lbltmname" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                                   <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                             </FooterTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" >
                           
                            <ItemTemplate>
                                <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

<%--                        <asp:TemplateField AccessibleHeaderText="DF_ENHANCE_CAPACITY" HeaderText="Capacity(in KVA)" Visible="false">
                           
                            <ItemTemplate>
                                <asp:Label ID="lblEnhancecapacity" runat="server" Text='<%# Bind("DF_ENHANCE_CAPACITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField AccessibleHeaderText="" HeaderText="Failure Entry" Visible="false">           
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
     
                        <asp:TemplateField HeaderText="Action" >
                            <ItemTemplate>
                                <center>
                                  <%--  <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                        Width="12px" />--%>
                                      <asp:LinkButton runat="server"  CommandName="CreateNew" ID="lnkCreate" >
                                      <img src="../Styles/images/Create.png" style="width:20px" />Declare Failure</asp:LinkButton>
                                      <asp:LinkButton runat="server"  CommandName="Create" ID="lnkUpdate"  visible="false" >
                                      <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                      <asp:LinkButton runat="server"   ID="lnkWaiting"  visible="false" >
                                      <img id="lnkbtnwait" src="../img/Manual/Wait.png" style="width:20px" />Waiting for Approval</asp:LinkButton>
                                      <asp:LinkButton runat="server" tooltip="Click To Download Estimation report" CommandName="Preview"  ID="lnkEstPrev"  visible="false" >
                                      <img id="Img1" src="../img/Manual/Pdficon.png" style="width:20px" /></asp:LinkButton>

                                </center>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <center>
                                    <asp:Label ID="lblHeader" runat="server" Text="Action" ></asp:Label>
                                </center>
                            </HeaderTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField AccessibleHeaderText="DT_PROJECTTYPE" HeaderText="Project Type" Visible="false">                                       
                            <ItemTemplate>
                                <asp:Label ID="lblProjectType" runat="server" Text='<%# Bind("DT_PROJECTTYPE") %>' style="word-break: break-all;" width="60px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="5" PreviousPageText="Last" />
                </asp:GridView>
                        </div>
                        <div class="span7"></div>
                                 <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Failure Details and To Declare TC as Failure.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Declare Failure Click On <u>Declare Failure</u> Button
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Failure is Declared By Section Officer, Estimation Report Can Be Downloded By Clicking On PDF Icon
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To View Already Failure Details Click on Already Created Radio Button
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
