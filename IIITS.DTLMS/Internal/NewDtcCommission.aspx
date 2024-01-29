<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="NewDtcCommission.aspx.cs" Inherits="IIITS.DTLMS.Internal.NewDtcCommission" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

      <script type="text/javascript" >
          function ValidateMyForm() {

              if (document.getElementById('<%= cmbCircle.ClientID %>').value.trim() == "--Select--") {
                  alert('Select Circle')
                  document.getElementById('<%= cmbCircle.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= cmbDivision.ClientID %>').value.trim() == "--Select--") {
                  alert('Select Division')
                  document.getElementById('<%= cmbDivision.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= cmbsubdivision.ClientID %>').value.trim() == "--Select--") {
                  alert('Select SubDivision')
                  document.getElementById('<%= cmbsubdivision.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= cmbSection.ClientID %>').value.trim() == "--Select--") {
                  alert('Select Section')
                  document.getElementById('<%= cmbSection.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= cmbFeeder.ClientID %>').value.trim() == "--Select--") {
                  alert('Select Feeder')
                  document.getElementById('<%= cmbFeeder.ClientID %>').focus()
                  return false
              }

              if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                  alert('Enter Valid  DTC Code')
                  document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtDTCName.ClientID %>').value.trim() == "") {
                  alert('Enter Valid DTC Name')
                  document.getElementById('<%= txtDTCName.ClientID %>').focus()
                  return false
              }



              if (document.getElementById('<%= txtTCCode.ClientID %>').value.trim() == "") {
                  alert('Enter valid DTr Code')
                  document.getElementById('<%= txtTCCode.ClientID %>').focus()
                  return false
              }


          }



          function DisplayFullImage(ctrlimg) {
              txtCode = "<HTML><HEAD>"
        + "</HEAD><BODY TOPMARGIN=0 LEFTMARGIN=0 MARGINHEIGHT=0 MARGINWIDTH=0><CENTER>"
        + "<IMG src='" + ctrlimg.src + "' BORDER=0 NAME=FullImage "
        + "onload='window.resizeTo(document.FullImage.width,document.FullImage.height)'>"
        + "</CENTER>"
        + "</BODY></HTML>";
              mywindow = window.open('', 'image', '');
              mywindow.document.open();
              mywindow.document.write(txtCode);
              mywindow.document.close();

          }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
 </asp:ScriptManager>
 <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
               Commissioning of DTC
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
                <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="cmdClose" runat="server" Text="Close"                                     
                            CssClass="btn btn-primary" 
                          OnClientClick="javascript:window.location.href='EnumerationView.aspx'; return false;" /></div>
                                      
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
                                                   <label class="control-label">Circle Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                            <asp:DropDownList ID="cmbCircle" runat="server"  AutoPostBack="true" 
                                                                TabIndex="1" onselectedindexchanged="cmbCircle_SelectedIndexChanged" >                                   
                                                            </asp:DropDownList>
                                                           <asp:TextBox ID="txtStatus" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                       </div>
                                                    </div>
                                               </div>

                                               <div class="control-group">
                                                   <label class="control-label">Division Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbDivision" runat="server"  AutoPostBack="true" 
                                                                TabIndex="2" onselectedindexchanged="cmbDivision_SelectedIndexChanged" >                                   
                                                            </asp:DropDownList>
                                                            
                                                       </div>
                                                    </div>
                                               </div>
                                                
                                               <div class="control-group">
                                                   <label class="control-label">Sub Division Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbsubdivision" runat="server"  AutoPostBack="true" 
                                                                TabIndex="3" onselectedindexchanged="cmbsubdivision_SelectedIndexChanged">                                   
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtEnumDetailsId" runat="server" MaxLength="50" Visible ="false" Width="50px" ></asp:TextBox>
                                                       </div>
                                                    </div>
                                               </div>

                                               <div class="control-group">
                                                   <label class="control-label">Section Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server"  TabIndex="4">                                   
                                                            </asp:DropDownList>
                                                       </div>
                                                    </div>
                                               </div>

                                               <div class="control-group">
                                                   <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbFeeder" runat="server"  TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="cmbFeeder_SelectedIndexChanged" >                                   
                                                            </asp:DropDownList>
                                                       </div>
                                                    </div>
                                               </div>

                                               <div class="control-group">
                        <label class="control-label">DTC Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:HiddenField ID="hdfTcCode" runat="server" />
                                 <asp:HiddenField ID="hdfDTRImagePath" runat="server" />
                                  <asp:HiddenField ID="hdfDTCImagePath" runat="server" />
                                <asp:TextBox ID="txtDTCId"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="9" Visible="false"></asp:TextBox>                    
                                <asp:TextBox ID="txtDTCCode"  runat="server" 
                                          MaxLength="9" TabIndex="1"></asp:TextBox>            
                            </div>
                        </div>
                    </div>
   
              
                 <div class="control-group">
                        <label class="control-label">DTC Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                                      
                                <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" 
                                    onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="2" ></asp:TextBox><br />
                            </div>
                        </div>
                    </div>
                      
                    <div class="control-group">
                        <label class="control-label">Internal Code</label>
                        <div class="controls">
                            <div class="input-append">
                                      <asp:TextBox ID="txtWOslno" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                       <asp:TextBox ID="txtWFOId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                       <asp:TextBox ID="txtActiontype" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" TabIndex="4" onkeypress="javascript:return AllowCharNum(event);" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                                                      

                      <div class="control-group">
                        <label class="control-label">Connected KW</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectedKW" runat="server" MaxLength="6"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="5"   ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                                             <div class="control-group">
                                                <label class="control-label">DTC Code Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupDTCCode" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif" 
                                                            TabIndex="19" />
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Structure Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupDTCStructure" runat="server" AllowMultiple="False" accept=".jpg,.jpeg,.png,.gif" 
                                                            TabIndex="19" />
                                                    </div>
                                                </div>
                                            </div>
                                              <asp:TextBox ID="txtDTCCodePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                               <asp:TextBox ID="txtDTCStructurePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                    
                                                        
                    
                                                        
                      
                </div>
                <div class="span5"> 
                    <div class="control-group">
                        <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTCCode" runat="server" MaxLength="10" Text="0"  Enabled="false"
                                        onkeypress="javascript:return OnlyNumber(event);" TabIndex="17"></asp:TextBox>   
                                <asp:TextBox ID="txtOldTCCode" runat="server" Visible="false" Width="20px"></asp:TextBox>     
                                                    
                            </div>
                        </div>
                    </div>      
                                  
                     <div class="control-group">
                        <label class="control-label">DTr Make<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTCMake" runat="server" Text="0" Enabled="false" TabIndex="19" ></asp:TextBox>
                                                                        
                            </div>
                        </div>
                    </div>
                          
                     <div class="control-group">
                        <label class="control-label">DTr Capacity(in KVA)<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtCapacity" runat="server" Text="0"  Enabled="false" TabIndex="20" ></asp:TextBox>
                                                                      
                            </div>
                        </div>
                    </div>      
                                  
                  
                          <%-- <div class="control-group">
                        <label class="control-label">Connection Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectionDate" runat="server" TabIndex="21" MaxLength="10" ></asp:TextBox>
                                     <asp:CalendarExtender ID="txtConnectionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtConnectionDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                   
                            </div>
                        </div>
                    </div>--%>
                    
                    <div class="control-group">
                        <label class="control-label">Commission Date<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtCommisionDate" runat="server" TabIndex="24" MaxLength="10" ></asp:TextBox>
                                      <asp:CalendarExtender ID="txtCommisionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtCommisionDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                       
                            </div>
                        </div>
                    </div>           
                    
                    <div class="control-group">
                        <label class="control-label">Last Service Date</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtServiceDate" runat="server" TabIndex="23" MaxLength="10" ></asp:TextBox>
                                      <asp:CalendarExtender ID="txtServiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtServiceDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                         
                            </div>
                        </div>
                    </div>
                    
                    <div class="control-group">
                        <label class="control-label">Project/Scheme Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbprojecttype" runat="server"  TabIndex="14">
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>
                   
                    <div class="control-group">
                        <label class="control-label">Feeder Change Date</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtFeederChngDate" runat="server" TabIndex="25"  MaxLength="10" ></asp:TextBox>
                                       <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtFeederChngDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                          
                            </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">KWH Reading</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtKWHReading" runat="server" MaxLength="10"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="7"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Connected HP</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="6"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
      
                     </div>  
  
                                      </div>                
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                               <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span2">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" 
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" 
                                                TabIndex="26" onclick="cmdSave_Click"/>
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary"  TabIndex="27" onclick="cmdReset_Click"/><br />
                                    </div>
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>


                                      <div class="form-horizontal">
                                       <div class="row-fluid">
                                        <div class="span1"></div>
                                          <div class="span5">
                                               <div class="control-group" runat="server" id="dvDTCCode" style="display:none">
                                               <div align="center">
                                                     <label >DTC Code Photo </label>
                                                   <div align="center"> 
                                                           <asp:Image ID="imgDTCCode"  BorderColor="lightgray" BorderWidth="3px"  Height="58px" Width="400px" 
                                                            runat="server" ImageUrl = "../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);"    />
                                                    </div>
                                                </div>
                                             </div>  
                                           </div>
                                            <div class="span5">
                                               <div class="control-group" runat="server" id="dvDTrCode" style="display:none">
                                               <div align="center">
                                                     <label >DTr Code Photo </label>
                                                   <div align="center"> 
                                                           <asp:Image ID="imgDTrCode"  BorderColor="lightgray" BorderWidth="3px"  Height="58px" Width="400px" 
                                                            runat="server" ImageUrl = "../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);"    />
                                                    </div>
                                                </div>
                                             </div>  
                                           </div>
                                        </div>
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
