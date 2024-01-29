<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="UploadCirculars.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.UploadCirculars" %>

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
              
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="col-md-12">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Circular Uploads</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

                        
        <div class="container-fluid">
             <br />
             <asp:FileUpload ID="FileUpload1" runat="server"   Font-Size="Medium" Height="28px" Width="301px" />  
        <p>  
            <asp:Button ID="btnUpload" runat="server"  class="btn btn-success" BorderStyle="Solid" Font-Bold="True" Font-Italic="False" 
                Font-Size="large" Height="38px" OnClick="FTPUpload" Text="Upload" Width="150px" />  
             
        </p>  

         </div>

<div class="container-fluid">
             <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false"   OnRowDataBound="GridView1_RowDataBound"  >
     <Columns>
        <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="File Name" ItemStyle-Width="500"   />
     
        <asp:TemplateField>
        <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="Download" OnClick="DownloadFile"
            CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField>
        <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" runat="server" ForeColor="Red"  Text="Delete" OnClick="DeleteFile1"
            CommandArgument='<%# Eval("Name") %>'  ></asp:LinkButton>
        </ItemTemplate>
    </asp:TemplateField>

</Columns>
</asp:GridView>
</div>

                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    
            <!-- END PAGE CONTENT-->
        </div>
         </div>
             </div>
             </div>
</asp:Content>
