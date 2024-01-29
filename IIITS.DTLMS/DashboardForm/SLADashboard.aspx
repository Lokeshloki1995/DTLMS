<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="SLADashboard.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.SLADashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">SLA
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

        </div>
    </div>




    <asp:ScriptManager ID="scriptmgr" runat="server"></asp:ScriptManager>
    <div class="col-md-10">
        <div class="space10"></div>
        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget blue">
            <div class="widget-title">
                <h4>
                    <i class="icon-reorder"></i>ABSTRACT</h4>
                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                    class="icon-remove"></a></span>
            </div>
            <div class="widget-body">


                <div class="widget-body form">
                    <!-- BEGIN FORM-->
                    <div class="form-horizontal">
                        <div class="row-fluid">

                            <asp:HiddenField ID="hdfLocationCode" runat="server" />

                            <asp:HiddenField ID="hdoffcode" runat="server" />
                            <asp:HiddenField ID="hdmoduleid" runat="server" />

                            <asp:GridView ID="GridSlaAbstarct" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                PageSize="10" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="FAILURE" HeaderText="FAILURE" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="ESTIMATION" HeaderText="ESTIMATION" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="MINOR WORK ORDER" HeaderText="MINOR WORK ORDER" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="RECEIVE DTR" HeaderText="RECEIVE DTR" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="INVOICE DTR" HeaderText="INVOICE DTR" ItemStyle-Width="20" />


                                    <asp:BoundField DataField="MAJOR WORK ORDER" HeaderText="MAJOR WORK ORDER" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="INDENT" HeaderText="INDENT" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="COMMISSION" HeaderText="COMMISSION" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="DECOMMISSION" HeaderText="DECOMMISSION" ItemStyle-Width="20" />
                                    <asp:BoundField DataField="RI" HeaderText="RI" ItemStyle-Width="20" />
                                      <asp:BoundField DataField="CR" HeaderText="CR" ItemStyle-Width="20" />




                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="col-md-10">
        <div class="space10"></div>
        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget blue">
            <div class="widget-title">
                <h4>
                    <i class="icon-reorder"></i>DETAILS</h4>
                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                    class="icon-remove"></a></span>
            </div>
            <div class="widget-body">
                <div style="float: right" id="backbtn" runat="server">
                    <div class="span2">
                        <asp:Button ID="cmdBack" runat="server" Text="Back"
                            CssClass="btn btn-primary" OnClick="cmdBack_Click" /><br />
                    </div>

                </div>
                <div class="widget-body form">
                    <!-- BEGIN FORM-->
                    <div class="form-horizontal">
                        <div class="row-fluid">
                            <asp:GridView ID="GrdSlaDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                PageSize="10" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="GrdSlaDetails_SelectedIndexChanging" ShowFooter="true" OnRowCommand="GrdSlaDetails_RowCommand" OnSorting="GrdSlaDetails_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="headoffname" HeaderText="SUB DIVISION" Visible="false"
                                        HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeadName" runat="server" Text='<%# Bind("headoffname") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtsubdiv" runat="server" placeholder="Enter SubDiv Name" Width="100px" MaxLength="9"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="offname" HeaderText="OFFICE" Visible="true"
                                        HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOff" runat="server" Text='<%# Bind("offname") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panesec" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtsection" runat="server" placeholder="Enter the Section" Width="100px" MaxLength="9"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="offcode" HeaderText="offcode" Visible="false"
                                        HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloffcode" runat="server" Text='<%# Bind("offcode") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="bo_order" HeaderText="bo_order" Visible="false"
                                        HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmoduleorder" runat="server" Text='<%# Bind("bo_order") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="boname" HeaderText="PHASE" Visible="true"
                                        HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsamname" runat="server" Text='<%# Bind("boname") %>' Style="word-break: break-all"> </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Panel ID="panelphase" runat="server" DefaultButton="imgBtnSearch">
                                                <asp:TextBox ID="txtphase" runat="server" placeholder="Enter the phase" Width="100px" MaxLength="9"></asp:TextBox>
                                            </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="three_days" HeaderText="PENDING < 3 DAYS" Visible="true"
                                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#4ef14e">
                                        <ItemTemplate>
                                            <%-- <asp:Label ID="lblthree" runat="server" Text='<%# Bind("three_days") %>' Style="word-break: break-all"> </asp:Label>--%>
                                            <asp:LinkButton ID="lblthree" runat="server" Text='<%# Bind("three_days") %>' Style="word-break: break-all;"
                                                CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="seven_days" HeaderText="PENDING FROM 3 TO 7 DAYS" Visible="true"
                                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#ff5722">
                                        <ItemTemplate>
                                            <%--    <asp:Label ID="lblseven" runat="server" Text='<%# Bind("seven_days") %>' Style="word-break: break-all"> </asp:Label>--%>
                                            <asp:LinkButton ID="lblseven" runat="server" Text='<%# Bind("seven_days") %>' Style="word-break: break-all;"
                                                CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="fifteen_days" HeaderText="PENDING FROM 8 TO 15 DAYS" Visible="true"
                                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="red">
                                        <ItemTemplate>
                                            <%-- <asp:Label ID="lblfifteen" runat="server" Text='<%# Bind("fifteen_days") %>' Style="word-break: break-all"> </asp:Label>--%>
                                            <asp:LinkButton ID="lblfifteen" runat="server" Text='<%# Bind("fifteen_days") %>' Style="word-break: break-all;"
                                                CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="thirty_days" HeaderText="PENDING FROM 16 TO 30 DAYS" Visible="true"
                                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="red">
                                        <ItemTemplate>
                                            <%--  <asp:Label ID="lblthirty" runat="server" Text='<%# Bind("thirty_days") %>' Style="word-break: break-all"> </asp:Label>--%>
                                            <asp:LinkButton ID="lblthirty" runat="server" Text='<%# Bind("thirty_days") %>' Style="word-break: break-all;"
                                                CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="more_than_thirty_days" HeaderText="PENDING FROM > 30 DAYS" Visible="true"
                                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="red">
                                        <ItemTemplate>
                                            <%-- <asp:Label ID="lblmorethan" runat="server" Text='<%# Bind("more_than_thirty_days") %>' Style="word-break: break-all"> </asp:Label>--%>
                                            <asp:LinkButton ID="lblmorethan" runat="server" Text='<%# Bind("more_than_thirty_days") %>' Style="word-break: break-all;"
                                                CommandName="view"></asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:templatefield headertext="Number">
				<itemtemplate>
					<%# AddNumberToTotal(Container.DataItem) %>
				</itemtemplate>
				<footerstyle font-bold="true" />
				<footertemplate>
					<%# Total %>
				</footertemplate>
			</asp:templatefield>--%>

                                </Columns>
                            </asp:GridView>




                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>







</asp:Content>
