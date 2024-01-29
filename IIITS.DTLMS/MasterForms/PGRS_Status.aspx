<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PGRS_Status.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.PGRS_Status" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="../Scripts/functions.js" type="text/javascript"></script>

                <script type="text/javascript">
        $(document).ready(function () {

            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })
        });
       
    </script>
   <script  type="text/javascript">
       $(".card__container--closed").on('click', function () {

       });
   </script>

        <style type="text/css">
        table#ContentPlaceHolder1_grdPGRSDetails {
    table-layout: fixed;
    overflow: scroll;
    display: -webkit-inline-box;
    width: 100%!important;
}
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <br />

    <div class="row-fluid">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
                           <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 30px; color: white"></i></a>
            <div class="widget blue">
                <div class="widget-title">
                    <h4>
                        <i class="icon-reorder"></i>DISTRIBUTION TRANSFORMERS </h4>
     

                    <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                </div>
                <div class="widget-body">
                    <div class="widget-body form">
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span1">
                                </div>
                                <div class="span5">

                                    <div class="control-group">
                                        <label class="control-label">
                                            PGRS Number</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtPgrsNo" runat="server" MaxLength="10"> </asp:TextBox>

                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="span5">

                                    <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="cmdSearch_Click" />
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                </div>

                                <%--   </div>
                                <div class="span1">
                                    </div>  
                                   </div>--%>

                           <div class="space20"></div>
                           <div class="space20"></div>
                                   <div class="space20"></div>
                           <div class="space20"></div>

                                <asp:GridView ID="grdPGRSDetails" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                    ShowFooter="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server">
                                    <HeaderStyle CssClass="both" />
                                    <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DTC CODE" HeaderText="DTC CODE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="DTR CODE" HeaderText="DTR CODE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("DF_EQUIPMENT_ID") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="FAILURE DATE" HeaderText="FAILURE DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlDfDate" runat="server" Text='<%# Bind("DF_DATE") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DTR SLNO" HeaderText="DTR SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PGRS NUMBER" HeaderText="PGRS NUMBER" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpgrsDocket" runat="server" Text='<%# Bind("DF_PGRS_DOCKET") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="PGRS DATE" HeaderText="PGRS DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocketDate" runat="server" Text='<%# Bind("DF_PGRS_DOCKET_DATE") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TIMS CODE" HeaderText="TIMS CODE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlTimeCode" runat="server" Text='<%# Bind("DT_TIMS_CODE") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PGRS UPDATE TIME" HeaderText="PGRS UPDATE TIME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUpdateTime" runat="server" Text='<%# Bind("DF_PGRS_UPDATE_TIME") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PGRS STATUS" HeaderText="PGRS STATUS" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lnlStatus" runat="server" Text='<%# Bind("DF_PGRS_STATUS") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DESCRIPTION" HeaderText="DESCRIPTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("DF_PGRS_DESCRIPTION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>



                        <!-- END FORM-->
                    </div>
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
        </div>
    </div>

</asp:Content>
