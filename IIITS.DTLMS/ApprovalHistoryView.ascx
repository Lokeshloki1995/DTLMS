<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApprovalHistoryView.ascx.cs" Inherits="IIITS.DTLMS.ApprovalHistoryView" %>
<style>
    table#ContentPlaceHolder1_ApprovalHistoryView_grdApprovalHistory tr td:nth-child(4) {
    width: 50%;
}
</style>
<div class="row-fluid">
    <div class="span12">
        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget blue">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Approval History</h4>
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
                            <%-- <div class="span1"></div>--%>
                         <%--   <div class="span12" runat="server" id="dvDTCPanel">

                                <asp:Label ID="lblDTCLoad" runat="server" Text="DTC Code"></asp:Label>
                                &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtDTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                &nbsp&nbsp&nbsp&nbsp 
                                   
                                   <asp:Label ID="lbl3" runat="server" Text="DTC Name"></asp:Label>
                                &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>
                                &nbsp&nbsp&nbsp&nbsp 
                                 
                                   <asp:Label ID="lbl2" runat="server" Text="DTr Code"></asp:Label>
                                &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtDTrCode" runat="server" ReadOnly="true"></asp:TextBox>
                                &nbsp&nbsp&nbsp&nbsp 
                            </div>
                            <div class="space20"></div>
                            <div class="span12">
                                <asp:Label ID="Label1" runat="server" Text="Work Name :" ForeColor="slategray" Font-Size="15px"></asp:Label>
                                <asp:Label ID="lblWorkName" runat="server" ForeColor="#2FADE7" Font-Size="15px"></asp:Label>
                            </div>

                            <div class="span12">
                                <asp:Label ID="Label2" runat="server" Text="Current Status :" ForeColor="slategray" Font-Size="15px"></asp:Label>
                                <asp:Label ID="lblCurrentStatus" runat="server" ForeColor="#2FADE7" Font-Size="15px"></asp:Label>
                                <asp:TextBox ID="txtRecordId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtBOId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                            </div>--%>
                            <asp:HiddenField ID="hdfDataID" runat="server" />
                            <asp:HiddenField ID="hdfAutoID" runat="server" />
                            <div class="space20"></div>
                            <asp:GridView ID="grdApprovalHistory"
                                AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdApprovalHistory_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ACTIVITY" HeaderText="Activity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("BONAME") %>' Style="word-break: break-all"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="INITIATOR" HeaderText="Approver">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("INITIATOR") %>' Style="word-break: break-all" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_CR_ON" HeaderText="Created On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("WO_CR_ON") %>' Style="word-break: break-all" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_USER_COMMENT" HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDTrCode" runat="server"  Text='<%# Bind("WO_USER_COMMENT") %>' Style="word-break: break-all" ></asp:Label>
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
