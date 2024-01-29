<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Repairerwoexcel.aspx.cs" Inherits="IIITS.DTLMS.Transaction.Repairerwoexcel" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>


    <div class="row-fluid">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Upload Excel</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>

                    </span>
                </div>
                <div class="widget-body">

                    <div class="widget-body form">
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span1"></div>
                                <div class="span1">
                                    <div class="control-group">
                                        <label class="control-label">Upload File<span class="Mandotary">*</span></label>

                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:FileUpload ID="fupUpload" runat="server" />
                                                <asp:Button ID="cmdUpload" class="btn btn-primary" runat="server" Text="Upload" OnClick="cmdUpload_click" />
                                            </div>
                                        </div>
                                    </div>

                                </div>



                            </div>
                        </div>
                        <br />
                        <br />

                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
