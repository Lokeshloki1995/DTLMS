<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TalukView.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.TalukView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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




<div >
      
<div class="container-fluid">
 <%--BEGIN PAGE HEADER--%>
<div class="row-fluid">
<div class="span12">
<!-- BEGIN THEME CUSTOMIZER-->
                 
<!-- END THEME CUSTOMIZER-->
<!-- BEGIN PAGE TITLE & BREADCRUMB-->
<h3 class="page-title">
Taluk View
</h3>
<%--        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
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
<h4><i class="icon-reorder"></i>Taluk View</h4>

<span class="tools">
<a href="javascript:;" class="icon-chevron-down"></a>
<a href="javascript:;" class="icon-remove"></a>
</span>
</div>
<div class="widget-body">




<div style="float:right" >
                                <div class="span6">
             <asp:Button ID="cmdNewTaluk" class="btn btn-success" Text="New Taluk" 
        runat="server" onclick="cmdNewTaluk_Click"   />
                                              
                                              <br /></div>

    <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickTaluk" /><br />
                                          </div>


                                            </div>

                      
                                <div class="space20"></div>

     <div>
                             <asp:GridView ID="grdTalukdetails" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover" runat="server"
                                    ShowFooter="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                     AlternatingRowStyle-BorderStyle="None" 
                                 OnPageIndexChanging="grdTalukdetails_PageIndexChanging" 
                                 onrowcommand="grdTalukdetails_RowCommand" OnSorting="grdTalukdetails_Sorting" AllowSorting="true">
                             <HeaderStyle CssClass="both"/>
<AlternatingRowStyle BorderStyle="None"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL NO" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Tlk_ID" HeaderText="Id" Visible="false">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtId" runat="server" Text='<%# Bind("TQ_SLNO") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTlkId" runat="server" Text='<%# Bind("TQ_SLNO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="Dist_NAME" HeaderText="District Name" Visible="true" SortExpression="DT_NAME">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDistName" runat="server" Text='<%# Bind("DT_NAME") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDistName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                    Width="200px"></asp:Label>
                                            </ItemTemplate>
                                         <FooterTemplate>
                                         <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                         <asp:TextBox ID="txtDistrictName" runat="server" placeholder="Enter District Name" Width="150px" CssClass="span12"></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="Dist_NAME" HeaderText="Taluk Code" Visible="true" SortExpression="TQ_CODE">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTalkCode" runat="server" Text='<%# Bind("TQ_CODE") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTalkCode" runat="server" Text='<%# Bind("TQ_CODE") %>' Style="word-break: break-all;"
                                                    Width="200px"></asp:Label>
                                            </ItemTemplate>
                                         <FooterTemplate>
                                         <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                         <asp:TextBox ID="txtTalukCode" runat="server" placeholder="Enter Taluk Code" Width="150px" CssClass="span12"></asp:TextBox>
                                            <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                           </asp:Panel>
                                        </FooterTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="TQ_NAME" HeaderText="Taluk Name" Visible="true" SortExpression="TQ_NAME">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTalkName" runat="server" Text='<%# Bind("TQ_NAME") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTalkName" runat="server" Text='<%# Bind("TQ_NAME") %>' Style="word-break: break-all;"
                                                    Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" OnClick="imgBtnEdit_Click"
                                                        ImageUrl="~/Styles/images/edit64x64.png" Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                 <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                </asp:GridView>


</div>

 <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
</div>
</div>
</div>

<!-- END FORM-->        

<!-- END PAGE CONTENT-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Taluk Details and To Add New Taluk 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Edit Existing Details Click On <u>Edit</u> LiknkButton
                    </p>
                      <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Add New Taluk Click On <u>New Taluk</u> LiknkButton
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
