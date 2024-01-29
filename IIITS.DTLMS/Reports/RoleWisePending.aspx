<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RoleWisePending.aspx.cs" Inherits="IIITS.DTLMS.Reports.RoleWisePending" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                     ROLES WISE PENDING FAILURE COUNT
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <%-- <asp:Button ID="Button1" runat="server" Text="Store View" 
                                      OnClientClick="javascript:window.location.href='StoreView.aspx'; return false;"
                            CssClass="btn btn-primary" />--%>
                </div>
            </div>
              <!-- END PAGE HEADER-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                           <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>ROLES WISE PENDING FAILURE COUNT
                                   <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px;padding:0px 0px 0px 0px!important"></i></a>
                            </h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                                             
                                                                

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone<span class="Mandotary"> *</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbZone_SelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                           <div class="control-group">
                                                <label class="control-label">
                                                    Circle
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged"
                                                            >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1"  >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                        </div>
                                        <%-- another span--%>
                                        <%--<div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDivision" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbSubDivision_SelectedIndexChanged"
                                                            >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    O & M Section</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOMSection" runat="server" TabIndex="1" AutoPostBack="true"  >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                                                                                                    
                                        </div>--%>
                                        <div class="span1">
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="text-center" >
                                       
                                     
                                            <asp:Button ID="cmdReport" runat="server" Text="Generate Report" CssClass="btn btn-primary" OnClick="btnGenerate_Click"/>
                                  
                                    
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                     
                                       
                                      

                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
               <!-- BEGIN PAGE CONTENT-->
         </div>
      </div>
     <!-- MODAL-->
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
                        <i class="fa fa-info-circle"></i>This Report is used to View the Failure pending count roles wise to the selected location
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>You Can Take Report by selecting Circle or Division or Subdivision or Section
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>you can Take Full Report by seclecting nothing in the form
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>By Clicking Generate Report Button crystal Report will be Generate After that you can download it in to PDF file
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