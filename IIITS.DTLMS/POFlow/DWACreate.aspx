<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DWACreate.aspx.cs" Inherits="IIITS.DTLMS.POFlow.DWACreate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>

    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>

    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" />
<%--    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
<%--     <script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>

    

    
     



    <script src="../Scripts/functions.js" type="text/javascript"></script>
   


    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
              $('#<%=cmdExtend.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;

        function Address(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if ( // space
               !(code == 32) &&// Special characters
                !(code == 35) &&
                !(code == 64) &&
                !(code > 43 && code < 60) &&
                !(code > 37 && code < 42) &&
                 !(code == 47) &&
              !(code > 64 && code < 94) && // upper alpha (A-Z)
              !(code > 96 && code < 126)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }
        

       
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">F
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->

                    <h3 class="page-title" runat="server" id="Update">Detailed Work Award Creation
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
                    <asp:Button ID="cmdClose" runat="server" Text="View DWA Details"
                        OnClientClick="javascript:window.location.href='DWAView.aspx'; return false;"
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
                            <h4 id="UpdateRepairer" runat="server"><i class="icon-reorder"></i>Detailed Work Award Creation</h4>
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
                                                <label class="control-label">Choose Licenced Contractor<span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdndwaid" runat="server" />

                                                        <asp:DropDownList ID="cmdIsChooseLicencedContractor" runat="server" TabIndex="14"
                                                            AutoPostBack="true" OnTextChanged="GetContractorDetails">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Licence Number</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtLicenceNumber" runat="server" MaxLength="50" TabIndex="1"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">GST Number</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtGSTNumber" runat="server" MaxLength="50" TabIndex="1"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Address</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="200"
                                                            Style="resize: none" Height="60px" TextMode="MultiLine"
                                                            onkeypress="return Address(event,this);" TabIndex="13" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Licence Reg Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLicenceRegDate" runat="server" MaxLength="50" TabIndex="1"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Licence Valid Up To</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLicenceValidUpTo" runat="server" MaxLength="50" TabIndex="1"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">E-mail ID </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContractorEmailId" Style="width: 210px" runat="server"
                                                            MaxLength="50" onkeyup="Javascript:checkEmail()" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Contact Number</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="10"
                                                            TabIndex="1" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5" rowspan="2">

                                            <div class="control-group">
                                                <label class="control-label">DWA Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDWANumber" runat="server" MaxLength="20" TabIndex="6"
                                                            onkeypress="javascript:return AlphanumericLEC(event);"
                                                             AutoComplete="off">
                                                        </asp:TextBox>
                                                        <%--oncopy="return false" onpaste="return false" oncut="return false"--%>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DWA Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDWADate" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);" CssClass="auto-style1" OnTextChanged="loadDWA_Expire_Date"  AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtDWADate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDWADate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DWA Expire Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDWAPeriod" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);" CssClass="auto-style1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtDWAPeriod_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDWAPeriod" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" id="ExtendedDiv" runat="server">
                                                <label class="control-label">Extended UpTo<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtExtendedUpTo" runat="server" MaxLength="4" TabIndex="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);" CssClass="auto-style1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtExtendedUpTo_CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtExtendedUpTo" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Select Project<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmdIsChooseProject" runat="server" TabIndex="14"
                                                            AutoPostBack="true"  OnTextChanged="cmdIsChooseProject_SelectedIndexChanged"
                                                           >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            



                                             <div class="control-group" id="divProjectName" runat="server"  style="display:none">
                                                <label class="control-label">Enter Project Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtProject" runat="server" MaxLength="20"
                                                         AutoComplete="off" AutoPostBack="true" CssClass="auto-style1" 
                                                            onkeypress="return Address(event,this);" TabIndex="12" ReadOnly="false" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Name of the Work<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWork" runat="server" MaxLength="200"
                                                            onkeypress="return Address(event,this);" TabIndex="12"
                                                             AutoComplete="off">
                                                        </asp:TextBox>
                                                        <%--oncopy="return false" onpaste="return false" oncut="return false"--%>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    DWA Amount <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDWAAmt" runat="server" MaxLength="9"
                                                            onkeypress="javascript:return AllowNumber(this,event);"
                                                             AutoComplete="off"></asp:TextBox>
                                                        <%--oncopy="return false" onpaste="return false" oncut="return false"--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:Panel ID="pnlfile" runat="server">
                                                <div class="control-group" id="fileup1" runat="server">
                                                    <label class="control-label">Document<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:FileUpload ID="fupdDoc" runat="server" accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                            <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                            <asp:TextBox ID="txtdwafilepath" runat="server" Visible="false"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                        </div>
                                        <div class="span1"></div>
                                        <%--  <div id="mdlPopupContainer" runat="server">--%>

                                        <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                                            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                                        <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                                            <div style="display: none">
                                                <asp:Button ID="btnshow" runat="server" Text="Button" />
                                            </div>
                                            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Width="550px">
                                                <div class="widget blue">
                                                    <div class="widget-title">
                                                        <h4>Extend DWA Period </h4>
                                                        <div class="space20"></div>
                                                        <div class="span1"></div>
                                                        <div class="space20">
                                                            <div class="span1"></div>

                                                            <div class="span5">

                                                                <div class="control-group" style="font-weight: bold">
                                                                    <label class="control-label">
                                                                        Awarded Date
                                                                        <span class="Mandotary">*</span></label>

                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtAwardDate" runat="server" MaxLength="4"
                                                                                TabIndex="6"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"
                                                                                CssClass="auto-style1"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtAwardDate_CalendarExtender1"
                                                                                runat="server" CssClass="cal_Theme1"
                                                                                TargetControlID="txtAwardDate" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="control-group" style="font-weight: bold">
                                                                    <label class="control-label">
                                                                        Awarded Up-To
                                                                        <span class="Mandotary">*</span></label>

                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:TextBox ID="txtAward" runat="server" MaxLength="4" TabIndex="6"
                                                                                onkeypress="javascript:return AllowNumber(this,event);"
                                                                                CssClass="auto-style1"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="txtAward_CalendarExtender1" runat="server"
                                                                                CssClass="cal_Theme1"
                                                                                TargetControlID="txtAward" Format="dd/MM/yyyy">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div align="center">
                                                                    <div class="control-group" style="font-weight: bold">
                                                                        <label class="control-label">
                                                                            Extending Up-To
                                                                            <span class="Mandotary">*</span></label>
                                                                        <div class="controls">
                                                                            <div class="input-append">
                                                                                <asp:TextBox ID="txtExtend" runat="server" MaxLength="4"
                                                                                    TabIndex="6"
                                                                                    onkeypress="javascript:return AllowNumber(this,event);"
                                                                                    CssClass="auto-style1"></asp:TextBox>
                                                                                <asp:CalendarExtender ID="txtExtend_CalendarExtender1"
                                                                                    runat="server" CssClass="cal_Theme1"
                                                                                    TargetControlID="txtExtend" Format="dd/MM/yyyy">
                                                                                </asp:CalendarExtender>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="control-group" style="font-weight: bold">
                                                                    <label class="control-label">
                                                                        Document
                                                                        <span class="Mandotary">*</span></label>
                                                                    <div class="controls">
                                                                        <div class="input-append">
                                                                            <asp:FileUpload ID="DocumentUpload" runat="server"
                                                                                accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                                            <asp:Label ID="lblDocumentname" runat="server"
                                                                                Text="Initial Text"></asp:Label>
                                                                            <asp:TextBox ID="txtDocumnetpath" runat="server"
                                                                                Visible="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="span5">
                                                                    <div class="control-group" style="font-weight: bold">
                                                                        <div class="controls">
                                                                            <div class="input-append">
                                                                                <div class="span10">
                                                                                    <asp:Button ID="cmdCancel" runat="server" CssClass="btn btn-primary"
                                                                                        TabIndex="10" Text="Cancel" />
                                                                                </div>
                                                                                <div class="span1">
                                                                                    <asp:Button ID="cmdExtend" runat="server" CssClass="btn btn-primary"
                                                                                        TabIndex="10" Text="Save Extend DWA" OnClick="SaveExtendDwaclick" AutoPostBack="true" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="space20" align="center">

                                                                    <div class="form-horizontal" align="center">
                                                                        <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="space20"></div>
                                                        <div class="space20"></div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>

                                    </div>
                                </div>

                            </div>
                            <div class="space20"></div>
                            <div class="space20"></div>
                            <div class="container-fluid">
                                <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" 
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    OnPageIndexChanging="gvFiles_PageIndexChanging" OnSorting="gvFiles_Sorting" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <%--Text="VIEW"      Text="VIEW"               --%>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet"
                                            HeaderText="Download PO Documents" ItemStyle-Width="700" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue"
                                                    Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile"
                                                    CommandArgument='<%# Eval("Name") %>'> 
                                                </asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                                  
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue"
                                                       Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadFiledwnld"
                                                       CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="space20"></div>
                            <div class="space20"></div>
                            <div class="text-center">

                                <asp:Button ID="cmdExtendDwa" runat="server" Text="Extend DWA" CausesValidation="false"
                                    CssClass="btn btn-primary" OnClick="Showmodelpopup" AutoPostBack="true" />

                                <asp:Button ID="cmdSave" runat="server" Text="Save" CausesValidation="false"
                                    CssClass="btn btn-primary" OnClick="SaveAwardDetails" />

                                <asp:Button ID="cmdUpdate" runat="server" Text="Update" CausesValidation="false"
                                    CssClass="btn btn-primary" OnClick="SaveAwardDetails" />


                                <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                    CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />

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



        </div>
    </div>


    <!-- END PAGE CONTENT-->


</asp:Content>
