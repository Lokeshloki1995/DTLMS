<%@ Page Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PermanentWO.aspx.cs" Inherits="IIITS.DTLMS.PermanentDecomm.PermanentWO" %>

<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function noCopyMouse(e) {
            var isRight = (e.button) ? (e.button == 2) : (e.which == 3);


            if (isRight) {
                alert('Copy & paste is not possible');
                return false;
            }
            return true;
        }

        function noCopyKey(e) {
            var forbiddenKeys = new Array('c', 'x', 'v');
            var keyCode = (e.keyCode) ? e.keyCode : e.which;
            var isCtrl;


            if (window.event)
                isCtrl = e.ctrlKey
            else
                isCtrl = (window.Event) ? ((e.modifiers & Event.CTRL_MASK) == Event.CTRL_MASK) : false;


            if (isCtrl) {
                for (i = 0; i < forbiddenKeys.length; i++) {
                    if (forbiddenKeys[i] == String.fromCharCode(keyCode).toLowerCase()) {
                        return false;
                    }
                }
            }
            return true;
        }


        function ValidateMyForm() {

            if (document.getElementById('<%= cmbIssuedBy.ClientID %>').value == "--Select--") {
                alert('Select Issued By')
                document.getElementById('<%= cmbIssuedBy.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "--Select--") {
                alert('Select Capacity')
                document.getElementById('<%= cmbCapacity.ClientID %>').focus()
                return false
            }


            if (document.getElementById('<%= txtDeDate.ClientID %>').value.trim() == "") {
                alert('Enter DeCommissioning Date')
                document.getElementById('<%= txtDeDate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDeAmount.ClientID %>').value.trim() == "") {
                alert('Enter DeCommissioning amount')
                document.getElementById('<%= txtDeAmount.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDecAccCode.ClientID %>').value.trim() == "") {
                alert('Select DeCommissioning Account Code')
                document.getElementById('<%= txtDecAccCode.ClientID %>').focus()
                return false
            }

            var FromdateInput = document.getElementById('<%= txtDeDate.ClientID %>').value;
            var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
            if (!FromdateInput.match(goodDate)) {
                alert("Please enter valid DeCommission WO date");
                document.getElementById('<%= txtDeDate.ClientID %>').focus()
                return false;
            }
        }
    </script>

    <style type="text/css">
        .auto-style1 {
            left: -4px;
            top: -5px;
        }
         input#ContentPlaceHolder1_txtDeWoNo2 {
            width: 45px !important;
            text-align: center;


        }
    </style>

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
                    <h3 class="page-title">Permanent Work Order</h3>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>

                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-danger" OnClick="cmdClose_Click" />
                </div>
            </div>

            <div class="row-fluid" runat="server" id="dvBasic">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Basic Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

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
                                                <label class="control-label">
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure Id" Visible="false"></asp:Label>
                                                   <%-- <span class="Mandotary">*</span>--%>

                                                </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        sWFOAutId
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                        <asp:TextBox ID="txtFailureId" runat="server"
                                                            onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="1" Visible="false"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"
                                                            OnClick="cmdSearch_Click" TabIndex="2" Visible="false"/>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Transformer Centre Code</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfSubdivName" runat="server" />
                                                        <asp:HiddenField ID="hdfdivName" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfAppDesc" runat="server" />
                                                        <asp:HiddenField ID="hdfGuarenteeType" runat="server" />
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox></br/>
                                                       <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtFailType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTCDetails_Click">View Transformer Centre Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Transformer Centre Name</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="span5">

                                             <div class="control-group" style="display: none">
                                                    <label class="control-label">
                                                        <asp:Label ID="lblDateText" runat="server" Text="Failure Date" ></asp:Label>
                                                     </label>
                        
                                                    <div class="controls">
                                                        <div class="input-append">
                                                        <asp:TextBox ID="txtActiontype" runat="server"  MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            <br />
                                            <div class="control-group">
                                                <label class="control-label">DTr Code</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Declared By</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeclaredBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <br />
                                                    <asp:LinkButton ID="lnkBudgetstat" runat="server"
                                                        Style="font-size: 12px; color: Blue" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="space20"></div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>

            <div class="row-fluid" runat="server" id="dvWorkOrder">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Work Order</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>

                        </div>

                        <div class="widget-body">

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->

                                <div class="form-horizontal">
                                    <asp:Panel ID="pnlApproval" runat="server">
                                        <div class="row-fluid">
                                            <div class="span1"></div>


                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Issued By<span class="Mandotary"> *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbIssuedBy" runat="server" TabIndex="3" Enabled="false">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtWOId" runat="server" Visible="false" Width="20px"
                                                                MaxLength="100"></asp:TextBox>
                                                            <asp:TextBox ID="txtDTCId" runat="server" Visible="false" Width="20px"
                                                                MaxLength="100"></asp:TextBox>
                                                            <asp:TextBox ID="txtTCId" runat="server" Visible="false" Width="20px"
                                                                MaxLength="100"></asp:TextBox>
                                                        </div> 
                                                    </div>
                                                </div>

                                                <div class="control-group" runat="server" id="dvSection" style="display:none">
                        <label class="control-label">Section<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSection" runat="server" TabIndex="3">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvRepairer" style="display: none">
                                                <label class="control-label">Repairer<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server" TabIndex="3">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="dvOFCheck" style="display: none">
                                                <p style="font-weight: 600">
                                                    <asp:CheckBox ID="ChkOFCheck" AutoPostBack="true" runat="server" OnCheckedChanged="ChkOFCheck_CheckedChanged" />Oil Filteration Required</p>
                                                <%--<asp:Label ID="lblOFCheck" runat="server" Text="Oil Filteration2 Required"></asp:Label>--%>
                                            </div>
                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA) <span class="Mandotary">*</span> </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCapacity" runat="server" Visible="false" Width="20px"
                                                            onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="4"></asp:TextBox>
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="4">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: none">
                                                <%--<label class="control-label" id ="lblDtcScheme" title="DTC SCHEME"> <span class="Mandotary"> *</span> </label>--%>
                                                <asp:Label ID="lblDtcScheme" CssClass="control-label" runat="server"> Nature of Work <span class="Mandotary"> *</span></asp:Label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDtc_Scheme_Type" runat="server" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="cmbDtc_Scheme_Type_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <%--<asp:LinkButton ID="LinkButton1" runat="server"
                                                Style="font-size: 12px; color: Blue" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>--%>

                                            <div class="control-group" style="display: none">
                                                <label class="control-label">Upload Document</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupWODocument" runat="server" AllowMultiple="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="space20"></div>
                                        <div class="space20"></div>
                                        <div class="row-fluid">


                                            <div class="span6" id="dvDecomm" runat="server">
                                                <!-- BEGIN BASIC PORTLET-->
                                                <div class="widget blue">
                                                    <div class="widget-title">
                                                        <h4><i class="icon-reorder"></i>Decommissioning</h4>
                                                        <span class="tools">
                                                            <%--                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>--%>
                                                        </span>
                                                    </div>
                                                    <div class="widget-body">
                                                        <div class="widget-body form">
                                                            <!-- BEGIN FORM-->
                                                            <div class="form-horizontal">
                                                                <div class="row-fluid">
                                                                    <div class="span5">
                                                                        <div class="control-group">
                                                                            <label class="control-label">Work Order No <span class="Mandotary">*</span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDeWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="65px" onkeypress="javascript:return AllowOnlyChar(event)"></asp:TextBox>
                                                                                     <asp:TextBox ID="txtDeWoNo2"  Text="-" runat="server" MaxLength="5" TabIndex="5" Width="45px" Enabled="false"></asp:TextBox>
                                                                      <%--              <asp:TextBox ID="txtDeWoNo2" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyChar(event)"></asp:TextBox>--%>
                                                                                    <asp:TextBox ID="txtDeWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="55px" onkeypress="javascript:return OnlyNumber(event)"></asp:TextBox>
                                                                                    <%-- <asp:TextBox ID="txtDeWoNo" runat="server" MaxLength="17" TabIndex="9"   ></asp:TextBox>    --%>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Date<span class="Mandotary">*</span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDeDate" runat="server" MaxLength="10" TabIndex="10" CssClass="auto-style1"></asp:TextBox>
                                                                                    <ajax:CalendarExtender ID="CalendarExtender_txtDeDate" runat="server" CssClass="cal_Theme1"
                                                                                        Format="dd/MM/yyyy" TargetControlID="txtDeDate">
                                                                                    </ajax:CalendarExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">Amount <span class="Mandotary">*</span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDeAmount" runat="server" MaxLength="10"
                                                                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="11"></asp:TextBox>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="control-group">
                                                                            <label class="control-label">A/C Code <span class="Mandotary">*</span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:TextBox ID="txtDecAccCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                        MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- END BASIC PORTLET-->
                                            </div>
                                        </div>

                                        <div class="space20"></div>

                                        <%--  </div>--%>
                                </div>
                                </asp:Panel>
                    <!-- END SAMPLE FORM PORTLET-->

                            </div>


                        </div>

                    </div>
                </div>

                <uc1:ApprovalHistoryView ID="ApprovalHistoryView" runat="server" />


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

                <div class="form-horizontal" align="center">

                    <div class="span3"></div>
                    <div class="span2">
                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                            CssClass="btn btn-primary"
                            OnClick="cmdSave_Click" TabIndex="13" />
                    </div>
                    <div class="span2">
                        <asp:Button ID="cmdViewEstimate" runat="server" Text="View Estimate"
                            CssClass="btn btn-primary"
                            OnClick="cmdViewEstimate_Click" TabIndex="13" />
                    </div>
                    <%-- <div class="span1"></div>--%>
                    <div class="span5">
                        <asp:Button ID="cmdReset" runat="server" Text="Reset" Visible="false"
                            CssClass="btn btn-primary" OnClick="cmdReset_Click" TabIndex="14" /><br />
                        <br />
                    </div>
                    <div class="span7"></div>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>

            </div>
        </div>
    </div>

    <!-- END PAGE CONTENT-->
    </div>


</asp:Content>

