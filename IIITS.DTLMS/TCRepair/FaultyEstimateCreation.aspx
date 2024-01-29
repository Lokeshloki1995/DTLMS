<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="FaultyEstimateCreation.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.FaultyEstimateCreation" %>

<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/functions.js" type="text/javascript"></script>
     <script type="text/javascript">
 
function preventMultipleSubmissions() {
  $('#<%=cmdSave.ClientID %>').prop('disabled', true);
}
window.onbeforeunload = preventMultipleSubmissions;
 
</script>
    <script type="text/javascript">
        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }
        }
</script>
 <style>
    .MyClass label {float: right;margin:-5px 0px 0px 8px}
     .MyClass input {margin-left:20px}
    
   
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">

            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Create Repairer Estimation
                    </h3>



                    <div class="span7">
                    </div>


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
                        CssClass="btn btn-primary" OnClick="cmdClose_Click" />
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

                                            <%-- <label class="control-label">
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure Id"></asp:Label>
                                                    <span class="Mandotary">*</span></label>--%>

                                            <div class="controls">
                                                <div class="input-append">
                                                    sWFOAutId
                                                    
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                    <asp:HiddenField ID="txtestId" runat="server" />
                                                    <asp:HiddenField ID="hdfEnhancementId" runat="server" />
                                                    <%--<asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"
                                                            TabIndex="2" OnClick="cmdSearch_Click" />--%>
                                                </div>
                                            </div>



                                            <%--                                                <label class="control-label">Transformer Centre Code</label>--%>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                    <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                    <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    <asp:HiddenField ID="hdfAppDesc" runat="server" />
                                                    <asp:HiddenField ID="hdfEstId" runat="server" />
                                                    <asp:HiddenField ID="hdfStatusflag" runat="server" />
                                                    <asp:HiddenField ID="hdfkavika" runat="server" />
                                                    <asp:HiddenField ID="hdfOfficeCode" runat="server" />

                                                    <%--                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox></br/>--%>
                                                    <%--                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>--%>
                                                </div>
                                            </div>

                                            <asp:HiddenField ID="hdfCrBy" runat="server" />


                                            <div class="control-group">
                                                <label class="control-label">Capacity</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCapacity" runat="server" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Estimation No</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Guarantee Type <span class='Mandotary'>*</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGuarenteeType" runat="server">
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="AGP" Text="AGP"></asp:ListItem>
                                                            <asp:ListItem Value="WRGP" Text="WRGP"></asp:ListItem>
                                                            <asp:ListItem Value="WGP" Text="WGP"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <asp:HiddenField ID="hdfGuarenteeSource" runat="server" />
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Select Winding Type <span class='Mandotary'>*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMaterialType" runat="server" OnSelectedIndexChanged="cmbMaterialType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem Value="1">Aluminium Winding</asp:ListItem>
                                                            <asp:ListItem Value="2">Copper Winding</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Select Rating Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRateType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbRateType_SelectedIndexChanged">
                                                            <asp:ListItem  Value="0">--select--</asp:ListItem>
                                                            <asp:ListItem  Value="1">Star Rate</asp:ListItem>
                                                            <asp:ListItem  Value="2">Conventional</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label id="lblRepairer" class="control-label">Repairer <span class='Mandotary'>*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server" OnSelectedIndexChanged="cmbRepairer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">DTr Code</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtTCId" runat="server" MaxLength="100" Visible="false" Width="20px"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <%-- <div class="control-group">
                                                <label class="control-label">Declared By</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeclaredBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkBudgetstat" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">Estimation Date <span class="Mandotary">*</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEstDate" runat="server" autocomplete="off" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtEstDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divRepairerFail" runat="server">
                                                <br />
                                                    <div class="control-group">
                                                    <label id="lblFailcoilType" class="control-label">Select Coil Type <span class='Mandotary'>*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbFailcoilType" runat="server" OnSelectedIndexChanged="cmbFailType_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0">--select--</asp:ListItem>
                                                               <asp:ListItem Value="1">Single Coil</asp:ListItem>
                                                                <asp:ListItem Value="2">Multi Coil</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />


                                            </div>
                                            <div class="control-group" >
                                                <label class="control-label"> Phase  <span class='Mandotary'>*</span></label>
                                                <div class="input-append" >
                                                        <asp:CheckBoxList ID="rybPhase" runat="server"  CssClass="MyClass" OnSelectedIndexChanged="rybPhase_SelectedIndexChanged" AutoPostBack="true">
                                                         <asp:ListItem Value="1">R-Phase</asp:ListItem>
                                                          <asp:ListItem Value="2">Y-Phase</asp:ListItem> 
                                                         <asp:ListItem Value="3">B-Phase</asp:ListItem>
                                                       </asp:CheckBoxList>
                                                </div>
                                            </div>
                                         
                                        </div>
                                    </div>

                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div id="divAnnuFile" runat="server">
                                        <div class="row-fluid">
                                            <div class="span1"></div>
                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Select Annexture Type <span class='Mandotary'>*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbFileType" runat="server">
                                                                <asp:ListItem Value="0">--select--</asp:ListItem>
                                                                <%--<asp:ListItem Value="1">Annexture 1</asp:ListItem>--%>
                                                                <asp:ListItem Value="2">Annexture 2</asp:ListItem>
                                                                <asp:ListItem Value="3">Annexture 3</asp:ListItem>
                                                                <asp:ListItem Value="4">Annexture 4</asp:ListItem>
                                                                <asp:ListItem Value="5">Others</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="span6">
                                                <div class="control-group">
                                                    <label class="control-label">Select File</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:FileUpload ID="fupAnx" runat="server" />
                                                            <asp:Button ID="cmdAdd" class="btn btn-primary" runat="server" Text="Add" OnClick="cmdAdd_Click" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="row-fluid">
                                        <div class="span3"></div>
                                        <div class="span5">
                                            <asp:GridView ID="grdDocuments" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                runat="server" AllowSorting="true" OnRowCommand="grdDocuments_RowCommand" OnRowDeleting="grdDocuments_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="SlNo" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="File Name" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="File Type" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%# Bind("TYPE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="File Path" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPath" runat="server" Text='<%# Bind("PATH") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:LinkButton runat="server" CommandName="Delete" ID="lnkDelet" ToolTip="Delete">
                                        <img src="../img/Manual/Reject.png" style="width:20px" /></asp:LinkButton>
                                                                <asp:LinkButton runat="server" CommandName="View" ID="lnkView" ToolTip="Download" Visible="false">
                                         <img src="../img/Manual/Pdficon.png" style="width:20px" /></asp:LinkButton>
                                                            </center>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <center>
                                                                <asp:Label ID="lblHeader" runat="server" Text="Action"></asp:Label>
                                                            </center>
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                
                                     <div class="space20">
                                    </div>
                                      <div class="span10">
                                <asp:Label ID="lblNote" runat="server" Font-Bold="true" Text="" ForeColor="Red"></asp:Label>
                            </div>
                            </div>
                            <div class="space20"></div>
                                </div>
                                 <div id="divQuantityUpload" runat="server" visible="false">
                                        <div class="row-fluid">
                                            <div class="span1"></div>                                        
                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Click here to Download Excel format<span class='Mandotary'>*</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:Button ID="btnDownload" runat="server" Text="Download File" 
                                                         CssClass="btn btn-primary" OnClick="cmdDownload_Click" />            
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                        
                                             <div class="span6">
                                                <div class="control-group">
                                                    <label class="control-label">Select Excel File</label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:FileUpload ID="FtpUpload" runat="server" />
                                                            <asp:Button ID="btnUpload" class="btn btn-primary" runat="server" Text="Add" OnClick="cmdUpload_Click" />
                                                        </div>
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
            <!-- END PAGE CONTENT-->



            <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div id="divMaterialCost" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Material Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>
                                                        <asp:GridView ID="grdMaterialMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"
                                                            AllowSorting="true" OnRowDataBound="grdMaterialMast_RowDataBound">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtMaterialName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="200px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_ID" HeaderText="Material Item Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMaterialItemId" runat="server" Text='<%# Bind("MRIM_ITEM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtMQuantity" runat="server" Width="100px" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMqty" runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Quantity" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("RESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBaserate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblMatTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblMaterialTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Select Data">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:CheckBox ID="chkmaterial" runat="server" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkmaterial" runat="server" />
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
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>
                    <div id="divLabourCost" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Labour Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>
                                                        <asp:GridView ID="grdLabourMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"
                                                            AllowSorting="true" OnRowDataBound="grdLabourMast_RowDataBound">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabourId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtLabourName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabourName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="200px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_ID" HeaderText="Labour Item Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabourItemId" runat="server" Text='<%# Bind("MRIM_ITEM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtLqty" runat="server" Width="100px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLqty" runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Quantity" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabQuantity" runat="server" Text='<%# Bind("RESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabrate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabtax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>


                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblMabTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllabtotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLabourTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Select Data">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:CheckBox ID="chkElabour" runat="server" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkElabour" runat="server" />
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
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>
                    <div id="divSalvages" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Salvages Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>


                                                        <asp:GridView ID="grdSalvageMast" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"
                                                            AllowSorting="true" OnRowDataBound="grdSalvageMast_RowDataBound">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Material Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalvageId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Material Name "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtSalvageName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalvageName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="200px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_ID" HeaderText="Salvage Item Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalvageItemId" runat="server" Text='<%# Bind("MRIM_ITEM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtSqty" runat="server" Width="100px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSqty" runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Quantity" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalQuantity" runat="server" Text='<%# Bind("RESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsalunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsalrate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsaltax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSalTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsaltotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSalvageTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all; font-weight: bold;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Select Data">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:CheckBox ID="chkESalvage" runat="server" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkESalvage" runat="server" />
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                    <div class="space20"></div>
                                                    <div>
                                                        <asp:Label ID="lbltotMsg" Font-Bold="true" runat="server" Text="Total Charges ( Material + Labour - Salvage ) = "></asp:Label>
                                                        <asp:Label ID="lblTotalAmount" Font-Bold="true" runat="server" Text=""></asp:Label>
                                                </div>
                                                </div>
                                            </div>
                                            <div class="space20"></div>
                                            <!-- END FORM-->




                                        </div>
                                    </div>

                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>




                     <div id="Oilcost" runat="server">
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORMPORTLET-->
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4><i class="icon-reorder"></i>Oil Cost Details</h4>
                                        <span class="tools">
                                            <a href="javascript:;" class="icon-chevron-down"></a>
                                        </span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="row-fluid">
                                                    <div>


                                                        <asp:GridView ID="GridOil" AutoGenerateColumns="false" PageSize="10" AllowPaging="false"
                                                            EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            runat="server" ShowHeaderWhenEmpty="True"  ShowFooter="true"
                                                            OnRowDataBound="grdOilMast_RowDataBound">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ID" HeaderText="Oil Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilId" runat="server" Text='<%# Bind("MRIM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_NAME" HeaderText="Oil Type "
                                                                    Visible="true" SortExpression="MRIM_ITEM_NAME">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtOilName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilName" runat="server" Text='<%# Bind("MRIM_ITEM_NAME") %>'
                                                                            Style="word-break: break-all;" Width="250px"></asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField AccessibleHeaderText="MRIM_ITEM_ID" HeaderText="Oil Item Id">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilItemId" runat="server" Text='<%# Bind("MRIM_ITEM_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:TextBox ID="txtOqty" runat="server" Width="100px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtOqty" runat="server" Width="100px" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Quantity" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilQuantity" runat="server" Text='<%# Bind("RESTM_ITEM_QNTY") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilunitName" runat="server" Text='<%# Bind("MD_NAME") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilunit" runat="server" Text='<%# Bind("MRI_MEASUREMENT") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Base Rate">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOilrate" runat="server" Text='<%# Bind("MRI_BASE_RATE") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                      <FooterTemplate>
                                                                        <asp:Label ID="lblOilTot" Font-Bold="true" runat="server" Text="TOTAL"
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate" Visible="false">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOiltax" runat="server" Text='<%# Bind("MRI_TAX") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                  

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total">

                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOiltotal" runat="server" Text='<%# Bind("MRI_TOTAL") %>'
                                                                            Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblOilTotal" Font-Bold="true" runat="server"
                                                                            Style="word-break: break-all; font-weight: bold;" Width="100px"></asp:Label>
                                                                        </center>
                                                                    </FooterTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Select Data">
                                                                    <EditItemTemplate>
                                                                        <center>
                                                                            <asp:CheckBox ID="chkEOil" runat="server" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkEOil" runat="server" />
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                    <div class="space20"></div>
                                                    <div>
