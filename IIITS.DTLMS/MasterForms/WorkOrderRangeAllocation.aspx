<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="WorkOrderRangeAllocation.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.WorkOrderRangeAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;

    </script>
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script language="Javascript" type="text/javascript">
        function onlyAlphabets(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }

        function Numbers(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code > 47 && code < 58)) {
                e.preventDefault();
            }
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
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Work Order Range Allocation
                    </h3>
                    <ul class="breadcrumb" style="display: none;">
                        <li class="pull-right search-wrap">
                            <form action="WorkOrderRangeAllocation.aspx" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="Text1" type="text" />
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: -34px; margin-right: 12px; margin-bottom: 15px;">
                    <asp:Button ID="cmdWORngAllctnView" class="btn btn-primary" Text="Work Order Range Allocation View"
                        OnClientClick="javascript:window.location.href='WorkOrderRangeAllocationView.aspx'; return false;" runat="server" />
                </div>
                <div style="float: right; margin-top: -34px; margin-right: 12px; margin-bottom: 15px;">
                  <%--  <asp:Button ID="cmdClose" class="btn btn-primary" Text="Close"
                        OnClientClick="javascript:window.location.href='WorkOrderRangeAllocationView.aspx'; return false;" runat="server" />--%>
                     <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-danger" OnClick="cmdClose_Click" />
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Work Order Range Allocation</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Division <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" Readonly="true" AutoPostBack="true" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Account Head <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbAccHead" AutoPostBack="true" runat="server"
                                                            OnSelectedIndexChanged="cmbAccHead_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Allocation Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtallocationDate" runat="server" MaxLength="7" TabIndex="8"></asp:TextBox>
                                                     <%--   <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtallocationDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Work Order No Start Range <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWostRng1" runat="server" MaxLength="1" Width="50px" AutoComplete="Off" TabIndex="7" AutoPostBack="true" OnTextChanged="txtWostRng1_TextChanged" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                        <asp:TextBox ID="txtWostRng2" runat="server" MaxLength="1" Width="50px" Text="-" ReadOnly="true" TabIndex="8"></asp:TextBox>
                                                        <asp:TextBox ID="txtWostRng3" runat="server" MaxLength="4" Width="80px" TabIndex="9" AutoPostBack="true" AutoComplete="Off" OnTextChanged="txtQuantity_TextChanged" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Financial Year <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFinancialYear" runat="server" MaxLength="9" ReadOnly="true" TabIndex="8"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:HiddenField ID="hdfrecordid" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Account Head Description <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtaccHeadDesc" runat="server" MaxLength="50" TextMode="MultiLine" Style="resize: none" ReadOnly="true" TabIndex="8"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Quantity <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="4" AutoComplete="Off" TabIndex="8" OnTextChanged="txtQuantity_TextChanged" AutoPostBack="true" onkeypress="return Numbers(event,this);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Work Order No End Range <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWoEdRng1" runat="server" MaxLength="1" Width="50px" TabIndex="7" ReadOnly="true" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                        <asp:TextBox ID="txtWoEdRng2" runat="server" MaxLength="1" Width="50px" Text="-" ReadOnly="true" TabIndex="8"></asp:TextBox>
                                                        <asp:TextBox ID="txtWoEdRng3" runat="server" MaxLength="4" Width="80px" TabIndex="9" ReadOnly="true" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                       <%-- <div class="space20"></div>
                                        <div class="text-center">
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="143px"
                                                OnClick="cmdSave_Click" />
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" Width="116px"
                                                TabIndex="11" OnClick="cmdReset_Click" />
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
      <%--  </div>--%>

        <div class="row-fluid" runat="server" id="dvComments" style="display: none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Comments for Approve/Modify</h4>
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
              </div>
         <div class="space20"></div>
                                        <div class="text-center">
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="143px"
                                                OnClick="cmdSave_Click" />
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" Width="116px"
                                                TabIndex="11" OnClick="cmdReset_Click" />
                                        </div>


    </div>
</asp:Content>
