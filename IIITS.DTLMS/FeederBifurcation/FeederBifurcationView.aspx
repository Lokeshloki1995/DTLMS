<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="FeederBifurcationView.aspx.cs" Inherits="IIITS.DTLMS.FeederBifurcation.FeederBifurcationView" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script type="text/javascript">
         function characterAndnumbers(evt) {
             evt = (evt) ? evt : event;
             var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
             ((evt.which) ? evt.which : 0));
             if (charCode > 31 && (charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode > 31 && (charCode < 48 || charCode > 57)) {

                 return false;
             }
             return true;
         }

         // Dtc allow Chatractes and Numbers to paste
         function cleanSpecialChars(t) {
             debugger;
             t.value = t.value.toString().replace(/[^a-zA-Z 0-9\t\n\r]+/g, '');
             //alert(" Special charactes are not allowed!");
         }
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Feeder Bifurcation View
                </h3>
                <%--<a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>--%>
                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <%--<form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button"><i class="icon-search"></i></button>
                            </div>
                        </form>--%>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>

        <div class="row-fluid">
            <div class="span12">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i> Feeder Bifurcation View</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>

                </div>


                <asp:GridView ID="grdFdrView" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                    AutoGenerateColumns="false" PageSize="10" ShowFooter="true"
                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                    runat="server"
                    AllowSorting="true" OnRowCommand="grdFdrView_RowCommand"  onpageindexchanging="grdFdrView_PageIndexChanging"  OnRowEditing="grdFdrView_RowEditing"  >
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="FBS_ID" HeaderStyle-ForeColor="White"  HeaderText="Feeder Bifurcation ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFBS_ID" runat="server" Text='<%# Bind("FBS_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField AccessibleHeaderText="OLD_FEEDER_CODE"  HeaderText="Old Feeder Code">
                            <ItemTemplate>
                                <asp:Label ID="lblOldFeederCode" runat="server" Text='<%# Bind("OLD_FEEDER_CODE") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                              <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                <asp:TextBox ID="txtOldFeederCode" MaxLength="6" runat="server" placeholder="Enter Old Feeder Code" Width="150px" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"  ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="NEW_FEEDER_CODE"   HeaderText="New Feeder Code" >
                            <ItemTemplate>
                                <asp:Label ID="lblNewFeederCode" runat="server" Text='<%# Bind("NEW_FEEDER_CODE") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                              <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                <asp:TextBox ID="txtNewFeederCode" MaxLength="6" runat="server" placeholder="Enter New Feeder Code" Width="150px"  onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"  ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="COUNT_DTC"    HeaderText="Number of DTC's" >
                            <ItemTemplate>
                                <asp:Label ID="lblNumberofDTC" runat="server" Text='<%# Bind("COUNT_DTC") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="SECTION_OFFICER"    HeaderText="Section Officer" >
                            <ItemTemplate>
                                <asp:Label ID="lblSectionOfficer" runat="server" Text='<%# Bind("SECTION_OFFICER") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                              <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                <asp:TextBox ID="txtSectionOfficer" MaxLength="20" runat="server" placeholder="Enter Section Officer" Width="150px"  onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"  ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="CREATED_ON"     HeaderText="Created On">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Bind("CREATED_ON") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="APPROVED_ON"   HeaderText="Approved On" >
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedOn" runat="server" Text='<%# Bind("APPROVED_ON") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="APPROVED_BY"   HeaderText="Approved By" >
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedBy" runat="server" Text='<%# Bind("APPROVED_BY") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField AccessibleHeaderText="STATUS"   HeaderText="Status" >
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                            </ItemTemplate>
                              <FooterTemplate>
                              <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                                <asp:TextBox ID="txtStatus" MaxLength="20" runat="server" placeholder="Enter Status" Width="150px"  onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"  ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <center>
                        <%--            <asp:ImageButton ID="imgBtnEdit" tooltip="Click To Approve or View"  ButtonType="LinkButton" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="Edit"
                                        Width="12px" />--%>                  
                                            <asp:ImageButton  ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="Edit" Width="12px" />
                                     <asp:LinkButton runat="server" tooltip="Click To Download report" CommandName="GenerateReport"  ID="lnkReportDownload"  visible="true" >
                                      <img id="Img1" src="../img/Manual/Pdficon.png" style="width:20px" /></asp:LinkButton>
                                </center>
                            </ItemTemplate>
                            <FooterTemplate>
                                   <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="Search" />
                             </FooterTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>


            </div>
        </div>


    </div>

</asp:Content>
