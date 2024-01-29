<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="WorkOrder.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.WorkOrder" %>

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

            //         if (document.getElementById('<%= txtType.ClientID %>').value.trim() != "3") {
            //             if (document.getEleme
            ntById('<%= txtFailureId.ClientID %>').value.trim() == "") {
                //                 alert('Enter  Failure Id')
                //                 document.getElementById('<%= txtFailureId.ClientID %>').focus()
                //                 return false
                //             }
                //         }
                //         if (document.getElementById('<%= txtFailureDate.ClientID %>').value == "") {
                //             alert('Enter Failure Date')
                //             document.getElementById('<%= txtFailureDate.ClientID %>').focus()
                //             return false
                //         }
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

                //         if (document.getElementById('<%= txtComWoNo1.ClientID %>').value.trim() == "") {
                //             alert('Enter Commissioning WO Number')
                //             document.getElementById('<%= txtComWoNo1.ClientID %>').focus()
                //             return false
                //         }


                if (document.getElementById('<%= txtCommdate.ClientID %>').value.trim() == "") {
                    alert('Enter Commissioning Date')
                    document.getElementById('<%= txtCommdate.ClientID %>').focus()
                    return false
                }


                if (document.getElementById('<%= txtCommAmount.ClientID %>').value.trim() == "") {
                    alert('Enter Commissioning Amount')
                    document.getElementById('<%= txtCommAmount.ClientID %>').focus()
                    return false
                }
                if (document.getElementById('<%= txtAcCode.ClientID %>').value.trim() == "") {
                    alert('Select Commissioning Account Code')
                    document.getElementById('<%= txtAcCode.ClientID %>').focus()
                    return false
                }

                //         if (document.getElementById('<%= txtDeWoNo1.ClientID %>').value.trim() == "") {
                //             alert('Enter DeCommissioning Wo No')
                //             document.getElementById('<%= txtDeWoNo1.ClientID %>').focus()
                //             return false
                //         }

                if (document.getElementById('<%= txtDeDate.ClientID %>').value.trim() == "") {
                    alert('Enter DeCommissioning Date')
                    document.getElementById('<%= txtDeDate.ClientID %>').focus()
                    return false
                }
<%--                if (document.getElementById('<%= txtDeCrDate.ClientID %>').value.trim() == "") {
                    alert('Enter Credited Date')
                    document.getElementById('<%= txtDeCrDate.ClientID %>').focus()
                    return false
                }--%>
<%--                if (document.getElementById('<%= txtDeAmount.ClientID %>').value.trim() == "") {
                    alert('Enter DeCommissioning amount')
                    document.getElementById('<%= txtDeAmount.ClientID %>').focus()
                    return false
                }--%>
<%--                if (document.getElementById('<%= txtDeCrAmount.ClientID %>').value.trim() == "") {
                    alert('Enter Credited amount')
                    document.getElementById('<%= txtDeCrAmount.ClientID %>').focus()
                    return false
                }--%>
                if (document.getElementById('<%= txtDecAccCode.ClientID %>').value.trim() == "") {
                    alert('Select DeCommissioning Account Code')
                    document.getElementById('<%= txtDecAccCode.ClientID %>').focus()
                    return false
                }

<%--                if (document.getElementById('<%= txtDeCrAccCode.ClientID %>').value.trim() == "") {
                    alert('Select Credited Account Code')
                    document.getElementById('<%= txtDeCrAccCode.ClientID %>').focus()
                    return false
                }--%>

                var FromdateInput = document.getElementById('<%= txtCommdate.ClientID %>').value;
                var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
                if (!FromdateInput.match(goodDate)) {
                    alert("Please enter valid Commission WO date");
                    document.getElementById('<%= txtCommdate.ClientID %>').focus()
                    return false;
                }

                var FromdateInput = document.getElementById('<%= txtDeDate.ClientID %>').value;
                var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
                if (!FromdateInput.match(goodDate)) {
                    alert("Please enter valid DeCommission WO date");
                    document.getElementById('<%= txtDeDate.ClientID %>').focus()
                    return false;
                }

<%--                var FromdateInput = document.getElementById('<%= txtDeCrDate.ClientID %>').value;
                var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
                if (!FromdateInput.match(goodDate)) {
                    alert("Please enter valid Credited WO date");
                    document.getElementById('<%= txtDeCrDate.ClientID %>').focus()
                    return false;
                }--%>

<%--                var FromdateInput = document.getElementById('<%= txtOFDate.ClientID %>').value;
                var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
                if (!FromdateInput.match(goodDate)) {
                    alert("Please enter valid OilFilter WO date");
                    document.getElementById('<%= txtOFDate.ClientID %>').focus()
                    return false;
                }--%>

                var FromdateInput = document.getElementById('<%= txtCreditDate.ClientID %>').value;
                var goodDate = /^(0[1-9]|[12][0-9]|3[01])[\- \/.](?:(0[1-9]|1[012])[\- \/.](19|20)[0-9]{2})$/;
                if (!FromdateInput.match(goodDate)) {
                    alert("Please enter valid Credit WO date");
                    document.getElementById('<%= txtCreditDate.ClientID %>').focus()
                    return false;
                }

            }
        }
    </script>

          <script language="Javascript" type="text/javascript">
              function onlyAlphabets(e, t) {
                  var code = ('charCode' in e) ? e.charCode : e.keyCode;
                  if (  !(code > 64 && code < 91) && // upper alpha (A-Z)
                    !(code > 96 && code < 123)) { // lower alpha (a-z)
                      e.preventDefault();
                  }
              }

              function Numbers(e, t) {
                  var code = ('charCode' in e) ? e.charCode : e.keyCode;
                  if ( !(code > 47 && code < 58)) { 
                      e.preventDefault();
                  }
              }
             </script>

   <%--  <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtWOdate.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })
             });
        </script>--%>
    <style type="text/css">
        .auto-style1 {
            left: -4px;
            top: -5px;
        }

        input#ContentPlaceHolder1_txtComWoNo1 {
            /*width: 45px !important;*/
            width: 80px !important;
        }

        input#ContentPlaceHolder1_txtComWoNo2 {
            width: 80px !important;
              text-align: center;
        }txtDeCrWoNo2

        input#ContentPlaceHolder1_txtComWoNo3 {
            /*width: 45px !important;*/
            width: 80px !important;
        }

        input#ContentPlaceHolder1_txtDeWoNo1 {
            /*width: 45px !important;*/
            width: 50px !important;
        }

        input#ContentPlaceHolder1_txtDeWoNo2 {
            width: 60px !important;
            text-align: center;
        }

        input#ContentPlaceHolder1_txtDeWoNo3 {
            /*width: 45px !important;*/
             width: 80px !important;
        }

        input#ContentPlaceHolder1_txtOFWoNo1 {
            /*width: 45px !important;*/
            width: 50px !important;
        }

        input#ContentPlaceHolder1_txtOFWoNo2 {
            width: 45px !important;
        }

        input#ContentPlaceHolder1_txtOFWoNo3 {
            width: 45px !important;
        }
        input#ContentPlaceHolder1_txtDeCrWoNo2 {
            width: 45px !important;
            text-align: center;
        }
        label.control-label {
            font-size:13px!important;
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
                    <h3 class="page-title">Work Order</h3>
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


            <!-- END PAGE HEADER-->

            <!-- BEGIN PAGE CONTENT-->
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
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure Id"></asp:Label>
                                                    <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        sWFOAutId
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                          <asp:HiddenField ID="hdnewfailid" runat="server" />
                                                        <asp:TextBox ID="txtFailureId" runat="server"
                                                            onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="1"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"
                                                            OnClick="cmdSearch_Click" TabIndex="2" />

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
                                                         <asp:HiddenField ID="hdfboid" runat="server" />
                                                         <asp:HiddenField ID="hdfoilqtytank" runat="server" />
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                       <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtFailType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue;" OnClick="lnkDTCDetails_Click">View Transformer Centre Details</asp:LinkButton>
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
                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="lblDateText" runat="server" Text="Failure Date"></asp:Label>
                                                </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFailureDate" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
<%--                                             <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="Label1" runat="server" Text="PGRS Code"></asp:Label>
                                                </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPGRS" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        
                                                    </div>
                                                </div>
                                            </div>--%>

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
                                                        Style="font-size: 12px; color: Blue;cursor:pointer" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
                                            <div class="space20"></div>
                                   <div class="container-fluid">
                                              <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false" 
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"   
                                                      OnPageIndexChanging="gvFiles_PageIndexChanging" OnSorting="gvFiles_Sorting" AllowSorting="true">
                                                 <Columns>
         <%--                                         <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                   <ItemTemplate>
                                                       <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                      <%--Text="VIEW"      Text="VIEW"               --%>
                                                 <%--</asp:TemplateField>--%>
                                                  <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Transformer Survey Report" ItemStyle-Width="300"   />     
                                                 <asp:TemplateField>
                                               <ItemTemplate>
                                           <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Width="150"  Text="<i class='icon-eye-open'></i> VIEW"   OnClick="DownloadFile"
                                              CommandArgument='<%# Eval("Name") %>'> 
                                           </asp:LinkButton>
                                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                  
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" width="150" Text="<i class='icon-download-alt'></i> DOWNLOAD"   OnClick="DownloadFiledwnld"
                                              CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
              

                                      </Columns>
                                      </asp:GridView>
                                   </div>
            <!-- END PAGE CONTENT-->

            <!-- BEGIN PAGE CONTENT-->
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

                                                <div class="control-group" runat="server" id="dvSection" style="display: none">
                                                    <label class="control-label">Section<span class="Mandotary"> *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="cmdflowType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                  <div class="control-group" runat="server" id="dvTypeofflow" style="display: none">
                                                    <label class="control-label">Select Type<span class="Mandotary" > *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmdflowType" runat="server" TabIndex="3"  AutoPostBack="true" OnSelectedIndexChanged="cmdflowType_SelectedIndexChanged">                                                     
                                                               <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                <asp:ListItem Value="1">PTK</asp:ListItem>
                                                                <asp:ListItem Value="2">TTK</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    

                                                    <div class="controls">
                                                        <div class="input-append">
                                                             <label class="control-label" style="color:red;" >Note:</label>
                                                          <label class="control-label"style="color:red;" >1 ->PTK to Transformer Supplied by HESCOM (PTK)</label>
                                                          <label class="control-label" style="color:red;">2 ->TTK to Transformer Supplied By Vendor (TTK)</label>
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

<%--                                                <div runat="server" id="dvOFCheck" style="display: none">
                                                    <p style="font-weight: 600">
                                                        <asp:CheckBox ID="ChkOFCheck" AutoPostBack="true" runat="server" OnCheckedChanged="ChkOFCheck_CheckedChanged" />Oil Filteration Required</p>
                                                    <%--<asp:Label ID="lblOFCheck" runat="server" Text="Oil Filteration2 Required"></asp:Label>--%>
                                                </div>
                                                <div runat="server" id="dvCreditWO" style="display: none">
                                                    <p style="font-weight: 600">
                                                        <asp:CheckBox ID="ChkCredit" AutoPostBack="true" runat="server" OnCheckedChanged="ChkCredit_CheckedChanged" />Credit Work Order</p>
                                                </div>
                                            </div>

                                            <div class="span5">

                                                                  
                                                <div class="control-group">
                                                    <%--<label class="control-label" id ="lblDtcScheme" title="DTC SCHEME"> <span class="Mandotary"> *</span> </label>--%>
                                                    <asp:Label ID="lblDtcScheme" CssClass="control-label" runat="server"> Nature of Work <span class="Mandotary"> *</span></asp:Label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDtc_Scheme_Type" runat="server" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="cmbDtc_Scheme_Type_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </div>
                                                         <br />
                                                        
                                                       <%-- <a href="/MasterForms/BudgetStatus.aspx" style="font-size: 12px; color: Blue;cursor:pointer">View Budget Status</a>--%>
                                                    <%--<asp:LinkButton ID="lnkBudgetstat2" runat="server"
                                                        Style="font-size: 12px; color: Blue;cursor:pointer" OnClick="lnkBudgetstat_Click" >View Budget Status</asp:LinkButton>--%>
                                                    </div>
                                                </div>
                                                 <div runat="server" id="dvPGRSminor" style="display: none">
                                                    <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="Label2" runat="server" Text="PGRS Code"></asp:Label>
                                                </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPGRSminor" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>                                                    
                                                    </div>
                                                </div>
                                            </div>
                                             </div>
                                                <%--<asp:LinkButton ID="LinkButton1" runat="server"  
                                    style="font-size:12px;color:Blue" onclick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>--%>

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

                                                <div class="span6" runat="server" id="dvComm">
                                                    <!-- BEGIN BASIC PORTLET-->
                                                    <div class="widget blue">
                                                        <div class="widget-title">
                                                            <h4><i class="icon-reorder"></i>Commissioning</h4>
                                                            <span class="tools"></span>
                                                        </div>
                                                        <div class="widget-body">
                                                            <div class="widget-body form">
                                                                <!-- BEGIN FORM-->
                                                                <div class="form-horizontal">
                                                                    <div class="row-fluid">
                                                                        <div class="span5">
                                                                            <div class="control-group">
                                                                                <label class="control-label">Work Order No  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtComWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="80px" Enabled="false" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>                       
                                                                                        <asp:TextBox ID="txtComWoNo2"  runat="server" MaxLength="5" TabIndex="5" Width="80px" Enabled="false"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtComWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="80px" Enabled="false" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                                                         <asp:TextBox ID="txtComWoNo4" runat="server" MaxLength="6" TabIndex="5" Width="80px" Enabled="false" onkeypress="return Numbers(event,this);"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>


                                                                            <div class="control-group">
                                                                                <label class="control-label">Date <span class="Mandotary">*</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCommdate" runat="server" MaxLength="10" TabIndex="6" CssClass="auto-style1"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender_txtCommdate" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtCommdate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Amount  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCommAmount" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                                                            MaxLength="6" TabIndex="7"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtAcCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="15" TabIndex="8" ReadOnly="true"></asp:TextBox>
                                                                                        <asp:DropDownList ID="cmbAcCode" runat="server" Visible="false" >
                                                                                        </asp:DropDownList> <label id="lblACcodedesc"  runat="server"  class="Mandotary"  visible="false">Note : * Please select the right account code for <br /> Commissioning and Decommissioning.</label>
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
                                                                        <div class="span12">
                                                                            <div class="control-group">
                                                                                <label class="control-label"> Decommission Work Order No <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                      <%--  <asp:TextBox ID="txtDeWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharNotanySpecial(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeWoNo2" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharHyphen(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="45px" onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>--%>
                                                                                        
                                                                                          <asp:TextBox ID="txtDeWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="45px" Enabled="false" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                                                      <asp:TextBox ID="txtDeWoNo2" runat="server" MaxLength="6" TabIndex="5" Width="60px" Enabled="false" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="45px" Enabled="false" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeWoNo4" runat="server" MaxLength="6" TabIndex="5" Width="45px" Enabled="false" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                                                       
                                                                                         <%-- <asp:TextBox ID="txtDeWoNo" runat="server" MaxLength="17" TabIndex="9"   ></asp:TextBox>    --%>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

<%--                                                                             <div class="control-group">
                                                                                <label class="control-label"> Credit Work Order No <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeCrWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="50px" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeCrWoNo2" runat="server" Text="-" MaxLength="5" TabIndex="5" Width="45px"  Enabled="false"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeCrWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="80px" onkeypress="return Numbers(event,this);"></asp:TextBox>
                                                                                     
                                                                                    </div>
                                                                                </div>
                                                                            </div>--%>

                                                                            <div class="control-group">
                                                                                <label class="control-label"> Decommission Date  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeDate" runat="server" MaxLength="10" TabIndex="10" CssClass="auto-style1"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender_txtDeDate" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtDeDate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

<%--                                                                             <div class="control-group">
                                                                                <label class="control-label">Credit Date  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeCrDate" runat="server" MaxLength="10" TabIndex="10" CssClass="auto-style1"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender_txtDeCrDate" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtDeCrDate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>--%>

                                                                            <div class="control-group">
                                                                                <label class="control-label"> Decommission Amount <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeAmount" runat="server" MaxLength="6"
                                                                                            onkeypress="javascript:return AllowNumber(this,event);" TabIndex="11"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>
<%--                                                                            <div class="control-group">
                                                                                <label class="control-label"> Credit Amount <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeCrAmount" runat="server" MaxLength="6"
                                                                                            onkeypress="javascript:return AllowNumber(this,event);" TabIndex="11"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>--%>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Decommission A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDecAccCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>

<%--                                                                             <div class="control-group">
                                                                                <label class="control-label">Credit A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeCrAccCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>--%>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- END BASIC PORTLET-->
                                                </div>

<%--                                                <div class="span6" runat="server" id="dvOilFileration">
                                                    <!-- BEGIN BASIC PORTLET-->
                                                    <div class="widget blue">
                                                        <div class="widget-title">
                                                            <h4><i class="icon-reorder"></i>Oil Filteration</h4>
                                                            <span class="tools">
                                                            </span>
                                                        </div>
                                                        <div class="widget-body">
                                                            <div class="widget-body form">
                                                                <!-- BEGIN FORM-->
                                                                <div class="form-horizontal">
                                                                    <div class="row-fluid">
                                                                        <div class="span5">
                                                                            <div class="control-group">
                                                                                <label class="control-label">Work Order No  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtOFWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="50px" onkeypress="javascript:return AllowOnlyCharNotanySpecial(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtOFWoNo2" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharHyphen(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtOFWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="45px" onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Date <span class="Mandotary">*</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtOFDate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtOFDate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Amount  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtOFAmount" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                                                            MaxLength="6" TabIndex="7"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtOFAccCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="6" TabIndex="8" ReadOnly="true"></asp:TextBox>

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
                                                </div>--%>
                                                     <div class="span6" runat="server" id="dvTTKorPTKflow">
                                                    <!-- BEGIN BASIC PORTLET-->
                                                    <div class="widget blue">
                                                        <div class="widget-title">
                                                           <%-- <h4><i class="icon-reorder"></i>TTK Flow </h4>--%>
                                                                <h4   runat="server" id="tltttk">TTK Flow          
                                                                    </h4>
                                                                
                                                                 <h4   runat="server" id="tltptk">PTK Flow                                    
                                                                </h4>
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
                                                                                <label class="control-label">Work Order No<span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtTTKWO1" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharNotanySpecial(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtTTKWO2" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharHyphen(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtTTKWO3" runat="server" MaxLength="6" TabIndex="5" Width="45px" onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="control-group">
                                                                                <label class="control-label">Date <span class="Mandotary">*</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtWOdate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtWOdate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                             <div id="dvttk" runat="server" visible="false">
                                                                              <div class="control-group">
                                                                                <label class="control-label">TTK Auto Generated<span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtttkAuto" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>                                                             
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                                 <div class="control-group">
                                                                                <label class="control-label">Vendor Name<span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtVendor" runat="server" MaxLength="50"></asp:TextBox>                                                             
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            </div> 
                                                                         <br />
                                                                            <div class="control-group">
                                                                                <label class="control-label">DWA Name </label>
                                                                                <div class="controls"> 
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDWAName" runat="server" MaxLength="50"></asp:TextBox>                                                             
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                             <div class="control-group">
                                                                                <label class="control-label">DWA Date </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDWADate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtDWADate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            
                                                                                <div class="control-group">
                                                                                <label class="control-label">Amount<span class="Mandotary"> *</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtTTKAmount" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                                                            MaxLength="6" TabIndex="7"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>  
                                                                             <div class="control-group">
                                                                                 <label class="control-label">Star Rate<span class="Mandotary">*</span></label>
                                                                             <div class="controls">
                                                                             <div class="input-append">
                                                                             <asp:DropDownList ID="cmbRating" runat="server" TabIndex="15"                                                            >
                                                                             </asp:DropDownList>
                                                                       
                                                                           </div>
                                                                          </div>
                                                                        </div>  
                                                                             <div class="control-group">
                                                                                <label class="control-label">A/C Code<span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtACCcode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="15" TabIndex="8" ReadOnly="true"></asp:TextBox>                                                       
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

                                                <div class="span6" runat="server" id="divCreditWO" style="visibility:hidden">
                                                    <!-- BEGIN BASIC PORTLET-->
                                                    <div class="widget blue">
                                                        <div class="widget-title">
                                                            <h4><i class="icon-reorder"></i>Credit Work Order</h4>
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
                                                                                <label class="control-label">Credit Work Order No <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCreditWO1" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharNotanySpecial(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtCreditWO2" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return AllowOnlyCharHyphen(event)"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtCreditWO3" runat="server" MaxLength="6" TabIndex="5" Width="45px" onkeypress="javascript:return isAlphaNumeric(event)"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Date <span class="Mandotary">*</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCreditDate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtCreditDate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Amount<span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCreditAmount" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                                                            MaxLength="6" TabIndex="7"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCreditACCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="15" TabIndex="8" ReadOnly="true"></asp:TextBox>

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

                    <div class="text-center" align="center">

                       
                      
                            <asp:Button ID="cmdSave" runat="server" Text="Save"
                                CssClass="btn btn-primary"
                                OnClick="cmdSave_Click" TabIndex="13" />
                       
                            <asp:Button ID="cmdViewEstimate" runat="server" Text="View Estimate"
                                CssClass="btn btn-primary"
                                OnClick="cmdViewEstimate_Click" TabIndex="13" />
                       
                            <asp:Button ID="cmdReset" runat="server" Text="Reset" Visible="true"
                                CssClass="btn btn-danger" OnClick="cmdReset_Click" TabIndex="14" /><br />
                          
                       
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div><br />


                </div>
            </div>
        </div>

        <!-- END PAGE CONTENT-->
    </div>


</asp:Content>
