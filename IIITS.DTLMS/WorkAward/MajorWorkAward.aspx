<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MajorWorkAward.aspx.cs" Inherits="IIITS.DTLMS.WorkAward.MajorWorkAward" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
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
                    <h3 class="page-title">MAJOR WORK AWARD
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
<%--                             <asp:Button ID="Button1"  runat="server" Text="Close" CssClass="btn btn-primary" OnClick="cmdClose_Click"   OnClientClick="javascript:window.location.href='MajorWorkAwardView.aspx'; return false;"  />--%>
                    <asp:Button ID="cmdClose"  runat="server" Text="Close" CssClass="btn btn-danger" OnClick="cmdClose_Click"  />
                </div>
            </div>

            <div class="row-fluid" runat="server" id="dvWrkOrder">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Bill Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                          <asp:Panel ID="pnlApproval" runat="server">
                                 

                                        

                                            <div class="control-group" >
                                                <label class="control-label">
                                                   Division <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDiv" AutoPostBack="true" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged" runat="server" TabIndex="1">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </div>
                                            <div id="billdetails" runat="server" visible="false">
                                                  <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Work Order No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWONo" runat="server" TabIndex="1"></asp:TextBox>
                                                        <asp:TextBox ID="txtWOId" runat="server" TabIndex="1" Visible="false"></asp:TextBox>
                                                         <asp:HiddenField ID="hdfEstNo" runat="server" />
                                                         <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                         <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfofficecode" runat="server" />
                                                         <asp:HiddenField ID="hdfTcCode" runat="server" />
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" /><br />

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO Amount</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWoAmount" runat="server" ReadOnly="true"></asp:TextBox>
                                                        
                                                    </div>
                                                </div>
                                            </div>

                                                 <div class="control-group">
                                                <label class="control-label">
                                                    WO Inncurred Cost</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInncured" runat="server" ReadOnly="true"></asp:TextBox>
                                                       <asp:Button ID="cmdAdd" runat="server" Text="Add" OnClick="cmdAdd_Click" OnClientClick="return ValidateForm1();" CssClass="btn btn-primary" />
                                                    </div>
                                                </div>
                                            </div>
                                                </div>
                                       


                                        <div class="span5" runat="server" id="dvOld">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    WO Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWodate" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            

                                             <div class="control-group">
                                                <label class="control-label">
                                                    Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                
                                                <div class="controls">                                                     
                                                      <asp:LinkButton ID="lnkBudgetstat" runat="server"
                                                        Style="font-size: 12px; color: Blue;cursor:pointer" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                </div>
                                                
                                            </div>
                                            </div>

                                        </div>
                                            
                                              </asp:Panel>
                                    </div>

                                    <div class="row-fluid">
                                        <div class="span3"></div>
                                        <div class="span7">
                                            <asp:GridView ID="grdWorkOrder" AutoGenerateColumns="false" PageSize="5"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" TabIndex="16" Style="width: 600px;" ShowFooter="true">
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="RWO_NO" HeaderText="Make" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("RWO_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldfid" runat="server" Text='<%# Bind("RWO_EST_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("RWO_DATE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="PO_RATING" HeaderText="Rating" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRatingId" runat="server" Text='<%# Bind("RWO_AMT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                    CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                       
                                        <div class="text-center" align="center" style="display:none">
                                            <div class="span3"></div>
                                            <div class="span1">
                                                <asp:Button ID="btnProceed" runat="server" Text="Proceed"
                                                    OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                                            </div>
                                            <div class="span1">
                                                <asp:Button ID="btnReset" runat="server" Text="Reset"
                                                    CssClass="btn btn-primary" />
                                            </div>
                                        </div>

                                        <div class="space20"></div>
                                        <div class="form-horizontal" align="center">

                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Estimation Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                         <div class="span1"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Work Award No<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWOAId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtWOANo" runat="server"></asp:TextBox>
                                                        <asp:TextBox ID="txtActionType" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Work Award Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWOADate" runat="server"></asp:TextBox>
                                                        <asp:CalendarExtender ID="WOACalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtWOADate">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="space20"></div>
                                        <div class="span11">
                                           

                                            <asp:GridView ID="grdestimation" AutoGenerateColumns="false" PageSize="15"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                runat="server" TabIndex="16" ShowFooter="true" OnPageIndexChanging="grdestimation_PageIndexChanging" OnRowCommand="grdestimation_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="RWO_SLNO" HeaderText="Make" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWoId" runat="server" Text='<%# Bind("RWO_SLNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="SD_SUBDIV_NAME" HeaderText="SubDivision" SortExpression="SD_SUBDIV_NAME">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWono" runat="server" Text='<%# Bind("SD_SUBDIV_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="OM_NAME" HeaderText="Section" SortExpression="OM_NAME">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSLNO" runat="server" Text='<%# Bind("OM_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" SortExpression="TM_NAME">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity" SortExpression="TC_CAPACITY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMAKE" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                
                                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="Serial No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount12" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="TC Code" SortExpression="TC_CODE">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount18" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="RWO_NO" HeaderText="WONO" SortExpression="WO_NO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount28" runat="server" Text='<%# Bind("RWO_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="RWO_DATE" HeaderText="WO Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGst12" runat="server" Text='<%# Bind("RWO_DATE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField AccessibleHeaderText="RWO_AMT" HeaderText="Workorder Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGst18" runat="server" Text='<%# Bind("RWO_AMT") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="RWO_INNC_COST" HeaderText="Inncurred Amount(Includes Oil)">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGst19" runat="server" Text='<%# Bind("RWO_INNC_COST") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                            
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                    CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="space20"></div>

                                    <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                                        <div class="span12">
                                            <!-- BEGIN SAMPLE FORMPORTLET-->
                                            <div class="widget blue">
                                                <div class="widget-title">
                                                    <h4><i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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
                                                                        <label class="control-label">Comments<span class="Mandotary"> *</span></label>
                                                                        <div class="controls">
                                                                            <div class="input-append">
                                                                                <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine"
                                                                                    Width="550px" Height="125px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                        <div class="text-center">
                                          
                                                <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                                     
                                     
                                                <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                    CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                    
                                            
                                                <asp:Button ID="cmdReport" runat="server" Text="View Report" Visible="false"
                                                    CssClass="btn btn-info" OnClick="cmdReport_Click"  />
                                        
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>





        </div>
    </div>
    
</asp:Content>
