<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TCTesting.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TCTesting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateSave() {

            if (document.getElementById('<%= cmbTestedBy.ClientID %>').value == "--Select--") {
                alert('Select Tested By')
                document.getElementById('<%= cmbTestedBy.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtTestedOn.ClientID %>').value.trim() == "") {
                alert('Enter valid Tested On')
                document.getElementById('<%= txtTestedOn.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbTestLocation.ClientID %>').value == "--Select--") {
                alert('Select Testing Location')
                document.getElementById('<%= cmbTestLocation.ClientID %>').focus()
                return false
            }

        }

        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);

        }

        window.onbeforeunload = preventMultipleSubmissions;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Transformer Inspection at Repair Center          
                                      
                    </h3>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='TestPendingSearch.aspx'; return false;"
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
                            <h4><i class="icon-reorder"></i>Deliver DTR</h4>
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
                                                <label class="control-label">Store</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Enabled="false">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No.</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--<asp:TextBox ID="txtWoNo" runat="server" MaxLength="25" Visible="false"></asp:TextBox> --%>
                                                        <%--<asp:TextBox ID="txtWoNo" runat="server" MaxLength="25" Visible="false"></asp:TextBox> --%>
                                                        <asp:TextBox ID="txtPONo" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <%--    <div class="control-group">
                                                <label class="control-label">Remarks</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPO_Remarks" runat="server" MaxLength="25" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <%-- <div class="control-group">
                                            <label class="control-label">Issue Date</label>                                            
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:TextBox ID="txtIssueDate" runat="server" MaxLength="25" Enabled="false"></asp:TextBox> 
                                                      <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" TargetControlID="txtIssueDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>   
                                                </div>
                                            </div>
                                        </div>
                                       <div class="control-group">
                                            <label class="control-label">Old Purchase Order No.</label>                                            
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                      <%--<asp:TextBox ID="txtWoNo" runat="server" MaxLength="25" Visible="false"></asp:TextBox> --%>
                                            <%--<asp:TextBox ID="txtOldPONo" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>   
                                                </div>
                                            </div>
                                        </div>--%>
                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Tested By<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTestedBy" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Tested On<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSelectedDetailsId" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                        <asp:TextBox ID="txtTestedOn" runat="server" MaxLength="25"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="TestedOnCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtTestedOn" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Testing Location<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTestLocation" runat="server">
                                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Vendor Premises"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Department"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group" style="display: none" runat="server">
                                                <label class="control-label">Upload<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupTestDocument" runat="server" AllowMultiple="False" accept="jpg,jpeg,png" />
                                                        <asp:TextBox ID="txtTestDocfilepath" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="space20"></div>



                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->
                            <%--<div id = "div1" style= "height:600px; width:1050px;" runat="server">--%>

                            <%--     style="overflow:scroll; height:600px;" --%>
                            <div id="divResult" runat="server">
                                <asp:GridView ID="grdDeliverDetails" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found"
                                    OnRowCommand="grdDeliverDetails_RowCommand"
                                    OnRowDataBound="grdDeliverDetails_RowDataBound">
                                    <Columns>



                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;" Width="40px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="40px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO Number" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Testing Result">
                                            <ItemTemplate>
                                                <div style="float: right; width: 160px; margin-left: 35px;">
                                                    <span class="span5">
                                                        <asp:RadioButton ID="rdbPass" runat="server" Text="Pass" GroupName="a" CssClass="radio" />
                                                        <%--                                        <asp:RadioButton ID="rdbScrap" runat="server" Text="Scrap" GroupName="a" CssClass="radio" />--%>
                                                    </span>
                                                    <span class="span5">
                                                        <asp:RadioButton ID="rdbFail" runat="server" Text="Fail" GroupName="a" CssClass="radio" />
                                                        <%--                                        <asp:RadioButton ID="rdbSendToStore" runat="server" Text="None" GroupName="a" CssClass="radio" />--%>
                                                    </span>

                                                    <%--<asp:CheckBox ID="chkPass" runat="server" Text="Pass" CssClass="checkbox"> </asp:CheckBox>
                                        <asp:CheckBox ID="chkFail" runat="server" Text="Fail" CssClass="checkbox"> </asp:CheckBox>--%>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Height="40px" TextMode="MultiLine" Style="resize: none" MaxLength="200" onkeyup="return ValidateTextlimit(this,200);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Upload Document">
                                            <ItemTemplate>
                                                <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" Width="180px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltransid" runat="server" Text='<%# Bind("RSD_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--  <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnDownload" runat="server" Height="12px" ImageUrl="https://www.flaticon.com/premium-icon/folder_3121602"
                                                        CommandName="Download" Width="12px" " />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Remove">
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
                            <div class="form-horizontal text-center">
                                <div class="span5"></div>
                                <div class="span1">
                                    <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateSave()"
                                        CssClass="btn btn-primary" OnClick="cmdSave_Click" onchange="javascript:preventMultipleSubmissions();" />
                                </div>
                                <div class="span1">
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        CssClass="btn btn-primary" OnClick="cmdReset_Click" />
                                </div>
                                <div class="span5"></div>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                <asp:HiddenField ID="hdfResult" runat="server" />
                                <asp:HiddenField ID="hdfRemarks" runat="server" />
                            </div>
                            <div class="space20"></div>
                            <%--                 </div>--%>
                            <!-- END SAMPLE FORM PORTLET-->
                        </div>
                    </div>
                    <!-- END PAGE CONTENT-->
                </div>
            </div>

        </div>
    </div>
</asp:Content>
