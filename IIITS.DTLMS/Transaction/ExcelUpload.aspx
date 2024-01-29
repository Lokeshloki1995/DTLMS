<%@ Page Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ExcelUpload.aspx.cs" Inherits="IIITS.DTLMS.Transaction.ExcelUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="https://kit.fontawesome.com/yourcode.js" type="text/javascript"></script>
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span9">

                <h3 class="page-title">Bulk Upload
                                    <div style="float:right;margin-top: 30px; margin-right:-372px">
                    <asp:Button ID="Close" runat="server" Text="Back"
                       OnClick="cmdClose_Click"
                        CssClass="btn btn-danger" />
                </div>
                </h3>
                <div class="span1">
                        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 26px" ></i></a>
                    </div>


                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button"><i class="icon-search"></i></button>
                            </div>
                        </form>
                    </li>
                </ul>

                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
           

        </div>
    </div>
     <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Upload Excel KWH Reading</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                           
                            </span>
                        </div>
                        <div class="widget-body">

                        <div class="widget-body form">
                               <div class="form-horizontal">
                                    <div class="row-fluid">
                                    <div class="span1"></div>

                                   <div class="form-horizontal">

                                    <asp:Label ID="Label1" runat="server" Font-Bold="true"  Text="NOTE:-1 Please Download  the Below Template File . " ForeColor="blue"></asp:Label><br />
                                 <asp:Label ID="Label2" runat="server" Font-Bold="true"   Text="NOTE:-2 Fill The DTC Reading  And Upload The Same File !  " ForeColor="blue"></asp:Label>
                                </div>
              <div class="space20"></div>
             <div class="control-group">      
               <h4> <i class="fa fa-download" style="color:blue"></i>
               <asp:LinkButton ID="lnkBtn" runat="server"  Text="Download Template" OnClick="DownloadFile"  style="color:blue"></asp:LinkButton>
               </h4>
           </div>
     <div class="form-horizontal">
 <div class="row-fluid">
    <div class="span1">
        <div class="control-group">
            <label class="control-label">Upload File<span class="Mandotary">*</span></label>

            <div class="controls">
                <div class="input-append">
                    <asp:FileUpload ID="fupUpload" runat="server" />
                    <asp:Button ID="cmdUpload" class="btn btn-success" runat="server" Text="Upload" OnClick="cmdUpload_click" />
                </div>
            </div>
        </div>
          
    </div>
                 
                           
        </div>
          
                       
                           </div>
                            <br />
                            <br />
             <div class="controls">
                <asp:Label ID="lblNote" runat="server" Font-Bold="true" Text="* Note: Please Uplaod file in DT_CODE,DT_KWH_READING,DT_AVERAGE_LOAD(in KVA),DT_PEAK_LOAD(in KVA),.. Format Only..... " ForeColor="Red"></asp:Label>
                            <%--<asp:Image ID="impPrev"  Height="200px" src="../img/KwhReading.png" "/> --%> 
                           
                             <img src="/img/kwhreadingnew.PNG" alt="" />
                              

                            </div>
                            </div>
                            </div>
                        </div>
                    </div>
         </div>
     <div class="form-horizontal" align="center">
     <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
     </div>

     <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Upload the Excel to Update the KWH Reading
                        .
                    </p>
                    
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
