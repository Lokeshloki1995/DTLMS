<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="NewTcMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.NewTcMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

          function preventMultipleSubmissions() {
              $('#<%=cmdSave.ClientID %>').prop('disabled', true);

          }
        window.onbeforeunload = preventMultipleSubmissions;
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

        function ValidateSave() {
            if (document.getElementById('<%= txtDINo.ClientID %>').value.trim() == "") {
              alert('Enter valid PO Number')
              document.getElementById('<%= txtDINo.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtAllotmentDate.ClientID %>').value.trim() == "") {
              alert('Select valid Purchasing Date')
              document.getElementById('<%= txtAllotmentDate.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtQuantity.ClientID %>').value.trim() == "") {
              alert('Enter valid Quantity')
              document.getElementById('<%= txtQuantity.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtSupplier.ClientID %>').value.trim() == "") {
              alert('Select Supplier Name')
              document.getElementById('<%= txtSupplier.ClientID %>').focus()
              return false
          }

      }
      function ValidateMyForm() {


          if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
              alert('Enter DTr Code')
              document.getElementById('<%= txtTcCode.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtSerialNo.ClientID %>').value.trim() == "") {
              alert('Enter valid DTr serial No')
              document.getElementById('<%= txtSerialNo.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbMake.ClientID %>').value == "--Select--") {
              alert('Select DTr Make Name')
              document.getElementById('<%= cmbMake.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "-Select-") {
              alert('Select Capacity')
              document.getElementById('<%= cmbCapacity.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= cmbRating.ClientID %>').value == "-Select-") {
              alert('Select The Rating')
              document.getElementById('<%= cmbRating.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtDINum.ClientID %>').value.trim() == "") {
              alert('Enter The Dispatch Instrution Number')
              document.getElementById('<%= txtDINum.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtManufactureDate.ClientID %>').value.trim() == "") {
              alert('Enter valid Manufacturing Date')
              document.getElementById('<%= txtManufactureDate.ClientID %>').focus()
                   return false
               }

               if (document.getElementById('<%= txtTcLifeSpan.ClientID %>').value.trim() == "") {
              alert('Enter Life Span')
              document.getElementById('<%= txtTcLifeSpan.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtWarrentyPeriod.ClientID %>').value.trim() == "") {
                alert('Enter Valid warranty Period')
              document.getElementById('<%= txtWarrentyPeriod.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtOilCapacity.ClientID %>').value.trim() == "") {
              alert('Enter Valid Oil Capacity')
              document.getElementById('<%= txtOilCapacity.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtWeight.ClientID %>').value.trim() == "") {
              alert('Enter Valid Weight of DTr')
              document.getElementById('<%= txtWeight.ClientID %>').focus()
                  return false
              }


          }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <h3 class="page-title">New DTR Inward
                                        <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="Button1" runat="server" Text="Inward View"
                        OnClientClick="javascript:window.location.href='TcInwardView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>
                    </h3>
