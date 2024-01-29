<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="AlterNativeSupply.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.AlterNativeSupply" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="space20"></div>

    <div class="container-fluid">

         <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Change Alternative Source
                    </h3>
                    </div>
             <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-danger" OnClick="cmdClose_Click" />
                </div>
             </div>
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i> Alternative Source</h4>
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
                                            <label class="control-label">Transformer Centre Code <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfDTCcode" runat="server" />

                                                    <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="9"></asp:TextBox>
                                                    <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" /><br />
                                                    <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue">View Transformer Centre Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>



                                        <div class="control-group">
                                            <label class="control-label">Transformer Centre Name </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />

                                                    <asp:TextBox ID="txtFailureOfficCode" runat="server" MaxLength="100" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtTCId" runat="server" MaxLength="100" Visible="false" Width="20px"></asp:TextBox>
                                                    <asp:TextBox ID="txtDTCName" runat="server" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Load KW  </label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtLoadKW" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Width="20px" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Load Hp </label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                    <asp:TextBox ID="txtLoadHP" runat="server" ReadOnly="true" onkeypress="return OnlyNumber(event)" MaxLength="10"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA) </label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtCapacity" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Section</label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtLocation" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Condition Of TC</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtConditionOfTC" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">DTr Code </label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfDTRCode" runat="server" />
                                                    <%--<%--ReadOnly="true" : Note: Moved Readonly from the clint side to Server side on 24-08-2023. --%> 
                                                    <asp:TextBox ID="txtTcCode" runat="server" MaxLength="50" ></asp:TextBox>
                                                    <asp:Button ID="cmdDTRSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdDTRSearch_Click" /><br />                                                 
                                                    <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue">View DTr Details</asp:LinkButton>
                                                    
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">DTr Make </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    <asp:TextBox ID="txtFailurId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
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
                                            <label class="control-label">Last Repaired By </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtLastRepairer" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Last Repaired Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtLastRepairDate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Rating</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtrate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Alternate Replacement<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbReplaceEntry" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                                                 <div class="text-center">

           
                <asp:Button ID="cmdSave" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
         
           
                <asp:Button ID="cmdReset" runat="server" Text="Reset"
                    CssClass="btn btn-danger" />

         
        
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

        </div>

                                <div class="span1"></div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
           
        </div>
        




        <!-- END FORM-->




    </div>
</asp:Content>
