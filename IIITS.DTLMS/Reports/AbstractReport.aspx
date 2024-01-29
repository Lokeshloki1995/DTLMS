<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="AbstractReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.AbstractReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/functions.js" type="text/javascript"></script>

<script type="text/javascript">
        function ValidateMyForm() {
           // if (document.getElementById('<%= txtFromDate.ClientID %>').value.trim() == "") {
                //alert('Enter From Date')
            //    document.getElementById('<%= txtFromDate.ClientID %>').focus()
            //    return false
            //}

          //  if (document.getElementById('<%= txtToDate.ClientID %>').value.trim() == "") {
            //    alert('Enter To Date')
               // document.getElementById('<%= txtToDate.ClientID %>').focus()
                //return false
            //}

<%--           if (document.getElementById('<%= cmbZone1.ClientID %>').value.trim() == "--Select--") {
               alert('Please Select the Zone')
                document.getElementById('<%= cmbZone1.ClientID %>').focus()
              return false
            }--%>

        }

    function ValidateMyFormsecond()
    {
<%--        if (document.getElementById('<%= cmbZone2.ClientID %>').value.trim() == "--Select--") {
            alert('Please Select the Zone')
            document.getElementById('<%= cmbZone2.ClientID %>').focus()
               return false
           }--%>

    }

            </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                      Abstract Report
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
                            CssClass="btn btn-primary" />--%></div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Failure Abstract Report</h4> <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px;color:white"></i></a> 
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
                                      
                                        <%-- another span--%>
                                       
                                        
                                         <div class="span5">
                                       <%-- <div class="control-group">
                                                <label class="control-label">
                                                    From Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Zone </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone1" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbZone1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle1" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv1" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbDiv1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
    <%--                                          <div class="control-group">
                                                <label class="control-label">
                                                    From Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                        <div class="span5">
                                       <%-- <div class="control-group">
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv1" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbSubDiv1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    O & M Section</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection1" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Feeder Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeederName1" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
      <%--                                      <div class="control-group">
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>

                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>

                                    <div class="text-center">
                                       
                                    
                                            <asp:Button ID="cmdReport" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                                OnClick="cmdReport_Click" OnClientClick="javascript:return ValidateMyForm()" />
                                     
                                                <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="btn btn-danger" 
                                                     onclick="BtnReset1_Click" />
                                          
                                        <asp:Button ID="Button2" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                            onclick="Export_clickFailureAbstract" OnClientClick="javascript:return ValidateMyForm()" /><br />
                                          </div>
                                        
                                       
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
            <!-- END PAGE CONTENT-->

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>CR ABSTRACT</h4> <a href="#" data-toggle="modal" data-target="#myModal1" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px;color:white"></i></a>
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
                                                        <asp:DropDownList ID="cmbZone2" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbZone2_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle2" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbCircle2_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv2" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbDiv2_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">
                                                    From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
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
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv2" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbSubDiv2_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    O & M Section</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Feeder Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeederName2" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">
                                                    To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br /> 
                                    <div  class="text-center">
                                       
                                                <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyFormsecond()"
                                                    CssClass="btn btn-primary" TabIndex="10" OnClick="btnGenerate_Click" />
                                           
                                                <asp:Button ID="BtnReset2" runat="server" Text="Reset" CssClass="btn btn-danger" 
                                                    TabIndex="11" onclick="BtnReset2_Click" />
                                          
                                                <asp:Button ID="Button1" runat="server" Text="Export Excel" OnClientClick="javascript:return ValidateMyFormsecond()" CssClass="btn btn-info" 
                                                    TabIndex="12" onclick="Export_clickCrdeatails" />
                                           
                                          
                                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                   
                                <!-- END FORM-->
                           
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>



            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>CR DETAILS</h4> <a href="#" data-toggle="modal" data-target="#myModal2" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px;color:white"></i></a>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span3">
                                        
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCCode" runat="server" placeholder="Enter DTC CODE"></asp:TextBox>
                                                    
                                                </div>
                                            </div>
                                        </div>

                                        </div>
                                        <div class="text-center">

                                        
                                               <asp:Button ID="cmdSearch" Text="Search" class="btn btn-primary" runat="server" 
                                                    onclick="cmdSearch_Click" />
                                        

                                    
                                                <asp:Button ID="BtnReset" runat="server" Text="Reset" 
                                                    CssClass="btn btn-danger" TabIndex="11" onclick="BtnReset_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="span5">
                                </div>
                                <div class="row-fluid">
                                 
                                        <div class="form-horizontal" align="center">
                                     
                                            <div class="text-center">
                                     <br />
                                            </div>
                                            <div class="span7">
                                            </div>
                                            <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                  
                                </div>
                                <!-- END FORM-->

                                <div class="span5">
                                </div>

                                <div >
                                    <asp:HiddenField ID="FailureID" runat="server" />
                                      <asp:GridView ID="GrdCrDetail" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table table-striped table-bordered table-advance table-hover" 
                                          onrowcommand="GrdCrDetail_RowCommand" PageSize="10" AllowPaging="true" 
                                        onpageindexchanging="GrdCrDetail_PageIndexChanging" >
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="DF_DTC_CODE" HeaderText="DTC CODE" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DF_EQUIPMENT_ID" HeaderText="DTR Code" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTccode" runat="server" Text='<%# Bind("DF_EQUIPMENT_ID") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WORK ORDER NO"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>'
                                                            Style="word-break: break-all;" Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="WO_NO_DECOM" HeaderText="WORK ORDER DECOMM NO" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWODECOM" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WORK ORDER Date" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWODate" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="VIEW" Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <center>
                                                         <asp:LinkButton runat="server"  CommandName="View" ID="lnkView" ToolTip="View">
                                                         <img src="../img/Manual/view.png" style="width:20px" /></asp:LinkButton> 
                                                        </center>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        </div>
                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>


<%--            <div>
                <asp:Button ID="Button1" runat="server" Text="makewiseReport" 
                    onclick="Button1_Click" />
            </div>--%>

        </div>
    </div>

    <!-- MODAL-->
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
                        <i class="fa fa-info-circle"></i>* This Report Will Display Failure Abstract Report</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* In this Report you will get DTC Failure pending by 1 Day, Fort Night, 1 Month, Greater Than 1 Month</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

        <!-- MODAL-->
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
                        <i class="fa fa-info-circle"></i>* This Report Will Display CR Completed Records</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* You Can Take Report by selecting FromDate or ToDate or Circle or Division or Subdivision or Section</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

    <!-- MODAL-->
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
                        <i class="fa fa-info-circle"></i>* This Report Will Display CR Report </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Enter Dtc code in textbox then click on search</p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>* Multiple records will Display. you can select any one record and click view to generate Report</p>
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
