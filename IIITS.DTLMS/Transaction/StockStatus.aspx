<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StockStatus.aspx.cs" Inherits="IIITS.DTLMS.Transaction.StockStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table td {
            text-align: center;
        }

        .table th {
            text-align: center;
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
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Stock Status
                    </h3>
                 
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="Text1" type="text">
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
                            <h4><i class="icon-reorder"></i>Stock Status
                                   <a style="float:right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px;padding:0px 0px 0px 3px!important"></i></a>
                            </h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">



                            <div style="float: right">
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info" Visible ="true"
                                        OnClick="Export_clickStockStatus" /><br />
                                </div>


                            </div>

                            <%--  <div class="space20"> </div>--%>

                            <!-- END FORM-->

                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <%-- <div class="span1"></div>--%>
                                    <div class="span5" style="visibility:hidden">
                                        <div class="control-group">
                                            <label class="control-label">Store Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdStore" runat="server" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="cmdStore_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5" style="visibility: hidden">
                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="cmbCapacity_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                            <asp:GridView ID="grdStockDetails" AutoGenerateColumns="false" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false" EmptyDataText="No Records Found"
                                runat="server" AllowSorting="true" OnRowDataBound="grdStockDetails_RowDataBound" OnRowCommand="grdStockDetails_RowCommand">
                               
                                <Columns>
                                    

                                    <asp:TemplateField AccessibleHeaderText="SM_NAME" HeaderText="STORE NAME">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblsmname" runat="server" Text='<%# Bind("SM_NAME") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblStoreName" runat="server" Text="TOTAL" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="10" HeaderText="10">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl10" runat="server" Text='<%# Bind("A") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf10" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="15" HeaderText="15">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl15" runat="server" Text='<%# Bind("B") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf15" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="25" HeaderText="25">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl25" runat="server" Text='<%# Bind("C") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf25" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="50" HeaderText="50">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl50" runat="server" Text='<%# Bind("D") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf50" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="63" HeaderText="63">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl63" runat="server" Text='<%# Bind("E") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf63" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="100" HeaderText="100">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl100" runat="server" Text='<%# Bind("F") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf100" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="125" HeaderText="125">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl125" runat="server" Text='<%# Bind("G") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf125" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="150" HeaderText="150">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl150" runat="server" Text='<%# Bind("H") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf150" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="160" HeaderText="160">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl160" runat="server" Text='<%# Bind("I") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf160" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="200" HeaderText="200">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl200" runat="server" Text='<%# Bind("J") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf200" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="250" HeaderText="250">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl250" runat="server" Text='<%# Bind("K") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf250" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="300" HeaderText="300">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl300" runat="server" Text='<%# Bind("L") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf300" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="315" HeaderText="315">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl315" runat="server" Text='<%# Bind("M") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf315" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="400" HeaderText="400">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl400" runat="server" Text='<%# Bind("N") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf400" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="500" HeaderText="500">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl500" runat="server" Text='<%# Bind("O") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf500" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="630" HeaderText="630">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl630" runat="server" Text='<%# Bind("P") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf630" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="750" HeaderText="750">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl750" runat="server" Text='<%# Bind("Q") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf750" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="1000" HeaderText="1000">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl1000" runat="server" Text='<%# Bind("R") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf1000" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="1250" HeaderText="1250">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl1250" runat="server" Text='<%# Bind("S") %>' CommandName="View"></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblf1250" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="TOTAL">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblTOTAL" runat="server" Text='<%# Bind("TOTAL") %>'></asp:LinkButton></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblfTOTAL" runat="server" style="font-weight:bold"></asp:Label></FooterTemplate>
                                    </asp:TemplateField>



                                    <%--<asp:BoundField DataField="SM_NAME" HeaderText="STORE NAME" ItemStyle-BorderStyle="Solid" SortExpression="SM_NAME" />--%>
                                    <%--<asp:BoundField DataField="10" HeaderText="10" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="15" HeaderText="15" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="25" HeaderText="25" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="50" HeaderText="50" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="63" HeaderText="63" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="100" HeaderText="100" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="125" HeaderText="125" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="150" HeaderText="150" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="160" HeaderText="160" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="200" HeaderText="200" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="250" HeaderText="250" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="300" HeaderText="300" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="315" HeaderText="315" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="400" HeaderText="400" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="500" HeaderText="500" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="630" HeaderText="630" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="750" HeaderText="750" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="1000" HeaderText="1000" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="1250" HeaderText="1250" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" ItemStyle-BorderStyle="Solid" />--%>
                                </Columns>

                            </asp:GridView>

                            <asp:GridView ID="grdStockStatus"
                                AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false" EmptyDataText="No Records Found"
                                runat="server" OnPageIndexChanging="grdStockStatus_PageIndexChanging" OnDataBound="grdStockStatus_DataBound1"
                                OnSorting="grdStockStatus_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:BoundField DataField="SM_NAME" HeaderText="Store Name" ItemStyle-BorderStyle="Solid" SortExpression="SM_NAME" />
                                    <%--                                    <asp:BoundField DataField="SM_OFF_CODE" HeaderText="Location" ItemStyle-BorderStyle="Solid" SortExpression="SM_OFF_CODE"/>--%>
                                    <asp:BoundField DataField="TC_CAPACITY" HeaderText="Capacity(in KVA)" ItemStyle-BorderStyle="Solid" />
                                    <asp:BoundField DataField="TC_CODE" HeaderText="StockCount" ItemStyle-BorderStyle="Solid" />
                                    <%--<asp:TemplateField AccessibleHeaderText="SM_ID" HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStoreId" runat="server" Text='<%# Bind("SM_ID") %>'></asp:Label>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                    

                                    <asp:TemplateField AccessibleHeaderText="Store Name" HeaderText="Store Name">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("SM_NAME") %>'></asp:Label>
                                                
                                        </ItemTemplate>
                                          <FooterTemplate>
                                           <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtstoreName" runat="server"  Width="150px"  placeholder="Enter store Name" ToolTip="Enter Name to Search"></asp:TextBox>
                                        </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                   
                                  <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location">
                                       
                                        <ItemTemplate>

                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("SM_OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                           <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch" >
                                           <asp:TextBox ID="txtLocation" runat="server"  Width="150px"  placeholder="Enter Location  Name" ToolTip="Enter Location Name to Search"></asp:TextBox>
                                           </asp:Panel>
                                           
                                         </FooterTemplate>   
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="Capacity" HeaderText="Capacity(in KVA)">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9"/>
                                    </FooterTemplate>  
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="StockCount" HeaderText="Stock Count">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockCount" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                           
                                    </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                CausesValidation="false" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>



                                    <%-- <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="CAPACITY">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblcapacity" runat="server" Style="text-align:center" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                                
                                        </ItemTemplate>
                                         
                                    </asp:TemplateField>
                                   
                                  <asp:TemplateField AccessibleHeaderText="STORENAME" HeaderText="STORENAME">
                                       
                                        <ItemTemplate>

                                            <asp:Label ID="lblStoreName" runat="server" Style="width:20px" Text='<%# Bind("STORENAME") %>'></asp:Label>
                                        </ItemTemplate>
                                           
                                    </asp:TemplateField>--%>
                                </Columns>

                            </asp:GridView>

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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View Stock Status
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

</asp:Content>
