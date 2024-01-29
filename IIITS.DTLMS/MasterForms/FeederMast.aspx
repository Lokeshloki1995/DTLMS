<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FeederMast.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.FeederMast" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <script type="text/javascript">
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>

    <script>
        function onlyAlphabets(e, t) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code == 32) &&
                !(code > 39 && code < 42) &&
                !(code > 45 && code < 48) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }
        function AlphanumericLEC(e) {
            debugger;
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (
              !(code == 32) &&
              !(code == 45) &&
              !(code > 46 && code < 58) &&
              !(code > 64 && code < 91) && // upper alpha (A-Z)
              !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }
        function preventSpecialCharacters(e) {
            debugger;
            const allowedChars = /^[a-zA-Z0-9]+$/;
            const pressedKey = e.key;

            if (!allowedChars.test(pressedKey)) {
                e.preventDefault();
            }
        }
    </script>

    <style type="text/css">
        .modalBackground {
            /* background-color: Gray; */
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background: #fff;
            width: 600px;
            height: 500px;
        }
    </style>

    <script type="text/javascript">



        function Validate() {

            if (document.getElementById('<%= cmbdistrict.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbdistrict.ClientID %>').value.trim() == "") {
                alert('Select the   District')
                document.getElementById('<%= cmbdistrict.ClientID %>').focus()
        return false
    }


    if (document.getElementById('<%=cmbTaluk.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%=cmbTaluk.ClientID %>').value.trim() == "") {
                alert('Select  the Taluk')
                document.getElementById('<%= cmbTaluk.ClientID %>').focus()
        return false
    }

    if (document.getElementById('<%= cmbStation.ClientID %>').value.trim() == "--Select--" || document.getElementById('<%= cmbStation.ClientID %>').value.trim() == "") {
                alert('Select the Station')
                document.getElementById('<%= cmbStation.ClientID %>').focus()
                return false
            }

        }
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title" runat="server" id="Create">Feeder Master
                    </h3>
                    <h3 class="page-title" runat="server" id="Update">Update Feeder Master
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
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdFeederView" class="btn btn-primary" Text="Feeder View"
                        OnClientClick="javascript:window.location.href='FeederView.aspx'; return false;" runat="server" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateFeeder" runat="server"><i class="icon-reorder"></i>Feeder Master</h4>
                            <h4 id="UpdateFeeder" runat="server"><i class="icon-reorder"></i>Update Feeder Master</h4>
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
                                                <label class="control-label">District Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbdistrict" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbDistrict_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Taluk Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTaluk" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbTaluk_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <!--AFTER DIST TALUK -->
                                            <div class="control-group">
                                                <label class="control-label">Station Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStation" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbStation_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtFeederId" runat="server" MaxLength="25" Visible="false">0</asp:TextBox>
                                                        <asp:HiddenField ID="hdfTaluk" runat="server" />
                                                        <asp:HiddenField ID="hdfStation" runat="server" />
                                                        <asp:HiddenField ID="hdfBank" runat="server" />
                                                        <asp:HiddenField ID="hdfBus" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" id="dbank" runat="server" visible="False">
                                                <label class="control-label">Bank Name<span> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbBank" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbBank_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" id="dbus" runat="server" visible="False">
                                                <label class="control-label">Bus Name<span></span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbbus" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Office Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" MaxLength="25"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label"></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:Label ID="lblSlno" runat="server"></asp:Label>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFeederCode" runat="server" MaxLength="6"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Feeder Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFeederName" runat="server" MaxLength="100" oonkeypress="javascript:return AllowOnlyAlphanumericNotAllowSpecial(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Feeder Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbType_SelectedIndexChanged">
                                                            <%--   <asp:ListItem Text="---SELECT---" Value="0" />
                                    <asp:ListItem  Value="Rural mixed" />
                                    <asp:ListItem  Value="IPP" />
                                    <asp:ListItem  Value="Urban" />--%>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Feeder Category<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbCat" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Shared<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbInt" runat="server">
                                                            <asp:ListItem Text="---SELECT---" Value="0" />
                                                            <asp:ListItem Text="Yes" Value="1" />
                                                            <asp:ListItem Text="No" Value="2" />

                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Connected Transformer Centre capacity (KVA)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--<asp:TextBox ID="txtCapacity" runat="server" MaxLength="4" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>  --%>
                                                        <asp:DropDownList ID="cmbCapacity" runat="server">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">MDM FeederCode<span> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtMdmFeederCode" runat="server" MaxLength="25" onkeypress="preventSpecialCharacters(event);"></asp:TextBox>
                                                        <%--  <asp:DropDownList ID="cmbCapacity" runat="server" >                                      
                                  </asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Total No of Transformer Centre<span> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTotalDtc" runat="server" MaxLength="25" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <%--  <asp:DropDownList ID="cmbCapacity" runat="server" >                                      
                                  </asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center">


                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                            OnClientClick="javascript:return Validate()" CssClass="btn btn-primary"
                                            OnClick="cmdSave_Click" />



                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />

                                        <div class="span7"></div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                    </div>

                                    <div class="space20"></div>
                                    <div class="form-horizontal">
                                        <div class="span8">
                                            <asp:Label ID="lblTxt" runat="server" ForeColor="Green" Text="Note : Add or Replace Office Code by Selecting/DeSelecting from checkbox"></asp:Label>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                            <%-- <asp:Panel ID="pnlControls" runat="server" Height="500px"  BackColor="White">
                            --%>
                        </div>
                        <div class="space20"></div>

                    </div>
                </div>
            </div>

            <!-- END PAGE CONTENT-->
        </div>

        <div class="space20"></div>
        <div class="space20"></div>
        <div class="space20"></div>
        <div class="space20"></div>

        <asp:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup" CancelControlID="cmdClose"
            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
        <div style="width: 100%; vertical-align: middle" align="center">



            <asp:Panel ID="pnlControls" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>Select Office Codes And Click On Proceed</h4>
                        <div class="space20"></div>


                        <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" OnPageIndexChanging="GrdOffices_PageIndexChanging" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%" OnRowDataBound="GrdOffices_RowDataBound"
                            AllowPaging="True" DataKeyNames="OFF_CODE" OnRowCommand="GrdOffices_RowCommand">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Subdivision Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Subdivision Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStaDesc" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all" Width="150px"> </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Office Name" Width="200px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>


                        <%--    <div style="background:#fff!important"class="row">--%>
                        <div class="row-fluid">
                            <div class="span1"></div>
                            <div class="span2">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" Style="left: 0px; top: 0px" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span2">

                                <div class="control-group">

                                    <div class="controls">
                                        <div class="input-append">
                                            <%--onclick="btnClose_Click"--%>
                                            <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary" Text="Cancel" />

                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>

        </div>


    </div>


</asp:Content>
