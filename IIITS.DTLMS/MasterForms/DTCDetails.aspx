<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCDetails.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTCDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= ddldtcmeters.ClientID %>').value.trim() == "--Select--") {
                alert('Enter Transformer Centre Meter')
                document.getElementById('<%= ddldtcmeters.ClientID %>').focus()
                return false
            }
            <%--if (document.getElementById('<%= cmbBreaker.ClientID %>').value == "--Select--") {
                  alert('Select Breaker Type')
                  document.getElementById('<%= cmbBreaker.ClientID %>').focus()
                  return false
            }--%>
            if (document.getElementById('<%= ddlArresters.ClientID %>').value == "--Select--") {
                alert('Select Lightining Arrest')
                document.getElementById('<%= ddlArresters.ClientID %>').focus()
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
                    <h3 class="page-title">Commisioning of Transformer Centre
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
                    <asp:Button ID="cmdClose" runat="server" Text="DTC View"
                        CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='DTCView.aspx'; return false;" />
                </div>

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Commissioning of DTC</h4>
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
                                                <label class="control-label">Transformer Centre Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTCCode" runat="server"
                                                            MaxLength="6" TabIndex="1" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Platform Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbPlatformType" runat="server"
                                                            TabIndex="8">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtDTCId" runat="server" TabIndex="15" MaxLength="10" Visible="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <%--<div class="control-group">
                        <label class="control-label">Breaker Type</label>
                        <label class="control-label">HT Isolation Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlBreakertype"   runat="server"
                                     TabIndex="9">                                    
                                </asp:DropDownList>   
                            </div>
                        </div>
                    </div>--%>



                                            <div class="control-group" style="display:none">
                                                <label class="control-label">Transformer Centre Meters Available<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddldtcmeters" runat="server"
                                                            TabIndex="10">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">HT Protection</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlhtprotection" runat="server"
                                                            TabIndex="11">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">LT Protection</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlLTProtection" runat="server"
                                                            TabIndex="12">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Grounding</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlgrounding" runat="server"
                                                            TabIndex="13">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Lightning Arresters<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlArresters" runat="server"
                                                            TabIndex="14">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">RAPDRP</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRAPDRP" runat="server"
                                                            TabIndex="14">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" style="display: block">
                                                <label class="control-label">HT Line Length (in KM)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txthtLine" runat="server" TabIndex="15" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);" placeholder="Eg: 111.111"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">LT Line Length (in KM)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtltLine" runat="server" TabIndex="16" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);" placeholder="Eg: 111.111"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Transformer Centre Name</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50"
                                                            onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="2" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Load Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbLoadtype" runat="server" TabIndex="14">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" style="display: none">
                                                <label class="control-label">Nature of Work</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbprojecttype" runat="server" TabIndex="14">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">WDV(In RS)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDepreciation" runat="server" TabIndex="16" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Latitude</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLatitude" runat="server" TabIndex="16" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Longitude</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtlongitude" runat="server" TabIndex="16" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Availability OF GOS</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGOS" runat="server"
                                                            TabIndex="14">
                                                            <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" style="display: block">
                                                <label class="control-label">Circuit 1</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCircute1" runat="server" TabIndex="15" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: block">
                                                <label class="control-label">Circuit 2</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCircute2" runat="server" TabIndex="15" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: block">
                                                <label class="control-label">Circuit 3</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCircute3" runat="server" TabIndex="15" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: block">
                                                <label class="control-label">Circuit 4</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCircute4" runat="server" TabIndex="15" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--<div class="control-group">
                        <label class="control-label">Breaker Type<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbBreaker" runat="server"
                                     TabIndex="14">
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>--%>
                                        </div>

                                    </div>
                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>

                                <div class="text-center">



                                    <asp:Button ID="cmbBack" runat="server" Text="Back"
                                        CssClass="btn btn-primary"
                                        TabIndex="26" OnClick="cmbBack_Click" />


                                    <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click"
                                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary"
                                        TabIndex="26" />



                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        CssClass="btn btn-danger" OnClick="cmdReset_Click" TabIndex="27" /><br />

                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                </div>
                            </div>


                        </div>

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
