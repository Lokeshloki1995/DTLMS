<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Transiloil.aspx.cs" Inherits="IIITS.DTLMS.WorkAward.Transiloil" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script src="../js/jquery-OneEightTwo.min.js" type="text/javascript"></script>

 <script src="jquery.printElement.js" type="text/javascript"></script>
<script type="text/javascript">
     function printpage() {
        $("#MainContent_lblName").html($("#MainContent_TextBox1").val());
        $("#MainContent_lblSchool").html($("#MainContent_TextBox2").val());
        $("#printable").printElement();
    }

</script>
    <style type="text/css">
#printable { display: none; }
@media print
 {
     #nonprintable { display: none; }
     #printable { display: block; }
 }

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    @{Html.BeginForm("CreateDocument", "Home", FormMethod.Get);
{


    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Create Transiloil
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
                    <asp:Button ID="cmdClose"  runat="server" Text="Close" CssClass="btn btn-primary"  OnClientClick="javascript:window.location.href='/Approval/ApprovalInbox.aspx'; return false;"  />
                </div>
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>WorkAward Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                       
                                      <%--   <div id="printable">--%>

                                        <div class="span6">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Work Award No</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                       
                                                        <asp:TextBox ID="txtWOANo" ReadOnly="true" runat="server"></asp:TextBox>



                                                         <asp:TextBox ID="txtUrl" runat="server" Text = ""  Visible="false" />
                                                        <asp:TextBox ID="txtWOAId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtActionType" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtWOId" runat="server" TabIndex="1" Visible="false"></asp:TextBox>
                                                         <asp:HiddenField ID="hdfEstNo" runat="server" />
                                                         <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                         <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                         <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                             </div>
                                            <div class="span6">
                                                <div class="control-group">
                                                    <label style="width: 41%!important;margin-bottom: 20px;"class="control-label">
                                                        Actual Tank Capaity/workaward</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtactualoil" ReadOnly="true" runat="server"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                       

                                        <div style="margin-left: -1px;"class="span6">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Work Award Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWOADate"  ReadOnly="true" runat="server"></asp:TextBox>
                                                       <%-- <asp:CalendarExtender ID="WOACalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtWOADate">
                                                        </asp:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="space20"></div>
                                        <div class="span11">
                                        </div>
                                        <div class="space20"></div>

                                    </div>

                                    <div class="space20"></div>


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
                                <i class="icon-reorder"></i>Oil Details</h4>
                             <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <div class="form-horizontal">
                                         <asp:Panel ID="pnlApproval" runat="server" Enabled="true">
                                        <div class="row-fluid">

                                            <div class="span1"></div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                       Num Of Barrels<span class='Mandotary'>*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtbarrel" MaxLength="10" onkeypress="javascript:return OnlyNumber(event)" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                 <div class="control-group">
                                                <label class="control-label">
                                                    Oil Issued Date<span class='Mandotary'>*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtissuedate"  runat="server"></asp:TextBox>
                                                       <asp:CalendarExtender ID="txtissuedateid" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtissuedate">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                                  <div class="control-group">
                                                    <label class="control-label">
                                                       Material folio<span class='Mandotary'>*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtmtrialf0lio" MaxLength="10" onkeypress="javascript:return OnlyNumber(event)" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                                  <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                       Total Oil(in ltr's)<span class='Mandotary'>*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtoil" MaxLength="10" onkeypress="javascript:return OnlyNumber(event)" runat="server"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                       <div class="control-group">
                                                    <label id="lbloiltype" class="control-label">Select Oil Type <span class='Mandotary'>*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmboiltype" runat="server" >
                                                               
                                                                <asp:ListItem Value="0">--select--</asp:ListItem>
                                                               <asp:ListItem Value="1">New  Oil</asp:ListItem>
                                                                <asp:ListItem Value="2">Reclaimed Oil</asp:ListItem>
                                                                 <asp:ListItem Value="3">Ester Oil</asp:ListItem>
                                                                 <asp:ListItem Value="4">New Transil Oil</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            </div>
                                             </asp:Panel> 
                                            <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                                        <div class="span12">
                                            <!-- BEGIN SAMPLE FORMPORTLET-->
                                           
                                                <%--<div class="widget-title">
                                                    <h4><i class="icon-reorder"></i>Comments for Approve/Reject</h4>
                                                    <span class="tools">
                                                        <a href="javascript:;" class="icon-chevron-down"></a>
                                                        <a href="javascript:;" class="icon-remove"></a>
                                                    </span>
                                                </div>--%>
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            
        </div>



            <div class="text-center" align="center">
               
                    <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary"  OnClick="cmdSave_Click"/>

                <asp:Button ID="cmdViewEstimate" runat="server" Text="View Workaward"
                                CssClass="btn btn-primary"
                                OnClick="cmdWorkaward_Click" TabIndex="13" />

                <%-- <asp:ImageButton ID="imgBtnEdit" runat="server" Height="50px" Width="50px" OnClick="imgBtnEdit_Click"
                 ImageUrl="https://th.bing.com/th/id/OIP.nB8C2pjiPdl5nu9ccWhQnQHaHU?w=204&h=202&c=7&r=0&o=5&dpr=1.25&pid=1.7"  />--%>  <%-- OnClick="printpage()--%>

                 <asp:ImageButton ID="imgBtnEdit" runat="server" Height="50px" Width="50px" OnClick="PDF_onclick"
                 ImageUrl="https://th.bing.com/th/id/OIP.nB8C2pjiPdl5nu9ccWhQnQHaHU?w=204&h=202&c=7&r=0&o=5&dpr=1.25&pid=1.7"  />



                </div>

        </div>
    
</asp:Content>
