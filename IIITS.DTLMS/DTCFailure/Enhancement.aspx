<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Enhancement.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.Enhancement" %>

<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {

            if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                alert('Select Valid Transformer Centre Code')
                document.getElementById('<%= txtDTCCode.ClientID %>').focus()
              return false
          }
            //          if (document.getElementById('<%= txtEnhanceDate.ClientID %>').value.trim() == "") {
            //              alert('Select the Enhancement date')
            //              document.getElementById('<%= txtEnhanceDate.ClientID %>').focus()
            //              return false
            //          }
            if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "--Select--") {
                alert('Select Enhanced Capacity')
                document.getElementById('<%= cmbCapacity.ClientID %>').focus()
              return false
          }

          if (document.getElementById('<%= txtReason.ClientID %>').value.trim() == "") {
                alert('Enter the Enhancement Reason')
                document.getElementById('<%= txtReason.ClientID %>').focus()
              return false
          }



            //          if (document.getElementById('<%= txtDTCRead.ClientID %>').value == "") {
            //              alert('Enter the KWH reading')
            //              document.getElementById('<%= txtDTCRead.ClientID %>').focus()
            //              return false
            //          }

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
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Declare Capacity Enhancement 
                    </h3>
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
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Declare Enhancement</h4>
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

                                                    <label class="control-label">Transformer Centre Code<span class="Mandotary">*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="9"></asp:TextBox>
                                                            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"  visible="false"
                                                                OnClick="cmdSearch_Click" /><br />
                                                            <asp:LinkButton ID="lnkDTCDetails" runat="server"
                                                                Style="font-size: 12px; color: Blue" OnClick="lnkDTCDetails_Click">View Transformer Centre Details</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>



                                                <div class="control-group">
                                                    <label class="control-label">Transformer Centre Name </label>


                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                            <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                            <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                            <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                            <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                            <asp:TextBox ID="txtDTCName" runat="server" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>



                                                <div class="control-group">
                                                    <label class="control-label">Service Date </label>


                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                            <asp:HiddenField ID="hdfEnhanceOffcode" runat="server" />
                                                            <asp:TextBox ID="txtServiceDate" runat="server" ReadOnly="true"
                                                                MaxLength="10"></asp:TextBox>


                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Load KW  </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtLoadKW" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Load Hp </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtLoadHP" runat="server" ReadOnly="true" onkeypress="return OnlyNumber(event)" MaxLength="10"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>



                                                <div class="control-group">
                                                    <label class="control-label">Transformer Centre Commission Date </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtConnectionDate" runat="server" MaxLength="10" ReadOnly="true" onkeypress="javascript:return AllowNumber(event);"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">DTr Commission Date</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtDTrCommDate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                            <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" 
                                    TargetControlID="txtDTrCommDate" Format="dd/MM/yyyy"></asp:CalendarExtender>--%>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="control-group">
                                                    <label class="control-label">Capacity to be Enhanced</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtCapacity" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <br />
                                                <div class="control-group">
                                                    <label class="control-label">Reason  <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtReason" runat="server" Width="600px" Height="100px" TextMode="MultiLine"
                                                                Style="resize: none" onkeyup="return ValidateTextlimit(this,5000);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                            

                                            </div>

                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Section  </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtLocation" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfTCId" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">DTr Code </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTcCode" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox><br />
                                                            <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>

                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:Panel ID="pnlApproval" runat="server">
                                                <div class="control-group">
                                                    <label class="control-label">DTr Make </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTCMake" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">DTr Serial Number </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTCSlno" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Capacity Enhanced(in KVA)<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="4"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group" style="display: none">
                                                    <label class="control-label">Enhancement Date <span class="Mandotary">*</span> </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtEnhanceDate" runat="server" ReadOnly="true" MaxLength="10"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtEnhanceDate" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>                                               
                                                <div class="control-group">
                                                    <label class="control-label">Transformer Centre Reading </label>

                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtDTCRead" runat="server" MaxLength="10"
                                                                onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                     <div class="control-group">
                                                    <label class="control-label">Work Name</label>

                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtwork" runat="server" TextMode="MultiLine" style="width: 402px; height: 41px; resize: none" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                    <div class="control-group" id="DivUpload" runat="server">
                                                        <label class="control-label">Upload LTVR File<span class="Mandotary">*</span> </label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:FileUpload ID="fupLTVR" runat="server" />
                                                                <asp:HiddenField ID="HdfLTRVpath" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    </asp:Panel>
                                                        
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtDtcId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtEnhancementId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    

                                    <div class="space5" runat="server" id="DivDownload" visible="false">
                                    <asp:LinkButton ID="lnkDwnld"  runat="server" OnClick="lnkDwnld_Click">
                                            <img alt="" src="../img/Manual/Pdficon.png" style="width:20px" />Download LTVR Report</asp:LinkButton>
                                                                                                        
                                                <asp:HiddenField ID="hdfLTVRPath" runat="server" />
                                        </div>
                                </div>

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

                <div class="text-center">

                 
                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                            OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary"
                            OnClick="cmdSave_Click" />
                  
                   
                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                            CssClass="btn btn-danger" OnClick="cmdReset_Click" />

                
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                </div><br />

            </div>
        </div>
    </div>
</asp:Content>
