<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DTLMS.Master"  CodeBehind="TcAllotment.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TcAllotement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
      </script>
    <script type="text/javascript">
    function checkRadioBtn(id) {
        var gv = document.getElementById('<%=grdDIPendingTC.ClientID %>');

        for (var i = 1; i < gv.rows.length; i++) {
            var radioBtn = gv.rows[i].cells[0].getElementsByTagName("input");

            // Check if the id not same
            if (radioBtn[0].id != id.id) {
                radioBtn[0].checked = false;
            }
        }
    }
</script>

 <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title" runat="server" id="Create">Tc Allocation
               <%-- <div style="float: right">

                    <asp:Button ID="cmdClose" runat="server" Text="PO View"
                        CssClass="btn btn-primary" /><br />
                </div>--%>
                    <div style="float: right">

                    <asp:Button ID="cmdClose" runat="server" style="margin-top: 14px; margin-right: 1px;" Text="Allotment View"
                        CssClass="btn btn-primary" OnClick="cmdClose_Click" /><br />
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
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4 id="CreateDI"  runat="server"><i class="icon-reorder"></i>Create Tc Allotment</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                            </span>
                        </div>
                        
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                       
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">DI Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDiId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtDINumber" runat="server" MaxLength="20" onkeypress="javascript:return AllowOnlyAlphanumericNotAllowSpecial(event);"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" /><br />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                         <div class="span2"></div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Quantity <span class="Mandotary">*</span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTotalQuantity" ReadOnly="true" runat="server" MaxLength="8" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                   <div class="space20"></div>
                                    <div class="row-fluid">
                                       
                                        <div class="span6">
                                                <asp:GridView ID="grdDIPendingTC" AutoGenerateColumns="false" PageSize="5"
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" TabIndex="16" style="width:600px;"   OnPageIndexChanging="grdDIPendingTC_PageIndexChanging">
                                    <Columns>
   <%--                                <asp:TemplateField AccessibleHeaderText="DI_ID" HeaderText="Select" >                                
                                        <ItemTemplate>                                       
                                            <asp:CheckBox ID="chkSelect" runat="server"  />
                                        </ItemTemplate>
                                   </asp:TemplateField>--%>          

                                         
                                        <asp:TemplateField  HeaderText="Select">
                                            <ItemTemplate>
                                               <asp:RadioButton ID="chkSelect" runat="server" onclick="checkRadioBtn(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

            
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="DI_ID" HeaderText="DI_ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDI" runat="server" Text='<%# Bind("sDI_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIM_DI_NO" HeaderText="Dispatch No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDINO" runat="server" Text='<%# Bind("sDIM_DI_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PB_MAKE" HeaderText="Make" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("sDI_MAKE_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Width="150" Text='<%# Bind("sTM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Store" HeaderText="Store_Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstore" runat="server" Text='<%# Bind("sSM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("sDI_CAPACITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="PO_RATING" HeaderText="Rating"  Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRatingId" runat="server" Text='<%# Bind("sDI_STARTTYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="RATING" HeaderText="Rating">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRating" runat="server" width="150" Text='<%# Bind("sMD_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      
                                        <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="Total Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("sDI_QUANTITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
<%--                                        <asp:TemplateField AccessibleHeaderText="PENDING" HeaderText="Pending Quantity" Visible ="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPenQuantity" runat="server" Text='<%# Bind("sDI_QUANTITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <asp:TemplateField AccessibleHeaderText="START_RANGE" HeaderText="START_RANGE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstartrange" runat="server" Text='<%# Bind("sDI_TC_START_RANGE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField AccessibleHeaderText="END_RANGE" HeaderText="END_RANGE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblendrange" runat="server" Text='<%# Bind("sDI_TC_END_RANGE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIM_ID" HeaderText="DIM_ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldimid" runat="server" Text='<%# Bind("sDIM_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIM_PO_ID" HeaderText="DIM_PO_ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpoid" runat="server" Text='<%# Bind("sDIM_PO_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                 <asp:TemplateField>
                                               <ItemTemplate>
