<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransRepairer.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TransRepairer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })
        });

    </script>
    <script type="text/javascript">
        function ValidateMyForm() {
            if (Page_ClientValidate()) {
                if (document.getElementById('<%= txtRepairName.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Repairer name')
                    document.getElementById('<%= txtRepairName.ClientID %>').focus()
                    return false
                }

                if (document.getElementById('<%= cmbDivision.ClientID %>').value == "-Select-") {
                    alert('Select Division Name')
                    document.getElementById('<%= cmbDivision.ClientID %>').focus()
                    return false
                }

                if (document.getElementById('<%= txtRepairPhnNo.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Repairer Phone No')
                    document.getElementById('<%= txtRepairPhnNo.ClientID %>').focus()
                    return false

                }

                if (document.getElementById('<%= txtRepairEmailId.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Repairer EmailId')
                    document.getElementById('<%= txtRepairEmailId.ClientID %>').focus()
                    return false
                    sea
                    if (document.getElementById('<%= txtRepairPhnNo.ClientID %>').value.trim() == "") {
                        alert('Please Enter 11 digit Valid Phone Number')
                        document.getElementById('<%= txtRepairPhnNo.ClientID %>').focus()
                        return false
                    }


                    if (document.getElementById('<%= txtRepairAddress.ClientID %>').value.trim() == "") {
                        alert('Enter Valid Register Address')
                        document.getElementById('<%= txtRepairAddress.ClientID %>').focus()
                        return false
                    }
                }
            }
        }


    </script>

    <script language="Javascript" type="text/javascript">


        function onlyAlphabets(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if ( // space
              !(code == 32) && // Special characters
                !(code > 39 && code < 42) &&
                !(code > 45 && code < 48) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }




    </script>

    <script language="javascript" type="text/javascript">
        function Address(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if ( // space
               !(code == 32) &&// Special characters
                !(code == 35) &&
                !(code == 64) &&
                !(code > 43 && code < 58) &&
                !(code > 39 && code < 42) &&
                 !(code == 47) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }
    </script>


    <style type="text/css">
        .auto-style1 {
            left: -3px;
            top: -5px;
        }

        .auto-style2 {
            left: -4px;
            top: -5px;
        }
    </style>


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
                    <h3 class="page-title" runat="server" id="Create">Repairer Details
                    </h3>
                    <h3 class="page-title" runat="server" id="Update">Update Repairer Details
                    </h3>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="Text1" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Repairer View"
                        OnClientClick="javascript:window.location.href='TransRepairerView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateRepairer" runat="server"><i class="icon-reorder"></i>Repairer Details</h4>
                            <h4 id="UpdateRepairer" runat="server"><i class="icon-reorder"></i>Update Repairer Details</h4>
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
                                                <label class="control-label">Repairer Name <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtRepairName" runat="server" MaxLength="50" TabIndex="1" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" TabIndex="2" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" MaxLength="50"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Phone Number
                                                    <br />
                                                    <span>(with STD Code)</span><span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRepairPhnNo" runat="server"
                                                            MaxLength="11"
                                                            onkeypress="javascript:return OnlyNumberHyphen(this,event);" TabIndex="3"></asp:TextBox>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Email Id <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--    <asp:TextBox ID="txtRepairEmailId" runat="server" MaxLength="50"
                                                            CausesValidation="True" TabIndex="4"></asp:TextBox>
                                                        </br>--%>
                                                        <%--                                      <asp:RegularExpressionValidator runat="server" ID="regular" ControlToValidate="txtRepairEmailId"
                                          ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
                                          ErrorMessage="Please enter a valid email id!!!!" ForeColor="Red"
                                          Display="Dynamic" Font-Size="Small" />--%>
                                                        <asp:TextBox ID="txtRepairEmailId" Style="width: 210px" runat="server" MaxLength="50" onkeyup="Javascript:checkEmail()"></asp:TextBox>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Contact Person Name <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContactPerson" onkeypress="return onlyAlphabets(event,this);" runat="server" MaxLength="50" TabIndex="5"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Mobile Number <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10"
                                                            onkeypress="javascript:return OnlyNumber(event);" TabIndex="8"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Fax No </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="20" TabIndex="9" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Contract Period(in Months) </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContractPeriod" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DWA NO </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDwaNO" runat="server" MaxLength="50" TabIndex="6" CssClass="auto-style2"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DWA Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDwaDate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtDwaDate_CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDwaDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5" rowspan="2">

                                            <div class="control-group">
                                                <label class="control-label">Contract Start Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtStartDate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtStartDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtStartDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Contract End Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEndDate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);" CssClass="auto-style1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtEndDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtEndDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Black Listed <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbIsBlack" runat="server"
                                                            OnSelectedIndexChanged="cmbIsBlack_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="10">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">BlackListed Upto<span class="Mandotary" runat="server" id="blocklist" visible="false"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtBlackUpto" runat="server" MaxLength="10" TabIndex="11"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtdateExtender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtBlackUpto" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Register Address<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRepairerId" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txthdOfficeCode" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>

                                                        <asp:TextBox ID="txtRepairAddress" runat="server" MaxLength="500" Height="60px"
                                                            TextMode="MultiLine" Style="resize: none"
                                                            onkeypress="return Address(event,this);" TabIndex="12"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Communication  Address </label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtCommAddress" runat="server" MaxLength="200"
                                                            Style="resize: none" Height="60px" TextMode="MultiLine"
                                                            onkeypress="return Address(event,this);" TabIndex="13"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks <%--<span class="Mandotary"> *</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Style="resize: none" MaxLength="200"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Repairer Type<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmdIsRepairerType" runat="server"
                                                            OnSelectedIndexChanged="cmbIsBlack_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="10">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                            <asp:ListItem Value="1">Mega Repairer</asp:ListItem>
                                                            <asp:ListItem Value="0">Local Repairer</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" id="fileup1" runat="server">
                                                <label class="control-label">Repairer Award upload<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupdDoc" runat="server" accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                        <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <asp:CheckBox ID="chkExtension" margin="Top:12px" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="chkExtension_CheckedChanged" />
                                                   <asp:Label ID="Lbl" runat="server" Visible="false"
                                                    Text="Tick For Extension"></asp:Label>
                                               <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                 <%--  <asp:ImageButton ID="ImageButton_new" runat="server" />--%>
                                                <asp:LinkButton ID="lnkDownload22" runat="server" Visible="false" ForeColor="Blue" Text="<i class='icon-download-alt'></i> DOWNLOAD DOCUMENT" OnClick="DownloadFiledwnld1"
                                                       CommandArgument='<%# Eval("Name") %>'> </asp:LinkButton>
                                                <asp:TextBox ID="txtRepawardpath" runat="server" Visible="false"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center">

                                        <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click" CausesValidation="false"
                                            OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />

                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                            OnClick="cmdReset_Click" CssClass="btn btn-danger" /><br />

                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->



                        </div>
                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->

                    <div class="widget blue" id="DivExtension" runat="server" style="display: none">
                        <div class="widget-title">
                            <h4 id="H2" runat="server"><i class="icon-reorder"></i>Extension Record</h4>
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
                                                <label class="control-label">Extension OM</label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtExtOM" runat="server" MaxLength="50" TabIndex="1" onkeypress="javascript:return AllowSpecialchar(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Extension Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtExtDate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtExtDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtExtDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Extension Period(in Months) </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtExtPeriod" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>




                                        </div>
                                        <div class="span5" rowspan="2">

                                            <div class="control-group">
                                                <label class="control-label">Extension Start Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtExtStartDate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtExtStartDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtExtStartDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Extension End Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtExtEndDate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtExtEndDate_CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtExtEndDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="form-horizontal" align="center">
                                        <div class="span3"></div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span1">
                                            <asp:Button ID="btnExtReset" OnClick="btnExtReset_Click" runat="server" Text="Reset" CausesValidation="false" CssClass="btn btn-primary" /><br />
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->



                        </div>
                    </div>

                </div>
            </div>


            <!-- END PAGE CONTENT-->
        </div>

        <asp:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup" CancelControlID="cmdClose"
            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
        <div style="width: 100%; vertical-align: middle" align="center">



            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="550px" Width="500px">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>Select Office Codes And Click On Proceed</h4>
                        <div class="space20"></div>


                        <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%"
                            AllowPaging="True" DataKeyNames="OFF_CODE" OnPageIndexChanging="GrdOffices_PageIndexChanging" OnRowCommand="GrdOffices_RowCommand" OnRowDataBound="GrdOffices_RowDataBound">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Office Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStaDesc" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all" Width="150px"> </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Office Name" Width="200px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="row-fluid">
                            <div class="span1"></div>
                            <div class="span2">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span2">

                                <div class="control-group">

                                    <div class="controls">
                                        <div class="input-append">
                                            <%--onclick="btnClose_Click"--%>
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Cancel" />

                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>

        </div>

    </div>

</asp:Content>
