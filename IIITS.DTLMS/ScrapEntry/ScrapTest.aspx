<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="ScrapTest.aspx.cs" Inherits="IIITS.DTLMS.ScrapEntry.ScrapTest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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

        function onlynumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode < 48 || charCode > 57) {

                return false;
            }
            return true;
        }

        function AllowOnlyAlphanumericNotAllowSpecial(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
           ((evt.which) ? evt.which : 0));

            if ((charCode > 47 && charCode < 58) ||
                (charCode > 64 && charCode < 91) ||
                (charCode > 96 && charCode < 123)) {
                return true;
            }

            else { return false; }
        }

        function DisableSplChars(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 92) {
                return false;
            } else {

                return true;
            }
        }

    </script>
    <style type="text/css">
        .handPointer {
            cursor: pointer;
        }

        .blockpointer {
            cursor: not-allowed;
        }
    </style>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">

            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Scrap Transformer Search
                                                   <div class="row-fluid" runat="server" id="dvclose" style="display: none">
                                                       <div style="float: right">

                                                           <asp:Button ID="cmdClose" runat="server" Style="margin-top: 14px; margin-right: 1px;" Text="Close"
                                                               CssClass="btn btn-primary" OnClick="cmdClose_Click" /><br />
                                                       </div>
                                                   </div>
                    </h3>
                    <%--                        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="row-fluid" runat="server" id="dvscraphead">
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Scrap Transformer Search</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div class="form-horizontal">
                                        <%--                                    <div style="width: 300px; margin-left: 45px; padding">
                                        <div style="float: left">
                                            <asp:RadioButton ID="rdbPendingForTest" GroupName="a" CssClass="radio" Checked="true"
                                                runat="server" Text="Pending For Test" Font-Size="Medium" />
                                        </div>
                                        <div style="float: right">
                                            <asp:RadioButton ID="rdbAlreadyTested" GroupName="a" CssClass="radio" runat="server"
                                                Text="Already Tested" Font-Size="Medium" />
                                        </div>
                                    </div>--%>
                                        <div class="row-fluid">
                                            <div style="height: 50px;">
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Store</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbStore" runat="server" TabIndex="3" Enabled="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Make</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbMake" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">
                                                        Capacity(in KVA)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="1">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group" style="display: none">
                                                    <label class="control-label">
                                                        Repairer</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbRepairer" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group" style="display: none">
                                                    <label class="control-label">
                                                        Supplier</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbSupplier" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span1">
                                            </div>
                                        </div>
                                        <div class="space20">
                                        </div>
                                        <div class="space20">
                                        </div>
                                        <div class="text-center">

                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                                OnClick="cmdLoad_Click" TabIndex="4" />


                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                TabIndex="5" OnClick="cmdReset_Click" />


                                            <asp:Button ID="cmdexport" runat="server" Visible="false" Text="Export Excel" CssClass="btn btn-info"
                                                OnClick="Export_clickscraptest" />

                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                            <div style="width: 1080px; overflow: auto; margin-left: 70px">


                                <%--                                <asp:GridView ID="grdFaultTC" AutoGenerateColumns="False" DataKeyNames="TC_ID" CssClass="table table-striped table-bordered table-advance table-hover"
                                    PageSize="10" ShowFooter="true"
                                    runat="server" OnRowCommand="grdFaultTC_RowCommand" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" TabIndex="9" OnRowDataBound="grdFaultTC_RowDataBound"
                                    HorizontalAlign="Center"   OnSorting="grdFaultTC_Sorting" AllowSorting="true">--%>


                                <asp:GridView ID="grdFaultTC" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                    AutoGenerateColumns="false" PageSize="10" DataKeyNames="TC_ID"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnPageIndexChanging="grdFaultTC_PageIndexChanging"
                                    OnRowCommand="grdFaultTC_RowCommand" ShowFooter="True"
                                    TabIndex="5" OnSorting="grdFaultTC_Sorting" AllowSorting="true">
                                    <HeaderStyle CssClass="both" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all"
                                                    Width="90px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtdtrcode" runat="server" Width="110px" MaxLength="8" placeholder="Enter DTr Code" ToolTip="Enter DTr Code to Search"  onkeypress="return AllowOnlyAlphanumericNotAllowSpecial(event,this);"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <asp:Panel ID="panel3" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtdtrslno" runat="server" Width="110px" MaxLength="25" placeholder="Enter DTr Slno" ToolTip="Enter DTr Slno to Search"  onkeypress="return AllowOnlyAlphanumericNotAllowSpecial(event,this);"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="Search" TabIndex="9" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all"
                                                    Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star Rate" SortExpression="TC_STAR_RATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStarrate" runat="server" Text='<%# Bind("TC_STAR_RATE") %>' Style="word-break: break-all"
                                                    Width="120px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all"
                                                    Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--                                        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" SortExpression="TS_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Testing Result" Visible="false">
                                            <ItemTemplate>
                                                <div style="float: right; width: 120px; margin-left: 15px;">
                                                    <asp:RadioButton ID="rdbScrap" runat="server" Text="Scarp" GroupName="a" CssClass="radio" />
                                                    <asp:RadioButton ID="rdbTest" runat="server" Text="Send For Test" GroupName="a" CssClass="radio" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="TC STATUS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCStatus" runat="server" Text='<%# Bind("TC_STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" Visible="false">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                        CommandName="Submit" Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Test Document" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDwnld" runat="server" CommandName="Download">
                                                 <img src="../img/Manual/Pdficon.png" style="width:20px" />Download Test Report</asp:LinkButton>
                                                <asp:LinkButton ID="lnkNodownload" runat="server" Enabled="false">
                                                 <img src="../img/Manual/nodoc.png" style="width:20px" />Report Not Available</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove" Visible="false">
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
                            <div class="form-horizontal" align="center">
                                <div class="span3">
                                </div>
                                <div class="span3">
                                    <asp:Button ID="Load" runat="server" Text="Add Selected DTr" Visible="false" CssClass="btn btn-success" OnClick="cmdTcLoad_Click" />
                                </div>
                                <div class="space20">
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="container-fluid">
                        <div class="row-fluid" runat="server" id="dvcheckedTc" style="display: none">



                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4>
                                            <i class="icon-reorder"></i>Load DTr Details</h4>
                                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                            class="icon-remove"></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="row-fluid" runat="server" id="dvHead" style="display: none">
                                                <div class="widget-body">
                                                    <div class="widget-body form">
                                                        <div class="form-horizontal">
                                                            <div class="row-fluid">
                                                                <div class="span5">
                                                                    <div class="control-group">
                                                                        <label class="control-label">
                                                                            Scrap OM No<span class="Mandotary"> *</span></label>
                                                                        <div class="controls">
                                                                            <div class="input-append">
                                                                                <asp:TextBox ID="txtOmNo" runat="server" MaxLength="100"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="control-group">
                                                                        <label class="control-label">Date Of Scrapping<span class="Mandotary"> *</span></label>
                                                                        <div class="controls">
                                                                            <div class="input-append">

                                                                                <asp:TextBox ID="txtOmdate" runat="server" MaxLength="25"></asp:TextBox>
                                                                                <ajax:CalendarExtender ID="ReferenceCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtOmdate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row-fluid" runat="server" id="dvdocument">
                                                                    <div class="span5">
                                                                        <div class="control-group">
                                                                            <label class="control-label">Upload OM Document<span class="Mandotary"> *</span></label>
                                                                            <div class="controls">
                                                                                <div class="input-append">
                                                                                    <asp:FileUpload ID="fupDoc" runat="server" AllowMultiple="False"
                                                                                        TabIndex="17" />
                                                                                          <asp:Label ID="lblOmFilename" runat="server" Text="Initial Text"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                              

                                                              
                                                                        <div class="control-group">
                                                                            <label class="control-label">Total Selected DTrs<span class="Mandotary"> *</span></label>
                                                                            <div class="controls">
                                                                             <div class="input-append">
                                                                                <asp:TextBox ID="TxtTotalDtrs" runat="server" MaxLength="100" Enabled="false"></asp:TextBox>
                                                                            </div>
                                                                            </div>
                                                                       </div>
                                                                    </div>
                                                                          </div>
                                                              <%--  </div>--%>

                                                                <div class="row-fluid" runat="server" id="dvViewdocument" style="display: none">
                                                                    <div class="span5">
                                                                        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false"
                                                                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                                            AllowSorting="true" Visible="true">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Budget Document" ItemStyle-Width="300" />
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile"
                                                                                            CommandArgument='<%# Eval("Name") %>'> 
                                                                                        </asp:LinkButton>
                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Text="<i class='icon-download-alt'></i> DOWNLOAD" OnClick="DownloadFiledwnld"
                                                       CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--style="overflow: scroll; height: 450px;"--%>
                                            <div id="div1" runat="server">
                                                <asp:GridView ID="grdloadcheckeddtr" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                                    AutoGenerateColumns="false"  DataKeyNames="TC_ID"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" 
                                                    OnRowCommand="grdloadcheckeddtr_RowCommand"                                       
                                                    runat="server" OnPageIndexChanging="grdFaultTC_PageIndexChanging"
                                                    TabIndex="5" OnSorting="grdFaultTC_Sorting" AllowSorting="true">
                                                    <Columns>

                                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all"
                                                                    Width="90px"></asp:Label>
                                                            </ItemTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all"
                                                                    Width="100px"></asp:Label>
                                                            </ItemTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all"
                                                                    Width="150px"></asp:Label>
                                                            </ItemTemplate>


                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all"
                                                                    Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all"
                                                                    Width="80px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--                                        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" SortExpression="TS_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Testing Result" Visible="false">
                                                            <ItemTemplate>
                                                                <div style="float: right; width: 120px; margin-left: 15px;">
                                                                    <asp:RadioButton ID="rdbScrap" runat="server" Text="Scarp" GroupName="a" CssClass="radio" />
                                                                    <asp:RadioButton ID="rdbTest" runat="server" Text="Send For Test" GroupName="a" CssClass="radio" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="TC STATUS" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTCStatus" runat="server" Text='<%# Bind("TC_STATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" Visible="false">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                                        CommandName="Submit" Width="12px" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Reason For Scrap">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" runat="server" Height="40px" TextMode="MultiLine" Style="resize: none" MaxLength="200" onkeyup="return ValidateTextlimit(this,200);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Upload OM Document" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" Width="180px" />
                                                            
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remove">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                        CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Reason For Scrap">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblremarks" runat="server" Text='<%# Bind("STD_REMARKS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>

                                        </div>
                                    </div>


                                </div>

                                <div class="space20">
                                </div>
                                <asp:TextBox ID="txtfilepath" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtViewOmNo" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtViewOmdate" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtViewOmID" runat="server" Visible="false"></asp:TextBox>
                                   <asp:HiddenField ID="hdfRemarks" runat="server" />

                            </div>
                        </div>


                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="space20">
                                </div>
                                <div class="space20">
                                </div>
                                <div class="form-horizontal" align="center">
                                    <div class="span3">
                                    </div>
                                    <div class="span3">
                                        <asp:Button ID="cmdSend" runat="server" Text="Declare Scrap" CssClass="btn btn-primary"
                                            OnClick="cmdSend_Click" Visible="false" TabIndex="7" />

                                    </div>

                                    <div class="space20">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--    added by sandeep--%>
                    <div class="container-fluid">
                        <div class="row-fluid" runat="server" id="dvviewscrapdtr" style="display: none">
                            <!-- BEGIN PAGE HEADER-->
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN THEME CUSTOMIZER-->
                                    <br />
                                    <!-- END THEME CUSTOMIZER-->
                                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                    <h3 class="page-title">Scrap View
                        <%--                   <a style="float:right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>   
                                    </h3>
                                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                            </div>
                            <!-- END PAGE HEADER-->
                            <!-- BEGIN PAGE CONTENT-->
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORMPORTLET-->
                                    <div class="widget blue">
                                        <div class="widget-title">
                                            <h4><i class="icon-reorder"></i>Scrap View</h4>
                                            <span class="tools">
                                                <a href="javascript:;" class="icon-chevron-down"></a>
                                                <a href="javascript:;" class="icon-remove"></a>
                                            </span>
                                        </div>
                                        <div class="widget-body">

                                            <div class="space20"></div>
                                            <!-- END FORM-->

                                            <div class="space20"></div>
                                            <div class="row-fluid">



                                                <div id="dvviewscrap" style="overflow: scroll; height: 450px;" runat="server">
                                                    <asp:GridView ID="grdviewscrapdtr" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                                        AutoGenerateColumns="false" PageSize="10" DataKeyNames="STD_TC_ID"
                                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                        OnRowCommand="grdloadcheckeddtr_RowCommand"
                                                        runat="server" OnPageIndexChanging="grdviewscrapdtr_PageIndexChanging"
                                                        TabIndex="5" AllowSorting="true">
                                                        <Columns>


                                                            <asp:TemplateField AccessibleHeaderText="STD_TC_ID" HeaderText="STD_TC_ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("STD_TC_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="STD_TC_CODE" HeaderText="DTr Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("STD_TC_CODE") %>' Style="word-break: break-all"
                                                                        Width="90px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="STD_CR_BY" HeaderText="Cr_By" SortExpression="STD_CR_BY">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("STD_CR_BY") %>' Style="word-break: break-all"
                                                                        Width="100px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all"
                                                                        Width="150px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all"
                                                                        Width="60px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all"
                                                                        Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="STD_REMARKS" HeaderText="Reason For Scrap">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblremarks" runat="server" Text='<%# Bind("STD_REMARKS") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="Edit" Visible="false">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                                            CommandName="Submit" Width="12px" />
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <%--    closed--%>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
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
                    <p style="color: Black"><i class="fa fa-info-circle"></i>This Web Page Can Be Used To Declare DTr as Scrap.</p>
                    <p style="color: Black"><i class="fa fa-info-circle"></i>Once We Select The Check Box Then Click On Add Selected Dtr Button It Will load the Selected DTr Details.</p>
                    <p style="color: Black"><i class="fa fa-info-circle"></i>Once DTr Details Loaded Then Fill The Mandatory Fields And Click On Declare Scrap Button.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->
</asp:Content>