<%--                    <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                                           <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
            <!-- END PAGE HEADER-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Allotment Details</h4>
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
                                                <%--<label class="control-label">Purchase Order No<span class="Mandotary"> *</span></label>--%>
                                                <label class="control-label">Dispatch Number<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfPOId" runat="server" />
                                                        <asp:TextBox ID="txtDIId" runat="server" MaxLength="50" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtDINo" runat="server" MaxLength="100"  ></asp:TextBox>
                                                        <asp:Button ID="btnDISearch" runat="server" Text="S"
                                                            CssClass="btn btn-primary" OnClick="btnPoSearch_Click"  />

                                                    </div>
                                                </div>
                                            </div>
                                            <%-- <div class="control-group" ">
                                                <label class="control-label" >Allotment Number<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">--%>
                                                        <asp:DropDownList ID="cmbALTNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbALTNo_SelectedIndexChanged"  Visible="false">
                                                        </asp:DropDownList>

                                                <%--    </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <%--<label class="control-label">Purchase Order Date</label>--%>
                                                <label class="control-label">Purchase Date</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAllotmentDate" runat="server" MaxLength="10"
                                                            Enabled="False"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="PurchaseCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtAllotmentDate">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>

                                            <asp:LinkButton ID="lnkPoDownload" runat="server" Visible="false" OnClick="lnkPoDownload_Click" >  <img src="../img/Manual/Pdficon.png" style="width:20px" />Click Here to Download PO</asp:LinkButton>



                                        </div>

                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Total Quantity Ordered</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8"
                                                            onkeypress="javascript:return OnlyNumber(event);" Enabled="False"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Supplier Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--  <asp:TextBox ID="txtSupplyId" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"  ></asp:TextBox>
                                                                  <asp:Button ID="btnSupplyId" Text="S" class="btn btn-primary" runat="server" />--%>
                                                       <%-- <asp:DropDownList ID="cmbSupplier" runat="server" Enabled="true"></asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtSupplier" runat="server" 
                                                            Enabled="False"  ></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>

                                            <asp:LinkButton ID="lnkDI" runat="server" Visible="false" OnClick="lnkDI_Click" >  <img src="../img/Manual/Pdficon.png" style="width:20px" />Click Here to Download Delivery Inst</asp:LinkButton>
                                        </div>
                                        <div class="span1"></div>

                                    </div>



                                    <div class="space20"></div>

                                    <asp:GridView ID="grdAltQuantity" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        AutoGenerateColumns="false" PageSize="10"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        runat="server" OnPageIndexChanging="grdPOQuantity_PageIndexChanging">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                           <%-- <asp:TemplateField AccessibleHeaderText="ALT_ID" HeaderText="AllotmentId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAltId" runat="server" Text='<%# Bind("ALT_ID") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                          <%--   <asp:TemplateField AccessibleHeaderText="ALT_NO" HeaderText="Allotment number" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAltNo" runat="server" Text='<%# Bind("ALT_NO") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField AccessibleHeaderText="ALT_DI_NO" HeaderText="Dispatch number" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAltDINo" runat="server" Text='<%# Bind("DI_NO") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity(in KVA)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("CAPACITY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            
                                           <%--  <asp:TemplateField AccessibleHeaderText="Division" HeaderText="Division Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField AccessibleHeaderText="REQ_QNTY" HeaderText="Requested Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReqCapacity" runat="server" Text='<%# Bind("REQ_QNTY") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PENDINGCOUNT " HeaderText="Pending Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingCapacity" runat="server" Text='<%# Bind("PENDINGCOUNT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RATING" HeaderText="Rating">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRating" runat="server" Text='<%# Bind("RATING") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>
                               


                                </div>
                                <div class="widget-body form">
                                    <div class="text-center">
                                        <asp:Button ID="cmdloadpo" runat="server" Text="Load"
                                                CssClass="btn btn-primary" OnClick="cmbALTNo_SelectedIndexChanged" />
                                        
                                            <asp:Button ID="cmdResetPO" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="cmdResetPO_Click" />
                                  
                                    </div>
                                </div>

                            </div>



                            <!-- END FORM-->
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>DTr Details</h4>
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
                                                <label class="control-label">Dispatch Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDINum" runat="server" ReadOnly="true" MaxLength="20" onkeypress="javascript:return AllowOnlyAlphanumericNotAllowSpecial(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                            <div class="control-group">
                                                <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <%--<asp:TextBox ID="txtTcCode" runat="server" onkeypress="javascript:return OnlyNumber(event);" MaxLength="6"></asp:TextBox>  AutoPostBack="true" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged" --%>
                                                         <asp:DropDownList ID="txtTcCode" runat="server"   AutoPostBack="true"  OnTextChanged="cmbtxttccode_SelectedIndexChanged" >
                                                        </asp:DropDownList>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Serial No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="20"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                          <%--   <div class="control-group">
                                                <label class="control-label" ID="cmbDiv1">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">--%>
                                                        <asp:DropDownList ID="cmbDiv" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                 <%--   </div>
                                                </div>
                                            </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">DTr Make<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbMake_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbCapacity_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Rating<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRating" runat="server" TabIndex="15">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>





                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Manufacturing Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="ManufactureCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtManufactureDate">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Life Span<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcLifeSpan" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>

                                                    </div>
                                                </div>

                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">warranty Period(in Month)<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWarrentyPeriod" runat="server" MaxLength="2" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                     TargetControlID="txtWarrentyPeriod"></ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Oil Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOilType" runat="server" TabIndex="15">
                                                             <%-- <asp:ListItem Selected="True" Value="1"> Mineral oil </asp:ListItem>
                                                              <asp:ListItem Value="2"> Ester Oil </asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Oil Capacity(in Litre)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilCapacity" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>

                                                    </div>
                                                </div>

                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Weight of DTr(in KG)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWeight" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>

                                                    </div>
                                                </div>

                                            </div>
                                             <div class="control-group" id="DivUpload" runat="server">
                                                <label class="control-label">Upload Name Plate Photo <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="FileNamePlate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif" />
                                                        <asp:Label ID="lblNamePlate" runat="server" Text="Initial Text"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group" id="Div1" runat="server">
<%--                                                <label class="control-label">Upload SS Plate <span class="Mandotary">*</span></label>--%>
                                                <label class="control-label">Upload DTr Code Photo <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="FileSSplate" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif" />
                                                        <asp:Label ID="lblSSPlate" runat="server" Text="Initial Text"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>





                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center">

                                       
                                            <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="cmdAdd_Click"
                                                OnClientClick="javascript:return ValidateMyForm();" />
                               
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                   
                                      
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdTCDetails" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" TabIndex="16" OnRowCommand="grdTCDetails_RowCommand"
                                OnPageIndexChanging="grdTCDetails_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <%--   <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="Division Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="DIV_ID" HeaderText="Division ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDivID" runat="server" Text='<%# Bind("DIV_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>


                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MAKE_ID" HeaderText="Make ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMakeID" runat="server" Text='<%# Bind("MAKE_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="LIFE_SPAN" HeaderText="Life Span">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLifeSpan" runat="server" Text='<%# Bind("LIFE_SPAN") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_OIL_CAPACITY" HeaderText="Oil Capacity" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOilCapacity" runat="server" Text='<%# Bind("TC_OIL_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_OIL_TYPE" HeaderText="Oil Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOilType" runat="server" Text='<%# Bind("TC_OIL_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_WEIGHT" HeaderText="Weight" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("TC_WEIGHT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star Rate" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTcstarRate" runat="server" Text='<%# Bind("TC_STAR_RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <%--   <asp:TemplateField AccessibleHeaderText="TC_ALT_NO" HeaderText="Allotment No" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAltno" runat="server" Text='<%# Bind("TC_ALT_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                      <asp:TemplateField AccessibleHeaderText="NAME_PLATE" HeaderText="NAME PLATE" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNameplate" runat="server" Text='<%# Bind("NAME_PLATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField AccessibleHeaderText="SS_PLATE" HeaderText="SS_PLATE " Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblssplate" runat="server" Text='<%# Bind("SS_PLATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField AccessibleHeaderText="SERVICE_DATE" HeaderText="Supplier" Visible="false">                                     
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceDate" runat="server" Text='<%# Bind("SERVICE_DATE") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <center>
                                                <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                    Width="12px" CommandName="editT" OnClientClick="return confirm ('Are you sure, you want to Edit the Details');" />
                                                <asp:ImageButton ID="img" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" CommandName="remove"
                                                    Width="12px" OnClientClick="return confirm ('Are you sure, you want to Remove');" />
                                            </center>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <center>
                                                <asp:Label ID="lblHead" runat="server" Text="Action"></asp:Label>
                                            </center>
                                        </HeaderTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>
                            <div class="widget-body form">
                                <div class="form-horizontal" align="center">
          


                                        <asp:Button ID="cmdSave" runat="server" Text="Save" Visible="false"
                                            OnClientClick="javascript:return ValidateSave();" CssClass="btn btn-primary"
                                            OnClick="cmdSave_Click" onchange="javascript:preventMultipleSubmissions();" />
                            
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used For New DTR Inward.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Allotment Order No can Be Searched by Clicking On Search Button Or Allotment Order No Can Be Entered Directly to the TextBox.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>In Dtr Details Section All The Details Regarding The DTR Can Be Entered.
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>After Filling all The Required Filed,Click Add Button To Save The DTR Details.
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
