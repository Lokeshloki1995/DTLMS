<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ApprovalInbox.aspx.cs" Inherits="IIITS.DTLMS.Approval.ApprovalInbox" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .table th, .table td {
            padding: 5px !important;
            line-height: 20px;
            text-align: left;
            vertical-align: top;
            border-top: 1px solid #ddd;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(/img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .auto-style1 {
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
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">My  Approval Inbox
                    </h3>
                    <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>My Approval Inbox</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <%-- <div style="float:left" >--%>
                                    <%--  <div class="span8">--%>
                                    <div class="span2">
                                        <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:HiddenField ID="hdfRefType" runat="server" />
                                    </div>
                                    <div class="span2">
                                        <asp:RadioButton ID="rdbPending" runat="server" Text="Pending for Approval" CssClass="radio"
                                            GroupName="a" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbPending_CheckedChanged" />
                                    </div>
                                    <div class="span2">
                                        <asp:RadioButton ID="rdbAlready" runat="server" Text="Already Approved"
                                            CssClass="radio" GroupName="a" AutoPostBack="true" OnCheckedChanged="rdbAlready_CheckedChanged" />
                                    </div>
                                    <div class="span2">
                                        <asp:RadioButton ID="rdbRejected" runat="server" Text="Rejected"
                                            CssClass="radio" GroupName="a" AutoPostBack="true" OnCheckedChanged="rdbRejected_CheckedChanged" />
                                    </div>

                                   


                                    <div class="span2">
                                        <asp:HiddenField ID="hdfLocationCode" runat="server" />
                                        <asp:LinkButton ID="lnkChange" runat="server" style="color:#08c" OnClick="lnkChange_Click">Select Location</asp:LinkButton>
                                    </div>
                                    <%--   </div>--%>
                                    <div class="span2">
                                        <asp:Label ID="lblLoc" runat="server" Text="Location :" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="lblLocation" runat="server" Font-Bold="true" ForeColor="CadetBlue"
                                            Font-Size="Medium"></asp:Label>
                                    </div>
                                     <div class="space20"></div>



                                </div>
                            </div>

                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <%-- <div class="span1"></div>--%>
                                    <div class="span5">
                                        <div class="control-group">

                                            <label class="control-label">Subject</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubject" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">From Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="auto-style1"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="txtFromDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">

                                            <label class="control-label">Sent By</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSentBy" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">To Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="auto-style1"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                                Width="116px" OnClick="cmdLoad_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="space20"></div>

                            <!-- END FORM-->


                            <asp:GridView ID="grdApprovalInbox" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdApprovalInbox_PageIndexChanging"
                                OnRowCommand="grdApprovalInbox_RowCommand"
                                OnRowDataBound="grdApprovalInbox_RowDataBound" OnSorting="grdApprovalInbox_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="WO_ID" HeaderText="WFObject ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWFOId" runat="server" Text='<%# Bind("WO_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_RECORD_ID" HeaderText="Record Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecordId" runat="server" Text='<%# Bind("WO_RECORD_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                             


                                    <asp:TemplateField AccessibleHeaderText="WO_BO_ID" HeaderText="BO Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBOId" runat="server" Text='<%# Bind("WO_BO_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_APPROVE_STATUS" HeaderText="BO Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApproveStatus" runat="server" Text='<%# Bind("WO_APPROVE_STATUS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="BO_NAME" HeaderText="Subject" SortExpression="BO_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFormName" runat="server" Text='<%# Bind("BO_NAME") %>' Style="word-break: break-all;"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFormName" runat="server" placeholder="Enter Form Name"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_DESCRIPTION" HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" Text='<%# Bind("WO_DESCRIPTION") %>' Style="word-break: break-all;" Width="180px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDesc" runat="server" placeholder="Enter Description"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="USER_NAME" HeaderText="Approved By" SortExpression="USER_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCrby" runat="server" Text='<%# Bind("USER_NAME") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="CREATOR" HeaderText="Created By" SortExpression="CREATOR">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCrby1" runat="server" Text='<%# Bind("CREATOR") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <%--<FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>--%>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="CR_ON" HeaderText="Approved On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCrOn" runat="server" Text='<%# Bind("CR_ON") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="RO_NAME" HeaderText="Role Name" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRole" runat="server" Text='<%# Bind("RO_NAME") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" SortExpression="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Action Taken" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActionTaken" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="CURRENT_STATUS" HeaderText="Current Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrentStatus" runat="server" Text='<%# Bind("CURRENT_STATUS") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <%--for office code--%>
                                    <asp:TemplateField AccessibleHeaderText="WO_REF_OFFCODE" HeaderText="Office Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("WO_REF_OFFCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <center>
                                                <%--<asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create" 
                                                Width="12px" />--%>
                                                <asp:LinkButton runat="server" CommandName="Modify" ID="lnkModify" ToolTip="Modify And Approve" Visible="true">
                                        <img src="../img/Manual/MA.png" style="width:20px" /></asp:LinkButton>
                                                <asp:LinkButton runat="server" CommandName="Approve" ID="lnkApprove" ToolTip="Approve">
                                        <img src="../img/Manual/Approve.png" style="width:15px" /></asp:LinkButton>
                                                <asp:LinkButton runat="server" CommandName="Reject" ID="lnkReject" ToolTip="Reject">
                                        <img src="../img/Manual/Reject.png" style="width:20px" /></asp:LinkButton>

                                                <asp:LinkButton runat="server" CommandName="View" ID="lnkView" ToolTip="View" Visible="false">
                                         <img src="../img/Manual/view.png" style="width:20px" /></asp:LinkButton>

                                                <asp:LinkButton runat="server" CommandName="History" ID="lnkHistory" ToolTip="View History">
                                         <img src="../img/Manual/View1.jpg" style="width:20px" /></asp:LinkButton>

                                            </center>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <center>
                                                <asp:Label ID="lblHeader" runat="server" Text="Action"></asp:Label>
                                            </center>
                                        </HeaderTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WOA_ID" HeaderText="Current Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWFAutoId" runat="server" Text='<%# Bind("WOA_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_WFO_ID" HeaderText="Current Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWFDataId" runat="server" Text='<%# Bind("WO_WFO_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_INITIAL_ID" HeaderText="Initial ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInitialId" runat="server" Text='<%# Bind("WO_INITIAL_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="WO_DATA_ID" HeaderText="WO_DATA_ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldataId" runat="server" Text='<%# Bind("WO_DATA_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>

                        </div>

                        <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                        <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                            <div style="display: none">
                                <asp:Button ID="btnshow" runat="server" Text="Button" />
                            </div>
                            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="162px" Width="434px" Style="display: none">
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4>Approve </h4>
                                        <div class="space20"></div>
                                        <%--<div class="row-fluid">--%>
                                        <div class="span1"></div>
                                        <div class="space20">
                                            <div class="span1"></div>

                                            <div class="span5">

                                                <div class="control-group" style="font-weight: bold">
                                                    <label class="control-label">Comment / Remarks<span class="Mandotary"> *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append" align="center">
                                                            <div class="span3"></div>
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                                onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>



                                                <div>
                                                    <div class="control-group" style="font-weight: bold">

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <div class="span3"></div>
                                                                <div class="span7">
                                                                    <asp:Button ID="cmdApprove" runat="server" CssClass="btn btn-primary"
                                                                        OnClick="cmdApprove_Click" TabIndex="10" Text="Approve" />
                                                                </div>
                                                                <div class="span1">
                                                                    <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary"
                                                                        TabIndex="10" Text="Close" />
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
                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>

                                </div>
                            </asp:Panel>
                        </div>

                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>


            <!-- END PAGE CONTENT-->
        </div>
    </div>


    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used to Approve,Modify and Approve and Reject the Record
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Can Filter Records By Selecting <u>Pending for Approval</u>, <u>Already Approved</u> and <u>Rejected</u> Radio Button
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->
    <ajax:ModalPopupExtender ID="mdlLocations" runat="server" TargetControlID="btnshow" CancelControlID="imgbtnClose"
        PopupControlID="pnlLocations" BackgroundCssClass="modalBackground" />
    <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
        <div style="display: none">
            <asp:Button ID="Button1" runat="server" Text="Button" />
        </div>
        <asp:Panel ID="pnlLocations" runat="server" BackColor="White" Height="362px" Width="534px">
            <div class="widget blue">
                <div class="widget-title">
                    <h4>Select Office</h4>
                    <div style="float: right">
                        <asp:ImageButton ID="imgbtnClose" runat="server" ImageUrl="~/img/Manual/close1.png" Width="40px" />
                    </div>
                    <div class="space20"></div>
                    <%--<div class="row-fluid">--%>
                    <div class="span1"></div>
                    <div class="space20">
                        <div class="span1"></div>

                        <div class="span6">

                            <asp:GridView ID="grdOffices" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" ShowFooter="true"
                                ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                PageSize="6" AllowPaging="True"
                                OnRowCommand="grdOffices_RowCommand"
                                OnPageIndexChanging="grdOffices_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Office Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="70px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffName" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Office Name" Width="200px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <center>
                                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="submit">Select</asp:LinkButton>
                                            </center>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>

                            <div class="space20" align="center">

                                <div class="form-horizontal" align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="space20"></div>
                <div class="space20"></div>

            </div>
        </asp:Panel>
</asp:Content>