<%--                                           <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Width="150" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadDiRecords"
                                              CommandArgument='<%# Eval("DIM_DI_NO") %>'></asp:LinkButton>
                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Width="150"  Text="<i class='icon-download-alt'></i> DOWNLOAD"   OnClick="DownloadDiRecords"
                                              CommandArgument='<%# Eval("sDIM_DI_NO") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                                    </asp:GridView>
                                                    </div>

                            
                                </div> <br />
                                       <div class="span6">
                                
                                      <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false" 
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"   
                                                      OnPageIndexChanging="gvFiles_PageIndexChanging"  AllowSorting="true">
                                                 <Columns>
                                                  <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                   <ItemTemplate>
                                                       <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                 </asp:TemplateField>
                                                  <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Download Delivery Documents" ItemStyle-Width="350"   />     
                                                 <asp:TemplateField>
                                               <ItemTemplate>
                                           <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile"
                                              CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue" Width="150"  Text="<i class='icon-download-alt'></i> DOWNLOAD"   OnClick="DownloadFiledwnld"
                                              CommandArgument='<%# Eval("Name") %>'> </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
              

                                      </Columns>
                                      </asp:GridView>
                                       
                                           
                                <%-- <asp:LinkButton ID="lnkPoDownload" runat="server" Visible="false" OnClick="lnkPoDownload_Click">  <img src="../img/Manual/Pdficon.png" style="width:20px" />Click Here to Download PO</asp:LinkButton>--%>
                                    </div>


                              <div class="space20"></div>
                                <div class="form-horizontal" align="center">

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                             <div class="control-group" id="DivUpload" runat="server">
                                                <label class="control-label">Upload Document <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" accept=".xlsx,.xls"/> 
                                                        <asp:Label ID="Label1" runat="server" Text="Initial Text"></asp:Label>
                                                             <asp:Button ID="cmdupload" class="btn btn-primary" runat="server" Text="Upload" OnClick="cmdUpload_click" />
                                                    </div>
                                                              <div class="form-horizontal" align="center">
                                                               <asp:Label ID="lblmsg" runat="server" ForeColor="Green"></asp:Label>
                                                              </div>
                                                </div>
                                            </div>



                                                <div class="form-horizontal" align="center">
                                                    <div class="input-append">
                                                             <asp:Button ID="cmdsave" class="btn btn-primary" runat="server" Text="Save" OnClick="cmdsave_click" />
                                                          <asp:Button ID="cmdReset" class="btn btn-primary" runat="server" Text="Reset" OnClick="cmdreset_click" />
                                                    </div>
                                                </div>
                                
                            </div>
                        </div>
                        <!-- END SAMPLE FORM PORTLET-->
                    </div>
                </div>
            </div>

            <!-- BEGIN PAGE CONTENT-->

                
