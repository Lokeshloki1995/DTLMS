<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="ApprovalPending.aspx.cs" Inherits="IIITS.DTLMS.Approval.ApprovalPending" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <script type="text/javascript">

    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip(); 
    });

    function onlyAlphabetsnum(e, t) {
        var code = ('charCode' in e) ? e.charCode : e.keyCode;
        if (!(code > 47 && code < 58) &&
          !(code > 64 && code < 91) && // upper alpha (A-Z)
          !(code > 96 && code < 123)) { // lower alpha (a-z)
            e.preventDefault();
        }
    }
    function onlyAlphabets(e, t) {
        var code = ('charCode' in e) ? e.charCode : e.keyCode;
        if (
            //!(code > 47 && code < 58) &&
          !(code > 64 && code < 91) && // upper alpha (A-Z)
          !(code > 96 && code < 123)) { // lower alpha (a-z)
            e.preventDefault();
        }
    }
    function onlyAlphabetsnumandper(e, t) {
        var code = ('charCode' in e) ? e.charCode : e.keyCode;
        if (!(code > 47 && code < 58) &&
            !(code > 64 && code < 91) && // upper alpha (A-Z)
            !(code > 96 && code < 123) && // lower alpha (a-z)
            !(code != 37)) { // % character
            e.preventDefault();
        }
    }
    function OnlyNumber(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        } else {

            return true;
        }
    }
    function cleanSpecial(t) {

        t.value = t.value.toString().replace(/[^a-zA-Z 0-9 _ ./\t\n\r\-]+/g, '');
    }


        </script>

    <style type="text/css">
        .Normal
        {
            background-color: Lime;
            vertical-align: middle;
            text-align: center;
            position: absolute;
        }
        .Warning
        {
            font-weight: bold;
            text-align: center;
        }
    </style>
     <style type="text/css">
         .ascending th a {
        background:url(/img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(/img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
         background: url(/img/sort_both.png) no-repeat;
         display: block;
          padding: 0 4px 0 20px;
     }
    </style>
    
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        Failure Pending Details
                    </h3>
                    <ul class="breadcrumb" style="display: none;">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="Text1" type="text">
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>
                                </button>
                            </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
            </div>
            <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>Approval Pending Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span5">
                                          <div class="control-group">
                                            <label class="control-label"> Division</label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true"
                                                             TabIndex="1" onselectedindexchanged="cmbDiv_SelectedIndexChanged"> </asp:DropDownList>   
                                                </div>
                                              
                                            </div>
                                            </div>

                                             <div class="control-group">
                                            <label class="control-label">Sub Division</label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                          <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true"
                                                             TabIndex="1" onselectedindexchanged="cmbSubDiv_SelectedIndexChanged" > </asp:DropDownList>   
                                                    </div>
                                                   
                                                </div>
                                        </div>  
                                        </div>
                                        <div class="span5">
                                          <div class="control-group">
                                            <label class="control-label">Section </label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                          <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true"
                                                             TabIndex="1" > </asp:DropDownList>   
                                                    </div>
                                                </div>
                                         </div>
                                          <div class="control-group">
                                            <label class="control-label">Type</label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbType" runat="server"  TabIndex="1" > 
                                                     </asp:DropDownList>   
                                                </div>
                                            </div>
                                         </div>   

                                            <div class="span5">
                                                <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" Width="116px"
                                                    OnClick="cmdLoad_Click" />
                                            </div>
                                            <%--  <div class="span5">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickApprovalPending" /><br />
                                          </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="widget-body">
                           <div style="float: right">
                            </div>
                            <!-- END FORM-->
                            <div style="float: right">
                                <%--<div class="span2">
                                    <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                                        OnClick="cmbExport_Click" Visible="false" /> <br />
                                </div>--%>
                                <asp:HiddenField ID="hdfOffCode" runat="server" />
                            </div>
                            <div class="space20">
                            </div>
                            <div style=" overflow: auto;">
                                <asp:GridView ID="grdApprovePending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" OnPageIndexChanging="grdApprovePending_PageIndexChanging"
                                    OnRowCreated="grdApprovePending_RowCreated" 
                                    OnRowCommand="grdApprovePending_RowCommand" 
                                    onrowdatabound="grdApprovePending_RowDataBound"  OnSorting="grdApprovePending_Sorting" AllowSorting="true">
                               <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="Transformer Centre Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">

                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="100px"  onkeypress="return onlyAlphabets(event,this);" MaxLength="9"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure ID" Visible="true" SortExpression="DF_ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDf_Id" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>

                                                <asp:TextBox ID="txtDF_ID" runat="server" MaxLength="9" onkeypress="return OnlyNumber(event)" onchange=" return cleanSpecial(this)" autocomplete="off" placeholder="Enter Failure Id " Width="100px" oncopy="return true" onpaste="return true">
                                                </asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="Transformer Centre Name" Visible="true" SortExpression="DT_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">

                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name "  Width="150px"
                                                   onkeypress="return onlyAlphabetsnumandper(event,this);" MaxLength="20"></asp:TextBox>

                                                

                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OMSECTION" HeaderText="Section Name" Visible="true" SortExpression="OMSECTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmSectionName" runat="server" Text='<%# Bind("OMSECTION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                                 
                                        <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                    Width="210px" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                  
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                   </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
  
</asp:Content>
