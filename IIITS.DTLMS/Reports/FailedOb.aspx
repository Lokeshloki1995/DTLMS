<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailedOb.aspx.cs" Inherits="IIITS.DTLMS.Reports.FailedOb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            cursor: pointer !important;
        }

        .modal-body {
            visibility: hidden;
            display: none;
        }

            /*.modal-body:target {
                visibility: visible;
                display: block;
            }*/
    </style>
    <script type="text/javascript">
        function DisplayModel() {
            $('.modal-body').css("visibility", "visible");
            $('.modal-body').css("display", "block");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <br />
    <div class="row-fluid">
        <div class="span12">
            <h3 class="page-title">
                <%-- <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
            </h3>
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4>
                        <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS </h4>
                    <%--                    <a data-toggle="modal" data-target="#myModal1" title="Click For Help" href="javascript:;" ><i class="fa fa-exclamation-circle" style="font-size: 30px; cursor:pointer; color: white"></i></a>--%>
                    <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" onclick="DisplayModel()"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
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
                                            Zone
                                        </label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="true" TabIndex="2"
                                                    OnSelectedIndexChanged="cmbZone_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">
                                            Division</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="4">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <%--  <div class="control-group">
                                                <label class="control-label">
                                                    From Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
                                </div>
                                <div class="span5">


                                    <div class="control-group">
                                        <label class="control-label">
                                            Circle
                                        </label>
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
                                                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                    TargetControlID="txtFromDate" Format="MM-yyyy">
                                                </ajax:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div style="visibility:hidden">
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="8"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="MMM-yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
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
                        <i class="fa fa-info-circle"></i>* This Report is Used to view the List of OB Count, Failed, Replaced and To Replaced Transformers.
                    </p>

                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* The OB Count is Considered After the Estimation Complition Transformers Count which are Pending From Previous Month, Replaced is Considered as Inoived Transformers Count , TO Be Replaced is Considered as Pending for Invoice Transformers Count And Failed is Consiedered as After Estimation in Selected Month.
                    </p>

                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Based on the Loaction, the Report is Can be Genrated And User Should Select the Month to Generate the Report.
                    </p>

                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* By Clicking Generate Report Button crystal Report will be Generated and can be Downloaded in PDF Format.
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

