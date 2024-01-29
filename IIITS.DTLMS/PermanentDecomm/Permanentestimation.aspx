<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master" CodeBehind="Permanentestimation.aspx.cs" Inherits="IIITS.DTLMS.PermanentDecomm.PermanentEstimation" %>

<%@ Register Src="/ApprovalHistoryView.ascx" TagName="ApprovalHistoryView" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
   
        <div class="container-fluid">

            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Create Permanent Estimation
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
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-danger" OnClick="cmdClose_Click" />
                </div>

            </div>
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
                                                <div class="controls">
                                                    <div class="input-append">
                                                        sWFOAutId
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                        <asp:TextBox ID="txtDtcId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
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
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfAppDesc" runat="server" />
                                                        <asp:HiddenField ID="hdfEstId" runat="server" />
                                                        <asp:HiddenField ID="hdfStatusflag" runat="server" />
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"
                                                            OnClick="cmdSearch_Click" /><br />
                                                       <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>
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

                                                    </div>
                                                </div>
                                                </div>


                                                 <div class="control-group">
                                               

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                     
                                            
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
                                               </div>
                                              
                                                     <div class="span5">
                                              <div class="control-group" style="display:none">
                                                <label class="control-label">Declared By</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeclaredBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                                                                            <br />
                                                   
                                                    </div>
                                                </div>
                                            </div>
                                                       <br />
                                                         <br />
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
                                                        <div class="control-group">
                                                            <label class="control-label">Guarantee Type </label>
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

                                                        <div class="control-group" >
                                                            <label class="control-label">Select Wound Type <span class='Mandotary'>*</span></label>

                                                            <div class="controls">
                                                                <div class="input-append">
                                                                    <asp:DropDownList ID="cmbMaterialType" runat="server" OnSelectedIndexChanged="cmbMaterialType_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="0">--select--</asp:ListItem>
                                                                        <asp:ListItem Value="1">Aluminium Wound</asp:ListItem>
                                                                        <asp:ListItem Value="2">Copper Wound</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="control-group">
                                                            <label id="lblRepairer" class="control-label">Repairer</label>

                                                            <div class="controls">
                                                                <div class="input-append">
                                                                    <asp:DropDownList ID="cmbRepairer" runat="server" OnSelectedIndexChanged="cmbRepairer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                </div>
                                                                <br />
                                                                 <asp:LinkButton ID="lnkBudgetstat" runat="server"  
                                                        style="font-size:12px;color:Blue" onclick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                            </div>
                                                        </div>

                                                          <div class="control-group">
                                                               <label id="lblreasons" class="control-label">Reasons</label>
                                                                <div class="controls">
                                                                      <div class="input-append">
                                                                <asp:TextBox ID="txtreason" runat="server" MaxLength="250" TabIndex="4" TextMode="MultiLine"
                                                                    Width="210px" Height="55px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
                                                            </div>
                                                                    </div>
                                                              </div>

                                                        <div class="control-group" style ="display:none">
                                                            <label id="lblFailType" class="control-label">Select Failure Type <span class='Mandotary'>*</span></label>

                                                            <div class="controls">
                                                                <div class="input-append">
                                                                    <asp:DropDownList ID="cmbFailType" runat="server">
                                                                        <asp:ListItem Value="0">--select--</asp:ListItem>
                                                                        <asp:ListItem Value="1">Minor</asp:ListItem>
                                                                        <asp:ListItem Value="2">Major</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
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
                                      
                                    </div>
                                    </div>
                                </div>
                            </div>
                            <!-- END SAMPLE FORM PORTLET-->
                        </div>
                    </div>

                    <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
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
                                                                                <asp:Label ID="lbllabQuantity" runat="server" Text='<%# Bind("PESTM_ITEM_QNTY") %>'
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

                               <asp:Label ID="lblTotalCharges" Font-Bold="true" runat="server" Text="" Visible="false"></asp:Label>

                                                        </div>
                                                    </div>
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
                       
                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                       
                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" visible="false"></asp:Label>

                    </div>
                </div>
            </div>
    
</asp:Content>









