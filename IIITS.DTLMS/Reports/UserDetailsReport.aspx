<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="UserDetailsReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.UserDetailsReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
     <style type="text/css">
        .modalBackground {
    filter: alpha(opacity=70);
    opacity: 0.7;
}
       
          .modalPopup
    {
        background: #fff;
        width: 600px;
        height: 500px;
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
                    <h3 class="page-title" runat="server" id="Create">User Details
                    </h3>
                    <%--<h3 class="page-title" runat="server" id="Update">Update Feeder Master
                    </h3>--%>
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
                    <%--<asp:Button ID="cmdFeederView" class="btn btn-primary" Text="Feeder View"
                        OnClientClick="javascript:window.location.href='FeederView.aspx'; return false;" runat="server" />--%>
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateFeeder" runat="server"><i class="icon-reorder"></i>User Details</h4>
<%--                            <h4 id="UpdateFeeder" runat="server"><i class="icon-reorder"></i>Update Feeder Master</h4>--%>
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
                                                <label class="control-label">Zone <span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtZoneCode" runat="server" Style="resize: none"  TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                        <asp:Button ID="btnSearchZone" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearchZone_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">Division <span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDivisionCode" runat="server" Style="resize: none"  TextMode="MultiLine"></asp:TextBox>
                                                        <asp:Button ID="btnSearchDivision" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearchDivision_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                                <div class="control-group">
                                                <label class="control-label">Section <span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSectionCode" runat="server" Style="resize: none"  TextMode="MultiLine"></asp:TextBox>
                                                        <asp:Button ID="btnSearchSection" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearchSection_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                      

                                       <%-- <div class="span1"></div>--%>
                                    </div>

                                             
                                        <div class="span5">
                                          
                                      <div class="control-group">
                                                <label class="control-label">Circle <span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCircleCode" runat="server" Style="resize: none"  TextMode="MultiLine"></asp:TextBox>
                                                        <asp:Button ID="btnSearchCircle" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearchCircle_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                                <div class="control-group">
                                                <label class="control-label">Sub Division <span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSubDivisionCode" runat="server" Style="resize: none"  TextMode="MultiLine" ></asp:TextBox>
                                                        <asp:Button ID="btnSearchSubDivision" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearchSubDivision_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="space20"></div>

                                    <div class="text-center">

                                       
                                      <%--      <asp:Button ID="cmdSave" runat="server" Text="Save"
                                                OnClientClick="javascript:return Validate()" CssClass="btn btn-primary"
                                                OnClick="cmdSave_Click" />--%>
                                      
                         
                                    
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" OnClick="cmdReset_Click" 
                                             AutoPostBack="true"   CssClass="btn btn-danger" /><br />
                                   
                                        <div class="span7"></div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                </div>


                            </div>
                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                 <asp:HiddenField ID="hdfStatus" runat="server" />
                        </div>
                        <div class="space20"></div>

                    </div>
                </div>
            </div>

            <!-- END PAGE CONTENT-->
        </div>

        <div class="space20"></div>
        <div class="space20"></div>
        <div class="space20"></div>
        <div class="space20"></div>

        <asp:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup" CancelControlID="cmdClose"
            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
        <div style="width: 100%; vertical-align: middle" align="center">



            <asp:Panel ID="pnlControls" runat="server" CssClass="modalPopup" align="center" style = "display:none">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>Select Office Codes And Click On Proceed</h4>
                        <div class="space20"></div>


                        <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" OnPageIndexChanging="GrdOffices_PageIndexChanging" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%" OnRowDataBound="GrdOffices_RowDataBound"
                            AllowPaging="True" DataKeyNames="OFF_CODE" OnRowCommand="GrdOffices_RowCommand">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Subdivision Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Subdivision Name" Visible="true">
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


                    <%--    <div style="background:#fff!important"class="row">--%>
                   <div class="row-fluid">
                            <div class="span1"></div>
                            <div class="span2">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" style="left: 0px; top: 0px" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span2">

                                <div class="control-group">

                                    <div class="controls">
                                        <div class="input-append">
                                            <%--onclick="btnClose_Click"--%>
                                            <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary" Text="Cancel" />

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
