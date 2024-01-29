<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/DTLMS.Master" CodeBehind="BankCreate.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.BankCreate" %>

 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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

    <style>
          .modalPopup
    {
           background: #fff; 
        width: 550px;
        height: 500px;
    }
    </style>

    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">



        function ValidateMyForm() {

            if (document.getElementById('<%= txtBankName.ClientID %>').value.trim() == "") {
                alert('Enter Valid Bank Name')
                document.getElementById('<%= txtBankName.ClientID %>').focus()
                 return false
             }


             if (document.getElementById('<%= cmbsubDivision.ClientID %>').value == "--Select--") {
                alert('Select sub Division Name')
                document.getElementById('<%= cmbsubDivision.ClientID %>').focus()
                 return false
             }
            if (document.getElementById('<%=cmbStore.ClientID %>').value == "--Select--") {
                alert('Select store Name')
                document.getElementById('<%= cmbStore.ClientID %>').focus()
                return false
            }
             if (document.getElementById('<%= txtBankDescription.ClientID %>').value.trim() == "") {
                alert('Enter Valid Description')
                document.getElementById('<%= txtBankDescription.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtInchargeName.ClientID %>').value.trim() == "") {
                alert('Enter Bank incharge Name')
                document.getElementById('<%= txtInchargeName.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtEmailId.ClientID %>').value.trim() == "") {
                alert('Enter Valid Email Id')
                document.getElementById('<%= txtEmailId.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
                alert('Enter Mobile No')
                document.getElementById('<%= txtMobile.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtAddress.ClientID %>').value.trim() == "") {
                alert('Enter Valid Address')
                document.getElementById('<%= txtAddress.ClientID %>').focus()
                 return false
             }

         }
    </script>

        <script language="Javascript" type="text/javascript">


            function onlyAlphabets(e, t) {
                var code = ('charCode' in e) ? e.charCode : e.keyCode;
                if (  !(code == 32) && 
                    !(code > 39 && code < 42) &&
                    !(code > 45 && code < 48) &&
                  !(code > 64 && code < 91) && // upper alpha (A-Z)
                  !(code > 96 && code < 123)) { // lower alpha (a-z)
                    e.preventDefault();
                }
            }

            function Address(e, t) {
                var code = ('charCode' in e) ? e.charCode : e.keyCode;
                if ( !(code == 32) &&
                    !(code == 35) &&
                    !(code == 64) &&
                    !(code > 43 && code < 58) &&
                    !(code > 39 && code < 42) &&
                     !(code == 47) &&
                  !(code > 64 && code < 91) && // upper alpha (A-Z)
                  !(code > 96 && code < 123)) { // lower alpha (a-z)
                    e.preventDefault();
                }
            }

              function CheckLength() {
            var textbox = document.getElementById("<%=txtAddress.ClientID%>").value;
            if (textbox.trim().length >= 150) {
                return false;
            }
            else {
                return true;
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
                    <h3 class="page-title" runat="server" id="Create">Create Bank
                    </h3>
                     <h3 class="page-title" runat="server" id="Update">Update Bank
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
                    <asp:Button ID="Button1" runat="server" Text="Bank View"
                        OnClientClick="javascript:window.location.href='BankView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateStore" runat="server"><i class="icon-reorder"></i>Create Bank</h4>
                            <h4 id="UpdateStore" runat="server"><i class="icon-reorder"></i>Update Bank</h4>
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
                                            <div class="control-group" style="display:none">
                                                <label class="control-label">Bank Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">


                                                        <asp:TextBox ID="txtBankId" runat="server" MaxLength="10" Visible="false"></asp:TextBox>

                                                        <asp:TextBox ID="txtStoreCode" runat="server" MaxLength="10"
                                                            onkeypress="javascript:return OnlyNumber(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Bank Name<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtBankName" runat="server" MaxLength="25" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbsubDivision" runat="server" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" MaxLength="25"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Address<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAddress" runat="server" onkeypress="return Address(event,this);" Columns="50" Rows="2" Style="resize: none" TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Bank Description<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtBankDescription" runat="server" onkeyup="return ValidateTextlimit(this,250);" Style="resize: none" TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Store<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Visible="TRUE">
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                        </div>

                                        <%-- another span--%>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Incharge Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtInchargeName" runat="server" onkeypress="javascript:return AllowSpecialchar(event);"
                                                            MaxLength="100"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Email Id<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" onkeypress="javascript:return validateEmail(txtEmailId);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Mobile<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtMobile" runat="server" onkeypress="javascript:return OnlyNumber(event);" MaxLength="10"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                           <%-- <div class="control-group">
                                                <label class="control-label">Phone No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="11" onkeypress="javascript:return OnlyNumberHyphen(this,event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>--%>

                                           

                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center">

                                      
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click"
                                                OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                               
                              
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                       
                                     
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                            <div class="space20"></div>
                            <!-- END FORM-->


                        </div>
                    </div>

                    <!-- END SAMPLE FORM PORTLET-->
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
        <div  style="width: 100%; vertical-align: middle" align="center">

            <asp:Panel ID="pnlControls" runat="server" CssClass="modalPopup" align="center" style = "display:none">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>Select Office Codes And Click On Proceed</h4>
                        <div class="space20"></div>


                        <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%"
                            AllowPaging="True" DataKeyNames="OFF_CODE" OnPageIndexChanging="GrdOffices_PageIndexChanging" OnRowCommand="GrdOffices_RowCommand" OnRowDataBound="GrdOffices_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Office Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name" Visible="true">
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

                        <div class="space20"></div>

                        <div class="row-fluid">
                            <div class="span1"></div>
                            <div class="span2">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" style="left: 0px; top: 0px" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span2">

                                <div class="control-group">

                                    <div class="controls">
                                        <div class="input-append">
                                           
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