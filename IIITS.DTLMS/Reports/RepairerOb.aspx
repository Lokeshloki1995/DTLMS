<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RepairerOb.aspx.cs" Inherits="IIITS.DTLMS.Reports.RepairerOb" %>
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
                                <%--  <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>

                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS </h4>
                          <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
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
                                    Zone </label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="2"
                                             OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                  <div class="control-group">
                                    <label class="control-label" >
                                    Division</label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="4"
                                              >
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                      

                                      
                                    </div>
                                    <div class="span5">

                                         
                                              <div class="control-group">
                                    <label class="control-label">
                                    Circle </label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="3"
                                             OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>

                                          <div class="control-group">
                                                <label class="control-label">
                                                   Month<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate"  runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" 
                                                            TargetControlID="txtFromDate" Format="MM-yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                              

                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" align="center">


                                        <asp:Button ID="Export" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                            TabIndex="12" OnClick="Export_Click" /><br />
                                        <br />

                                        <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

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
                        <i class="fa fa-info-circle"></i>* This Report is Used to view the List of OB Count, Issued, Recived and Pending Transformers.
</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* The OB Count is Considered as Pending of Issuing Transformers Count which are Pending From Previous Month..</p>


                     <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Based on the Loaction, the Report is Can be Genrated And User Should Select the Month to Generate the Report.

</p>
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
