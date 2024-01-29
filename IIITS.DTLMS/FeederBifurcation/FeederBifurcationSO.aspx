<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FeederBifurcationSO.aspx.cs" Inherits="IIITS.DTLMS.FeederBifurcation.FeederBifurcationSO" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ChkBoxClass input {
            left: 25px;
            right: 25px;
            width: 25px;
            height: 25px;
            /*float: left;*/
        }

        input#ContentPlaceHolder1_chkIsMultipleFeeder {
            margin-right: 12px;
            margin-top: -4px;
            D: \HESCOM_Project\Projects\IIITS.DTLMS\FeederBifurcation\FeederBifurcationSO.aspx float: left;
        }

        table#ContentPlaceHolder1_grdOldDTC {
            float: left !important;
            margin-right: 6%;
            width: 26% !important;
        }

        table#ContentPlaceHolder1_gridview2 {
            width: 43% !important;
        }

        th {
            font-size: 14px !important;
        }

        td {
            font-size: 13px;
        }

        tr:nth-child(odd) {
            background: #FFF !important;
        }
    </style>

    <script type="text/javascript" language="javascript">

        function CheckAllEmp(Checkbox) {
            debugger;
            var GridVwHeaderChckbox = document.getElementById('<%=grdOldDTC.ClientID %>');
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                //alert(GridVwHeaderChckbox.rows[0].cells[2].getElementsByTagName("INPUT")[0].checked);
                GridVwHeaderChckbox.rows[i].cells[2].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
        function UnCheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById('<%=grdOldDTC.ClientID %>');

            GridVwHeaderChckbox.rows[0].cells[2].getElementsByTagName("INPUT")[0].checked = false;

        }

        function OnlyAlphabetNumbers(evt) {

            var inputValue = (evt.which) ? evt.which : window.event.keyCode;
            // allow letters and whitespaces only.
            if (!((inputValue >= 65 && inputValue <= 90) || (inputValue >= 48 && inputValue <= 57))) {
                return false;
            }
        }

        function ValidateSelectedDTC() {

            var grdOldDTC = document.getElementById('<%=grdOldDTC.ClientID %>');
            var status = false;

            for (i = 1; i < grdOldDTC.rows.length; i++) {
                if (grdOldDTC.rows[i].cells[2].getElementsByTagName("INPUT")[0].checked == true) {
                    status = true;
                }


            }
            debugger;
            //if (status == true) {
            //    // alert(document.getElementById("btnUpdate"));
            //    document.getElementById("btnUpdate").click();

            //}
            //else {
            //    alert("Please Select atleast one DTC Code in the first table");
            //}


        }

        // called after click on button Bifurcate  
        function finalValidateSelectedDTC() {
            debugger;
            var grdOldDTC = document.getElementById('<%=gridview2.ClientID %>');
            var status = false;
            for (i = 1; i < grdOldDTC.rows.length; i++) {
                //grdOldDTC.find('');
                if (grdOldDTC.rows[i].cells[6].getElementsByTagName("INPUT")[0].checked == true) {
                    status = true;
                }

            }
            debugger;
            if (status == true) {
                // alert(document.getElementById("btnUpdate"));
                document.getElementById("btnbifurcate").click();
            }
            else {
                alert("Please Select atleast one DTC Code in the first Table  ");
            }




        }
        //function to 
        function getValue(evt) {
            debugger;
            if (evt.value.length == 0) {
                return true;
            }

            if (evt.value.length == 1) {

                alert("Serial Number Cant Be One Digit");
                evt.value = '';
                return false;
            }

            var status = true;
            var textboxId = evt.id.toString().split("_")[3];
            var grdOldDTC = document.getElementById('<%=gridview2.ClientID %>');
            for (i = 0; i < grdOldDTC.rows.length - 1; i++) {
                if (i != textboxId) {
                    if (evt.value == $("input[id*=lblDTCseraial]")[i].value) {
                        evt.value = '';
                        $("input[id*=lblDTCseraial]")[i].focus();
                        alert("DTC Serial number already Exists");
                        status = false;
                        return false;
                    }
                }
            }
            debugger;
            if (status = false) {
                return false;
            }
            else {

                textboxId = parseInt(textboxId) + parseInt(1);
                grdOldDTC.rows[textboxId].cells[5].getElementsByTagName("INPUT")[0].checked = true;
                return true;
            }
        }


        function DontAllowDuplicates(evt) {
            var inputValue = evt.inputValue;
            var grdOldDTC = document.getElementById('<%=gridview2.ClientID %>');
            var status = false;
            for (i = 1; i < grdOldDTC.rows.length; i++) {
                if (grdOldDTC.rows[i].cells[5].getElementsByTagName("DT_SERIAL_NUMBER")[0] == inputValue) {
                    status = true;
                }
            }
        }
    </script>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <!-- BEGIN PAGE HEADER-->

    <div class="span11">
        <!-- BEGIN THEME CUSTOMIZER-->

        <!-- END THEME CUSTOMIZER-->
        <!-- BEGIN PAGE TITLE & BREADCRUMB-->

        <%-- <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>--%>
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
<br />
       <%-- <div style="float: right; margin-top: 20px; margin-right: 12px">
            <a target="_blank" href="/MasterForms/FeederMast.aspx" style="font-size: medium; color: darkorange; text-decoration: dotted">
                <asp:Label runat="server" Style="font-size: medium; font-weight: bold; color: green; text-decoration: dotted"><u>Click to Create New Feeder </u></asp:Label>
            </a>
        </div>--%>
        <!-- END PAGE TITLE & BREADCRUMB-->



        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <!--   <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
        <!--     <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Feeder bifurcation</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                  

                        <div class="space20"></div>
                        <!-- END FORM-->

        
       


        <div class="widget blue">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Feeder Bifurcation </h4>
                <span class="tools">
                    <a href="javascript:;" class="icon-chevron-down"></a>
                    <a href="javascript:;" class="icon-remove"></a>

                </span>
            </div>
            <div class="widget-body">

                <%--<div class="container-fluid" >

                    <div class="span1">
                         <asp:Button ID="btnAETApproval" Visible="true" Width="100px" runat="server" BackColor="Salmon" ForeColor="#ffffff" Style="top: -5px; left: 1px" CssClass="fc-button" Text="Reset"  OnClick="btnAETRedirect_click"></asp:Button>
                        </div>
                    </div>--%>

                  <asp:HiddenField ID="hdfId" runat="server" />
                <div class="container-fluid">

                  
                    <div class="span2">

                        <label class="control-label" style="color: black; font: 150px">
                            CIRCLE</label>
                        <asp:DropDownList ID="cmbCircle" runat="server" Style="color: darkblue; font: 100px; border-color: darkblue"
                            OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>



                    </div>
                    <div class="span2"></div>
                    <div class="span2">
                        <label class="control-label" style="color: black; font: 150px">
                            DIVISION
                        </label>
                        <asp:DropDownList ID="cmbDivision" runat="server" Style="color: darkblue; font: 100px; border-color: darkblue"
                            OnSelectedIndexChanged="cmbdivision_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>

                    </div>

                    <div class="span2"></div>
                    <div class="span2">
                        <label class="control-label" style="color: black; font: 150px">
                            SUBDIVISION</label>
                        <asp:DropDownList ID="cmbSubDivision" runat="server" Style="color: darkblue; font: 100px; border-color: darkblue"
                            OnSelectedIndexChanged="cmbSubdivision_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="container-fluid">
                    
                    <div class="span2">
                        <div class="control-group">
                            <label class="control-label" style="color: black; font: 150px">
                                OLD FEEDER CODE</label>
                            <asp:DropDownList ID="cmbFeeder" runat="server" Style="color: darkblue; font: 100px; border-color: darkblue" OnSelectedIndexChanged="cmbFeeder_SelectedIndexChanged" ToolTip="Select the Feeder Code for Bifurcating the DTC Code" AutoPostBack="true">
                            </asp:DropDownList>

                        </div>
                    </div>

                    <div class="span2"></div>
                     <div class="span2">
                        <div class="control-group">
                            <label class="control-label">
                                STATION CODE</label>
                            <asp:DropDownList ID="cmbStation" ToolTip="Select Station" runat="server" Style="color: darkblue; font: 100px; border-color: darkblue" OnSelectedIndexChanged="cmbStation_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList> 
                        </div>
                    </div>
                       <div class="span2"></div>
                    <div class="span2">
                        <div class="control-group">
                            <label class="control-label">
                                NEW FEEDER CODE</label>
                            <asp:DropDownList ID="cmbNewFeeder" ToolTip="Please select the to feeder" runat="server" Style="color: darkblue; font: 100px; border-color: darkblue" OnSelectedIndexChanged="cmbNewFeeder_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                 
                    <%--<div class="span2">
                        <label class="control-label" style="color: black; font: 150px">OM Date<span class="Mandotary">*</span>  </label>
                        <asp:TextBox ID="txtOmDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1"
                            TargetControlID="txtOmDate" Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </div>--%>
                </div>
              
            </div>
            <br />
            <br />
            <div class="text-center">
                <asp:Button ID="btnUpdate" Visible="false" Width="250px" runat="server" BackColor="SeaGreen" ForeColor="#ffffff"  CssClass="fc-button" Text="Add Selected DTCs to New Feeder" OnClick="btnUpdate_click" OnClientClick="ValidateSelectedDTC();" />
            <asp:Button ID="btnApprove"  runat="server" BackColor="#ff9933" ForeColor="White" Visible="false" CssClass="fc-button" Text="Approve" OnClick="btnApprove_click"   AUTOPOSTBACK="true" />
            <asp:Button ID="btnReset" Visible="true" Width="100px" runat="server" BackColor="Salmon" ForeColor="#ffffff"  CssClass="fc-button" Text="Reset" OnClick="btnReset_click"></asp:Button>
         
            <asp:Button ID="btnClose" Visible="true" Width="100px" runat="server" BackColor="Gray" ForeColor="#ffffff"  CssClass="fc-button" Text="Close" OnClick="btnClose_click"></asp:Button>
           <asp:Button ID="btnbifurcate" Width="150px" runat="server" BackColor="#ff9933" ForeColor="White" Visible="false"  CssClass="fc-button" Text="Bifurcate DTC's" OnClientClick="finalValidateSelectedDTC();" OnClick="btnbifurcate_click" AUTOPOSTBACK="true" />
        
            </div>
          <%--  <div style="width: 26.076923% !important; margin-top: -27px; margin-left: 43px" class="span3">
                <asp:CheckBox ID="chkIsMultipleFeeder" runat="server" Text="SELECT DTC FROM MUTIPLE FEEDERS" CssClass="ChkBoxClass" BorderColor="#0066ff" AutoPostBack="true"></asp:CheckBox>
            </div>--%>
            <br />
            <br />
            <div class="container-fluid">
                <div class="col-md-4">
                    <asp:GridView ID="grdOldDTC" DataKeyNames="DT_ID" runat="server" AutoGenerateColumns="false" >

                        <Columns>

                            <asp:TemplateField AccessibleHeaderText="DT_ID" HeaderText="DTCID" Visible="false">

                                <ItemTemplate>
                                    <asp:Label ID="lblDTCID" runat="server" Text='<%# Bind("DT_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" ItemStyle-Width="90px">

                                <ItemTemplate>
                                    <asp:Label ID="lblDTCCode" Width="70px" Font-Bold="true" Font-Size="Larger" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DTC_NAME" HeaderText="DTC Name" Visible="true">

                                <ItemTemplate>
                                    <asp:Label ID="lblDTCName" runat="server" Text='<%# Bind("DT_NAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField AccessibleHeaderText="DT_TC_ID" HeaderText="DTR Code" Visible="false" ItemStyle-Width="70px">

                                <ItemTemplate>
                                    <asp:Label ID="lblTCCode" Width="70px" Font-Bold="true" Font-Size="Larger" runat="server" Text='<%# Bind("DT_TC_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="false" ItemStyle-Width="70px">

                                <ItemTemplate>
                                    <asp:Label ID="lblSTATUS" Width="70px" Font-Bold="true" Font-Size="Larger" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Select">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="multipleSelect" CssClass="ChkBoxClass" runat="server" onclick="CheckAllEmp(this);"></asp:CheckBox>
                                </HeaderTemplate>


                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" CssClass="ChkBoxClass" runat="server" onclick="UnCheckAllEmp(this);"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>




                        </Columns>

                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#404040" Font-Size="Medium" Width="15px" />
                        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:GridView>
                </div>
                <div class="col-md-6">
                    <asp:GridView ID="gridview2" runat="server" AutoGenerateColumns="false" OnRowCommand="grdDtc_RowCommand">

                        <Columns>


                            <asp:TemplateField AccessibleHeaderText="DT_ID" HeaderText="DTCID" Visible="false">

                                <ItemTemplate>
                                    <asp:Label ID="lblDTCID" runat="server" Text='<%# Bind("DT_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Old DTC Code" Visible="true" ItemStyle-Width="100px">

                                <ItemTemplate>
                                    <asp:Label ID="lblDTCCode" Width="70px" Font-Bold="true" Font-Size="Larger" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DTC_NAME" HeaderText="DTC Name" ItemStyle-Width="200px" Visible="true">

                                <ItemTemplate>
                                    <asp:Label ID="lblDTCName" Width="200px" runat="server" Text='<%# Bind("DT_NAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="DT_TC_ID" HeaderText="DTR Code" Visible="false" ItemStyle-Width="70px">

                                <ItemTemplate>
                                    <asp:Label ID="lblTCCode" Width="70px" Font-Bold="true" Font-Size="Larger" runat="server" Text='<%# Bind("DT_TC_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:TemplateField AccessibleHeaderText="OldFeederCode" HeaderText="Old Feeder Code" Visible="true" ItemStyle-Width="70px">

                                <ItemTemplate>
                                    <asp:Label ID="lblOldFeederCode" runat="server" Text='<%# Bind("OLD_FEEDER_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="NewFeederCode" HeaderText="New Feeder Code" Visible="true" ItemStyle-Width="70px">

                                <ItemTemplate>
                                    <asp:Label ID="lblNewFeederCode" runat="server" Text='<%# Bind("NEW_FEEDER_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>




                            <asp:TemplateField AccessibleHeaderText="Serial Number" HeaderText="Serial Number" Visible="true" ItemStyle-Width="100px">

                                <ItemTemplate>
                                    <asp:Label ID="newfeederCode" runat="server" Text="FEEDERCODE" />
                                </ItemTemplate>

                                <ItemTemplate>
                                    <asp:TextBox ID="lblDTCseraial" runat="server" Text='<%# Bind("DT_SERIAL_NUMBER") %>' ToolTip="Please Enter the Last Three Digit" CssClass="ChkBoxClass"
                                        Width="30px" MaxLength="2" onblur="getValue(this)" OnKeyPress="javascript: return OnlyAlphabetNumbers(event);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                               <asp:TemplateField HeaderText="Select" ItemStyle-Width="30px">
                                    <ItemTemplate>
                                    <center>
                                        <asp:ImageButton ID="imgBtnAuto" runat="server" Height="33px" ImageUrl="~/Styles/images/autogenerate.png" CommandName="AutoGenerate"
                                            />
                                    </center>
                                </ItemTemplate>
                               </asp:TemplateField>

                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="30px">

                              
                                <%--<HeaderTemplate>
                        <asp:CheckBox ID="multipleSelect" CssClass="ChkBoxClass" runat="server" onclick="CheckAllEmp(this);"></asp:CheckBox>
                    </HeaderTemplate>
                         // OnKeyPress="javascript: return OnlyAlphabetNumbers(event);"
                                --%>


                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" CssClass="ChkBoxClass" runat="server" onclick="UnCheckAllEmp(this);"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <center>
                                        <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" CommandName="search"
                                            Width="12px" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#404040" Font-Size="Medium" Width="15px" />
                        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:GridView>
                </div>
            </div>
        </div>











        <%--<div class="span1">
        <asp:Button ID="btnUpdate" CssClass="fc-button" Style="top: 83px; left: -31px" runat="server" Font-Bold="true" Text="Add to Grid ---->" Visible="false" OnClick="btnUpdate_click" OnClientClick="ValidateSelectedDTC();" />
    </div>--%>


        <div class="span5">
        </div>

    </div>

    <div class="span7"></div>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <!--</div>




              <!--    </div>
            </div>

        </div>
    </div> -->


    <!-- </div> -->
</asp:Content>
