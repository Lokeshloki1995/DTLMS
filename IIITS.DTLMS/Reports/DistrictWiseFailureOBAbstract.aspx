<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DistrictWiseFailureOBAbstract.aspx.cs" Inherits="IIITS.DTLMS.Reports.DistrictWiseFailureOBAbstract" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidateForm()
        {
            if(document.getElementById('<%= txtFromDate.ClientID %>').value.trim() == "")
            {
                alert("Please Select the Month ");
                return false;
            }
            else
            {
                return true;
            }
        }
       

        </script>

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <div class="row-fluid">
            <div class="span12">
                                  <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>

                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS </h4>
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
                                            TabIndex="12" OnClientClick="return ValidateForm();" OnClick="Export_Click" /><br />
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
        


</asp:Content>