<%--                                                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Total Charges ( Material + Labour - Salvage ) =   "></asp:Label>--%>
                                                        <asp:Label ID="lbloilfinaltotal" Font-Bold="true" runat="server" Text="" Visible="false"></asp:Label>
                                                    </div>
                                                    <div>
                                                        <asp:Label ID="lblMessageDisplay" Font-Bold="true" runat="server" Text="Total Charges { ( Material + Labour - Salvage )+ Oil Cost } =   "></asp:Label>
                                                        <asp:Label ID="lblTotalCharges" Font-Bold="true" runat="server" Text=""></asp:Label>
                                                </div>
                                                </div>
                                                 
                                            </div>
                                            <div class="space20"></div>
                                            <!-- END FORM-->




                                        </div>
                                    </div>

                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

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


                <asp:Button ID="cmdCalc" runat="server" Text="Calculate"
                    CssClass="btn btn-primary" OnClick="cmdCalc_Click" />

                <asp:Button ID="cmdSave" runat="server" Text="Save"
                    CssClass="btn btn-primary" OnClick="cmdSave_Click" />

                <%--      <asp:Button ID="cmdViewPGRS" runat="server" Text="View PGRS"
                        CssClass="btn btn-primary" TabIndex="13" OnClick="cmdViewPGRS_Click" />--%>

                <asp:Button ID="cmdReset" runat="server" Text="Reset"
                    CssClass="btn btn-primary" OnClick="cmdReset_Click" /><br />

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            </div>

        </div>
    </div>
</asp:Content>