<%--            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Allocate Capacity`s </h4>
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
                                                <label class="control-label">Allotment Number<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtALTId" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtALTNumber" runat="server" MaxLength="20" onkeypress="javascript:return DisableSplChars(this,event);"></asp:TextBox>
                                                    
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="control-group">
                                                <label class="control-label">Allotment Date<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtALTDate" runat="server"  MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="ALTCalendar" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtALTDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                            <div class="control-group">
                                                <label class="control-label">Store<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbStore_SelectedIndexChanged" ></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                            <label class="control-label">Division <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDiv" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                                <label class="control-label">Make<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbMake_SelectedIndexChanged" ></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>   
                                          <div class="span6">
                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA) <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCapacity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbCapacity_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                              
                                        <div class="control-group">
                                            <label class="control-label">Rating<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbRating" runat="server" TabIndex="15"  AutoPostBack="true" OnSelectedIndexChanged="cmbRating_SelectedIndexChanged">
                                                    </asp:DropDownList>                                                   
                                                </div>
                                            </div>
                                        </div>
                                               <div class="control-group">
                                            <label class="control-label">Quantity <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                     <asp:Button ID="cmdAdd" runat="server" Enabled ="false" Text="Add" OnClientClick="return ValidateForm1();" CssClass="btn btn-primary" OnClick="cmdAdd_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Allotment Note<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:FileUpload ID="fupFile" runat="server"  accept=".jpg,.jpeg,.png,.gif,.pdf" />
                                                     <asp:Label ID="lblFilename" runat="server" Text="Initial Text"></asp:Label>
                                                   </div>
                                               
                                            </div>
                                        </div>
                                  
                                  <div class="control-group">
                                     <asp:Label ID="lblQuntity" runat="server" ForeColor="green"></asp:Label>
                             
                             </div>

                                    </div>                                 
                                    </div>
                                    <div class="space20"></div>
                                <div class="row-fluid">
                                    <div class="span1"></div>
                                    <div class="span5">

                                       
                                    </div>
                                  
                                </div>
                                <div class="space20"></div>
                                   
                                      
                                <asp:GridView ID="grdAllotement" AutoGenerateColumns="false" PageSize="10"
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No records Found"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" TabIndex="16" OnPageIndexChanging="grdAllotement_PageIndexChanging" OnRowCommand="grdAllotement_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ALT_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DI_ID" HeaderText="Division Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivId" runat="server" Text='<%# Bind("ALT_DIV_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField AccessibleHeaderText="ALT_NO" HeaderText="Allotment Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAltNo" runat="server" Text='<%# Bind("ALT_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="ALT_DI_NO" HeaderText="Dispatch number" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAltDINo" runat="server" Text='<%# Bind("ALT_DI_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ALT_DATE" HeaderText="Allotment Date" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAltDate" runat="server" Text='<%# Bind("ALT_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField AccessibleHeaderText="MAKE_ID" HeaderText="MAKE ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("ALT_MAKE_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="MAKE_ID" HeaderText="Make Name" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("MAKE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="DI_STORE_ID" HeaderText="StoreId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStoreId" runat="server" Text='<%# Bind("ALT_STORE_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="ALT_STORE" HeaderText="Store">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("ALT_STORE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                 <asp:TemplateField AccessibleHeaderText="DI_NAME" HeaderText="Division Name" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                        
                                                                                
                                        <asp:TemplateField AccessibleHeaderText="ALT_CAPACITY" HeaderText="Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("ALT_CAPACITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="ALT_CAPACITY" HeaderText="Capacity"  Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacityId" runat="server" Text='<%# Bind("ALT_CAPACITY_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ALT_STARRATE" HeaderText="Rating" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRating" runat="server" Text='<%# Bind("ALT_STAR_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ALT_STARRATENAME" HeaderText="Rating">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRatingName" runat="server" Text='<%# Bind("ALT_STARRATENAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ALT_QUANTITY" HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("ALT_QUANTITY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                         <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                            CommandName="EditQNTY" Width="12px"/>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                      
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnDelete" Title="Click To Delete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                        CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                       <div class="space20"></div>
                                             </div>
                                           <div class="container-fluid">
                                              <asp:GridView ID="grdDIdocs" runat="server" AutoGenerateColumns="false" PageSize="5" ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" ShowFooter="false" 
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"   
                                                      OnPageIndexChanging="gvFiles_PageIndexChanging" OnSorting="gvFiles_Sorting" AllowSorting="true">
                                                 <Columns>
                                                  <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                   <ItemTemplate>
                                                       <%#Container.DataItemIndex+1 %>
                                                       </ItemTemplate>
                                                 </asp:TemplateField>
                                                  <asp:BoundField DataField="Name" ItemStyle-ForeColor="BlueViolet" HeaderText="Download Allotment Documents" ItemStyle-Width="700"   />     
                                                 <asp:TemplateField>
                                               <ItemTemplate>
                                           <asp:LinkButton ID="lnkDownload" runat="server" ForeColor="Blue" Text="<i class='icon-eye-open'></i> VIEW" OnClick="DownloadFile1"
                                              CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>

                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        
                                                   <asp:LinkButton ID="lnkDownload1" runat="server" ForeColor="Blue"  Text="<i class='icon-download-alt'></i> DOWNLOAD"   OnClick="DownloadFiledwnld1"
                                              CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
              

                                      </Columns>
                                      </asp:GridView>
                                
                                      
                                <div class="space20"></div>
                                <div class="text-center">
                                   
                                        <asp:Button ID="btnSave" runat="server" Text="Save"
                                            OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="btnSaveUpdate_Click" />
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false"
                                            OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="btnSaveUpdate_Click" />
                                    

                                    
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
            </div>--%>
            <!-- END PAGE CONTENT-->

        </div>

    </div>
        <style>
    a#ContentPlaceHolder1_gvFiles_lnkDownload_0 {
    white-space: nowrap;
}
</style>
     </asp:Content>