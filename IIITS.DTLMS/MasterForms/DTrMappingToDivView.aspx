<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTrMappingToDivView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTrMappingToDivView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script type="text/javascript">

        $(function () {
            $.ajax({
                type: "POST",
                url: "TcMakeMasterView.aspx/LoadTCMakeMasterDetails",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                },
                error: function (response) {
                    //alert(response.d);
                }
            });
        });

         function OnSuccess(response) {
             var xmlDoc = $.parseXML(response.d);
             var xml = $(xmlDoc);
             var customers = xml.find("Table");
             var row = $("[id*=grdTcMake] tr:last-child").clone(true);
             $("[id*=grdTcMake] tr").not($("[id*=grdTcMake] tr:first-child")).remove();
             $.each(customers, function () {
                 var customer = $(this);
                 $("td", row).eq(0).html($(this).find("DRA_ID").text());
                 $("td", row).eq(1).html($(this).find("DRA_STARTRANGE").text());
                 $("td", row).eq(2).html($(this).find("DRA_ENDRANGE").text());
                 $("td", row).eq(2).html($(this).find("QUANTITY").text());
                 $("td", row).eq(2).html($(this).find("DDM_DIV_CODE").text());
                 $("[id*=grdTcMake]").append(row);
                 row = $("[id*=grdTcMake] tr:last-child").clone(true);
             });
         }




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
        background:url(img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

   

    .descending th a {
        background:url(img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
        background:url(img/sort_both.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .asc{
      
      
    }
     .modalPopup
    {
       
        width: 434px;
        height: 362px;
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
                   DTR Mapping To Division View
                   </h3>
<%--                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                         <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
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
                            <h4><i class="icon-reorder"></i> DTR Mapping View </h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                                <div style="float:right" >
                                <div class="span6">
                                   <asp:Button ID="cmdNew" runat="server" Text="Create New" 
                                              CssClass="btn btn-success" onclick="cmdNew_Click" /><br /></div>

                                     <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_click" /><br />
                                          </div>

                                            </div>
                                  
                                    <div class="space20"> </div>
                                 
                                <!-- END FORM-->
                           
                      
                            <asp:GridView ID="grdTcMake" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdTcMake_PageIndexChanging" 
                                    onrowcommand="grdTcMake_RowCommand" 
                                OnSorting="grdmakeDetails_Sorting" AllowSorting="true">
                             <HeaderStyle CssClass="both"/>
                             
                                <Columns>
                                   <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="DRA_ID" HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lbldraId" runat="server" Text='<%# Bind("DRA_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField AccessibleHeaderText="DRA_STARTRANGE" HeaderText="Start Range" SortExpression="DRA_STARTRANGE" >
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblstartrange" runat="server" Text='<%# Bind("DRA_STARTRANGE") %>' style="word-break: break-all;"></asp:Label>
                                        </ItemTemplate>
                                      <FooterTemplate>
                                      <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                      <asp:TextBox ID="txtstartrange" runat="server" placeholder="Enter Start Range" MaxLength="7"></asp:TextBox>
                                      </asp:Panel>
                                    </FooterTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="DRA_ENDRANGE" HeaderText="End Range" SortExpression="DRA_ENDRANGE" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblendrange" runat="server" Text='<%# Bind("DRA_ENDRANGE") %>' style="word-break: break-all;"></asp:Label>
                                        </ItemTemplate>
                                      <FooterTemplate>
                                      <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                      <asp:TextBox ID="txtendrange" runat="server" placeholder="Enter End Range" MaxLength="7" ></asp:TextBox>
                                      </asp:Panel>
                                    </FooterTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField AccessibleHeaderText="QUANTITY" HeaderText="Quantity" SortExpression="QUANTITY" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblquantity" runat="server" Text='<%# Bind("QUANTITY") %>' style="word-break: break-all;"></asp:Label>
                                        </ItemTemplate>
                                      <FooterTemplate>
                                      <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                      <asp:TextBox ID="txtQuantity" runat="server" placeholder="Enter Quantity" MaxLength="6" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                      </asp:Panel>
                                    </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DDM_DIV_CODE" HeaderText="Division Name" SortExpression="DDM_DIV_CODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldivcode" runat="server" Text='<%# Bind("DDM_DIV_CODE") %>' style="word-break: break-all;" ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                              <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                                      <asp:TextBox ID="txtdivcode" runat="server" placeholder="Enter Division Name" MaxLength="25"></asp:TextBox>
                                      
                                             <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                                  </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                     
                                </Columns>
                               
                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                               
                            </asp:GridView>
                       
                        </div>
                          <ajax:modalpopupextender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                                  PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                             <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                                <div style="display:none">
                                    <asp:Button ID="btnshow" runat="server" Text="Button"  />
                                 </div>
                                    <asp:Panel ID="pnlControls" runat="server" CssClass="modalPopup" align="center" style = "display:none">
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All DTr's Mapped to Division and New Can Be Added
                        </p>
                         
                      <%--   <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Existing DTr's which are Mapped to Division Can Be Edited By Clicking Edit Button
                        </p>--%>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New DTr's Can Be Mapped By Clicking Create New Button
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
