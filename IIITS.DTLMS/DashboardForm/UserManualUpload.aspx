<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="UserManualUpload.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.UserManualUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   <script src="../Scripts/functions.js" type="text/javascript"></script>
      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="container-fluid">
           <div class="row-fluid">
                <div class="span12">
                    <h3 class="page-title">User Manual Upload
                          <div style="float:right;margin-top: 20px;">
                    <asp:Button ID="Close" runat="server" Text="Back"
                       OnClick="cmdClose_Click"
                        CssClass="btn btn-danger" />
                </div>
                    </h3>
                                       
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


                    </div>
               </div><br />

          <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>User Manual Upload</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">

                                            <div class="control-group">
                                            <label class="control-label">Manual Type<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbManualType" runat="server" ></asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>

                                            
                                        <div class="control-group" id="DivUpload" runat="server">
                                            <label class="control-label">Upload Document <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" accept=".pdf" />
                                                    <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                            </div>
                                    </div>
                                    <br />
                                     <div class="text-center" align="center">

                                   
                                   
                                        <asp:Button ID="btnsubmit" runat="server" Text="Upload" CssClass="btn btn-primary"
                                             Height="30px" Width="80px" 
                                            OnClick="btnsubmit_Click" TabIndex="4" />
                                   
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            CssClass="btn btn-danger" TabIndex="5" OnClick="btnReset_Click" /><br />
                                  
                                    <div class="span7"></div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
         </div>

     </div>
</asp:Content>
