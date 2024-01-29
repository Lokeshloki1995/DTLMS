<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="BufferStock.aspx.cs" Inherits="IIITS.DTLMS.Reports.BufferStock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
   <br />
    <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Store Stock </h4>
                          <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                   
                                    
                                </div>
                            </div>
                          
                         
                               
                                    <div class="text-center" align="center">


                                        <asp:Button ID="Export" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                            TabIndex="12" OnClick="Export_Click" /><br />
                                        <br />

                                        <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                
                          
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>





                 <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Bank Stock </h4>
                          <a href="#" data-toggle="modal" data-target="#myModal1" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                   
                                    
                                </div>
                            </div>
                          
                         
                               
                                    <div class="text-center" align="center">


                                        <asp:Button ID="Button1" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                            TabIndex="12" OnClick="Export_Click1" /><br />
                                        <br />

                                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                
                          
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

    <br />
    <br />
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
                        <i class="fa fa-info-circle"></i>* The Report is Used to View the List of Transformers in the Store.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button crystal Report will be Generated and can be Downloaded in PDF Format.</p>

                    
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>


      <div class="modal fade" id="myModal1" role="dialog">
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
                        <i class="fa fa-info-circle"></i>* The Report is Used to View the List of Transformers in the Bank.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button crystal Report will be Generated and can be Downloaded in PDF Format.</p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
