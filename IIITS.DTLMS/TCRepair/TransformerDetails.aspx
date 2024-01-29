<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransformerDetails.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TransformerDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=btnsubmit.ClientID %>').prop('disabled', true);
         }

         window.onbeforeunload = preventMultipleSubmissions;

    </script>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <h3 class="page-title">Transformer Details
                          <div style="float: right; margin-top: 20px;">
                              <asp:Button ID="Close" runat="server" Text="Close"
                                  OnClick="cmdClose_Click"
                                  CssClass="btn btn-danger" />
                          </div>
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


            </div>
        </div>
        <br />

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Transformer Details</h4>
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
                                            <label class="control-label">DTr Code<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtDtrcode" runat="server" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfdtrid" runat="server" />
                                                    <asp:HiddenField ID="HdfOldGuaranteetype" runat="server" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">DTr Slno<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrslno" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Make Name<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMakename" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA)<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtCapacity" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Star Rate<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtStarrate" runat="server" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>



                                    <div class="span5">




                                        <div class="control-group">
                                            <label class="control-label">Manf Date<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtManfdate" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Repaired Count<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtRepairedcount" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Guarantee Type<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbGuaranteetype" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbGuaranteetype_SelectedIndexChanged" TabIndex="3">
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Remarks<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100"
                                                        ReadOnly="true" TextMode="MultiLine" Style="resize: none; font-weight: bold" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Reason<span class="Mandotary">* </span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtReason" runat="server" MaxLength="100" autocomplete="off"
                                                        TextMode="MultiLine" Style="resize: none"  onkeyup="javascript:ValidateTextlimit(this,100)"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <br />
                                <div class="text-center">

                                    <asp:Button ID="btnsubmit" runat="server" Text="Save" CssClass="btn btn-primary"
                                        Height="30px" Width="80px"
                                        OnClick="btnsubmit_Click" TabIndex="4" />

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
