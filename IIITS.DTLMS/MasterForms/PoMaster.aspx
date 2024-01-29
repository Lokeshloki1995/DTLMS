 <%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PoMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.PoMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--     <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>--%>

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

        function AllowOnlyAlphanumericNotAllowSpecial(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
           ((evt.which) ? evt.which : 0));

            if ((charCode > 47 && charCode < 58) ||
                (charCode > 64 && charCode < 91) ||
                (charCode > 96 && charCode < 123)) {
                return true;
            }

            else { return false; }
        }

        function DisableSplChars(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 92) {
                return false;
            } else {

                return true;
            }
        }


        function ConfirmDelete() {

            var result = confirm('Are you sure you want to Delete?');
            if (result) {
                return true;
            }
            else {

                return false;
            }
        }

    
        
       

        function ValidateForm() {

            if (document.getElementById('<%= txtPoNumber.ClientID %>').value.trim() == "") {
                alert('Enter PO Number')
                document.getElementById('<%= txtPoNumber.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtDate.ClientID %>').value.trim() == "") {
                alert('Select PO Date')
                document.getElementById('<%= txtDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbSupplier.ClientID %>').value.trim() == "--Select--") {
                alert('Select Supplier Name ')
                document.getElementById('<%= cmbSupplier.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtDelivery.ClientID %>').value.trim() == "") {
                alert('Select  Delivery Scheduled Date')
                document.getElementById('<%= txtDelivery.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRate.ClientID %>').value.trim() == "") {
                alert('Enter PO Amount')
                document.getElementById('<%= txtRate.ClientID %>').focus()
                return false
            }

        }

        function ValidateForm1() {
            if (document.getElementById('<%= ddlMake.ClientID %>').value.trim() == "-Select-") {
                alert('Select Make')
                document.getElementById('<%= ddlMake.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= ddlCapacity.ClientID %>').value.trim() == "--Select--") {
                alert('Select Capacity')
                document.getElementById('<%= ddlCapacity.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbRating.ClientID %>').value.trim() == "-Select-") {
                alert('Select Start Rating')
                document.getElementById('<%= cmbRating.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtQuantity.ClientID %>').value.trim() == "") {
                alert('Enter Quantity')
                document.getElementById('<%= txtQuantity.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtQuantity.ClientID %>').value.trim() == "0") {
                alert('Quantity should be Greater than 0')
                document.getElementById('<%= txtQuantity.ClientID %>').focus()
                return false
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
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create">Create Purchase Order
                <div style="float: right">

                    <asp:Button ID="cmdClose" runat="server" Text="PO View"
                        CssClass="btn btn-primary" style=" margin-top: 12px;margin-right: 2px;" OnClick="cmdClose_Click" /><br />
                </div>
                    </h3>
                </div>
                     <h3 class="page-title" runat="server" id="Update">Update Purchase Order 
                <div class="span9" style="float:right">

                    <asp:Button ID="btnPoView" runat="server"  Text="PO View"
                        CssClass="btn btn-primary" style="float: right;" OnClick="cmdClose_Click" /><br />
                </div>
           </h3>

               
                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>


                                </button>
                            </div>

                        </form>

                    </li>


                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
                <%-- <div style="float:right" >
                             
                                   <asp:Button ID="cmdClose" runat="server" Text="Indent View" 
                                              CssClass="btn btn-primary" onclick="cmdClose_Click" /><br /></div>
                --%>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreatePO" runat="server"><i class="icon-reorder"></i>Create Purchase Order</h4>
                            <h4 id="UpdatePO" runat="server"><i class="icon-reorder"></i>Update Purchase Order</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

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
                                                <label class="control-label">PO Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtPoNumber" runat="server" MaxLength="20"  autocomplete="off" onkeypress="javascript:return DisableSplChars(this,event);" ></asp:TextBox>
                                                           <%--<asp:regularexpressionvalidator id="regular1" controltovalidate="txtPoNumber" runat="server" errormessage="only chars and nums accept" validationexpression="[a-zA-Z0-9]+"></asp:regularexpressionvalidator>--%>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Supplier Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSupplier" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">PO Date<span class="Mandotary">*</span></label>
                                                <%--<label class="control-label">Rate Contract Order Date<span class="Mandotary">*</span></label>--%>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="ManufactureCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtDate">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span6">
                                           <div class="control-group">
                                                <label class="control-label">Delivery Scheduled<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDelivery" runat="server" MaxLength="10" AutoPostBack="true"
                                                            OnTextChanged="ValidateDeliveryScheduled"></asp:TextBox>

                                                        <ajax:CalendarExtender ID="DeliveryCalendar" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtDelivery">
                                                        </ajax:CalendarExtender>                                                       
                                                    </div><br />
                                                    <%--<asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                 ControlToCompare="txtDate" ControlToValidate="txtDelivery" ErrorMessage="Delivery Date must greater than PO Date" Operator="GreaterThan" 
                                                Type="Date" CultureInvariantValues="True"></asp:CompareValidator>--%>
                                                </div>                                                                                           
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">PO Amount <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRate" runat="server" MaxLength="12"   autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" id="DivUpload" runat="server">
                                                <label class="control-label">Upload Document <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif,.pdf"  /> 
                                                        <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                
                                     </div>

                                    </div>

                                </div>
                                <div class="space20"></div>
                                   <div class="container-fluid">
                                              <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false" 
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"   
                                                      OnPageIndexChanging="gvFiles_PageIndexChanging" OnSorting="gvFiles_Sorting" AllowSorting="true">
                                                 <Columns>
                                                  <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                   <ItemTemplate>
                                                       <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                      <%--Text="VIEW"      Text="VIEW"               --%>
                                                 </asp:TemplateField>
                                                  <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Download PO Documents" ItemStyle-Width="700"   />     
                                                 <asp:TemplateField>
                                               <ItemTemplate>
                                           <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue"  Text="<i class='icon-eye-open'></i> VIEW"   OnClick="DownloadFile"
                                              CommandArgument='<%# Eval("Name") %>'> 
                                           </asp:LinkButton>
                                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                  
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue"  Text="<i class='icon-download-alt'></i> DOWNLOAD"   OnClick="DownloadFiledwnld"
                                              CommandArgument='<%# Eval("Name") %>'> 
                                                   </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
              

                                      </Columns>
                                      </asp:GridView>
                                   </div>
                                <div class="space20">

                                <div class="form-horizontal" align="center">

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>

                            </div>
                        </div>
                        <!-- END SAMPLE FORM PORTLET-->
                    </div>
                </div>



            </div>

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Capacity Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
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
                                                    <label class="control-label">Make<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlMake" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Quantity <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantity" runat="server"   autocomplete="off" MaxLength="8" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA) <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlCapacity" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                               <label class="control-label">Rating<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRating" runat="server" TabIndex="15"                                                            >
                                                        </asp:DropDownList>
                                                                       <asp:TextBox runat="server" ID="gridid" Visible="false"></asp:TextBox>
                                                        <asp:Button ID="cmdAdd" runat="server" Text="Add" OnClick="cmdAdd_Click" OnClientClick="return ValidateForm1();" CssClass="btn btn-primary" />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span1"></div>
                                    </div>

                                    <div class="space20"></div>
                     

                                    <asp:GridView ID="grdPoMaster" AutoGenerateColumns="false" PageSize="5"
                                        ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" 
                                        runat="server" TabIndex="16" OnRowCommand="grdPoMaster_RowCommand" OnPageIndexChanging="grdPoMaster_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                              <asp:TemplateField AccessibleHeaderText="PO ID" HeaderText="PO ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPOId" runat="server" Text='<%# Bind("PO_ID") %>'></asp:Label>
                                                    <asp:TextBox ID="txtPOId" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="PO No" HeaderText="PO NO" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("PO_NO") %>'></asp:Label>
                                                    <asp:TextBox ID="txtPONo" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="Make" HeaderText="Make" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("MAKE_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Make" HeaderText="Make">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMake" runat="server" Text='<%# Bind("PB_MAKE") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SO_CAPACITY" HeaderText="Capacity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("PB_CAPACITY") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField AccessibleHeaderText="SO_CAPACITY" HeaderText="Capacity" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCapacityID" runat="server" Text='<%# Bind("PB_CAPACITY_ID") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField AccessibleHeaderText="SO_STARRATE" HeaderText="RatingId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRatingId" runat="server" Text='<%# Bind("PB_STARRATE") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SO_STARRATE" HeaderText="Rating">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRating" runat="server" Text='<%# Bind("PB_STAR_NAME") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("PB_QUANTITY") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgBtnEdit" title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                            CommandName="EditQNTY" Width="12px"/>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                            CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="space20"></div>
                                    <div class="text-center">
                                        
                                            <asp:Button ID="btnSave" runat="server" Text="Save"
                                                OnClientClick="return ValidateForm();" CssClass="btn btn-primary" OnClick="btnSave_Click1" />
                                
                                   
                                            <asp:Button ID="btnReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="btnReset_Click" />
                                    
                                    </div>

                                     
                                </div>

                            </div>
                          
                            <div class="space20"></div>
                              <div class="form-horizontal" align="center">

                                    <asp:Label ID="lblNote" runat="server" ForeColor="Red"></asp:Label>
                                </div>

                            <!-- END FORM-->
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->

        </div>

    </div>



</div>
</asp:Content>
