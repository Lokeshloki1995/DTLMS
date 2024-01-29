<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Capacitywise.aspx.cs" Inherits="IIITS.DTLMS.Reports.Capacitywise" %>
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
                            <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS FIELD WISE </h4>
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
                                         <div class="control-group">
                                                <label class="control-label">
                                                    From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
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
                                   <%-- <label class="control-label">
                                    Report Type <span class="Mandotary">*</span> </label>
                                  
                                        <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbreporttype" runat="server" >
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="EXISTING"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="INSTALLED"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>--%>
                                  
                                 </div>
                                                <label class="control-label">
                                                    To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
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

    <br />
    <br />

    <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS FAILURE FIELD WISE</h4>
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
                                    <div class="span5">

                                         <div class="control-group">
                                    <label class="control-label">
                                    Zone </label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbZone1" runat="server" AutoPostBack="true" TabIndex="2"
                                             OnSelectedIndexChanged="cmbZone1_SelectedIndexChanged">
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                  <div class="control-group">
                                    <label class="control-label" >
                                    Division</label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbDiv1" runat="server" AutoPostBack="true" TabIndex="4"
                                              >
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                         <div class="control-group">
                                                <label class="control-label">
                                                    From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate1" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
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
                                          <asp:DropDownList ID="cmbCircle1" runat="server" AutoPostBack="true" TabIndex="3"
                                             OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                              
                                                <label class="control-label">
                                                    To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate1" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
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


                                        <asp:Button ID="Button1" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                            TabIndex="12" OnClick="Export_Click1" /><br />
                                        <br />

                                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
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



   <%-- <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS ADDED</h4>
                        <a href="#" data-toggle="modal" data-target="#myModal2" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white"></i></a>
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
                                    Stores </label>
                                    <div class="controls">
                                       <div class="input-append">
                                          <asp:DropDownList ID="cmbstore" runat="server" AutoPostBack="true" TabIndex="3"
                                             >
                                          </asp:DropDownList>
                                       </div>
                                    </div>
                                 </div>
                                         </div>

                                     <div class="span5">

                                          <div class="control-group">
                                                <label class="control-label">
                                                    From Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate2" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender6" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate2" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                              
                                              
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate2" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate2" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
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


                                        <asp:Button ID="Button2" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                            TabIndex="12" OnClick="Export_Click2" /><br />
                                        <br />

                                        <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>--%>



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
                        <i class="fa fa-info-circle"></i>* The Report is Used to View the Transformers Capacity Wise.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* you can Take Full Report by seclecting the Report Type.</p>

                      <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You can Generate the report Based on From Date and To Date.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button Report will be Downloaded in Excel Format.</p>

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
                        <i class="fa fa-info-circle"></i>* The Report is Used to View the Failed Transformers Capacity Wise.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* you can Take Full Report by seclecting nothing in the From.</p>

                      <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You can Generate the report Based on From Date and To Date.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button Report will be Downloaded in Excel Format.</p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>


     <div class="modal fade" id="myModal2" role="dialog">
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
                        <i class="fa fa-info-circle"></i>* The Report is Used to View the New Added Transformers Capacity Wise.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* you can Take Full Report by seclecting nothing in the From.</p>

                      <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You can Generate the report Based on From Date and To Date.</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button Report will be Downloaded in Excel Format.</p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
