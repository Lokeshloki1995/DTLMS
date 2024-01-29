<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/DTLMS.Master" CodeBehind="CircularDetails.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.CircularDetails" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function openTab(th) {
            window.open(th.name, '_blank');
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style>
        table#ContentPlaceHolder1_gvFiles {
    width: 100%;
    text-align: center;
    margin-top:15px!important;
    margin-bottom:15px!important;
}
    </style>

         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                  Downloads
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
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Circular Downloads</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

       <%--                 
        <div>
            <asp:Label runat="server" Text="Upload" Font-Size="Medium"></asp:Label>
             <asp:FileUpload ID="FileUpload1" runat="server"   Font-Size="Medium" Height="38px" Width="301px" />  
        <p>  
            <asp:Button ID="btnUpload" runat="server"  BorderStyle="Solid" Font-Bold="True" Font-Italic="False" 
                Font-Size="X-Large" Height="48px" OnClick="btnUplaod_Click" Text="Upload" Width="226px" />  
            <asp:Label ID="Label1" runat="server"   Font-Size="Medium" Text="Label"></asp:Label>  
        </p>  

         </div>--%>



       <%--  <asp:Button ID="btnGetFiles" Text="Get Filenames from MyFiles Folder" runat="server" onclick="GetFiles" />--%>
        <div class="container-fluid">
        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false">
<Columns>
    <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="File Name" ItemStyle-Width="700"   />     
    <asp:TemplateField>
        <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="Download" OnClick="DownloadFile"
            CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
              

                 </Columns>
             </asp:GridView>
            </div>
    
    </div>
                      <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    
            <!-- END PAGE CONTENT-->
         </div>
         </div>
</asp:Content>
   