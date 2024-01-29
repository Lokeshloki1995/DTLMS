<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="UserRestrict.aspx.cs" Inherits="IIITS.DTLMS.UserRestrict" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
               <div class="space20"></div>
                  <div class="space20"></div>
            <div class="row-fluid">
                 <%-- <img src="Styles/images/Restrict1.jpg"  alt="" width="30px"/>--%>
          <img src="../img/Restrict1.jpg" alt="" style="height:55px" />
          <asp:Label ID="Label1" runat="server" Text="Sorry,You are not authorized to access this page.  " Font-Size="24px"></asp:Label>
           <asp:Button ID="cmdDashboard" runat="server" Text="Dashboard" OnClientClick="javascript:window.location.href='Dashboard.aspx'; return false;"
                                       CssClass="btn btn-primary" />
            </div>
         
            <div  class="form-horizontal">
         
                   
            </div>

            <!-- END PAGE CONTENT-->
         </div>
         
      </div>
</asp:Content>
