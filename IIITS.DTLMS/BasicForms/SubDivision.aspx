<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="SubDivision.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.SubDivision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script src="../Scripts/functions.js" type="text/javascript"></script>

                                    <script type="text/javascript">
        $(document).ready(function () {
            $('textarea').attr('maxlength', '300');
            $('input').keyup(function () {
                $('input').keyup(function () {
                    $(this).val($(this).val().replace(/^\s+/, ""));
                }

            )
            })
            $('textarea').keyup(function () {
                $('textarea').keyup(function () {
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

 <script type="text/javascript">

     function Validate() {

         if (document.getElementById('<%=cmbCircle.ClientID %>').value.trim() == "--Select--") {
             alert('Select  Circle')
             document.getElementById('<%= cmbCircle.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%=cmbDivision.ClientID %>').value.trim() == "--Select--") {
             alert('Select  Division')
             document.getElementById('<%= cmbDivision.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtSubDivCode.ClientID %>').value.trim() == "") {
             alert('Enter Sub-Division Code')
             document.getElementById('<%= txtSubDivCode.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtName.ClientID %>').value.trim() == "") {
             alert('Enter valid Sub-Division Name ')
             document.getElementById('<%= txtName.ClientID %>').focus()
             return false
         }

        // var Name = document.getElementById('<%= txtName.ClientID %>').value;
        // var Namecon = /^([a-zA-Z0-9])+(\s-\s)*([a-zA-Z0-9])+$/;
        // if (!Name.match(Namecon)) {
            // alert('Enter valid Sub-Division Name')
            // document.getElementById('<%= txtName.ClientID %>').focus()
               // return false
           // }

       
         if (document.getElementById('<%= txtOfficeHead.ClientID %>').value.trim() == "") {
             alert('Enter SubDivision head')
             document.getElementById('<%= txtOfficeHead.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtMobileNo.ClientID %>').value.trim() == "") {
             alert('Enter valid Mobile Number')
             document.getElementById('<%= txtMobileNo.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtPhoneNo.ClientID %>').value.trim() == "") {
             alert('Enter valid Phone No')
             document.getElementById('<%= txtPhoneNo.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtEmail.ClientID %>').value.trim() == "") {
             alert('Enter Email Id')
             document.getElementById('<%= txtEmail.ClientID %>').focus()
             return false
         }

     }
</script>

                <script language="Javascript" type="text/javascript">


                    function onlyAlphabets(e, t) {
                        var code = ('charCode' in e) ? e.charCode : e.keyCode;
                        if ( // space
                          !(code == 32) && // Special characters
                            !(code > 39 && code < 42) &&
                            !(code > 45 && code < 48) &&
                          !(code > 64 && code < 91) && // upper alpha (A-Z)
                          !(code > 96 && code < 123)) { // lower alpha (a-z)
                            e.preventDefault();
                        }
                    }

                    function Address(e, t) {
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
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
<div class="span10">
<!-- BEGIN THEME CUSTOMIZER-->
                 
<!-- END THEME CUSTOMIZER-->
<!-- BEGIN PAGE TITLE & BREADCRUMB-->
<h3 class="page-title" runat="server" id="Create">
Create SubDivision
</h3>
    <h3 class="page-title" runat="server" id="Update">
Update SubDivision
</h3>

<ul class="breadcrumb" style="display:none">
                       
<li class="pull-right search-wrap">
<form action="" class="hidden-phone">
<div class="input-append search-input-area">
<input class="" id="appendedInputButton" type="text">
<button class="btn" type="button"><i class="icon-search"></i> </button>
</div>
</form>
</li>
</ul>
<!-- END PAGE TITLE & BREADCRUMB-->

</div>
<div style="margin-top:20px" class="span2"><asp:Button ID="cmdSubDivisionView" class="btn btn-primary" Text="SubDivision View" OnClientClick="javascript:window.location.href='SubDivisionView.aspx'; return false;" runat="server" />  </div>
</div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4 runat="server" id="CreateSubDivision"><i class="icon-reorder"></i>SubDivision Office Entry</h4>
                             <h4 runat="server" id="UpdateSubDivision"><i class="icon-reorder"></i>Update SubDivision Office Entry</h4>
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
                                                <label class="control-label">Circle<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" 
                                                          AutoPostBack="true" onselectedindexchanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server"  AutoPostBack="true"
                                                            onselectedindexchanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                                     <div class="control-group">
                                                <label class="control-label">Sub-Division Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtSubDivCode" runat="server" onkeypress="return OnlyNumber(event)"  MaxLength="3"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">Sub Division Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="50" onkeypress="return Address(event,this);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                              <div class="control-group">
                                                <label class="control-label">SubDivision Head<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtOfficeHead" runat="server" MaxLength="20" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                                        <asp:TextBox ID="txtSubDivId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                             
                                           
                                           
                                        </div>
                                        <div class="span5">
                                         

                                          

                                            <div class="control-group">
                                                <label class="control-label">MobileNo<span class="Mandotary"> *</span></label>


                                                <div class="controls">
                                                    <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>  

                                            <div class="control-group">
                                                <label class="control-label">PhoneNo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                         <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="12" onkeypress="javascript:return OnlyNumberHyphen(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>  

                                            <div class="control-group">
                                                <label class="control-label">Email<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-prepend">
                                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" onkeypress="javascript:return validateEmail(txtEmailId);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>  

                                               <div class="control-group">
                                                <label class="control-label">Address</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                         <asp:TextBox ID="txtaddress" runat="server" MaxLength="300" Height="60px"
                                                        TextMode="MultiLine" Style="resize: none"
                                                         onkeypress="return Address(event,this);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>                                         

                                            </div>
    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center">

                                       
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return Validate()" CssClass="btn btn-success" />
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            onclick="cmdReset_Click"  CssClass="btn btn-danger" /><br />
                                   
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                            
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
         
      </div>
</asp:Content>
