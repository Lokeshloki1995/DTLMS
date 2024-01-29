<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCCommision.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTCCommision" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

      <script type="text/javascript" >
          function ValidateMyForm() {
              if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                  alert('Enter Valid  Transformer Centre Code')
                  document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtDTCName.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Transformer Centre Name')
                  document.getElementById('<%= txtDTCName.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtOMSection.ClientID %>').value.trim() == "") {
                  alert('Enter OM Section')
                  document.getElementById('<%= txtOMSection.ClientID %>').focus()
                  return false
              }
              //              if (document.getElementById('<%= txtInternalCode.ClientID %>').value.trim() == "") {
              //                  alert('Enter Valid Internal Code')
              //                  document.getElementById('<%= txtInternalCode.ClientID %>').focus()
              //                  return false
              //              }
              //              if (document.getElementById('<%= txtConnectedKW.ClientID %>').value.trim() == "") {
              //                  alert('Enter Connected KW')
              //                  document.getElementById('<%= txtConnectedKW.ClientID %>').focus()
              //                  return false
              //              }

              //              if (document.getElementById('<%= txtConnectedHP.ClientID %>').value.trim() == "") {
              //                  alert('Enter Connected HP')
              //                  document.getElementById('<%= txtConnectedHP.ClientID %>').focus()
              //                  return false
              //              }



              if (document.getElementById('<%= txtTCCode.ClientID %>').value.trim() == "") {
                  alert('Enter valid Transformer Code')
                  document.getElementById('<%= txtTCCode.ClientID %>').focus()
                  return false
              }


              //              if (document.getElementById('<%= txtCommisionDate.ClientID %>').value.trim() == "") {
              //                  alert('Enter Commission Date')
              //                  document.getElementById('<%=txtCommisionDate.ClientID %>').focus()
              //                  return false
              //              }

              //              if (document.getElementById('<%= txtServiceDate.ClientID %>').value.trim() == "") {
              //                  alert('Enter Valid Service Date')
              //                  document.getElementById('<%=txtServiceDate.ClientID %>').focus()
              //                  return false
              //              }


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

    <%--<script>
        $('#cmdSave').click(function () {
            $('#img').show();
            $.ajax({

                success: function (result) {
                    $('#img').hide();
                }
            })
        });
    </script>--%>


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
               Commissioning of Transformer Centre
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
                      <asp:Button ID="cmdClose" runat="server" Text="Transformer Centre View"                                     
                            CssClass="btn btn-primary" 
                          OnClientClick="javascript:window.location.href='DTCView.aspx'; return false;" 
                          onclick="cmdClose_Click" /></div>
                                      
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Commissioning of Transformer Centre</h4>
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
                        <label class="control-label">Select Feeder</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbFeeder" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbFeeder_SelectedIndexChanged">
                                     <%-- AutoPostBack="True" onselectedindexchanged="cmbFeeder_SelectedIndexChanged"--%>
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>


                                    
                   <div class="control-group">
                        <label class="control-label">Transformer Centre Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:HiddenField ID="hdfTcCode" runat="server" />
                                 <asp:HiddenField ID="hdfDTRImagePath" runat="server" />
                                  <asp:HiddenField ID="hdfDTCImagePath" runat="server" />
                                <asp:HiddenField ID="hdnServicedate" runat="server" />
                                <asp:TextBox ID="txtDTCId"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="4" Visible="false"></asp:TextBox>                    
                                <asp:TextBox ID="txtDTCCode"  runat="server" ReadOnly="true" MaxLength="9" TabIndex="1"></asp:TextBox>            
                            </div>
                        </div>
                    </div>
   
              
                 <div class="control-group">
                        <label class="control-label">Transformer Centre Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                                      
                                <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" 
                                    onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="2" ></asp:TextBox><br />
                                    <asp:LinkButton ID="lnkDTCHistory" runat="server"  
                                    style="font-size:12px;color:Blue" onclick="lnkDTCHistory_Click" 
                                                        >View Transformer Centre History</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    
                      <div class="control-group">
                        <label class="control-label">O & M Section<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtOMSection" runat="server" MaxLength="10" TabIndex="3" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                        <asp:Button ID="btnOmSearch" runat="server" Text="S" 
                                             CssClass="btn btn-primary" />                
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
                                    <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" TabIndex="4"></asp:TextBox>               
                            </div>
                        </div>
                    </div>

                   <div class="control-group">
                        <label class="control-label">TIMS Code</label>
                        <div class="controls">
                            <div class="input-append">
                                      <asp:TextBox ID="txtTimsCode" runat="server" MaxLength="12" ></asp:TextBox>
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
                        <label class="control-label">Connected HP</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="6"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>    
                                
                       <%-- adde by santhosh start --%>                     
                                            
                                                        
                     <div class="control-group">
                        <label class="control-label">Transformer Centre Meters Available<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddldtcmeters"  runat="server" AutoPostBack="true"
                                     onselectedindexchanged="ddldtcmeters_SelectedIndexChanged" TabIndex="10">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>
                                            
                       <%-- End --%>                                              
                                            
                                                                          
                      <div class="control-group" id="DivKWHReading" runat="server">
                        <label class="control-label">KWH Reading</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtKWHReading" runat="server" MaxLength="11"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="7"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                         <div class="control-group">
                        <label class="control-label">Average Load (in KVA):</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtAvgLoad" runat="server" MaxLength="10"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="7"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                       <div class="control-group">
                        <label class="control-label">Peak Load (in KVA):</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtPeakLoad" runat="server" MaxLength="10"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="7"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">Surplus Capacity (in KVA):</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtSurplusCap" runat="server" MaxLength="10"  
                                        onkeypress="javascript:return AllowNumber(this,event);" TabIndex="7"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                  

                </div>
                <div class="span5"> 
                    <div class="control-group">
                        <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTCCode" runat="server" MaxLength="10"  Enabled="false"
                                        onkeypress="javascript:return OnlyNumber(event);" TabIndex="17"></asp:TextBox>
                                       <asp:Button ID="cmdSearch" runat="server" Text="S"  Visible="false"
                                       CssClass="btn btn-primary" onclick="cmdSearch_Click" TabIndex="18" />    
                                <asp:TextBox ID="txtOldTCCode" runat="server" Visible="false" Width="20px"></asp:TextBox>     
                                                    
                            </div>
                        </div>
                    </div>      
                                  
                     <div class="control-group">
                        <label class="control-label">DTr Make<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTCMake" runat="server" Enabled="false" TabIndex="19" ></asp:TextBox>
                                                                        
                            </div>
                        </div>
                    </div>
                          
                     <div class="control-group">
                        <label class="control-label">DTr Capacity(in KVA)<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtCapacity" runat="server"  Enabled="false" TabIndex="20" ></asp:TextBox>
                                                                      
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
                        <label class="control-label">Transformer Centre Commission Date<span class="Mandotary"> *</span></label>
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
                        <label class="control-label">MLA Constituency<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMla" runat="server" TabIndex="20" ></asp:TextBox>
                                                                      
                            </div>
                        </div>
                    </div> 

                     <div class="control-group">
                        <label class="control-label">MP Constituency<span class="Mandotary"> </span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMP" runat="server"  TabIndex="20" ></asp:TextBox>
                                                                      
                            </div>
                        </div>
                    </div> 
                  <div class="control-group">
                            <label class="control-label">Location Type<span class="Mandotary"> *</span></label>
                                     <div class="controls">
                                            <div class="input-append">
                                                 <asp:DropDownList ID="cmdloctype" runat="server" TabIndex="2" >
                                                  </asp:DropDownList>

                                             </div>
                                         </div>
                     </div>

         <div class="control-group">
                        <label class="control-label">Electrical Inspectorate Report Letter number</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtEleRateNo" runat="server" TabIndex="23" MaxLength="10" ></asp:TextBox>                                                             
                            </div>
                        </div>
                    </div>

                     
                     <div class="control-group">
                        <label class="control-label">Electrical Inspectorate Report Letter Date</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtEleDate" runat="server" TabIndex="23" MaxLength="10" ></asp:TextBox>
                                      <asp:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtEleDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                         
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
                                         <img src="../img/Process/Processing.gif" id="img" style="display:none"/ >
                                        <asp:Button ID="cmdSave" runat="server" Text="Save and Continue" 
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" 
                                                TabIndex="26" onclick="cmdSave_Click" />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary"  TabIndex="27" onclick="cmdReset_Click" /><br />
                                    </div>
                                     <div class="span6" align="right">
                                          <asp:Button ID="cmdNext" runat="server" Text="Next>>" 
                                             CssClass="btn btn-large"  TabIndex="28" Visible ="false" 
                                              onclick="cmdNext_Click" /><br />
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
                                                     <label >Transformer Centre Code Photo </label>
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
