<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="District.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.District" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        function ValidateMyForm() {

            if (document.getElementById('<%= txtDistrictCode.ClientID %>').value == "0") {
                alert('District Code an not be 0')
                document.getElementById('<%= txtDistrictCode.ClientID %>').focus()
                return false
            }
            
            if (document.getElementById('<%= txtDistrictCode.ClientID %>').value == "") {
                alert('Enter the District Code')
                document.getElementById('<%= txtDistrictCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDistrictName.ClientID %>').value == "") {
                alert('Enter the District Name')
                document.getElementById('<%= txtDistrictName.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbDivsionName.ClientID %>').value == "") {
                alert('Select Division Name')
                document.getElementById('<%= cmbDivsionName.ClientID %>').focus()
                 return false
            }
            if (document.getElementById('<%= cmbDivsionName.ClientID %>').value == "") {
                alert('Select Division Name')
                document.getElementById('<%= cmbDivsionName.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtOfficeCode.ClientID %>').value == "") {
                alert('Select Division Name')
                document.getElementById('<%= txtOfficeCode.ClientID %>').focus()
                return false
            }

           // var DistrictName = document.getElementById('<%= txtDistrictName.ClientID %>').value;
           // var DistrictNamecon = /^([a-zA-Z0-9])+(\s-\s)*([a-zA-Z0-9])+$/;
           // if (!DistrictName.match(DistrictNamecon)) {
               // alert('Enter valid District Name')
               // document.getElementById('<%= txtDistrictName.ClientID %>').focus()
               // return false
           // }

        }
     
    </script>
    
            <script language="Javascript" type="text/javascript">


                function onlyAlphabets(e, t) {
                    var code = ('charCode' in e) ? e.charCode : e.keyCode;
                    if ( // space
                       !(code == 32) &&// Special characters
                        !(code == 35) &&
                        !(code == 64) &&
                        !(code > 43 && code < 60) &&
                        !(code > 37 && code < 42) &&
                         !(code == 47) &&
                      !(code > 64 && code < 94) && // upper alpha (A-Z)
                      !(code > 96 && code < 126)) { // lower alpha (a-z)
                        e.preventDefault();
                    }
                }
            </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>  
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create">
                    Create District 
                </h3>
                 <h3 class="page-title" runat="server" id="Update">
                    Update District 
                </h3>
                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                        <div class="input-append search-input-area">
                            <input class="" id="appendedInputButton" type="text" />
                            <button class="btn" type="button">
                                <i class="icon-search"></i>
                            </button>
                        </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
           
            <div style="float: right; margin-top: 20px; margin-right: 12px">
                  <asp:Button ID="cmdClose" runat="server" Text="District View" 
                                      OnClientClick="javascript:window.location.href='DistrictView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue" >
                    <div class="widget-title" >
                        <h4 id="CreateDistrict" runat="server">
                            <i class="icon-reorder"></i>Create District</h4>
                           <h4 id="UpdateDistrict" runat="server">
                            <i class="icon-reorder"></i>Update District</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
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
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDistId" runat="server" MaxLength="4" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                District Code <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDistrictCode" onkeypress="javascript:return OnlyNumber(event);"
                                                        runat="server" MaxLength="1"></asp:TextBox>
                                                </div>
                                              
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                District Name <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDistrictName" runat="server" MaxLength="30" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span1">
                                    </div>
                                    <div class="span6">
                                        <div class="control-group">
                                             
                                                <label class="control-label">Division Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivsionName" runat="server" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" MaxLength="25" ReadOnly="True" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                </div>
                                <div class="space20">
                                </div>
                                <div class="text-center">
                                  
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                                            CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                           
                       
                                        <asp:Button ID="cmdClear" runat="server" Text="Reset" CssClass="btn btn-danger"
                                            OnClick="cmdClear_Click" /><br />
                           
                            
                                 
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                 <div class="space20">
                                </div>
                            </div>
                             <asp:GridView ID="grdLoadDiv" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%" OnPageIndexChanging="grdLoadDiv_PageIndexChanging" 
                            AllowPaging="True">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Div Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivCode" runat="server" Text='<%# Bind("DIV_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Division Code" Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Div Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV_NAME") %>' Style="word-break: break-all" Width="150px"> </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Div Name" Width="200px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                               
                            </Columns>
                        </asp:GridView>
                        </div>
                         
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
                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
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
                                           
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Cancel" />

                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>
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
</asp:Content>
